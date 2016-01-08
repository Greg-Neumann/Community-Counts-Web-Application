using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1servicetypesController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1servicetypes
        public ActionResult Index()
        {
            var c1servicetypes = db.C1servicetypes.OrderBy(c=>c.ServiceType);
            
            return View(c1servicetypes.ToList().OrderBy(s=>s.ServiceType));
        }

        // GET: C1servicetypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1servicetypes c1servicetypes = db.C1servicetypes.Find(id);
            if (c1servicetypes == null)
            {
                return HttpNotFound();
            }
            return View(c1servicetypes);
        }

        // GET: C1servicetypes/Create
        public ActionResult Create()
        {
            ViewBag.FunderCode = new SelectList(db.C1funders, "FunderCode", "FunderName");
            ViewBag.AttendanceType = new SelectList(db.refdatas.Where(a => a.RefCode == "Atte"), "idRefData", "RefCodeDesc");
            return View();
        }

        // POST: C1servicetypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idServiceType,ServiceType,AttendanceType,FunderCode,EmploymentTracked,BiometricTracked")] C1servicetypes c1servicetypes)
        {
            if (ModelState.IsValid)
            {
                db.C1servicetypes.Add(c1servicetypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FunderCode = new SelectList(db.C1funders, "FunderCode", "FunderName", c1servicetypes.FunderCode);
            ViewBag.AttendanceType = new SelectList(db.refdatas, "idRefData", "RefCode", c1servicetypes.AttendanceType);
            return View(c1servicetypes);
        }

        // GET: C1servicetypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1servicetypes c1servicetypes = db.C1servicetypes.Find(id);
            if (c1servicetypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.FunderCode = new SelectList(db.C1funders, "FunderCode", "FunderName", c1servicetypes.FunderCode);
            ViewBag.AttendanceType = new SelectList(db.refdatas.Where(a=>a.RefCode=="Atte"), "idRefData", "RefCodeDesc", c1servicetypes.AttendanceType);
            return View(c1servicetypes);
        }

        // POST: C1servicetypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idServiceType,ServiceType,AttendanceType,FunderCode,EmploymentTracked,BiometricTracked")] C1servicetypes c1servicetypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1servicetypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FunderCode = new SelectList(db.C1funders, "FunderCode", "FunderName", c1servicetypes.FunderCode);
            ViewBag.AttendanceType = new SelectList(db.refdatas, "idRefData", "RefCode", c1servicetypes.AttendanceType);
            return View(c1servicetypes);
        }

        // GET: C1servicetypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1servicetypes c1servicetypes = db.C1servicetypes.Find(id);
            if (c1servicetypes == null)
            {
                return HttpNotFound();
            }
            return View(c1servicetypes);
        }

        // POST: C1servicetypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1servicetypes c1servicetypes = db.C1servicetypes.Find(id);
            db.C1servicetypes.Remove(c1servicetypes);
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
