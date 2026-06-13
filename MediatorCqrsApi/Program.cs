using FluentValidation.AspNetCore;
using MediatorCqrsApi.Aplicacao.Profiles;
using MediatorCqrsApi.Configuracao;
using MediatorCqrsApi.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MediatorCqrsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

            builder.Services.AddControllers();
            builder.Services.DbContext(configuration);

            builder.Services.DependenciasDoEntity();
          
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining(typeof(MapperProfile)));
            builder.Services.AddAutoMapper(typeof(MapperProfile));


            // Configurar os servi�os de localizacao
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


            // Registrar os validadores a partir do assembly
            builder.Services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    //fv.RegisterValidatorsFromAssemblyContaining<EmpresaInserirValidator>();
                    fv.RegisterValidatorsFromAssembly(Assembly.Load("MediatorCqrsApi.Aplicacao"));
                });


            builder.Services.AddEndpointsApiExplorer();  //importacao do Swagge es e a��es definidos na API.
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(); //permitir um dom�nio acessem recursos em outro dom�nio
 

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Aplica as migrações pendentes automaticamente ao iniciar o sistema.
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Infra.Contexto.ContextoGenerico>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Falha ao aplicar migrações. Verifique se o SQL Server está disponível e se a ConnectionString está correta.");
                    throw;
                }
            }

            // Configurar a localizacao de idiomas
            var idiomas = new[] { "pt-BR", "en-US" };
            var localizacaoIdioma = new RequestLocalizationOptions()
                .SetDefaultCulture(idiomas[0])
                .AddSupportedCultures(idiomas)
                .AddSupportedUICultures(idiomas);

            app.UseRequestLocalization(localizacaoIdioma);

            app.Run();
        }
    }
}
