using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1qcsrtypeController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1qcsrtype
        public ActionResult Index()
        {
            return View(db.C1qcsrtype.ToList().OrderBy(o=>o.QCSRType));
        }

        // GET: C1qcsrtype/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qcsrtype c1qcsrtype = db.C1qcsrtype.Find(id);
            if (c1qcsrtype == null)
            {
                return HttpNotFound();
            }
            return View(c1qcsrtype);
        }

        // GET: C1qcsrtype/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1qcsrtype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idQCSRType,QCSRType,Signpost")] C1qcsrtype c1qcsrtype)
        {
            if (ModelState.IsValid)
            {
                db.C1qcsrtype.Add(c1qcsrtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1qcsrtype);
        }

        // GET: C1qcsrtype/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qcsrtype c1qcsrtype = db.C1qcsrtype.Find(id);
            if (c1qcsrtype == null)
            {
                return HttpNotFound();
            }
            return View(c1qcsrtype);
        }

        // POST: C1qcsrtype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idQCSRType,QCSRType,Signpost")] C1qcsrtype c1qcsrtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1qcsrtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1qcsrtype);
        }

        // GET: C1qcsrtype/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qcsrtype c1qcsrtype = db.C1qcsrtype.Find(id);
            if (c1qcsrtype == null)
            {
                return HttpNotFound();
            }
            return View(c1qcsrtype);
        }

        // POST: C1qcsrtype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1qcsrtype c1qcsrtype = db.C1qcsrtype.Find(id);
            //
            // block deletion if in use
            //
            var s = db.C1qcsr.Where(o => o.idCategory == id);
            if (s.Any())
            {
                @ViewBag.ResultMessage = "Cannot delete as this signpost/referral category is in-use";
            }
            db.C1qcsrtype.Remove(c1qcsrtype);
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
