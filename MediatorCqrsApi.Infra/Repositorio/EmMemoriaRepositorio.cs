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

        public Task Adicionar(Notificacao notificacao)
        {
            _databaseContext.NotificacaoCustomerizada.Add(notificacao);
            return Task.CompletedTask;
        }

        public Task<Notificacao> ObterPorId(string mensagem)
        {
            Notificacao notificacao = _databaseContext.NotificacaoCustomerizada.FirstOrDefault(a => a.Mensagem.Equals(mensagem));
            Console.WriteLine("Notificações: " + notificacao?.ToString()??"");
            return Task.FromResult(notificacao);
        }

        public Task<List<Notificacao>> ObterTodos()
        {
            List<Notificacao> notificacoes = _databaseContext.NotificacaoCustomerizada.ToList();
            Console.WriteLine("Total de notificações: " + notificacoes.Count);
            return Task.FromResult(notificacoes);
        }

    }
}
