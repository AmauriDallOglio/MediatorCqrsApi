using AutoMapper;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Usuarios
{
    public class UsuarioInserirHandler : IRequestHandler<UsuarioInserirRequest, UsuarioInserirResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEmMemoriaRepositorio _IEmMemoriaRepositorio;
        public UsuarioInserirHandler(IMapper mapper, IEmMemoriaRepositorio emMemoriaRepositorio)
        {
            _mapper = mapper;
            _IEmMemoriaRepositorio = emMemoriaRepositorio;
        }

        public async Task<UsuarioInserirResponse> Handle(UsuarioInserirRequest request, CancellationToken cancellationToken)
        {

            Usuario usuario = _mapper.Map<Usuario>(request);
    


            if (!usuario.IsValid())
            {
                foreach (var error in usuario.ValidationResult.Errors)
                {
              
                    Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
                }
            }


            UsuarioInserirResponse dto = _mapper.Map<UsuarioInserirResponse>(usuario);

            // Retorna o DTO da empresa inserida
            return await Task.FromResult(dto);

        }
    }
}
