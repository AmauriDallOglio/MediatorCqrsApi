namespace MediatorCqrsApi.Dominio.Util
{
    public interface IAtributoIdObrigatorio<TId> : IValidacaoEntity<TId>, IEntity<TId>
    {
    }

    public interface IEntity<TId> : IEntity
    {
        public TId Id { get; set; }
    }

    public interface IEntity
    {
    }
}