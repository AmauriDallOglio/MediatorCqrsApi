using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatorCqrsApi.Aplicacao.DML.Empresas;
using MediatorCqrsApi.Aplicacao.DML.Notificacoes;
using MediatorCqrsApi.Aplicacao.Profiles;
using MediatorCqrsApi.Aplicacao.Util;
using MediatorCqrsApi.Configuracao;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Infra.Contexto;
using MediatorCqrsApi.Infra.Repositorio;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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


            // Configurar os serviços de localização
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


            // Registrar os validadores a partir do assembly
            builder.Services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    //fv.RegisterValidatorsFromAssemblyContaining<EmpresaInserirValidator>();
                    fv.RegisterValidatorsFromAssembly(Assembly.Load("MediatorCqrsApi.Aplicacao"));
                });



            // Adiciona o MassTransit
            builder.Services.AddMassTransit(x =>
            {
                x.AddMediator(cfg =>
                {
                    // Adiciona seu handler aqui
                    //cfg.ConfigureMediator(mediator =>
                    //{
                    //    mediator.UseConsumer<ObterTodasNotificacaoHandler>(); // Certifique-se de que o handler está configurado corretamente
                    //});
                });

                // Configuração do transporte (InMemory)
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });


            // Adicione seu repositório e handlers aqui
            builder.Services.AddScoped<IEmMemoriaRepositorio, EmMemoriaRepositorio>();
            builder.Services.AddScoped<IRequestHandler<ObterTodasNotificacaoRequest, List<ObterTodasNotificacaoResponse>>, ObterTodasNotificacaoHandler>();



            builder.Services.AddEndpointsApiExplorer();  //import ção do Swagge es e ações definidos na API.
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(); //permitir um domínio acessem recursos em outro domínio



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



            // Configurar a localização
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
