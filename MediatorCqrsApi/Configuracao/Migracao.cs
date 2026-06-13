using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Infra.Contexto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MediatorCqrsApi.Configuracao
{
    internal static class Migracao
    {
        public static string ResolveConnectionString(IConfiguration configuration)
        {
            var connectionStringsGravacao = configuration["ConnectionStrings:Gravacao"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(connectionStringsGravacao))
            {
                var filePath = configuration["FileSettings:FilePath"] ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                {
                    connectionStringsGravacao = File.ReadAllText(filePath).Replace("\\\\", "\\");
                }
            }

            return connectionStringsGravacao;
        }

        public static void ApplyStartupMigrations(IServiceProvider applicationServices, IConfiguration configuration, bool reinstallDatabase, bool autoMigrate)
        {
            using var scope = applicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ContextoGenerico>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var connectionStringsGravacao = ResolveConnectionString(configuration);

            // Prefer a context constructed with the resolved connection string to avoid DI-registered empty connections
            ContextoGenerico activeDbContext = dbContext;
            bool createdTemp = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(connectionStringsGravacao))
                {
                    activeDbContext = new ContextoGenerico(new DbContextOptionsBuilder<ContextoGenerico>().UseSqlServer(connectionStringsGravacao).Options);
                    createdTemp = true;
                }

                // Ensure database exists on the server
                EnsureDatabaseExists(activeDbContext, logger, connectionStringsGravacao);

                // Initialize database: create if missing, apply pending migrations or reinstall when requested
                InitializeDatabase(activeDbContext, logger, reinstallDatabase, autoMigrate, connectionStringsGravacao);

                // Ensure Usuario.Id is Guid in database if mapping expects Guid (after migrations)
                EnsureUsuarioIdGuid(activeDbContext, logger);

                // Apply non-destructive schema changes based on model (adds, safe alters)
                ApplyNonDestructiveSchemaChanges(activeDbContext, logger, connectionStringsGravacao);

                // Detect destructive schema changes and generate script for review
                DetectDestructiveSchemaChanges(activeDbContext, logger, connectionStringsGravacao);

                SeedDatabase(activeDbContext, logger, connectionStringsGravacao);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao aplicar migrações e seed de dados. Verifique se o SQL Server está disponível e se a ConnectionString está correta.");
                throw;
            }
            finally
            {
                if (createdTemp)
                {
                    activeDbContext.Dispose();
                }
            }
        }

        private static void EnsureUsuarioIdGuid(ContextoGenerico dbContext, ILogger logger)
        {
            try
            {
                using var conn = dbContext.Database.GetDbConnection();
                if (string.IsNullOrWhiteSpace(conn.ConnectionString))
                {
                    logger.LogWarning("Database connection string is empty — skipping Usuario.Id conversion.");
                    return;
                }

                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Usuario' AND COLUMN_NAME = 'Id'";
                var result = cmd.ExecuteScalar();
                var dataType = result?.ToString();

                if (string.Equals(dataType, "int", StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogInformation("Usuario.Id is int in database — converting to uniqueidentifier (GUID) now.");

                    using var tx = conn.BeginTransaction();
                    cmd.Transaction = tx;

                    cmd.CommandText = "ALTER TABLE [Usuario] ADD [NewId] uniqueidentifier NOT NULL CONSTRAINT DF_Usuario_NewId DEFAULT NEWSEQUENTIALID();";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE [Usuario] SET [NewId] = NEWID() WHERE [NewId] IS NULL";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"
SELECT fk.name AS FKName, sch_child.name AS ChildSchema, tbl_child.name AS ChildTable, col_child.name AS ChildColumn
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.tables tbl_child ON fkc.parent_object_id = tbl_child.object_id
JOIN sys.columns col_child ON fkc.parent_object_id = col_child.object_id AND fkc.parent_column_id = col_child.column_id
JOIN sys.tables tbl_ref ON fkc.referenced_object_id = tbl_ref.object_id
JOIN sys.columns col_ref ON fkc.referenced_object_id = col_ref.object_id AND fkc.referenced_column_id = col_ref.column_id
JOIN sys.schemas sch_child ON tbl_child.schema_id = sch_child.schema_id
WHERE tbl_ref.name = 'Usuario' AND col_ref.name = 'Id'";

                    var fkReader = cmd.ExecuteReader();
                    var fks = new List<(string fkName, string schema, string table, string column)>();
                    while (fkReader.Read())
                    {
                        fks.Add((fkReader.GetString(0), fkReader.GetString(1), fkReader.GetString(2), fkReader.GetString(3)));
                    }

                    fkReader.Close();

                    foreach (var fk in fks)
                    {
                        var childFull = $"[{fk.schema}].[{fk.table}]";
                        var tempCol = "_NewIdTmp";

                        cmd.CommandText = $"ALTER TABLE {childFull} ADD [{tempCol}] uniqueidentifier NULL";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $@"UPDATE {childFull} SET [{tempCol}] = u.NewId FROM {childFull} c INNER JOIN [Usuario] u ON c.[{fk.column}] = u.Id";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"ALTER TABLE {childFull} ALTER COLUMN [{tempCol}] uniqueidentifier NOT NULL";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"ALTER TABLE {childFull} DROP CONSTRAINT [{fk.fkName}]";
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "SELECT name FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID('Usuario') AND type = 'PK'";
                    var pkNameObj = cmd.ExecuteScalar();
                    var pkName = pkNameObj?.ToString() ?? "PK_Usuario";

                    cmd.CommandText = $"ALTER TABLE [Usuario] DROP CONSTRAINT [{pkName}]";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "ALTER TABLE [Usuario] DROP COLUMN [Id]";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "EXEC sp_rename 'Usuario.NewId','Id','COLUMN'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "ALTER TABLE [Usuario] ADD CONSTRAINT PK_Usuario PRIMARY KEY (Id)";
                    cmd.ExecuteNonQuery();

                    foreach (var fk in fks)
                    {
                        var childFull = $"[{fk.schema}].[{fk.table}]";
                        var tempCol = "_NewIdTmp";
                        var oldCol = fk.column;

                        cmd.CommandText = $"ALTER TABLE {childFull} DROP COLUMN [{oldCol}]";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"EXEC sp_rename '{fk.schema}.{fk.table}.[{tempCol}]','{oldCol}','COLUMN'";
                        cmd.ExecuteNonQuery();

                        var fkNewName = fk.fkName;
                        cmd.CommandText = $"ALTER TABLE {childFull} ADD CONSTRAINT [{fkNewName}] FOREIGN KEY ([{oldCol}]) REFERENCES [Usuario](Id)";
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();

                    logger.LogInformation("Conversion of Usuario.Id to GUID completed successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while ensuring Usuario.Id is Guid.");
                throw;
            }
        }

        private static void InitializeDatabase(ContextoGenerico dbContext, ILogger logger, bool reinstallDatabase, bool autoMigrate, string? resolvedConnectionString = null)
        {
            try
            {
                var conn = dbContext.Database.GetDbConnection();
                var connStr = conn.ConnectionString;
                if (string.IsNullOrWhiteSpace(connStr) && !string.IsNullOrWhiteSpace(resolvedConnectionString))
                {
                    connStr = resolvedConnectionString;
                }

                if (string.IsNullOrWhiteSpace(connStr))
                {
                    logger.LogWarning("Database connection string is empty — skipping database initialization.");
                    return;
                }

                ContextoGenerico? tempCtx = string.IsNullOrWhiteSpace(dbContext.Database.GetDbConnection().ConnectionString)
                    ? new ContextoGenerico(new DbContextOptionsBuilder<ContextoGenerico>().UseSqlServer(connStr).Options)
                    : null;

                var targetCtx = tempCtx ?? dbContext;

                if (!targetCtx.Database.CanConnect())
                {
                    logger.LogInformation("Database not reachable — creating database and applying migrations.");
                    targetCtx.Database.Migrate();
                    return;
                }

                if (reinstallDatabase)
                {
                    logger.LogInformation("ReinstallOnStartup requested — dropping and recreating database.");
                    targetCtx.Database.EnsureDeleted();
                    targetCtx.Database.Migrate();
                    return;
                }

                var pending = targetCtx.Database.GetPendingMigrations()?.ToList() ?? new List<string>();
                if (pending.Any())
                {
                    if (autoMigrate)
                    {
                        logger.LogInformation("Applying {Count} pending migrations.", pending.Count);
                        targetCtx.Database.Migrate();
                    }
                    else
                    {
                        logger.LogWarning("There are {Count} pending migrations but AutoMigrate is disabled. Generate and apply migrations manually.", pending.Count);
                    }
                }
                else
                {
                    logger.LogInformation("No pending migrations detected; database is up-to-date.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while initializing database.");
                throw;
            }
        }

        private static void EnsureDatabaseExists(ContextoGenerico dbContext, ILogger logger, string? resolvedConnectionString = null)
        {
            try
            {
                var connString = resolvedConnectionString ?? dbContext.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrWhiteSpace(connString))
                {
                    logger.LogWarning("Connection string empty — skipping database existence check.");
                    return;
                }

                var builder = new SqlConnectionStringBuilder(connString);
                var targetDb = builder.InitialCatalog;
                if (string.IsNullOrWhiteSpace(targetDb))
                {
                    logger.LogWarning("No database specified in connection string — skipping create.");
                    return;
                }

                builder.InitialCatalog = "master";
                using var conn = new SqlConnection(builder.ConnectionString);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT DB_ID(@name)";
                var p = cmd.CreateParameter();
                p.ParameterName = "@name";
                p.Value = targetDb;
                cmd.Parameters.Add(p);
                var exists = cmd.ExecuteScalar();
                if (exists == DBNull.Value || exists == null)
                {
                    logger.LogInformation("Database '{Database}' does not exist — creating.", targetDb);
                    using var create = conn.CreateCommand();
                    create.CommandText = $"CREATE DATABASE [{targetDb}]";
                    create.ExecuteNonQuery();
                }
                else
                {
                    logger.LogInformation("Database '{Database}' exists.", targetDb);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while checking/creating database.");
                throw;
            }
        }

        private static void ApplyNonDestructiveSchemaChanges(ContextoGenerico dbContext, ILogger logger, string? resolvedConnectionString = null)
        {
            try
            {
                string connStr = resolvedConnectionString ?? dbContext.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrWhiteSpace(connStr))
                {
                    logger.LogWarning("Connection string empty — skipping non-destructive schema changes.");
                    return;
                }

                using var conn = new SqlConnection(connStr);
                conn.Open();

                var model = dbContext.Model;
                foreach (var entityType in model.GetEntityTypes())
                {
                    var tableName = entityType.GetTableName();
                    if (string.IsNullOrEmpty(tableName)) continue;

                    using var cmdCols = conn.CreateCommand();
                    cmdCols.CommandText = @"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table";
                    var p = cmdCols.CreateParameter();
                    p.ParameterName = "@table";
                    p.Value = tableName;
                    cmdCols.Parameters.Add(p);

                    var existing = new Dictionary<string, (string? dataType, int? maxLen, bool isNullable)>(StringComparer.OrdinalIgnoreCase);
                    using (var reader = cmdCols.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var col = reader.GetString(0);
                            var dt = reader.IsDBNull(1) ? null : reader.GetString(1);
                            int? max = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                            var isNull = reader.GetString(3) == "YES";
                            existing[col] = (dt, max, isNull);
                        }
                    }

                    foreach (var prop in entityType.GetProperties())
                    {
                        var columnName = prop.GetColumnName(StoreObjectIdentifier.Table(tableName, null));
                        if (string.IsNullOrEmpty(columnName)) continue;

                        if (!existing.ContainsKey(columnName))
                        {
                            var sqlType = MapClrTypeToSql(prop);
                            var nullable = prop.IsNullable ? "NULL" : "NULL";
                            var addSql = $"ALTER TABLE [{tableName}] ADD [{columnName}] {sqlType} {nullable};";
                            using var addCmd = conn.CreateCommand();
                            addCmd.CommandText = addSql;
                            addCmd.ExecuteNonQuery();
                            logger.LogInformation("Added column {Column} to table {Table}.", columnName, tableName);
                        }
                        else
                        {
                            var existingInfo = existing[columnName];
                            var maxLength = prop.GetMaxLength();
                            if (maxLength.HasValue && existingInfo.maxLen.HasValue)
                            {
                                var newLen = maxLength.Value;
                                if (existingInfo.maxLen.Value < newLen)
                                {
                                    var alter = $"ALTER TABLE [{tableName}] ALTER COLUMN [{columnName}] nvarchar({newLen}) NULL;";
                                    using var altCmd = conn.CreateCommand();
                                    altCmd.CommandText = alter;
                                    altCmd.ExecuteNonQuery();
                                    logger.LogInformation("Increased length of {Column} on {Table} to {Len}.", columnName, tableName, newLen);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while applying non-destructive schema changes.");
            }
        }

        private static string MapClrTypeToSql(IProperty prop)
        {
            var clrType = prop.ClrType;
            if (clrType == typeof(Guid)) return "uniqueidentifier";
            if (clrType == typeof(int)) return "int";
            if (clrType == typeof(bool)) return "bit";
            if (clrType == typeof(DateTime)) return "datetime2";
            if (clrType == typeof(decimal)) return "decimal(18,2)";
            if (clrType == typeof(double)) return "float";
            if (clrType == typeof(float)) return "real";
            if (clrType == typeof(long)) return "bigint";
            if (clrType == typeof(string))
            {
                var max = prop.GetMaxLength();
                if (max.HasValue)
                {
                    return $"nvarchar({max.Value})";
                }
                return "nvarchar(max)";
            }
            return "nvarchar(max)";
        }

        private static void DetectDestructiveSchemaChanges(ContextoGenerico dbContext, ILogger logger, string? resolvedConnectionString = null)
        {
            try
            {
                string connStr = resolvedConnectionString ?? dbContext.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrWhiteSpace(connStr))
                {
                    logger.LogWarning("Connection string empty — skipping destructive schema detection.");
                    return;
                }

                using var conn = new SqlConnection(connStr);
                conn.Open();

                var model = dbContext.Model;
                var sb = new StringBuilder();
                foreach (var entityType in model.GetEntityTypes())
                {
                    var tableName = entityType.GetTableName();
                    if (string.IsNullOrEmpty(tableName)) continue;

                    using var cmdCols = conn.CreateCommand();
                    cmdCols.CommandText = @"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table";
                    var p = cmdCols.CreateParameter();
                    p.ParameterName = "@table";
                    p.Value = tableName;
                    cmdCols.Parameters.Add(p);

                    var existing = new Dictionary<string, (string? dataType, int? maxLen, bool isNullable)>(StringComparer.OrdinalIgnoreCase);
                    using (var reader = cmdCols.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var col = reader.GetString(0);
                            var dt = reader.IsDBNull(1) ? null : reader.GetString(1);
                            int? max = reader.IsDBNull(2) ? null : reader.GetInt32(2);
                            var isNull = reader.GetString(3) == "YES";
                            existing[col] = (dt, max, isNull);
                        }
                    }

                    foreach (var prop in entityType.GetProperties())
                    {
                        var columnName = prop.GetColumnName(StoreObjectIdentifier.Table(tableName, null));
                        if (string.IsNullOrEmpty(columnName)) continue;

                        if (existing.TryGetValue(columnName, out var info))
                        {
                            var mapped = MapClrTypeToSql(prop);
                            if (!string.Equals(info.dataType, mapped, StringComparison.OrdinalIgnoreCase) && !mapped.StartsWith(info.dataType ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                            {
                                sb.AppendLine($"-- Type change for {tableName}.{columnName}: DB type {info.dataType} -> model {mapped}");
                                sb.AppendLine($"-- ALTER TABLE [{tableName}] ALTER COLUMN [{columnName}] {mapped} -- REVIEW AND APPLY MANUALLY");
                            }

                            var modelNotNull = !prop.IsNullable;
                            if (modelNotNull && info.isNullable)
                            {
                                sb.AppendLine($"-- Nullability change for {tableName}.{columnName}: DB allows NULL -> model requires NOT NULL");
                                sb.AppendLine($"-- Update existing NULLs then: ALTER TABLE [{tableName}] ALTER COLUMN [{columnName}] {mapped} NOT NULL -- REVIEW AND APPLY MANUALLY");
                            }
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    var dir = Path.Combine(AppContext.BaseDirectory, "migrations");
                    Directory.CreateDirectory(dir);
                    var file = Path.Combine(dir, $"destructive_changes_{DateTime.Now:yyyyMMddHHmmss}.sql");
                    File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
                    logger.LogWarning("Destructive schema changes detected. Script written to {File}. Review before applying.", file);
                }
                else
                {
                    logger.LogInformation("No destructive schema changes detected.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while detecting destructive schema changes.");
            }
        }

        private static void SeedDatabase(ContextoGenerico dbContext, ILogger logger, string? resolvedConnectionString = null)
        {
            try
            {
                var effectiveConnStr = dbContext.Database.GetDbConnection().ConnectionString;
                if (string.IsNullOrWhiteSpace(effectiveConnStr) && !string.IsNullOrWhiteSpace(resolvedConnectionString))
                {
                    using var tempContext = new ContextoGenerico(new DbContextOptionsBuilder<ContextoGenerico>()
                        .UseSqlServer(resolvedConnectionString).Options);
                    SeedDatabaseCore(tempContext, logger);
                    return;
                }

                if (!dbContext.Database.CanConnect())
                {
                    logger.LogWarning("Database cannot connect during seed step. Skipping seed.");
                    return;
                }

                SeedDatabaseCore(dbContext, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during database seed.");
                throw;
            }
        }

        private static void SeedDatabaseCore(ContextoGenerico dbContext, ILogger logger)
        {
            if (!dbContext.Empresa.Any())
            {
                var marvel = new Empresa
                {
                    Referencia = "Marvel",
                    Descricao = "Marvel Comics",
                    Inativo = false
                };

                var dc = new Empresa
                {
                    Referencia = "DC",
                    Descricao = "DC Comics",
                    Inativo = false
                };

                dbContext.Empresa.AddRange(marvel, dc);
                dbContext.SaveChanges();
                logger.LogInformation("Seeded Empresa records.");
            }

            if (!dbContext.Usuario.Any())
            {
                var marvelEmpresa = dbContext.Empresa.FirstOrDefault(e => e.Referencia == "Marvel");
                var dcEmpresa = dbContext.Empresa.FirstOrDefault(e => e.Referencia == "DC");

                if (marvelEmpresa == null || dcEmpresa == null)
                {
                    logger.LogWarning("Seed skipped because Empresa seed did not create required records.");
                    return;
                }

                var usuarios = new List<Usuario>
                {
                    new Usuario { Id_Empresa = marvelEmpresa.Id, Nome = "Homem-Aranha", Email = "spiderman@marvel.com" },
                    new Usuario { Id_Empresa = marvelEmpresa.Id, Nome = "Homem de Ferro", Email = "ironman@marvel.com" },
                    new Usuario { Id_Empresa = marvelEmpresa.Id, Nome = "Capitão América", Email = "capamerica@marvel.com" },
                    new Usuario { Id_Empresa = marvelEmpresa.Id, Nome = "Thor", Email = "thor@marvel.com" },
                    new Usuario { Id_Empresa = dcEmpresa.Id, Nome = "Batman", Email = "batman@dc.com" },
                    new Usuario { Id_Empresa = dcEmpresa.Id, Nome = "Superman", Email = "superman@dc.com" },
                    new Usuario { Id_Empresa = dcEmpresa.Id, Nome = "Mulher-Maravilha", Email = "wonderwoman@dc.com" },
                    new Usuario { Id_Empresa = dcEmpresa.Id, Nome = "Flash", Email = "flash@dc.com" }
                };

                dbContext.Usuario.AddRange(usuarios);
                dbContext.SaveChanges();
                logger.LogInformation("Seeded Usuario records.");
            }
        }
    }
}
