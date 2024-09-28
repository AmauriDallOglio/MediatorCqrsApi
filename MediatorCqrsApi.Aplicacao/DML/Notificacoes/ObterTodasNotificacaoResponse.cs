using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Aplicacao.DML.Notificacoes
{
    public class ObterTodasNotificacaoResponse
    {
        public string Id { get; } = string.Empty;
        public string Mensagem { get; } = string.Empty;
    }
}
