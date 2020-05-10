using System;
using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    public class GroupModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
