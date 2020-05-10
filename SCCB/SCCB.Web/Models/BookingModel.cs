using SCCB.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCCB.Web.Models
{
    /// <summary>
    /// Booking model.
    /// </summary>
    public class BookingModel
    {
        /// <summary>
        /// Gets or sets booking description.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets booking date.
        /// </summary>
        [Required]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets booking lesson number.
        /// </summary>
        [Required]
        public int? LessonNumber { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of user who created the booking.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of booked classroom.
        /// </summary>
        [NotEmptyGuid]
        public Guid ClassroomId { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of group for which booking was created.
        /// </summary>
        public Guid? GroupId { get; set; }
    }
}
