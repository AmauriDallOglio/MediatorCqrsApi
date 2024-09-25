using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Infra.Contexto;
using MediatorCqrsApi.Infra.Repositorio;

namespace MediatorCqrsApi.Configuracao
{
    public static class ConfiguracaoInterfaceRepositorio
    {
        public static void DependenciasDoEntity(this IServiceCollection services)
        {
            services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
            services.AddSingleton<EmMemoriaContexto>();
            services.AddSingleton<IEmMemoriaRepositorio, EmMemoriaRepositorio>();
            return;
        }

    }
}
