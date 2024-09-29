using MediatorCqrsApi.Aplicacao.Util;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Notificacoes
{
    public class ObterTodasNotificacaoRequest : IRequest<ResultadoOperacao<ObterTodasNotificacaoResponse>>
    {
        public string Erro { get; set; } = string.Empty;
    }
}
