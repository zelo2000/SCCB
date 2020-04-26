using SCCB.Core.DTO;
using System;
using System.Collections.Generic;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Group options model.
    /// </summary>
    public class GroupOptionsModel
    {
        /// <summary>
        /// Gets or sets group id.
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// Gets or sets groups.
        /// </summary>
        public IEnumerable<Group> Groups { get; set; }
    }
}
