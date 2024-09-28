using MediatorCqrsApi.Aplicacao.Util;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaInserirRequest : IRequest<ResultadoOperacao<EmpresaInserirResponse>>
    {
        public string Referencia { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
