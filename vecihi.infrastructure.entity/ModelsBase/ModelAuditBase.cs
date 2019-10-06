﻿using System;

namespace vecihi.infrastructure.entity.models
{
    public interface IModelAuditBase<Type> : IModelBase<Type>
        where Type : struct
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        Type CreatedBy { get; set; }
        Type? UpdatedBy { get; set; }
    }

    public abstract class ModelAuditBase<Type> : ModelBase<Type>, IModelAuditBase<Type>
        where Type : struct
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Type CreatedBy { get; set; }
        public Type? UpdatedBy { get; set; }
    }
}