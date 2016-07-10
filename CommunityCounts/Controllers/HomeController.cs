using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            @ViewBag.userHasNews = false;
            if (Request.IsAuthenticated)
            {
                // already logged in, check for user news
                ccMaster db = new ccMaster(null);
                @ViewBag.userHasNews = CS.userHasNews(db);
                ViewBag.RegYearDate = CS.getRegYear(db,true);
                ViewBag.RegYear = CS.getRegYear(db, false);            }
           
            return View();
        }
        public ActionResult ReadMore()
        {
            @ViewBag.userHasNews = false;
            if (Request.IsAuthenticated)
            {
                // already logged in, check for user news
                ccMaster db = new ccMaster(null);
                @ViewBag.userHasNews = CS.userHasNews(db);
            }
            return View();
        }
        public ActionResult Dismiss ()
        {
            // get users logon id and set users table to stop displaying new messages
            var userName = User.Identity.Name;
            ccMaster db = new ccMaster(null);
            var us = db.users.Where(u => u.Email == userName);
            bool nameIsPresent = us.Any();
            if (nameIsPresent)
            {
                var rec = us.First();
                rec.readNews = true;
                db.Entry(rec).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Community Counts is developed by Next Step Forward.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Community Counts is developed by Next Step Forward.";

            return View();
        }
        [Authorize]
        public ActionResult N01P01A()
        {
            return View();
        }
        [Authorize]
        public ActionResult N01P01B()
        {
            return View();
        }
        [Authorize]
        public ActionResult ChgYear()
        {
            ccMaster db = new ccMaster(null);
            ViewBag.idRegYear = new SelectList(db.regyears,"idRegYear","RegYear1",CS.getRegYearId(db));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChgYear([Bind(Include = "idRegYear")]regyear r)
        {
            ccMaster db = new ccMaster(null);
            var y = db.users.Where(u => u.Email == User.Identity.Name).First();
            y.idRegYear = r.idRegYear; // update the chosen registration year
            db.Entry(y).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}