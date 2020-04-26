using System;
using System.ComponentModel.DataAnnotations;
using SCCB.Core.Attributes;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Lesson model.
    /// </summary>
    public class LessonModel
    {
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets lesson number.
        /// </summary>
        [Required]
        public string LessonNumber { get; set; }

        /// <summary>
        /// Gets or sets weekday.
        /// </summary>
        [Required]
        [MaxLength(9)]
        public string Weekday { get; set; }

        /// <summary>
        /// Gets or sets group id.
        /// </summary>
        [Required]
        [NotEmptyGuid]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets lector id.
        /// </summary>
        [Required]
        [NotEmptyGuid]
        public Guid LectorId { get; set; }

        /// <summary>
        /// Gets or sets classroom id.
        /// </summary>
        [Required]
        [NotEmptyGuid]
        public Guid ClassroomId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is denominator.
        /// </summary>
        [Required]
        public bool IsDenominator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is enumerator.
        /// </summary>
        [Required]
        public bool IsEnumerator { get; set; }

        /// <summary>
        /// Gets or sets type.
        /// </summary>
        [Required]
        [MaxLength(9)]
        public string Type { get; set; }
    }
}
