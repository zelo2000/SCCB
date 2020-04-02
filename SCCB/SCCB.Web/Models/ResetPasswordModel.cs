using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string NewPasswordRepeat { get; set; }
    }
}
