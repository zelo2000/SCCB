using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.BookingService
{
    /// <summary>
    /// Booking service.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Add new booking.
        /// </summary>
        /// <param name="bookingDto">Booking.</param>
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

        /// <summary>
        /// Sets IsApproved property of Booking with specified id to true.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Task.</returns>
        Task Approve(Guid id);

        /// <summary>
        /// Removes Booking with specified id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Task.</returns>
        Task Remove(Guid id);

        /// <summary>
        /// Find bookings of user with <paramref name="userId"/> and his groups.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns><see cref="PersonalBookings"/> object.</returns>
        Task<PersonalBookings> FindPersonalBookings(Guid userId);
    }
}
