using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
