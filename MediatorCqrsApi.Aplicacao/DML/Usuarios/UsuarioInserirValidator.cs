using FluentValidation;
using Microsoft.Extensions.Localization;

namespace MediatorCqrsApi.Aplicacao.DML.Usuarios
{
    public class UsuarioInserirValidator : AbstractValidator<UsuarioInserirRequest>
    {
        public UsuarioInserirValidator(IStringLocalizer<UsuarioInserirValidator> localizer)
        {
            

        }
    }
}
