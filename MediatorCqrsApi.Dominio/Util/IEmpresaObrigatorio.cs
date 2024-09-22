using System.ComponentModel.DataAnnotations;

namespace MediatorCqrsApi.Dominio.Util
{
    public interface IEmpresaObrigatorio
    {
        [Required(ErrorMessage = "O Id do Empresa é obrigatório.")]
        public Guid Id_Empresa { get; set; }
    }
}
