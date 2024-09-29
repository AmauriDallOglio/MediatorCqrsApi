using FluentValidation;
using FluentValidation.Results;
using MediatorCqrsApi.Dominio.Util;

namespace MediatorCqrsApi.Dominio.Entidade
{
    public class Usuario : AbstractValidator<Usuario>
    {
     
        public int Id { get; set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public Usuario() { }

        public List<Notificacao> Erros = new List<Notificacao>();

        public Usuario DadosDoIncluir()
        {
            Erros.AddRange(RegrasValidacao.ValidaString(nameof(Nome), Nome, 5, 50));
            Erros.AddRange(RegrasValidacao.ValidarEmail(nameof(Email), Email, 5, 40));

            return this;
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
