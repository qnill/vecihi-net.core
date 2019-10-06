using System;
using vecihi.infrastructure.entity.dtos;

namespace vecihi.domain.Modules
{
    public class AutoCodeLogListDto : IDtoGetBase<Guid>
    {
        public Guid Id { get; set; }
        public string AutoCodeScreenCode { get; set; }
        public string Code { get; set; }
        public DateTime CodeGenerationDate { get; set; }
    }
}