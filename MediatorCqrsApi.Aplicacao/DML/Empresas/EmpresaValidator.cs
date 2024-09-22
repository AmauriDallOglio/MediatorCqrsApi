using FluentValidation;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaValidator : AbstractValidator<EmpresaInserirRequest>
    {
        public EmpresaValidator()
        {
            RuleFor(a => a.Referencia)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Referência deve ser informado");

            RuleFor(a => a.Descricao)
                .NotEmpty()
                .WithMessage("Descrição deve ser informado");
        }
    }
}
