using System.ComponentModel.DataAnnotations;

namespace MediatorCqrsApi.Dominio.Util
{
    public abstract class AtributoIdObrigatorio<TId> : IAtributoIdObrigatorio<TId>
    {
        public TId Id { get; set; }
       
    }
}
