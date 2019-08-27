namespace vecihi.infrastructure.entity
{
    public interface IEntityBase<Type>
        where Type : struct
    {
        Type Id { get; set; }
        bool IsDeleted { get; set; }
    }

    public abstract class EntityBase<Type> : IEntityBase<Type>
        where Type:struct
    {
        public Type Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
