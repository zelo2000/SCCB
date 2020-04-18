using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Properties identifying time when lesson takes place.
    /// </summary>
    public class LessonTime
    {
        /// <summary>
        /// Gets or sets day of the week.
        /// </summary>
        public string Weekday { get; set; }

        /// <summary>
        /// Gets or sets lesson number.
        /// </summary>
        public string LessonNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lesson takes place by numerator weeks.
        /// </summary>
        public bool IsNumerator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lesson takes place by denominator weeks.
        /// </summary>
        public bool IsDenominator { get; set; }
    }
}
