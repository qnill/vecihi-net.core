using System;
using System.ComponentModel.DataAnnotations;
using vecihi.helper.Attributes;
using vecihi.infrastructure.entity.dtos;
using static vecihi.helper.Const.Enums;

namespace vecihi.domain.Modules
{
    public class EmployeeAddDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(20), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class EmployeeUpdateDto : DtoUpdateBase<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }

    public class EmployeeListDto : DtoGetBase<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class EmployeeCardDto : EmployeeListDto
    {
        public DateTime? LastLoginDate { get; set; }
    }

    public class EmployeePagingDto : DtoPagingBase<Guid, EmployeeListDto>
    {
    }

    public class EmployeeFilterDto
    {
        [Filter(SearchType.Contains)]
        public string Name { get; set; }
        [Filter(SearchType.Contains)]
        public string Phone { get; set; }
        [Filter(SearchType.Contains)]
        public string Email { get; set; }
    }
}
