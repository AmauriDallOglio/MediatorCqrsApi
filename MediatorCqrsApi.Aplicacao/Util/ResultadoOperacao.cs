using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorCqrsApi.Aplicacao.Util
{
 
    public class ResultadoOperacao<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public List<string> Erros { get; set; } = new List<string>();

        public static ResultadoOperacao<T> AdionarSucesso(string mensagem)
        {
            return new ResultadoOperacao<T> { Sucesso = true, Mensagem = mensagem, Erros = new List<string>() };
        }

        public static ResultadoOperacao<T> AdicionarFalha(List<string> erros)
        {
            return new ResultadoOperacao<T> { Sucesso = false, Mensagem = "Falha na operação", Erros = erros };
        }
    }
}
