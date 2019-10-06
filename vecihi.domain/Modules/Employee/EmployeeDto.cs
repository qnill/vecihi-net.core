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
        [MaxLength(20)]
        public string Phone { get; set; }
        public string Title { get; set; }
    }

    public class EmployeeUpdateDto : EmployeeAddDto, IDtoUpdateBase<Guid>
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class EmployeeListDto : IDtoGetBase<Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }

        #region User

        [Order(ignore: true)]
        public Guid? UserId { get; set; }
        [Order("User.Email")]
        public string UserEmail { get; set; }

        #endregion
    }

    public class EmployeeCardDto : EmployeeListDto, IDtoAuditBase
    {
        #region Audit

        public string CreatedUserName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedUserName { get; set; }

        #endregion
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
        [Filter(SearchType.Contains, dbName: "User.Email")]
        public string UserEmail { get; set; }
    }

    public class InfoForJwtDto
    {
        public Guid Id { get; set; }
    }
}