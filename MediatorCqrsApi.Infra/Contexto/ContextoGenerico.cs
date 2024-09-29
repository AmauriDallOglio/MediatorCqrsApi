using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Infra.Mapeamento.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace MediatorCqrsApi.Infra.Contexto
{
    public class ContextoGenerico : DbContext
    {
 
        public ContextoGenerico(DbContextOptions<ContextoGenerico> options) : base(options)
        {
       
        }

        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }

 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfiguracaoMapeamento.Injetar(modelBuilder);
     

            base.OnModelCreating(modelBuilder);
        }

    }
}
