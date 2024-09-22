using MediatorCqrsApi.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace MediatorCqrsApi.Configuracao
{
    internal static class ConnectionString
    {
        public static void DbContext(this IServiceCollection services, IConfigurationRoot configuration)
        {
            string connectionStringsGravacao = configuration["ConnectionStrings:Gravacao"] ?? "";
            if (string.IsNullOrEmpty(connectionStringsGravacao))
            {
                string filePath = configuration["FileSettings:FilePath"] ?? "";
                connectionStringsGravacao = ReadConnectionStringFromFile(filePath);
            }
            services.AddDbContext<ContextoGenerico>(options => options.UseSqlServer(connectionStringsGravacao));

        }

        private static string ReadConnectionStringFromFile(string filePath)
        {
            try
            {
                string connectionString = File.ReadAllText(filePath).Replace("\\\\", "\\");
                return connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler a string de conexão do arquivo.", ex);
            }
        }
    }
}
