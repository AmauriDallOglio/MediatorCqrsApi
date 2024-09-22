using System.ComponentModel.DataAnnotations;

namespace MediatorCqrsApi.Dominio.Util
{
    public abstract class AtributoIdObrigatorio<TId> : IAtributoIdObrigatorio<TId>
    {
        //[Key]
        public TId Id { get; set; }
       
    }
}
