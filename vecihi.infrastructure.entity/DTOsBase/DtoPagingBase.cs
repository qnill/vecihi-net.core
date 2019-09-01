using System.Collections.Generic;

namespace vecihi.infrastructure.entity.dtos
{
    public abstract class DtoPagingBase<Type, ListDto>
        where Type : struct
        where ListDto : DtoGetBase<Type>
    {
        public int DataCount { get; set; }
        public double? Sum { get; set; }
        public IList<ListDto> Records { get; set; }

        public DtoPagingBase()
        {
            Records = new List<ListDto>();
        }
    }
}