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
        public string Title { get; set; }
    }

    public class EmployeeUpdateDto : DtoUpdateBase<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public class EmployeeListDto : DtoGetBase<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
    }

    public class EmployeeCardDto : EmployeeListDto
    {
    }

    public class EmployeePagingDto : DtoPagingBase<Guid, EmployeeListDto>
    {
    }

    public class EmployeeExportDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
    }

    public class EmployeeFilterDto
    {
        [Filter(SearchType.Contains)]
        public string Name { get; set; }
        [Filter(SearchType.Contains)]
        public string Phone { get; set; }
        [Filter(SearchType.Contains)]
        public string Title { get; set; }
    }

    public class InfoForJwtDto
    {
        public Guid Id { get; set; }
    }
}
