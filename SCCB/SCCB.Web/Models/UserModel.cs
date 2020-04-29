using System;
using System.ComponentModel.DataAnnotations;
using SCCB.Core.Attributes;

namespace SCCB.Web.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets First Name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets Last Name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Role.
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string Role { get; set; }
    }
}
