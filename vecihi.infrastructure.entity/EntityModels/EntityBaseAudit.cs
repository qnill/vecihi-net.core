using System;

namespace vecihi.infrastructure.entity
{
    public interface IEntityBaseAudit<Type> : IEntityBase<Type>
        where Type : struct
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        Type CreatedBy { get; set; }
        Type? UpdatedBy { get; set; }
    }

    public abstract class EntityBaseAudit<Type> : EntityBase<Type>, IEntityBaseAudit<Type>
        where Type : struct
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Type CreatedBy { get; set; }
        public Type? UpdatedBy { get; set; }
    }
}