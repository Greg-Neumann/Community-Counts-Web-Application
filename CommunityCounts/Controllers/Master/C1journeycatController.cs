using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1journeycatController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1journeycat
        public ActionResult Index()
        {
            return View(db.C1journeycat.ToList().Where(j=>j.idJourneyCat>0).OrderBy(j=>j.CatName));
        }

        // GET: C1journeycat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1journeycat c1journeycat = db.C1journeycat.Find(id);
            if (c1journeycat == null)
            {
                return HttpNotFound();
            }
            return View(c1journeycat);
        }

        // GET: C1journeycat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1journeycat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idJourneyCat,CatName,CatDesc")] C1journeycat c1journeycat)
        {
            if (ModelState.IsValid)
            {
                db.C1journeycat.Add(c1journeycat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1journeycat);
        }

        // GET: C1journeycat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1journeycat c1journeycat = db.C1journeycat.Find(id);
            if (c1journeycat == null)
            {
                return HttpNotFound();
            }
            return View(c1journeycat);
        }

        // POST: C1journeycat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idJourneyCat,CatName,CatDesc")] C1journeycat c1journeycat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1journeycat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1journeycat);
        }

        // GET: C1journeycat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1journeycat c1journeycat = db.C1journeycat.Find(id);
            if (c1journeycat == null)
            {
                return HttpNotFound();
            }
            return View(c1journeycat);
        }

        // POST: C1journeycat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1journeycat c1journeycat = db.C1journeycat.Find(id);
            //
            // in-use?
            //
            var inuse = from j in db.C1service.Where(j => j.JourneyedidCategory == id) select j;
            if (inuse.Any())
            {
                ModelState.AddModelError("", "This Journey Category cannot be deleted as it is in-use with Clients Activities already assigned to this.");
                return View();
            }
            db.C1journeycat.Remove(c1journeycat);
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
