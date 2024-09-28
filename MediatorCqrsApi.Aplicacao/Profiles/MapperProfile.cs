using AutoMapper;
using MediatorCqrsApi.Aplicacao.DML.Empresas;
using MediatorCqrsApi.Aplicacao.DML.Notificacoes;
using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Aplicacao.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Empresa, EmpresaInserirRequest>().ReverseMap();
            CreateMap<Empresa, EmpresaInserirResponse>().ReverseMap();


            //CreateMap<Notificacao, ObterTodasNotificacaoRequest>().ReverseMap();
            //CreateMap<Notificacao, ObterTodasNotificacaoResponse>().ReverseMap();
        }
    }



}
