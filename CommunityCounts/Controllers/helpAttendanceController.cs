using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityCounts.Controllers
{
    [Authorize]
    public class helpAttendanceController : Controller
    {
        // GET: helpAttendance
        public ActionResult Home()
        {
            return View();
        }
    }
}