using SCCB.Core.Attributes;
using SCCB.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Student identifier.
        /// </summary>
        public string StudentId { get; set; }

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

        /// <summary>
        /// Gets or sets Lector Position.
        /// </summary>
        public string Position { get; set; }
    }
}
