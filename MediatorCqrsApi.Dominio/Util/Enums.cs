using System.ComponentModel;

namespace MediatorCqrsApi.Dominio.Util
{
    public enum ModoCruds : short
    {
        [Description("Inserir")]
        Inserir = 1,

        [Description("Alterar")]
        Alterar = 2,

        [Description("Excluir")]
        Excluir = 3
    }

}
