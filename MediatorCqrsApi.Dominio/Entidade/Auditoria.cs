using MediatorCqrsApi.Dominio.Util;
using System.ComponentModel.DataAnnotations;

namespace MediatorCqrsApi.Dominio.Entidade
{
    public class Auditoria : AtributoIdObrigatorio<Guid>, IEmpresaObrigatorio
    {
        [Required(ErrorMessage = "O Id do Empresa é obrigatório.")]
        public Guid Id_Empresa { get; set; }

        [Required(ErrorMessage = "O nome da tabela é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O nome da tabela deve ter no máximo 50 caracteres.")]
        public string NomeTabela { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modo CRUD é obrigatório.")]
        [MaxLength(15, ErrorMessage = "O modo CRUD deve ter no máximo 15 caracteres.")]
        public string ModoCrud { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de cadastro é obrigatória.")]
        public DateTime DataCadastro { get; set; } 

        [MaxLength(500, ErrorMessage = "O histórico antes deve ter no máximo 500 caracteres.")]
        public string HistoricoAntes { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "O histórico depois deve ter no máximo 500 caracteres.")]
        public string HistoricoDepois { get; set; } = string.Empty;


        public Auditoria DadosDoIncluir(Guid idEmpresa, Guid? idRegistroAtual, string nomeTabela, string jsonData)
        {
 

            Id_Empresa = idEmpresa;
            NomeTabela = nomeTabela;
            ModoCrud = ModoCruds.Inserir.GetDescricao();
            DataCadastro = DateTime.Now;
            HistoricoAntes = "Não tem";
            HistoricoDepois = jsonData;
            return this;
        }

        public Auditoria DadosDoAlterar(Guid idEmpresa, Guid? idRegistroAtual, string nomeTabela, string jsonData)
        {
 

            Id_Empresa = idEmpresa;
            NomeTabela = nomeTabela;
            ModoCrud = ModoCruds.Alterar.GetDescricao();
            DataCadastro = DateTime.Now;
            HistoricoAntes = "Não tem";
            HistoricoDepois = jsonData;
            return this;
        }

        public Auditoria DadosDoExcluir(Guid idEmpresa, Guid? idRegistroAtual, string nomeTabela, string jsonData)
        {
 

            Id_Empresa = idEmpresa;
            NomeTabela = nomeTabela;
            ModoCrud = ModoCruds.Excluir.GetDescricao();
            DataCadastro = DateTime.Now;
            HistoricoAntes = "Não tem";
            HistoricoDepois = jsonData;
            return this;
        }

 

    }
}
