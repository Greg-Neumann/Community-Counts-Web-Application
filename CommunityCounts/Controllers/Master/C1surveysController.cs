using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "systemAdmin,superAdmin,canManageSurveys")]
    public class C1surveysController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1surveys
        public ActionResult Index()
        {
            var c1surveys = db.C1surveys.Include(c => c.C1servicetypes);
            return View(c1surveys.ToList().OrderBy(s=>s.SurveyName));
        }

        // GET: C1surveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1surveys c1surveys = db.C1surveys.Find(id);
            if (c1surveys == null)
            {
                return HttpNotFound();
            }
            return View(c1surveys);
        }

        // GET: C1surveys/Create
        public ActionResult Create()
        {
            ViewBag.idServiceype = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType");
            return View();
        }

        // POST: C1surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idServiceype,SurveyName,SurveyDesc,forAllClients,numTxtQ,numScaQ,active,forAnonymousClients")] C1surveys c1surveys)
        {
            if (c1surveys.numScaQ < 0)
            {
                ModelState.AddModelError("numScaQ", "Zero or a positive number, please(!)");
            }
            else
            {
                ModelState.Remove("numScaQ");
            }
            if (c1surveys.numTxtQ < 0)
            {
                ModelState.AddModelError("numTxtQ", "Zero or a positive number, please(!)");
            }
            else
            {
                ModelState.Remove("numTxtQ");
            }
            if (c1surveys.numScaQ == 0 && c1surveys.numTxtQ == 0)
            {
                ModelState.AddModelError("numTxtQ", "At least one question must be present in a survey");
            }
            if (ModelState.IsValid)
            {
                c1surveys.createdUser=User.Identity.Name;
                c1surveys.createdDateTime=System.DateTime.Now;
                db.C1surveys.Add(c1surveys);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idServiceype = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType", c1surveys.idServiceype);
            return View(c1surveys);
        }

        // GET: C1surveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1surveys c1surveys = db.C1surveys.Find(id);
            if (c1surveys == null)
            {
                return HttpNotFound();
            }
            ViewBag.idServiceype = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType", c1surveys.idServiceype);
            // 
            // permit increasing the number of questions in the survey (always)
            // Only permit reducing the number of questions in the survey if there aren't already results
            //
            ViewBag.gotNumResults = db.C1surressca.Where(s => s.idSurvey == c1surveys.idSurvey).Count() > 0;
            ViewBag.gotTxtResults = db.C1surrestxt.Where(t => t.idSurvey == c1surveys.idSurvey).Count() > 0;
            return View(c1surveys);
        }

        // POST: C1surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSurvey,idServiceype,SurveyName,SurveyDesc,forAllClients,numTxtQ,numScaQ,createdUser,createdDateTime,active,forAnonymousClients")] C1surveys c1surveys)
        {
            if (c1surveys.numScaQ<0)
            {
                ModelState.AddModelError("numScaQ","Zero or a positive number, please(!)");
            }
            else
            {
                ModelState.Remove("numScaQ");
            }
            if (c1surveys.numTxtQ<0)
            {
                ModelState.AddModelError("numTxtQ", "Zero or a positive number, please(!)");
            }
            else
            {
                 ModelState.Remove("numTxtQ");
            }
            if (c1surveys.numScaQ==0 && c1surveys.numTxtQ==0)
            {
                ModelState.AddModelError("numTxtQ", "At least one question must be present in a survey");
            }
          
            if (ModelState.IsValid)
            {
                c1surveys.updatedDateTime = System.DateTime.Now;
                c1surveys.updatedUser = User.Identity.Name;
                db.Entry(c1surveys).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idServiceype = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType", c1surveys.idServiceype);
            return View(c1surveys);
        }

        // GET: C1surveys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1surveys c1surveys = db.C1surveys.Find(id);
            if (c1surveys == null)
            {
                return HttpNotFound();
            }
            return View(c1surveys);
        }

        // POST: C1surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1surveys c1surveys = db.C1surveys.Find(id);
            //
            // any survey results exist for this survey?
            //
            var resScaled = db.C1surressca.Where(s => s.idSurvey == id);
            var resText = db.C1surrestxt.Where(t => t.idSurvey == id);
            if (resScaled.Any() || resText.Any())
            {
                @ViewBag.ResultMessage = "Cannot delete, there are results for this survey";
                return View(c1surveys);
            }
            db.C1surveys.Remove(c1surveys);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
