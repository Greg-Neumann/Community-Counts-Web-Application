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
    [Authorize]
    public class C1qcsrController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1qcsr
        public ActionResult Index()
        {
            var c1qcsr = db.C1qcsr.Include(c => c.C1qcsrtype).Include(c => c.regyear);
            return View(c1qcsr.ToList().OrderByDescending(o=>o.CreateDateTime));
        }

        // GET: C1qcsr/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qcsr c1qcsr = db.C1qcsr.Find(id);
            if (c1qcsr == null)
            {
                return HttpNotFound();
            }
            return View(c1qcsr);
        }

        // GET: C1qcsr/Create
        [Authorize(Roles = "superAdmin,systemAdmin,canManageQuicks")]
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.C1qcsrtype, "idQCSRType", "QCSRType");
            return View();
        }

        // POST: C1qcsr/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idQuickContactsSR,idCategory,comment")] C1qcsr c1qcsr)
        {
            var getRegYear = from a in db.regyears where (a.RegYear1 == "2015") select a; // fix registration year always as 2015 - will need changing!
            c1qcsr.idRegYear = getRegYear.First().idRegYear;
            if (ModelState.IsValid)
            {
                c1qcsr.CreateDateTime = System.DateTime.Now;
                db.C1qcsr.Add(c1qcsr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategory = new SelectList(db.C1qcsrtype, "idQCSRType", "QCSRType", c1qcsr.idCategory);
            return View(c1qcsr);
        }

        // GET: C1qcsr/Edit/5
         [Authorize(Roles = "superAdmin,systemAdmin,canManageQuicks")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qcsr c1qcsr = db.C1qcsr.Find(id);
            if (c1qcsr == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategory = new SelectList(db.C1qcsrtype, "idQCSRType", "QCSRType", c1qcsr.idCategory);
            return View(c1qcsr);
        }

        // POST: C1qcsr/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idQuickContactsSR,idCategory,CreateDateTime,idRegYear,comment")] C1qcsr c1qcsr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1qcsr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.C1qcsrtype, "idQCSRType", "QCSRType", c1qcsr.idCategory);
            return View(c1qcsr);
        }

        // GET: C1qcsr/Delete/5
         [Authorize(Roles = "superAdmin,systemAdmin,canManageQuicks")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qcsr c1qcsr = db.C1qcsr.Find(id);
            if (c1qcsr == null)
            {
                return HttpNotFound();
            }
            return View(c1qcsr);
        }

        // POST: C1qcsr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1qcsr c1qcsr = db.C1qcsr.Find(id);
            db.C1qcsr.Remove(c1qcsr);
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
