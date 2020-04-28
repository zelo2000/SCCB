using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.BookingService
{
    public interface IBookingService
    {
        /// <summary>
        /// Add new booking.
        /// </summary>
        /// <param name="booking">Booking.</param>
        /// <returns>Task.</returns>
        Task Add(Booking bookingDto);

        /// <summary>
        /// Find booking by date, lesson number, and classroom GUID. Search parameter is omitted if it's value is null.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <param name="classroomId">Classroom id.</param>
        /// <returns>IEnumerable of bookings with user, classroom and group included.</returns>
        Task<IEnumerable<BookingWithIncludedInfo>> FindBookingsWithIncludedInfo(DateTime? date, int? lessonNumber, Guid? classroomId);
    }
}
