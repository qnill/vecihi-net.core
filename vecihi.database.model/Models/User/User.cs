using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vecihi.database.model
{
    public class User : IdentityUser<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        // FK
        // Employee
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty("CreatedUser")]
        public virtual ICollection<Employee> EmployeesCreated { get; set; }


        // AutoCodeLog
        public virtual ICollection<AutoCodeLog> AutoCodeLogs { get; set; }

        public User()
        {
            Employees = new List<Employee>();
            AutoCodeLogs = new List<AutoCodeLog>();
        }
    }
}