using SCCB.Core.DTO;
using System;
using System.Collections.Generic;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Model for edit users on the admin page.
    /// </summary>
    public class EditUsersModel
    {
        /// <summary>
        /// Gets or sets user's role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets list of users.
        /// </summary>
        public IEnumerable<User> Users { get; set; }
    }
}
