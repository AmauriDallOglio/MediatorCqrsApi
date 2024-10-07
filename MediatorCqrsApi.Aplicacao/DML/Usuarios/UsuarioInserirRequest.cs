using MediatorCqrsApi.Aplicacao.Util;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Usuarios
{
    public class UsuarioInserirRequest : IRequest<ResultadoOperacao<UsuarioInserirResponse>>
    {
        public string Nome { get;  set; } = string.Empty;
        public string Email { get;  set; } = string.Empty ;
    }
}
