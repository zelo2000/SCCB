using Microsoft.AspNetCore.Mvc;

namespace SCCB.Web.Controllers
{
    /// <summary>
    /// Booking Controller.
    /// </summary>
    public class BookingController : Controller
    {
        /// <summary>
        /// Get method for calling personal booking page.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Personal()
        {
            return View();
        }

        /// <summary>
        /// Get method for calling booking create page.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
