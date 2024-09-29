using FluentValidation;
using MediatorCqrsApi.Dominio.Entidade;
using MediatorCqrsApi.Dominio.Interface;
using MediatorCqrsApi.Dominio.Util;
using Microsoft.Extensions.Localization;

namespace MediatorCqrsApi.Aplicacao.DML.Empresas
{
    public class EmpresaInserirValidator : AbstractValidator<EmpresaInserirRequest>
    {
        private readonly IEmMemoriaRepositorio _emMemoriaRepositorio;

        public EmpresaInserirValidator(IStringLocalizer<EmpresaInserirValidator> localizer, IEmMemoriaRepositorio emMemoriaRepositorio)
        {
            _emMemoriaRepositorio = emMemoriaRepositorio;

            RuleFor(x => x.Referencia)
                .Custom(async (referencia, context) =>
                {
                    List<Notificacao> erros = RegrasValidacao.ValidaString("Referência", referencia, 5, 50);
                    foreach (var erro in erros)
                    {
                        context.AddFailure(erro.Mensagem);
                        await _emMemoriaRepositorio.Adicionar(erro);
                    }
                });

            RuleFor(x => x.Descricao)
                .Custom(async (descricao, context) =>
                {
                    List<Notificacao> erros = RegrasValidacao.ValidaString("Descrição", descricao, 5, 300);
                    foreach (var erro in erros)
                    {
                        context.AddFailure(erro.Mensagem);
                        await _emMemoriaRepositorio.Adicionar(erro);
                    }
                });


            


            //// Validação da Referência
            //RuleFor(x => x.Referencia)
            //    .NotEmpty().WithMessage(localizer["Referencia_NaoPodeSerNuloOuVazio"])
            //    .Must(SerSomenteNumeros).WithMessage(localizer["Referencia_ApenasNumeros"])
            //    .Must(SerNumeroMaiorQueZero).WithMessage(localizer["Referencia_NumeroMaiorQueZero"])
            //    .MaximumLength(50).WithMessage(localizer["Referencia_TamanhoMaximoCaracter"]);

            //// Validação da Descrição
            //RuleFor(x => x.Descricao)
            //    .NotEmpty().WithMessage(localizer["Descricao_NaoPodeSerNuloOuVazio"])
            //    .Must(CampoObrigatorio).WithMessage(localizer["Descricao_CampoObrigatorio"])
            //    .Length(5, 300).WithMessage(localizer["Descricao_TamanhoMinimoMaximoCaracter"]);



            //RuleFor(request => request.Referencia)
            //    .NotEmpty().WithMessage(x => localizer["Referencia é obrigatória (Validador)!"]);
            //RuleFor(request => request.Descricao)
            //    .NotEmpty().WithMessage(x => localizer["Descrição é obrigatória (Validador)!"]);

            ////RuleFor(a => a.Referencia)
            ////   .NotEmpty().WithMessage("Referencia é obrigatório.")
            ////   .Matches("^[0-9]+$").WithMessage("O código deve conter apenas números.")
            ////   .Must(referencia => int.TryParse(referencia, out var number) && number > 0)
            ////   .WithMessage("O código deve ser um número maior que zero.")
            ////   .MaximumLength(50).WithMessage("Referencia não pode exceder 50 caracteres.");

            ////RuleFor(a => a.Descricao)
            ////    .NotEmpty().WithMessage("Referencia é obrigatório.")
            ////    .MaximumLength(300).WithMessage("Descricao não pode exceder 300 caracteres.");
        }

        // Método auxiliar para aplicar validações e adicionar falhas ao contexto
        //private void ValidarCampo(string campo, Func<string, List<string>> regraValidacao, CustomContext context)
        //{
        //    var erros = regraValidacao(campo); // Aplica a regra de validação passada
        //    if (erros.Any())
        //    {
        //        foreach (var erro in erros)
        //        {
        //            context.AddFailure(erro); // Adiciona cada erro encontrado ao contexto de validação
        //        }
        //    }
        //}


        //private bool SerSomenteNumeros(string referencia)
        //{
        //    var erros = RegrasValidacao.ApenasNumeros("Referencia", referencia);
        //    return string.IsNullOrEmpty(erros);
        //}

        //private bool SerNumeroMaiorQueZero(string referencia)
        //{
        //    var erros = RegrasValidacao.NumeroMaiorQueZero("Referencia", referencia);
        //    return string.IsNullOrEmpty(erros);
        //}

        //private bool CampoObrigatorio(string descricao)
        //{
        //    var erros = RegrasValidacao.CampoObrigatorio("Descrição");
        //    return string.IsNullOrEmpty(erros);
        //}
    }
}
 