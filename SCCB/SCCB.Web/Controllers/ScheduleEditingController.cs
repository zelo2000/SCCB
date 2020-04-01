using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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