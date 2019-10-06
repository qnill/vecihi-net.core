using System;

namespace vecihi.infrastructure.entity.dtos
{
    public interface IDtoAuditBase
    {
        // Created
        DateTime CreatedAt { get; set; }
        string CreatedUserName { get; set; }

        // Updated
        DateTime? UpdatedAt { get; set; }
        string UpdatedUserName { get; set; }
    }
}