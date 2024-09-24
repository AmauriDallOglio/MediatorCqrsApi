namespace MediatorCqrsApi.Dominio.Entidade
{
    public class Notificacao
    {
        public string Id { get; }
        public string Mensagem { get; }

        public Notificacao(string id, string mensagem)
        {
            Id = id;
            Mensagem = mensagem;
        }
    }
}
