using MediatorCqrsApi.Aplicacao.Util;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Notificacoes
{
    public class ObterTodasNotificacaoHandler : IRequestHandler<ObterTodasNotificacaoRequest, List<ObterTodasNotificacaoResponse>>
    {
        private readonly IEmMemoriaRepositorio _repositorio;

        public ObterTodasNotificacaoHandler(IEmMemoriaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<ObterTodasNotificacaoResponse>> Handle(ObterTodasNotificacaoRequest request, CancellationToken cancellationToken)
        {
            List<Notificacao> notificacoes = await _repositorio.ObterTodos();

            List<ObterTodasNotificacaoResponse> retorno = null;


            return retorno;
        }
    }
}