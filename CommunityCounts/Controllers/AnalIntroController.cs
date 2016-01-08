using System.Web.Mvc;

namespace CommunityCounts.Controllers
{
    [Authorize]
    public class AnalIntroController : Controller
    {
        // GET: AnalIntro
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Bespoke()
        {
            return View();
        }
        public ActionResult DDIntro()
        {
            return View();
        }
        public ActionResult vClient()
        {
            return View();
        }
        public ActionResult vClientActivity()
        {
            return View();
        }
        public ActionResult vClientAttendance()
        {
            return View();
        }
        public ActionResult vBookings()
        {
            return View();
        }
        public ActionResult vJourneys()
        {
            return View();
        }
        public ActionResult vAttendance()
        {
            return View();
        }
        public ActionResult vQCSR()
        {
            return View();
        }
        public ActionResult vSurveys()
        {
            return View();
        }
        public ActionResult vSurveyResultsNum()
        {
            return View();
        }
        public ActionResult vSurveyResultsTxt()
        {
            return View();
        }
    }
}