using System.Drawing;

namespace MediatorCqrsApi.Dominio.Util
{
    public static class RegrasValidacao
    {
        public static List<string> ValidaString(string nomeCampo, string valor, int min, int max)
        {
            var erros = new List<string>();

            var resultado = NaoPodeSerNuloOuVazio(nomeCampo, valor);
            if (!string.IsNullOrEmpty(resultado)) erros.Add(resultado);

   

            resultado = TamanhoMinimoMaximoCaracter(nomeCampo, valor, min, max);
            if (!string.IsNullOrEmpty(resultado)) erros.Add(resultado);

            return erros; // Retorna uma lista de erros, se houver
        }

        public static List<string> ValidarDescricao(string nomeCampo, string valor, int min, int max)
        {
            var erros = new List<string>();

            var resultado = NaoPodeSerNuloOuVazio(nomeCampo, valor);
            if (!string.IsNullOrEmpty(resultado)) erros.Add(resultado);

            resultado = CampoObrigatorio(nomeCampo);
            if (!string.IsNullOrEmpty(resultado)) erros.Add(resultado);

            resultado = TamanhoMinimoMaximoCaracter(nomeCampo, valor, min, max);
            if (!string.IsNullOrEmpty(resultado)) erros.Add(resultado);

            return erros; // Retorna uma lista de erros, se houver
        }

  
        public static string ApenasNumeros(string fieldName, string valor)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(valor, "^[0-9]+$"))
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

 
        public static string NumeroMaiorQueZero(string fieldName, string valor)
        {
            if (!int.TryParse(valor, out var number) || number <= 0)
            {
                return $"O campo {fieldName}, o código deve ser um número maior que zero.";
            }
            return string.Empty;
        }

    
        private static string NaoPodeSerNuloOuVazio(string fieldName, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return $"O campo {fieldName}, o valor não pode ser nulo ou vazio.";
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

 
        public static string CampoObrigatorio(string fieldName)
        {
            return $"O campo {fieldName}, é obrigatório.";
        }

        
        public static string TamanhoCampo(string fieldName, string value, int min, int max)
        {
            
            if (value.Length >= min && value.Length <= max)
            {
                return $"O campo {fieldName}, deve ter entre {min} e {max} caracteres.";
            }
            return string.Empty;
        }
         
 
    }
}
