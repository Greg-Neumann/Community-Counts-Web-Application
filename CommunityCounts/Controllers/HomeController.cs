using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;
using Microsoft.AspNet.Identity;
using System.Web;

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
                @ViewBag.userHasNews = CC.userHasNews(db);
            }
            return View();
        }
        public ActionResult ReadMore()
        {
            @ViewBag.userHasNews = false;
            if (Request.IsAuthenticated)
            {
                // already logged in, check for user news
                ccMaster db = new ccMaster(null);
                @ViewBag.userHasNews = CC.userHasNews(db);
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
    }
}