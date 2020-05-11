using System;
using System.Collections.Generic;

namespace SCCB.Core.Constants
{
    /// <summary>
    /// Term setting class.
    /// </summary>
    public static class TermSettings
    {
        /// <summary>
        /// Maximum lessons per day.
        /// </summary>
        public const int MaxLessons = 8;

        /// <summary>
        /// First day of the term.
        /// </summary>
        public static readonly DateTime FirstDay = new DateTime(2020, 2, 10);

        /// <summary>
        /// Time when the lesson with number equal to list index begins.
        /// </summary>
        public static readonly List<TimeSpan> BeginTimes = new List<TimeSpan>
        {
            new TimeSpan(8, 30, 0),
            new TimeSpan(10, 10, 0),
            new TimeSpan(11, 50, 0),
            new TimeSpan(13, 30, 0),
            new TimeSpan(15, 5, 0),
            new TimeSpan(16, 40, 0),
            new TimeSpan(18, 10, 0),
            new TimeSpan(19, 40, 0),
        };

        /// <summary>
        /// Time when the lesson with number equal to list index Ends.
        /// </summary>
        public static readonly List<TimeSpan> EndTimes = new List<TimeSpan>
        {
            new TimeSpan(9, 50, 0),
            new TimeSpan(11, 30, 0),
            new TimeSpan(13, 10, 0),
            new TimeSpan(14, 50, 0),
            new TimeSpan(16, 25, 0),
            new TimeSpan(18, 0, 0),
            new TimeSpan(19, 30, 0),
            new TimeSpan(21, 0, 0),
        };
    }
}
