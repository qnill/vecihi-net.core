using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vecihi.infrastructure.entity.models
{
    public interface IModelAuditBase<Type, UserEntity> : IModelBase<Type>
        where Type : struct
    {
        // Created
        DateTime CreatedAt { get; set; }
        Type CreatedBy { get; set; }
        UserEntity CreatedUser { get; set; }

        // Updated
        DateTime? UpdatedAt { get; set; }
        Type? UpdatedBy { get; set; }
        UserEntity UpdatedUser { get; set; }
    }

    public abstract class ModelAuditBase<Type, UserEntity> : ModelBase<Type>, IModelAuditBase<Type, UserEntity>
        where Type : struct
    {
        // Created
        public DateTime CreatedAt { get; set; }
        public Type CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual UserEntity CreatedUser { get; set; }

        // Updated
        public DateTime? UpdatedAt { get; set; }
        public Type? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual UserEntity UpdatedUser { get; set; }
    }
}