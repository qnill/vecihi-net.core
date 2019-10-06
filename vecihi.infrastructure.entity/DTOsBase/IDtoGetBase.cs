namespace vecihi.infrastructure.entity.dtos
{
    public interface IDtoGetBase<Type>
        where Type : struct
    {
        Type Id { get; set; }
    }
}