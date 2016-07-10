using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1resourcetypesController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1resourcetypes
        public ActionResult Index()
        {
            return View(db.C1resourcetypes.ToList().OrderBy(r=>r.ResourceType));
        }

        // GET: C1resourcetypes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1resourcetypes c1resourcetypes = db.C1resourcetypes.Find(id);
            if (c1resourcetypes == null)
            {
                return HttpNotFound();
            }
            return View(c1resourcetypes);
        }

        // GET: C1resourcetypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1resourcetypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResourceType,ResourceTypeDesc")] C1resourcetypes c1resourcetypes)
        {
            if (ModelState.IsValid)
            {
                db.C1resourcetypes.Add(c1resourcetypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1resourcetypes);
        }

        // GET: C1resourcetypes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1resourcetypes c1resourcetypes = db.C1resourcetypes.Find(id);
            if (c1resourcetypes == null)
            {
                return HttpNotFound();
            }
            return View(c1resourcetypes);
        }

        // POST: C1resourcetypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResourceType,ResourceTypeDesc")] C1resourcetypes c1resourcetypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1resourcetypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1resourcetypes);
        }

        // GET: C1resourcetypes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1resourcetypes c1resourcetypes = db.C1resourcetypes.Find(id);
            if (c1resourcetypes == null)
            {
                return HttpNotFound();
            }
            return View(c1resourcetypes);
        }

        // POST: C1resourcetypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            C1resourcetypes c1resourcetypes = db.C1resourcetypes.Find(id);
            //
            // do not let them delete the general bookable room type!
            //
            if (c1resourcetypes.ResourceType == "Room")
            {
                @ViewBag.ResultMessage = "You are not ever allowed to delete the general bookable room type";
                return View(c1resourcetypes);
            }
            db.C1resourcetypes.Remove(c1resourcetypes);
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
