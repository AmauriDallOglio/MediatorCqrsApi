namespace MediatorCqrsApi.Dominio.Entidade
{
    public class Notificacao
    {
        public string Campo { get; private set; }
        public string Mensagem { get; private set; }
        public TipoNotificacao Tipo { get; set; }

        public Notificacao() { }

        public Notificacao Incluir(string campo, string mensagem, TipoNotificacao tipo)
        {
            Campo = campo;
            Mensagem = mensagem;
            Tipo = tipo;
            return this;
        }
    }

    public enum TipoNotificacao
    {
        Erro = 0,
        Sucesso = 1
    }
}
