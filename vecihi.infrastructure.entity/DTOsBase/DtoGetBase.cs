using System.ComponentModel.DataAnnotations;

namespace vecihi.infrastructure.entity.dtos
{
    public abstract class DtoGetBase<Type>
        where Type : struct
    {
        [Required]
        public Type Id { get; set; }
    }
}