using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vecihi.infrastructure.entity.models;

namespace vecihi.database.model
{
    /// <summary>
    /// Can be used on tables with code fields.
    /// It is a dynamic structure that provides automatic code generation.
    /// A fixed code is defined for each screen in the project.<see cref = "helper.Const.ScreenCodes" />
    /// Sets a code format from the user interface to the relevant screen,
    /// the corresponding service is triggered and the automatic code is generated before
    /// the 'post' operation is performed on the code format defined screen.
    /// </summary>
    public class AutoCode : ModelBaseAudit<Guid>
    {
        /// <summary>
        /// Fixed screen codes
        /// <see cref="helper.Const.ScreenCodes"/>
        /// </summary>
        [Required, MaxLength(5)]
        public string ScreenCode { get; set; }
        /// <summary>
        /// Sample code format = "TC-{0}-VC"
        /// </summary>
        [Required, MaxLength(20)]
        public string CodeFormat { get; set; }
        public int LastCodeNumber { get; set; }

        // FK
        // AutoCodeLog
        public virtual ICollection<AutoCodeLog> AutoCodeLogs { get; set; }

        public AutoCode()
        {
            AutoCodeLogs = new List<AutoCodeLog>();
        }
    }

}