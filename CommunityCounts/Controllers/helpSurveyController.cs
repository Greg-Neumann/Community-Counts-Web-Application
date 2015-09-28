using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityCounts.Controllers
{
    [Authorize]
    public class helpSurveyController : Controller
    {
        // GET: helpSurvey
        public ActionResult Home()
        {
            return View();
        }
    }
}