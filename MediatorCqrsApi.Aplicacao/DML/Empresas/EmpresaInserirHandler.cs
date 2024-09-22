using AutoMapper;
using MediatorCqrsApi.Aplicacao.DML.Notification;
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
        private readonly NotificationContexto _notificationContext;
        private readonly IEmMemoriaRepositorio _IEmMemoriaRepositorio;
        public EmpresaInserirHandler(IMapper mapper, IEmpresaRepositorio iEmpresaRepositorio, NotificationContexto notificationContext, IEmMemoriaRepositorio emMemoriaRepositorio )
        {
            //_publish = publish;
            _mapper = mapper;
            //_mediator = mediator;
            _IEmpresaRepositorio = iEmpresaRepositorio;
            _notificationContext = notificationContext;
            _IEmMemoriaRepositorio = emMemoriaRepositorio;
        }

        public async Task<EmpresaInserirResponse> Handle(EmpresaInserirRequest request, CancellationToken cancellationToken)
        {

            Empresa entidade = _mapper.Map<Empresa>(request);

            _IEmpresaRepositorio.Inserir(entidade, true);

            EmpresaInserirResponse dto = _mapper.Map<EmpresaInserirResponse>(entidade);
            //EmpresaInserirResponse dto = new EmpresaInserirResponse();
            Console.WriteLine($"Empresa cadastrado com sucesso! Código {dto.Id}");

            Aplicacao.DML.Notification.Notification customer = new Aplicacao.DML.Notification.Notification("2", $"Empresa cadastrado com sucesso! Código {dto.Id}");
            _notificationContext.AddNotification(customer);

            Dominio.Entidade.Notificacao notificacao = new Dominio.Entidade.Notificacao();

             

            var aaaa = _IEmMemoriaRepositorio.Save(notificacao);

            await _IEmMemoriaRepositorio.Save(notificacao);

            return await Task.FromResult(dto);

        }


    }
}