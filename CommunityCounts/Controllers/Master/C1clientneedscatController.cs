using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers
{
    public class C1clientneedscatController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1clientneedscat
        public ActionResult Index()
        {
            return View(db.C1clientneedscat.ToList().OrderBy(a=>a.Category));
        }

        // GET: C1clientneedscat/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientneedscat c1clientneedscat = db.C1clientneedscat.Find(id);
            if (c1clientneedscat == null)
            {
                return HttpNotFound();
            }
            return View(c1clientneedscat);
        }

        // GET: C1clientneedscat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1clientneedscat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idNeedsCat,Category")] C1clientneedscat c1clientneedscat)
        {
            if (ModelState.IsValid)
            {
                db.C1clientneedscat.Add(c1clientneedscat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1clientneedscat);
        }

        // GET: C1clientneedscat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientneedscat c1clientneedscat = db.C1clientneedscat.Find(id);
            if (c1clientneedscat == null)
            {
                return HttpNotFound();
            }
            return View(c1clientneedscat);
        }

        // POST: C1clientneedscat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idNeedsCat,Category")] C1clientneedscat c1clientneedscat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1clientneedscat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1clientneedscat);
        }

        // GET: C1clientneedscat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientneedscat c1clientneedscat = db.C1clientneedscat.Find(id);
            if (c1clientneedscat == null)
            {
                return HttpNotFound();
            }
            return View(c1clientneedscat);
        }

        // POST: C1clientneedscat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1clientneedscat c1clientneedscat = db.C1clientneedscat.Find(id);
            if (db.C1clientneedsdetail.Where(a => a.idClientNeedsCat == c1clientneedscat.idNeedsCat).Any())
            {
                ModelState.AddModelError("","Cannot delete this Client Needs Category as it is in-use by Clients");
            }
            if (ModelState.IsValid)
            {
                db.C1clientneedscat.Remove(c1clientneedscat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1clientneedscat);
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
