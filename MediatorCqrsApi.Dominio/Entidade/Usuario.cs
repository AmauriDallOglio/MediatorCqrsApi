using MediatorCqrsApi.Dominio.Util;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediatorCqrsApi.Dominio.Entidade
{
    [Table("Usuario")]
    public class Usuario : AtributoIdObrigatorio<Guid>, IEmpresaObrigatorio
    {
        public Guid Id_Empresa { get; set; }
        public int Id { get; set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public Usuario() { }
 

        public Usuario DadosDoIncluir()
        {
 
            return this;
        }

        public List<Notificacao> Validar()
        {
            List<Notificacao> retorno = new List<Notificacao>();
            retorno.AddRange(RegrasValidacao.ValidaString(nameof(Nome), Nome, 5, 50));
            retorno.AddRange(RegrasValidacao.ValidarEmail(nameof(Email), Email, 5, 40));
            return retorno;
        }


        //public ValidationResult ValidationResult { get; private set; }
        //public Usuario(string name, string email)
        //{
        //    Name = name;
        //    Email = email;

        //    ValidationResult = new ValidationResult();
        //}

        //public Usuario(int id, string name, string email)
        //{
        //    Id = id;
        //    Name = name;
        //    Email = email;

        //    ValidationResult = new ValidationResult();
        //}

        //public virtual bool IsValid()
        //{
        //    ValidateName();
        //    ValidateEmail();

        //    ValidationResult = Validate(this);

        //    return ValidationResult.IsValid;
        //}

        //public void ValidateName()
        //{
        //    RuleFor(c => c.Name)
        //        .NotEmpty().WithMessage("Nome não pode estar em branco")
        //        .MaximumLength(30).WithMessage("Tamanho máximo é de 10");
        //}

        //public void ValidateEmail()
        //{
        //    RuleFor(c => c.Email)
        //        .NotEmpty().WithMessage("E-mail não pode estar em branco")
        //        .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage("E-mail invalido!")
        //        .MaximumLength(20).WithMessage("E-mail tamanho maximo de 10.");
        //}
    }
}
