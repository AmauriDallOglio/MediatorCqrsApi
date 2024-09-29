using MediatorCqrsApi.Dominio.Util;
using System.ComponentModel.DataAnnotations.Schema;


namespace MediatorCqrsApi.Dominio.Entidade
{
    [Table("Empresa")]
    public class Empresa : AtributoIdObrigatorio<Guid>
    {
        public string Referencia { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Inativo { get; set; }

        public Empresa() { }

        public Empresa DadosDoIncluir()
        {
            Inativo = false;
            return this;
        }

        public List<Notificacao> Validar()
        {
            List<Notificacao> retorno = new List<Notificacao>();
            retorno.AddRange(RegrasValidacao.ValidaString(nameof(Referencia), Referencia, 5, 50));
            retorno.AddRange(RegrasValidacao.ValidaString(nameof(Descricao), Referencia, 5, 300));
            return retorno;
        }

    }
}
