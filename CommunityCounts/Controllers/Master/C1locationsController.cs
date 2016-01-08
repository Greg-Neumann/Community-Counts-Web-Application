using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles="superAdmin,systemAdmin")]
    public class C1locationsController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1locations
        public ActionResult Index()
        {
            return View(db.C1locations.ToList().OrderBy(l=>l.LocationCode));
        }

        // GET: C1locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1locations c1locations = db.C1locations.Find(id);
            if (c1locations == null)
            {
                return HttpNotFound();
            }
            return View(c1locations);
        }

        // GET: C1locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idLocations,LocationCode,LocationName")] C1locations c1locations)
        {
            if (ModelState.IsValid)
            {
                db.C1locations.Add(c1locations);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1locations);
        }

        // GET: C1locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1locations c1locations = db.C1locations.Find(id);
            if (c1locations == null)
            {
                return HttpNotFound();
            }
            return View(c1locations);
        }

        // POST: C1locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idLocations,LocationCode,LocationName")] C1locations c1locations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1locations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1locations);
        }

        // GET: C1locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1locations c1locations = db.C1locations.Find(id);
            if (c1locations == null)
            {
                return HttpNotFound();
            }
            return View(c1locations);
        }

        // POST: C1locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1locations c1locations = db.C1locations.Find(id);
            db.C1locations.Remove(c1locations);
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
