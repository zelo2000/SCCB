using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCCB.Services.LessonService;

namespace SCCB.Web.ViewComponents
{
    /// <summary>
    /// View component for lessons list.
    /// </summary>
    public class LessonsForDayViewComponent : ViewComponent
    {
        private readonly ILessonService _lessonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonsForDayViewComponent"/> class.
        /// </summary>
        /// <param name="lessonService">Lesson service.</param>
        public LessonsForDayViewComponent(ILessonService lessonService)
        {
            _lessonService = lessonService ?? throw new ArgumentException(nameof(lessonService));
        }

        /// <summary>
        /// Method for calling partial view.
        /// </summary>
        /// <param name="groupId">Group identifier.</param>
        /// <param name="weekday">Weekday.</param>
        /// <returns>IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(Guid groupId, string weekday)
        {
            var lessonDtos = await _lessonService.FindByGroupIdAndWeekday(groupId, weekday);
            return View(lessonDtos);
        }
    }
}
