using System;
using System.ComponentModel.DataAnnotations.Schema;
using vecihi.infrastructure.entity;

namespace vecihi.database.model
{
    /// <summary>
    /// It keeps logs of the automatic code system.
    /// </summary>
    public class AutoCodeLogModel : EntityBase<Guid>
    {
        public int CodeNumber { get; set; }
        public DateTime CodeGenerationDate { get; set; }

        //FK
        //AutoCodeGenerator
        public Guid AutoCodeId { get; set; }
        public virtual AutoCodeModel AutoCode { get; set; }

        //User
        public Guid GeneratedBy { get; set; }
        [ForeignKey("GeneratedBy")]
        public virtual UserModel User { get; set; }
    }
}