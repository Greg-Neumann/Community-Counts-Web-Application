﻿using System.Web.Mvc;

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