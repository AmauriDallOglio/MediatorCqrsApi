using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaInserirRequest : IRequest<EmpresaInserirResponse>, INotification
    {
        public string Referencia { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
