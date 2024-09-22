using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Infra.Contexto;

namespace MediatorCqrsApi.Infra.Repositorio
{
    public class EmpresaRepositorio : RepositorioGenerico<Empresa>, IEmpresaRepositorio
    {
        private readonly ContextoGenerico _contexto;
        public EmpresaRepositorio(ContextoGenerico dbContext) : base(dbContext)
        {
            _contexto = dbContext;
        }

        public List<Empresa> BuscarTodosPorDescricao(string descricao)
        {
            var resultado = new List<Empresa>();
            if (string.IsNullOrEmpty(descricao))
            {
                resultado = _contexto.Empresa.ToList();
            }
            else
            {
                resultado = _contexto.Empresa.Where(b => b.Descricao.Contains(descricao)).ToList();
            }
            return resultado;
        }
    }
}
