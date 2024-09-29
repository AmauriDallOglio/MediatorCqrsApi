using AutoMapper;
using MediatorCqrsApi.Aplicacao.Util;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Usuarios
{
    public class UsuarioInserirHandler : IRequestHandler<UsuarioInserirRequest, ResultadoOperacao<UsuarioInserirResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEmMemoriaRepositorio _IEmMemoriaRepositorio;
        public UsuarioInserirHandler(IMapper mapper, IEmMemoriaRepositorio emMemoriaRepositorio)
        {
            _mapper = mapper;
            _IEmMemoriaRepositorio = emMemoriaRepositorio;
        }

        public async Task<ResultadoOperacao<UsuarioInserirResponse>> Handle(UsuarioInserirRequest request, CancellationToken cancellationToken)
        {

            Usuario usuario = _mapper.Map<Usuario>(request);
            usuario.DadosDoIncluir();
            if (usuario.Erros.Count > 0)
            {
                foreach (Notificacao erro in usuario.Erros)
                {
                    await _IEmMemoriaRepositorio.Adicionar(erro);
                }
                return (ResultadoOperacao<UsuarioInserirResponse>.AdicionarFalha(usuario.Erros));

            }


            UsuarioInserirResponse response = _mapper.Map<UsuarioInserirResponse>(usuario);
            var notificacao = new Notificacao().Incluir("EmpresaInserirHandler", $"Empresa cadastrada com sucesso! Código {response.Id}", TipoNotificacao.Sucesso);
            await _IEmMemoriaRepositorio.Adicionar(notificacao);


            return (ResultadoOperacao<UsuarioInserirResponse>.AdionarSucesso($"Empresa cadastrada com sucesso!"));


        }
    }
}
