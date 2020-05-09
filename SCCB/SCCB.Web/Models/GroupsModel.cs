using SCCB.Core.DTO;
using System.Collections.Generic;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Model for Groups Index view.
    /// </summary>
    public class GroupsModel
    {
        /// <summary>
        /// Gets or sets groups owned by user.
        /// </summary>
        public IEnumerable<Group> OwnedGroups { get; set; }

        /// <summary>
        /// Gets or sets groups where user is member.
        /// </summary>
        public IEnumerable<Group> MemberGroups { get; set; }
    }
}
