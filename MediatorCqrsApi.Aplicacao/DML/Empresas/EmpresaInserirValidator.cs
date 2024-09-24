using FluentValidation;
using Microsoft.Extensions.Localization;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaInserirValidator : AbstractValidator<EmpresaInserirRequest>
    {
        public EmpresaInserirValidator(IStringLocalizer<EmpresaInserirValidator> localizer)
        {

            RuleFor(request => request.Referencia)
                .NotEmpty().WithMessage(x => localizer["Referencia é obrigatória!"]);
            RuleFor(request => request.Descricao)
                .NotEmpty().WithMessage(x => localizer["Descrição é obrigatória!"]);

            //RuleFor(a => a.Referencia)
            //   .NotEmpty().WithMessage("Referencia é obrigatório.")
            //   .Matches("^[0-9]+$").WithMessage("O código deve conter apenas números.")
            //   .Must(referencia => int.TryParse(referencia, out var number) && number > 0)
            //   .WithMessage("O código deve ser um número maior que zero.")
            //   .MaximumLength(50).WithMessage("Referencia não pode exceder 50 caracteres.");




            //RuleFor(a => a.Descricao)
            //    .NotEmpty().WithMessage("Referencia é obrigatório.")
            //    .MaximumLength(300).WithMessage("Descricao não pode exceder 300 caracteres.");
        }
    }
}
 