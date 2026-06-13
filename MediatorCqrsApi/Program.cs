using FluentValidation.AspNetCore;
using MediatorCqrsApi.Aplicacao.Profiles;
using MediatorCqrsApi.Configuracao;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Infra.Contexto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Text;

namespace MediatorCqrsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddControllers();

            string connectionStringsGravacao = Migracao.ResolveConnectionString(configuration);

            builder.Services.AddDbContext<ContextoGenerico>(options =>
            {
                if (!string.IsNullOrWhiteSpace(connectionStringsGravacao))
                {
                    options.UseSqlServer(connectionStringsGravacao);
                }
                else
                {
                    options.UseSqlServer(string.Empty);
                }
            });

            builder.Services.DependenciasDoEntity();

            bool reinstallDatabase = configuration.GetValue<bool>("Database:ReinstallOnStartup");
            bool autoMigrate = configuration.GetValue<bool>("Database:AutoMigrate", true);
          
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

            Migracao.ApplyStartupMigrations(app.Services, configuration, reinstallDatabase, autoMigrate);

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
