using FluentValidation.AspNetCore;
using MediatorCqrsApi.Aplicacao.Profiles;
using MediatorCqrsApi.Configuracao;
using MediatorCqrsApi.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;
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


            // Configurar os servi�os de localiza��o
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


            // Registrar os validadores a partir do assembly
            builder.Services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    //fv.RegisterValidatorsFromAssemblyContaining<EmpresaInserirValidator>();
                    fv.RegisterValidatorsFromAssembly(Assembly.Load("MediatorCqrsApi.Aplicacao"));
                });


            builder.Services.AddEndpointsApiExplorer();  //import ��o do Swagge es e a��es definidos na API.
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(); //permitir um dom�nio acessem recursos em outro dom�nio
 

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseAuthentication();
            // Configurar a localiza��o
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
