using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Dominio.Interface
{
    public interface IEmMemoriaRepositorio
    {
        Task Adicionar(Notificacao notificacao);
        Task<Notificacao> ObterPorId(string mensagem);
        Task<List<Notificacao>> ObterTodos();
    }
}
