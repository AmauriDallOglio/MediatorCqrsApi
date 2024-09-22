using MediatorCqrsApi.Dominio.Entidade;
using Microsoft.EntityFrameworkCore;

namespace MediatorCqrsApi.Infra.Contexto
{
    public class ContextoGenerico : DbContext
    {
 
        public ContextoGenerico(DbContextOptions<ContextoGenerico> options) : base(options)
        {
       
        }

        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
    }
}
