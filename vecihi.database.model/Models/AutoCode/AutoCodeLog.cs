using System;
using vecihi.infrastructure.entity.models;

namespace vecihi.database.model
{
    /// <summary>
    /// It keeps logs of the automatic code system.
    /// </summary>
    public class AutoCodeLog : ModelBase<Guid>
    {
        public int CodeNumber { get; set; }
        public DateTime CodeGenerationDate { get; set; }
        public Guid GeneratedBy { get; set; }

        //FK
        //AutoCodeGenerator
        public Guid AutoCodeId { get; set; }
        public virtual AutoCode AutoCode { get; set; }
    }
}