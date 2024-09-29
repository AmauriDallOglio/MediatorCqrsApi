using MediatorCqrsApi.Aplicacao.Util;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Notificacoes
{
    public class ObterTodasNotificacaoHandler : IRequestHandler<ObterTodasNotificacaoRequest, ResultadoOperacao<ObterTodasNotificacaoResponse>>
    {
        private readonly IEmMemoriaRepositorio _repositorio;

        public ObterTodasNotificacaoHandler(IEmMemoriaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<ResultadoOperacao<ObterTodasNotificacaoResponse>> Handle(ObterTodasNotificacaoRequest request, CancellationToken cancellationToken)
        {
            List<Notificacao> notificacoes = await _repositorio.ObterTodos();

            ResultadoOperacao<ObterTodasNotificacaoResponse> retorno = new ResultadoOperacao<ObterTodasNotificacaoResponse>();
            retorno.Erros.AddRange(notificacoes.Select(a => a.Mensagem));


            return retorno;
        }
    }
}