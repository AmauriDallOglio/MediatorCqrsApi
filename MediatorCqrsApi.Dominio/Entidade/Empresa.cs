using MediatorCqrsApi.Dominio.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediatorCqrsApi.Dominio.Entidade
{
    [Table("Empresa")]
    public class Empresa : AtributoIdObrigatorio<Guid>
    {

        [Required(ErrorMessage = "A referência é obrigatória.")]
        [MaxLength(50, ErrorMessage = "A referência deve ter no máximo 50 caracteres.")]
        public string Referencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MaxLength(300, ErrorMessage = "A descrição deve ter no máximo 300 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Inativo é obrigatório.")]
        public bool Inativo { get; set; } 

    }
}
