using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Booking DTO.
    /// </summary>
    public class Booking
    {
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
        /// Gets or sets unique identifier of booked classroom.
        /// </summary>
        public Guid ClassroomId { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of group for which booking was created.
        /// </summary>
        public Guid? GroupId { get; set; }
    }
}
