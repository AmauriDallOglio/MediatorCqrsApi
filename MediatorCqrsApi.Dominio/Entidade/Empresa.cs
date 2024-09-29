using MediatorCqrsApi.Dominio.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace MediatorCqrsApi.Dominio.Entidade
{
    [Table("Empresa")]
    public class Empresa : AtributoIdObrigatorio<Guid>
    {
        public string Referencia { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Inativo { get; set; }

        public List<Notificacao> Erros { get; private set; } = new List<Notificacao>();
        public Empresa DadosDoIncluir()
        {
            Erros = RegrasValidacao.ValidaString(nameof(Referencia), Referencia, 5, 50);


            return this;
        }

        //public List<string> Validar()
        //{


        //    var retorno = RegrasValidacao.ValidaString(nameof(Referencia), Referencia, 5, 50);
        //    if (retorno.Any())
        //    {
        //        Erros.AddRange(retorno);
        //        Console.WriteLine(string.Join(", ", retorno));
        //    }

        //    retorno = RegrasValidacao.ValidarDescricao(nameof(Descricao), Descricao, 5, 300);
        //    if (retorno.Any())
        //    {
        //        Erros.AddRange(retorno);
        //        Console.WriteLine(string.Join(", ", retorno));
        //    }
        //    return Erros;
        //}

    }
}
