using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Change password view model.
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Gets or sets old password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets new password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets repeat of new password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string NewPasswordRepeat { get; set; }
    }
}
