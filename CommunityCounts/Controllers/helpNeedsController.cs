using System.Web.Mvc;

namespace CommunityCounts.Controllers
{
    [Authorize]
    public class helpNeedsController : Controller
    {
        // GET: helpSurvey
        public ActionResult Home()
        {
            return View();
        }
    }
}