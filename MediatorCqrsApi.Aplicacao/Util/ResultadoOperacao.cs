using MediatorCqrsApi.Dominio.Entidade;

namespace MediatorCqrsApi.Aplicacao.Util
{
    /// <summary>
    /// Manipulação result pattern, claricidade: o código fica mais fácil de entender, pois os resultados de sucesso e falha são explicitamente manipulados;
    /// Controle de fluxo: Melhora o controle de fluxo, evitadndo exceções desnecessárias;
    /// Desempenho: Retorna objetos de resultado é mais eficiente do que lançar exceções;
    /// Manutenção: Facilita a manutnção e testes do código, pois é mais simples verificar o resultadodas operações;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultadoOperacao<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public List<Notificacao> Notificacoes { get; set; } = new List<Notificacao>();

        public static ResultadoOperacao<T> AdionarSucesso(string mensagem)
        {
            return new ResultadoOperacao<T> { Sucesso = true, Mensagem = mensagem, Notificacoes = new List<Notificacao>() };
        }

        public static ResultadoOperacao<T> AdicionarFalha(List<Notificacao> erros)
        {
            return new ResultadoOperacao<T> { Sucesso = false, Mensagem = "Falha na operação", Notificacoes = erros };
        }
    }
}
