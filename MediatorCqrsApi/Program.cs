using FluentValidation;
using MediatorCqrsApi.Aplicacao.DML.Empresas;
using MediatorCqrsApi.Aplicacao.Profiles;
using MediatorCqrsApi.Configuracao;
using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Infra.Contexto;
using MediatorCqrsApi.Infra.Repositorio;

using Microsoft.Extensions.DependencyInjection;

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
            //builder.Services.AddScoped<IValidator<EmpresaInserirRequest>, EmpresaInserirValidator>();

        
            //builder.Services.AddMediator();
            builder.Services.AddSingleton<EmMemoriaContexto>();
            builder.Services.AddSingleton<IEmMemoriaRepositorio, EmMemoriaRepositorio>();
 

            //builder.Services.AddScoped<INotificationContexto, NotificacaoContexto>();

            //builder.Services.AddTransient<NotificacaoContexto>();
            //builder.Services.AddMvc(options => options.Filters.Add<NotificacaoFiltro>());

 
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


 


            app.Run();
        }
    }
}
