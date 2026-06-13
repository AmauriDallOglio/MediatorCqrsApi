using Microsoft.EntityFrameworkCore;
using MediatorCqrsApi.Infra.Mapeamento;

namespace MediatorCqrsApi.Infra.Mapeamento.Configuracao
{
    public class ConfiguracaoMapeamento
    {
        public static void Injetar(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EmpresaMapeamento());
            builder.ApplyConfiguration(new UsuarioMapeamento());

        }
    }
}
