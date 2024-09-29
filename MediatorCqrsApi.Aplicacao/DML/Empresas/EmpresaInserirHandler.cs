using AutoMapper;
using MediatorCqrsApi.Aplicacao.Util;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaInserirHandler : IRequestHandler<EmpresaInserirRequest, ResultadoOperacao<EmpresaInserirResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEmpresaRepositorio _IEmpresaRepositorio;
        private readonly IEmMemoriaRepositorio _IEmMemoriaRepositorio;
        public EmpresaInserirHandler(IMapper mapper, IEmpresaRepositorio iEmpresaRepositorio, IEmMemoriaRepositorio emMemoriaRepositorio )
        {
            _mapper = mapper;
            _IEmpresaRepositorio = iEmpresaRepositorio;
            _IEmMemoriaRepositorio = emMemoriaRepositorio;
        }

        public async Task<ResultadoOperacao<EmpresaInserirResponse>> Handle(EmpresaInserirRequest request, CancellationToken cancellationToken)
        {
            Empresa empresa = _mapper.Map<Empresa>(request);
            empresa.DadosDoIncluir();
            List<Notificacao> Erros = empresa.Validar();
            if (Erros.Count > 0)
            {
                foreach (Notificacao erro in Erros)
                {
                    await _IEmMemoriaRepositorio.Adicionar(erro);
                }
                return (ResultadoOperacao<EmpresaInserirResponse>.AdicionarFalha(Erros));
            }

            empresa = _IEmpresaRepositorio.Inserir(empresa, true);

            EmpresaInserirResponse response = _mapper.Map<EmpresaInserirResponse>(empresa);
            var notificacao = new Notificacao().Incluir("EmpresaInserirHandler", $"Empresa cadastrada com sucesso! Código {response.Id}", TipoNotificacao.Sucesso);
            await _IEmMemoriaRepositorio.Adicionar(notificacao);

  
            return (ResultadoOperacao<EmpresaInserirResponse>.AdionarSucesso($"Empresa cadastrada com sucesso! Código {response.Id}"));
        }
    }
}