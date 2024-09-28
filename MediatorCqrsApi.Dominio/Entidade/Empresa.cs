using MediatorCqrsApi.Dominio.Util;
using System.ComponentModel.DataAnnotations.Schema;


namespace MediatorCqrsApi.Dominio.Entidade
{
    [Table("Empresa")]
    public class Empresa : AtributoIdObrigatorio<Guid>
    {

        //[Required(ErrorMessage = "A referência é obrigatória. (Entidade)")]
        //[MaxLength(50, ErrorMessage = "A referência deve ter no máximo 50 caracteres.")]
        public string Referencia { get; set; } = string.Empty;

        //[Required(ErrorMessage = "A descrição é obrigatória. (Entidade)")]
        //[MaxLength(300, ErrorMessage = "A descrição deve ter no máximo 300 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        //[Required(ErrorMessage = "O campo Inativo é obrigatório. (Entidade)")]
        public bool Inativo { get; set; }


        public List<string> Validar()
        {
            List<string> erros = new List<string>();

            var retorno = RegrasValidacao.ValidaString(nameof(Referencia), Referencia, 5, 50);
            if (retorno.Any())
            {
                erros.AddRange(retorno);
                Console.WriteLine(string.Join(", ", retorno));
            }

            retorno = RegrasValidacao.ValidarDescricao(nameof(Descricao), Descricao, 5, 300);
            if (retorno.Any())
            {
                erros.AddRange(retorno);
                Console.WriteLine(string.Join(", ", retorno));
            }

            return erros;
       
 
        }

    }
}
