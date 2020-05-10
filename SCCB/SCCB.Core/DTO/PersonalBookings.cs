using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Personal bookings.
    /// </summary>
    public class PersonalBookings
    {
        /// <summary>
        /// Gets or sets bookings which user created.
        /// </summary>
        public IEnumerable<BookingWithIncludedInfo> MyBookings { get; set; }

        /// <summary>
        /// Gets or sets bookings for groups of user.
        /// </summary>
        public IEnumerable<BookingWithIncludedInfo> MyGroupsBookings { get; set; }
    }
}
