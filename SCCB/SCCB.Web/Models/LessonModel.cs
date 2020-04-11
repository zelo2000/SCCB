using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SCCB.Core.Attributes;

namespace SCCB.Web.Models
{
    public class LessonModel
    {
        public Guid Id { get; set; }
        public LectorModel Lector { get; set; }
        public ClassroomModel Classroom { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string LessonNumber { get; set; }

        [Required]
        [MaxLength(9)]
        public string Weekday { get; set; }

        [Required]
        [NotEmptyGuid]
        public Guid GroupId { get; set; }

        [Required]
        [NotEmptyGuid]
        public Guid LectorId { get; set; }

        [Required]
        [NotEmptyGuid]
        public Guid ClassroomId { get; set; }

        [Required]
        public bool IsDenominator { get; set; }

        [Required]
        [MaxLength(9)]
        public string Type { get; set; }

        [Required]
        public bool IsEnumerator { get; set; }
    }
}
