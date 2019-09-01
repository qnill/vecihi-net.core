using System.Collections.Generic;

namespace vecihi.infrastructure.entity.dtos
{
    public abstract class DtoPagingBase<Type, BaseGetDto>
        where Type : struct
        where BaseGetDto : DtoGetBase<Type>
    {
        public int DataCount { get; set; }
        public double? Sum { get; set; }
        public IList<BaseGetDto> Records { get; set; }

        public DtoPagingBase()
        {
            Records = new List<BaseGetDto>();
        }
    }
}