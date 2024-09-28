using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Notificacoes
{
    public class ObterTodasNotificacaoRequest : IRequest<List<ObterTodasNotificacaoResponse>>
    {
        public string Erro { get; set; } = string.Empty;
    }
}
