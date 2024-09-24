using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Dominio.Interface
{
    public interface IEmMemoriaRepositorio
    {
        Task Adicionar(Notificacao notificacao);
        Task<Notificacao> ObterPorId(Guid id);
        Task<List<Notificacao>> ObterTodos();
    }
}
