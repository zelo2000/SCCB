using Microsoft.AspNetCore.Mvc;

namespace SCCB.Web.Controllers
{
    public class ScheduleEditingController : Controller
    {
        public IActionResult Edit()
        {
            return View();
        }
    }
}