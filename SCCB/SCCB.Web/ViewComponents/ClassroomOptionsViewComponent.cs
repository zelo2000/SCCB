using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCCB.Core.DTO;
using SCCB.Services.ClassroomService;

namespace SCCB.Web.ViewComponents
{
    /// <summary>
    /// View component for classrooms list.
    /// </summary>
    public class ClassroomOptionsViewComponent : ViewComponent
    {
        private readonly IClassroomService _classroomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomOptionsViewComponent"/> class.
        /// </summary>
        /// <param name="classroomService">Сlassroom service.</param>
        public ClassroomOptionsViewComponent(IClassroomService classroomService)
        {
            _classroomService = classroomService ?? throw new ArgumentException(nameof(classroomService));
        }

        /// <summary>
        /// Method for calling partial view. Finds free classrooms for specified lesson time.
        /// </summary>
        /// <param name="time">Lesson time.</param>
        /// <returns>IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(LessonTime time)
        {
            var classroomsByBuilding = await _classroomService.FindFreeClassroomsGroupedByBuilding(time);
            return View(classroomsByBuilding);
        }
    }
}
