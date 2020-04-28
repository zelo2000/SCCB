using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Booking with User, Classroom and Group objects included.
    /// </summary>
    public class BookingWithIncludedInfo
    {
        /// <summary>
        /// Gets or sets unique identifier of booking.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets booking description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets booking date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets booking lesson number.
        /// </summary>
        public int LessonNumber { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of user who created the booking.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets user who created the booking.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of booked classroom.
        /// </summary>
        public Guid ClassroomId { get; set; }

        /// <summary>
        /// Gets or sets booked classroom.
        /// </summary>
        public Classroom Classroom { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of group for which booking was created.
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// Gets or sets group for which booking was created.
        /// </summary>
        public Group Group { get; set; }
    }
}
