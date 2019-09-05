using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace vecihi.database.model
{
    public class User : IdentityUser<Guid>
    {
        public DateTime? LastLoginDate { get; set; }

        // FK
        // Employee
        public virtual ICollection<Employee> Employees { get; set; }

        // AutoCodeLog
        public virtual ICollection<AutoCodeLog> AutoCodeLogs { get; set; }

        public User()
        {
            Employees = new List<Employee>();
            AutoCodeLogs = new List<AutoCodeLog>();
        }
    }
}