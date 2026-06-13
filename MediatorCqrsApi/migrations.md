**Migrations & Startup DB Behavior**

Resumo das rotinas de inicialização, migração automática, alterações de esquema e instruções de execução.

**Comportamento Atual**:
- **Criação do DB:** Ao iniciar, a aplicação resolve `ConnectionStrings:Gravacao` ou, se vazio, lê o arquivo em `FileSettings:FilePath`. Em seguida, verifica se o banco existe no servidor e o cria se estiver ausente.
- **Migrações automáticas:** Quando `Database:AutoMigrate` = `true` (padrão), o startup executa `Database.Migrate()` para aplicar migrations pendentes.
- **Reinstalação (destrutiva):** Se `Database:ReinstallOnStartup` = `true`, a aplicação executa `EnsureDeleted()` seguido de `Migrate()` (apaga e recria o banco) — usar apenas em desenvolvimento.
- **Seed inicial:** Se as tabelas `Empresa` ou `Usuario` estiverem vazias, o startup popula registros demo automaticamente.
- **Alterações de esquema:** O startup aplica alterações seguras automaticamente e detecta alterações destrutivas para gerar scripts de revisão.

**Flags de configuração** (em `appsettings.json` / `appsettings.Development.json`):
- `Database:AutoMigrate` (bool) — aplica migrations automaticamente. Default: `true`.
- `Database:ReinstallOnStartup` (bool) — apaga e recria o banco no startup. Default: `false`.

**Connection String**:
- A aplicação lê `ConnectionStrings:Gravacao` primeiro.
- Se não estiver definida, ela tenta ler o arquivo apontado por `FileSettings:FilePath`.
- Ajuste a connection string em `appsettings.json` ou crie o arquivo indicado contendo a string de conexão.

**O que foi implementado**:
1. Adicionar inicialização de banco no `Program.cs`.
2. Validar existência do database e criar se ausente.
3. Aplicar migrações EF Core automaticamente com `Database.Migrate()`.
4. Fazer seed inicial quando tabelas estiverem vazias.
5. Detectar alterações de esquema não destrutivas e aplicar `ALTER` automaticamente.
6. Lidar com alterações destrutivas e remoção de coluna, gerando script para revisão em `bin/Debug/net8.0/migrations/destructive_changes_YYYYMMDDHHMMSS.sql`.
7. Adicionar flags de configuração `Database:AutoMigrate` e `Database:ReinstallOnStartup`.
8. Registrar logs detalhados e mensagens de erro para operações de banco de dados.
9. Documentar comportamento no README e em instruções de execução.
10. Executar testes locais: rodar a aplicação e verificar criação, migração e seed.

**Alterações de esquema**:
- **Não destrutivas (automáticas):** o startup aplica alterações seguras como "adicionar coluna" e aumentar comprimento de colunas de texto detectadas no modelo.
- **Destrutivas (não aplicadas automaticamente):** mudanças de tipo de coluna ou ajustando para `NOT NULL` são consideradas destrutivas. A aplicação gera um script SQL para revisão manual.

**Fluxo recomendado ao detectar mudanças destrutivas**:
1. Rever o SQL gerado em `bin/Debug/net8.0/migrations/*.sql`.
2. Validar backup e impacto em ambiente de teste.
3. Aplicar manualmente em produção fora da janela de pico.

**Comandos `dotnet ef` úteis**:
- Gerar migration localmente:

  dotnet ef migrations add NomeDaMigration --project MediatorCqrsApi.Infra --startup-project MediatorCqrsApi

- Gerar script SQL a partir das migrations:

  dotnet ef migrations script --project MediatorCqrsApi.Infra --startup-project MediatorCqrsApi -o migration_script.sql

- Aplicar migrations via CLI:

  dotnet ef database update --project MediatorCqrsApi.Infra --startup-project MediatorCqrsApi

**Seed (dados iniciais)**:
- Seed de empresas e usuários demo é executado automaticamente quando as tabelas estão vazias.
- Se o seed falhar devido a problemas de connection string, corrija a configuração e reinicie a aplicação.

**Segurança e boas práticas**:
- Sempre faça backup antes de aplicar alterações destrutivas.
- Prefira criar migrations com `dotnet ef migrations add` em vez de depender apenas de alterações em runtime para produção.
- Use `Database:ReinstallOnStartup` somente em desenvolvimento ou em pipelines controlados.

**Troubleshooting**:
- "The ConnectionString property has not been initialized": verifique `appsettings.json` e `FileSettings:FilePath`.
- Falha de login (`Falha de logon do usuário 'sa'`): confirme credenciais, firewall e listener do SQL Server.

**Onde estão os pontos relevantes no código**:
- Inicialização e migrações: `MediatorCqrsApi/Program.cs`
- Rotina de migração e seed extraída para: `MediatorCqrsApi/Configuracao/Migracao.cs`
- Registro do DbContext: `MediatorCqrsApi/Configuracao/ConnectionString.cs`
