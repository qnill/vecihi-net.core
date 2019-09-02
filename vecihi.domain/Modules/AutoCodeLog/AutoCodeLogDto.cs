using System;
using vecihi.infrastructure.entity.dtos;

namespace vecihi.domain.Modules
{
    public class AutoCodeLogListDto : DtoGetBase<Guid>
    {
        public string AutoCodeScreenCode { get; set; }
        public string Code { get; set; }
        public DateTime CodeGenerationDate { get; set; }
    }
}