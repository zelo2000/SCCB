using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    /// <summary>
    /// User profile model.
    /// </summary>
    public class ProfileModel
    {
        public string StudentId { get; set; }

        public string LectorPosition { get; set; }

        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
    }
}
