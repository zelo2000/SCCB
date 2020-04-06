using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Web.Models
{
    public class LessonModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string LessonNumber { get; set; }

        [Required]
        [MaxLength(9)]
        public string Weekday { get; set; }

        [Required]
        public Guid GroupId { get; set; }

        [Required]
        public Guid LectorId { get; set; }

        [Required]
        public Guid ClassroomId { get; set; }

        [Required]
        public bool IsDenominator { get; set; }

        [Required]
        public bool IsEnumerator { get; set; }
    }
}
