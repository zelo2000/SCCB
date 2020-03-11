using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    public class LogInModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
