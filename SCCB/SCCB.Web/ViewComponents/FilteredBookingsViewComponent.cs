using Microsoft.AspNetCore.Mvc;
using SCCB.Services.BookingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Web.ViewComponents
{
    /// <summary>
    /// View component for bookings list.
    /// </summary>
    public class FilteredBookingsViewComponent : ViewComponent
    {
        private readonly IBookingService _bookingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilteredBookingsViewComponent"/> class.
        /// </summary>
        /// <param name="bookingService">Booking service.</param>
        public FilteredBookingsViewComponent(IBookingService bookingService)
        {
            _bookingService = bookingService ?? throw new ArgumentException(nameof(bookingService));
        }

        /// <summary>
        /// Invoke view component. Finds bookings according to parameters.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <param name="classroomId">Classroom id.</param>
        /// <returns>IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(DateTime? date, int? lessonNumber, Guid? classroomId)
        {
            var bookings = await _bookingService.FindBookingsWithIncludedInfo(date, lessonNumber, classroomId);
            return View(bookings);
        }
    }
}
