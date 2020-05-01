using Microsoft.AspNetCore.Mvc;
using SCCB.Services.ClassroomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Web.ViewComponents
{
    /// <summary>
    /// View component for classrooms list.
    /// </summary>
    public class ClassroomOptionsForBookingViewComponent : ViewComponent
    {
        private readonly IClassroomService _classroomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomOptionsForBookingViewComponent"/> class.
        /// </summary>
        /// <param name="classroomService">Сlassroom service.</param>
        public ClassroomOptionsForBookingViewComponent(IClassroomService classroomService)
        {
            _classroomService = classroomService ?? throw new ArgumentException(nameof(classroomService));
        }

        /// <summary>
        /// Method for calling partial view. Finds free classrooms for specified date and lesson number.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <returns>IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(DateTime? date, int? lessonNumber)
        {
            var classroomsByBuilding = await _classroomService.FindFreeClassroomsGroupedByBuilding(date, lessonNumber);
            return View(classroomsByBuilding);
        }
    }
}
