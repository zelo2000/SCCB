using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Email address model.
    /// </summary>
    public class EmailModel
    {
        /// <summary>
        /// Gets or sets email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
