using FluentValidation;
using FluentValidation.Results;

namespace MediatorCqrsApi.Dominio.Entidade
{
    public class Usuario : AbstractValidator<Usuario>
    {
     
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Usuario() { }

        public Usuario(string name, string email)
        {
            Name = name;
            Email = email;

            ValidationResult = new ValidationResult();
        }

        public Usuario(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;

            ValidationResult = new ValidationResult();
        }

 


        public ValidationResult ValidationResult { get; private set; }

        public virtual bool IsValid()
        {
            ValidateName();
            ValidateEmail();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        private void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Nome não pode estar em branco")
                .MaximumLength(30).WithMessage("Tamanho máximo é de 10");
        }

        private void ValidateEmail()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("E-mail não pode estar em branco")
                .Matches(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").WithMessage("E-mail invalido!")
                .MaximumLength(20).WithMessage("E-mail tamanho maximo de 10.");
        }
    }
}
