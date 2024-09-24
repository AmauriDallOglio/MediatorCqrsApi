using AutoMapper;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatR;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaInserirHandler : IRequestHandler<EmpresaInserirRequest, EmpresaInserirResponse>
    {
        private readonly IMapper _mapper;
        //private readonly IMediator _mediator;
        private readonly IEmpresaRepositorio _IEmpresaRepositorio;
        //private readonly IPublishEndpoint _publish;
        private readonly IEmMemoriaRepositorio _IEmMemoriaRepositorio;
        public EmpresaInserirHandler(IMapper mapper, IEmpresaRepositorio iEmpresaRepositorio, IEmMemoriaRepositorio emMemoriaRepositorio )
        {
            //_publish = publish;
            _mapper = mapper;
            //_mediator = mediator;
            _IEmpresaRepositorio = iEmpresaRepositorio;
            _IEmMemoriaRepositorio = emMemoriaRepositorio;
        }

        public async Task<EmpresaInserirResponse> Handle(EmpresaInserirRequest request, CancellationToken cancellationToken)
        {

            Empresa entidade = _mapper.Map<Empresa>(request);
            var resultado =  _IEmpresaRepositorio.Inserir(entidade, true);
            EmpresaInserirResponse dto = _mapper.Map<EmpresaInserirResponse>(entidade);

            Console.WriteLine($"Empresa cadastrada com sucesso! Código {dto.Id}");
            Notificacao notificacao = new Notificacao("2", $"Empresa cadastrada com sucesso! Código {dto.Id}");

            await _IEmMemoriaRepositorio.Adicionar(notificacao);
            var todasNotificacoes = await _IEmMemoriaRepositorio.ObterTodos();

            // Retorna o DTO da empresa inserida
            return await Task.FromResult(dto);

        }


    }
}