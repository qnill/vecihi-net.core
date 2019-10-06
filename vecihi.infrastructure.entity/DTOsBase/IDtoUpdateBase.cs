namespace vecihi.infrastructure.entity.dtos
{
    public interface IDtoUpdateBase<Type>
        where Type : struct
    {
        Type Id { get; set; }
    }
}