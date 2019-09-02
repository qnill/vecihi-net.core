using System;
using System.ComponentModel.DataAnnotations;
using vecihi.infrastructure.entity.models;

namespace vecihi.database.model
{
    /// <summary> 
    /// If you customize this class, you need to customize the classes 
    /// in the 'EmployeeDto' file so that the related services can work properly.
    /// </summary>
    public class Employee : ModelBaseAudit<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public Guid? UserId { get; set; }
    }
}