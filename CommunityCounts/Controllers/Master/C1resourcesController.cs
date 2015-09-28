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
    public class C1resourcesController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1resources
        public ActionResult Index()
        {
            var c1resources = db.C1resources.Include(c => c.C1locations).Include(c => c.C1resourcetypes);
            return View(c1resources.ToList().OrderBy(r=>r.C1locations.LocationCode).ThenBy(r=>r.ResourceType).ThenBy(r=>r.ResourceName));
        }

        // GET: C1resources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1resources c1resources = db.C1resources.Find(id);
            if (c1resources == null)
            {
                return HttpNotFound();
            }
            return View(c1resources);
        }

        // GET: C1resources/Create
        public ActionResult Create()
        {
            ViewBag.idLocation = new SelectList(db.C1locations, "idLocations", "LocationCode");
            ViewBag.ResourceType = new SelectList(db.C1resourcetypes, "ResourceType", "ResourceTypeDesc");
            return View();
        }

        // POST: C1resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idResource,idLocation,ResourceName,ResourceType")] C1resources c1resources)
        {
            if (ModelState.IsValid)
            {
                db.C1resources.Add(c1resources);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idLocation = new SelectList(db.C1locations, "idLocations", "LocationCode", c1resources.idLocation);
            ViewBag.ResourceType = new SelectList(db.C1resourcetypes, "ResourceType", "ResourceTypeDesc", c1resources.ResourceType);
            return View(c1resources);
        }

        // GET: C1resources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1resources c1resources = db.C1resources.Find(id);
            if (c1resources == null)
            {
                return HttpNotFound();
            }
            ViewBag.idLocation = new SelectList(db.C1locations, "idLocations", "LocationCode", c1resources.idLocation);
            ViewBag.ResourceType = new SelectList(db.C1resourcetypes, "ResourceType", "ResourceTypeDesc", c1resources.ResourceType);
            return View(c1resources);
        }

        // POST: C1resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idResource,idLocation,ResourceName,ResourceType")] C1resources c1resources)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1resources).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idLocation = new SelectList(db.C1locations, "idLocations", "LocationCode", c1resources.idLocation);
            ViewBag.ResourceType = new SelectList(db.C1resourcetypes, "ResourceType", "ResourceTypeDesc", c1resources.ResourceType);
            return View(c1resources);
        }

        // GET: C1resources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1resources c1resources = db.C1resources.Find(id);
            if (c1resources == null)
            {
                return HttpNotFound();
            }
            return View(c1resources);
        }

        // POST: C1resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1resources c1resources = db.C1resources.Find(id);
            db.C1resources.Remove(c1resources);
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
