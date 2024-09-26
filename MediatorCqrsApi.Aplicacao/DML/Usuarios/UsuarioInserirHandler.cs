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

            Usuario entidade = _mapper.Map<Usuario>(request);
   
            if (!entidade.IsValid())
            {
                foreach (var error in entidade.ValidationResult.Errors)
                {
                    Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
                }
            }


            UsuarioInserirResponse dto = _mapper.Map<UsuarioInserirResponse>(entidade);

            // Retorna o DTO da empresa inserida
            return await Task.FromResult(dto);

        }
    }
}
