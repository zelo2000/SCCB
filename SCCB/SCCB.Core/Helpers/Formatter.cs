using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SCCB.Core.Helpers
{
    /// <summary>
    /// Formatter.
    /// </summary>
    public class Formatter
    {
        /// <summary>
        /// Get day of the week from date.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <returns>Day of the week in Ukrainian language, starting from capital letter.</returns>
        public static string WeekdayUkrainian(DateTime date)
        {
            var weekday = date.ToString("dddd", new CultureInfo("uk-UA"));
            return weekday.First().ToString().ToUpper() + weekday.Substring(1);
        }
    }
}
