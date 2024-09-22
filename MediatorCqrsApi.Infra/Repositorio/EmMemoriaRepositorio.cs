using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Infra.Contexto;

namespace MediatorCqrsApi.Infra.Repositorio
{
    public class EmMemoriaRepositorio : IEmMemoriaRepositorio
    {
        private readonly EmMemoriaContexto _databaseContext;

        public EmMemoriaRepositorio(EmMemoriaContexto databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task Save(Notificacao customer)
        {
            _databaseContext.NotificacaoCustomerizada.Add(customer);
            return Task.CompletedTask;
        }

        public Task<Notificacao> Find(Guid id)
        {
            var customer = _databaseContext.NotificacaoCustomerizada.FirstOrDefault(a => a.Key.Equals(id));
            return Task.FromResult(customer);
        }
    }
}
