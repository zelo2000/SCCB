using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SCCB.Web.Models;

namespace SCCB.Web.Controllers
{
    /// <summary>
    /// Home Controller.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public HomeController(
            ILogger<HomeController> log)
        {
            _log = log;
        }

        /// <summary>
        /// Get method for calling home page.
        /// </summary>
        /// <param name="groupId">Group identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Index(Guid? groupId)
        {
            var model = new ScheduleViewModel() { GroupId = groupId };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
