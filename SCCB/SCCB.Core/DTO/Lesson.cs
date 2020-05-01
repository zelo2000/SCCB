using System;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Lesson DTO.
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// Gets or sets unique identifier of lesson.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets lesson lector.
        /// </summary>
        public Lector Lector { get; set; }

        /// <summary>
        /// Gets or sets lesson classroom.
        /// </summary>
        public Classroom Classroom { get; set; }

        /// <summary>
        /// Gets or sets lesson title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets lesson group id.
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets lesson lector id.
        /// </summary>
        public Guid LectorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lesson is denominator.
        /// </summary>
        public bool IsDenominator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lesson is enumerator.
        /// </summary>
        public bool IsEnumerator { get; set; }

        /// <summary>
        /// Gets or sets lesson type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets lesson weekday.
        /// </summary>
        public string Weekday { get; set; }

        /// <summary>
        /// Gets or sets lesson number.
        /// </summary>
        public int LessonNumber { get; set; }

        /// <summary>
        /// Gets or sets lesson classroom id.
        /// </summary>
        public Guid ClassroomId { get; set; }
    }
}
