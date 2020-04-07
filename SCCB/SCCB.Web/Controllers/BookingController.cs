using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace SCCB.Web.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Book()
        {
            return View();
        }
    }
}
