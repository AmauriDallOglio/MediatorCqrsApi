using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Dominio.Interface
{
    public interface IEmpresaRepositorio : IRepositorioGenerico<Empresa>
    {
        List<Empresa> BuscarTodosPorDescricao(string descricao);
    }
}
