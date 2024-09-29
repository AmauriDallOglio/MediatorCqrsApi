using MediatorCqrsApi.Dominio.Entidade;
using System.Text.RegularExpressions;

namespace MediatorCqrsApi.Dominio.Util
{
    public static class RegrasValidacao
    {
        public static List<Notificacao> ValidaString(string nomeCampo, string valor, int min, int max)
        {
            List<Notificacao> erros = new List<Notificacao>();

            var resultado = NaoPodeSerNuloOuVazio(nomeCampo, valor);
            if (!string.IsNullOrEmpty(resultado))
                erros.Add(new Notificacao().Incluir(nomeCampo, resultado, TipoNotificacao.Erro));

            resultado = TamanhoMinimoMaximoCaracter(nomeCampo, valor, min, max);
            if (!string.IsNullOrEmpty(resultado))
                erros.Add(new Notificacao().Incluir(nomeCampo, resultado, TipoNotificacao.Erro));

            return erros;
        }

        public static List<Notificacao> ValidarEmail(string nomeCampo, string valor, int min, int max)
        {
            List<Notificacao> erros = new List<Notificacao>();
            var resultado = NaoPodeSerNuloOuVazio(nomeCampo, valor);
            if (!string.IsNullOrEmpty(resultado))
                erros.Add(new Notificacao().Incluir(nomeCampo, resultado, TipoNotificacao.Erro));

            resultado = TamanhoMinimoMaximoCaracter(nomeCampo, valor, min, max);
            if (!string.IsNullOrEmpty(resultado))
                erros.Add(new Notificacao().Incluir(nomeCampo, resultado, TipoNotificacao.Erro));

            resultado = CaracteresDoEmail(nomeCampo, valor);
            if (!string.IsNullOrEmpty(resultado))
                erros.Add(new Notificacao().Incluir(nomeCampo, resultado, TipoNotificacao.Erro));

            
            return erros;
        }


        private static string CaracteresDoEmail(string fieldName, string valor)
        {
            if (!Regex.IsMatch(valor, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                return $"O campo {fieldName}, inválido!";
            }
            return string.Empty;
        }


        private static string ApenasNumeros(string fieldName, string valor)
        {
            if (!Regex.IsMatch(valor, "^[0-9]+$"))
            {
                return $"O campo {fieldName}, deve conter apenas números.";
            }
            return string.Empty;
        }

 
        private static string TamanhoMaximoCaracter(string fieldName, string valor, int tamanho)
        {
            if (valor.Length > tamanho)
            {
                return $"O campo {fieldName}, o valor não pode exceder {tamanho} caracteres.";
            }
            return string.Empty;
        }


        private static string NumeroMaiorQueZero(string fieldName, string valor)
        {
            if (!int.TryParse(valor, out var number) || number <= 0)
            {
                return $"O campo {fieldName}, deve ser um número maior que zero.";
            }
            return string.Empty;
        }

    
        private static string NaoPodeSerNuloOuVazio(string fieldName, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return $"O campo {fieldName}, não pode ser nulo ou vazio.";
            }
            return string.Empty;
        }

 
        private static string TamanhoMinimoMaximoCaracter(string fieldName, string valor, int min, int max)
        {
            if (string.IsNullOrEmpty(valor))
            {
                return $"O campo {fieldName}, não pode ser nulo ou vazio.";
            }

            if (valor.Length < min)
            {
                return $"O campo {fieldName}, deve ter pelo menos {min} caracteres.";
            }

            if (valor.Length > max)
            {
                return $"O campo {fieldName}, não pode exceder {max} caracteres.";
            }

            return string.Empty; // Retorna vazio se estiver dentro dos limites
        }


        private static string CampoObrigatorio(string fieldName)
        {
            return $"O campo {fieldName}, é obrigatório.";
        }


        private static string TamanhoCampo(string fieldName, string value, int min, int max)
        {
            
            if (value.Length >= min && value.Length <= max)
            {
                return $"O campo {fieldName}, deve ter entre {min} e {max} caracteres.";
            }
            return string.Empty;
        }
         
 
    }
}
