using FluentValidation;
using MediatorCqrsApi.Aplicacao.DML.Empresas;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorCqrsApi.Aplicacao.DML.Usuarios
{
    public class UsuarioInserirValidator : AbstractValidator<UsuarioInserirRequest>
    {
        public UsuarioInserirValidator(IStringLocalizer<UsuarioInserirValidator> localizer)
        {
            

        }
    }
}
