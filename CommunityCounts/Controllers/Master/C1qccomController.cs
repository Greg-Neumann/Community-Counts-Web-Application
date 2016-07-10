using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers
{
    [Authorize]
    public class C1qccomController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1qccom
        public ActionResult Index()
        {
            int idYear = CS.getRegYearId(db);
            var c1qccom = db.C1qccom.Include(c => c.C1qccomtype).Include(c => c.regyear).OrderByDescending(o=>o.CreateDateTime).Where(c=>c.idRegYear==idYear);
            @ViewBag.RegYear = CS.getRegYear(db, false);
            return View(c1qccom.ToList());
        }

        // GET: C1qccom/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qccom c1qccom = db.C1qccom.Find(id);
            if (c1qccom == null)
            {
                return HttpNotFound();
            }
            return View(c1qccom);
        }

        // GET: C1qccom/Create
         [Authorize(Roles = "superAdmin,systemAdmin,canManageQuicks")]
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.C1qccomtype, "idQCComType", "Category");
            return View();
        }

        // POST: C1qccom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idQuickContactsComment,Comment,idCategory")] C1qccom c1qccom)
        {
            c1qccom.idRegYear = CS.getRegYearId(db);
            if (ModelState.IsValid)
            {
                c1qccom.CreateDateTime = System.DateTime.Now;
                db.C1qccom.Add(c1qccom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategory = new SelectList(db.C1qccomtype, "idQCComType", "Category", c1qccom.idCategory);
            return View(c1qccom);
        }

        // GET: C1qccom/Edit/5
         [Authorize(Roles = "superAdmin,systemAdmin,canManageQuicks")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qccom c1qccom = db.C1qccom.Find(id);
            if (c1qccom == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategory = new SelectList(db.C1qccomtype, "idQCComType", "Category", c1qccom.idCategory);
            return View(c1qccom);
        }

        // POST: C1qccom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idQuickContactsComment,Comment,idCategory,CreateDateTime,idRegYear")] C1qccom c1qccom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1qccom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.C1qccomtype, "idQCComType", "Category", c1qccom.idCategory);
            return View(c1qccom);
        }

        // GET: C1qccom/Delete/5
         [Authorize(Roles = "superAdmin,systemAdmin,canManageQuicks")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qccom c1qccom = db.C1qccom.Find(id);
            if (c1qccom == null)
            {
                return HttpNotFound();
            }
            return View(c1qccom);
        }

        // POST: C1qccom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1qccom c1qccom = db.C1qccom.Find(id);
            db.C1qccom.Remove(c1qccom);
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
