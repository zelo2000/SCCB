using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        public string PasswordRepeat { get; set; }
    }
}
