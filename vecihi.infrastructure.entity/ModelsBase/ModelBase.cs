namespace vecihi.infrastructure.entity.models
{
    public interface IModelBase<Type>
        where Type : struct
    {
        Type Id { get; set; }
        bool IsDeleted { get; set; }
    }

    public abstract class ModelBase<Type> : IModelBase<Type>
        where Type:struct
    {
        public Type Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}