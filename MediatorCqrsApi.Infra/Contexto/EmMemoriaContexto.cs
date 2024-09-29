using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Infra.Contexto
{
    public class EmMemoriaContexto
    {
        public ISet<Notificacao> NotificacaoCustomerizada { get; } = new HashSet<Notificacao>();

    }
}
