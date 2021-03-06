﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vecihi.infrastructure.entity.models;

namespace vecihi.database.model
{
    /// <summary> 
    /// If you customize this class, you need to customize the classes 
    /// in the 'EmployeeDto' file so that the related services can work properly.
    /// </summary>
    public class Employee : ModelAuditBase<Guid, User>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        public string Title { get; set; }

        // FK
        // User
        public Guid? UserId { get; set; }
        [InverseProperty("Employees")]
        public virtual User User { get; set; }
    }
}