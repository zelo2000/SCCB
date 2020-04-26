using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Log in model.
    /// </summary>
    public class LogInModel
    {
        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
