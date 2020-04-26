using Microsoft.AspNetCore.Mvc;

namespace SCCB.Web.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult PersonalBooking()
        {
            return View();
        }

        public IActionResult LessonBooking()
        {
            return View();
        }
    }
}
