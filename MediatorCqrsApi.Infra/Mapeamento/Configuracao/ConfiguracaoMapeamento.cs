using Microsoft.EntityFrameworkCore;

namespace MediatorCqrsApi.Infra.Mapeamento.Configuracao
{
    public class ConfiguracaoMapeamento
    {
        public static void Injetar(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EmpresaMapeamento());

        }
    }
}
