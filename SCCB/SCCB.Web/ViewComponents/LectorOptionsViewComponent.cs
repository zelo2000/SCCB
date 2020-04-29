using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SCCB.Core.DTO;
using SCCB.Services.LectorService;

namespace SCCB.Web.ViewComponents
{
    /// <summary>
    /// View component for lectors list.
    /// </summary>
    public class LectorOptionsViewComponent : ViewComponent
    {
        private readonly ILectorService _lectorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LectorOptionsViewComponent"/> class.
        /// </summary>
        /// <param name="lectorService">Lector service.</param>
        public LectorOptionsViewComponent(ILectorService lectorService)
        {
            _lectorService = lectorService ?? throw new ArgumentException(nameof(lectorService));
        }

        /// <summary>
        /// Method for calling partial view.
        /// </summary>
        /// <param name="time">Lesson time.</param>
        /// <returns>IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(LessonTime time)
        {
            var lectors = await _lectorService.FindFreeLectors(time);
            return View(lectors);
        }
    }
}
