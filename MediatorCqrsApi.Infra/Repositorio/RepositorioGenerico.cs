using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Infra.Contexto;

namespace MediatorCqrsApi.Infra.Repositorio
{
    public class RepositorioGenerico<TEntity> : IRepositorioGenerico<TEntity> where TEntity : class
    {
        private readonly ContextoGenerico _dbContext;
        public RepositorioGenerico(ContextoGenerico dbContext)
        {
            _dbContext = dbContext;
        }

     
 
        public TEntity Inserir(TEntity entidade, bool finalizar)
        {
            _dbContext.Set<TEntity>().Add(entidade);
            if (finalizar)
            {
                Comitar();
            }

            return entidade;
        }

        public void Comitar()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao tentar salvar as alterações no banco de dados.", ex);
            }
        }

   

    }
}
