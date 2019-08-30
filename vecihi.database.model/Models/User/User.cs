using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vecihi.infrastructure.entity;

namespace vecihi.database.model
{
    /// <summary> 
    /// If you customize this class, you need to customize the classes 
    /// in the 'UserDto' file so that the related services can work properly.
    /// </summary>
    public class User : EntityBaseAudit<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public Guid? ExternalAuthId { get; set; }

        //Custom code is here

        //FK
        //AutoCodeLog
        public virtual ICollection<AutoCodeLog> AutoCodeLogs { get; set; }

        public User()
        {
            AutoCodeLogs = new List<AutoCodeLog>();
        }
    }
}