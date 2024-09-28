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
            empresa = _IEmpresaRepositorio.Inserir(empresa, true);

            List<string> resultadoValidacao = empresa.Validar();  
            if (resultadoValidacao.Count > 0)
            {
                return (ResultadoOperacao<EmpresaInserirResponse>.AdicionarFalha(resultadoValidacao));
            }

            Empresa empresa1 = new Empresa();
            List<string> resultadoValidacao2 = empresa1.Validar();
            if (resultadoValidacao2.Count > 0)
            {
                return (ResultadoOperacao<EmpresaInserirResponse>.AdicionarFalha(resultadoValidacao2));
            }


            EmpresaInserirResponse dto = _mapper.Map<EmpresaInserirResponse>(empresa);

     
            Notificacao notificacao = new Notificacao("2", $"Empresa cadastrada com sucesso! Código {dto.Id}");
            await _IEmMemoriaRepositorio.Adicionar(notificacao);

  
            return (ResultadoOperacao<EmpresaInserirResponse>.AdionarSucesso($"Empresa cadastrada com sucesso! Código {dto.Id}"));


        }


    }
}