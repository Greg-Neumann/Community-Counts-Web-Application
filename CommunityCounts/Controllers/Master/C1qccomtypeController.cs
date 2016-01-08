using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1qccomtypeController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1qccomtype
        public ActionResult Index()
        {
            return View(db.C1qccomtype.ToList().OrderBy(o=>o.Category));
        }

        // GET: C1qccomtype/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qccomtype c1qccomtype = db.C1qccomtype.Find(id);
            if (c1qccomtype == null)
            {
                return HttpNotFound();
            }
            return View(c1qccomtype);
        }

        // GET: C1qccomtype/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1qccomtype/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idQCComType,Category")] C1qccomtype c1qccomtype)
        {
            if (ModelState.IsValid)
            {
                db.C1qccomtype.Add(c1qccomtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1qccomtype);
        }

        // GET: C1qccomtype/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qccomtype c1qccomtype = db.C1qccomtype.Find(id);
            if (c1qccomtype == null)
            {
                return HttpNotFound();
            }
            return View(c1qccomtype);
        }

        // POST: C1qccomtype/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idQCComType,Category")] C1qccomtype c1qccomtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1qccomtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1qccomtype);
        }

        // GET: C1qccomtype/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1qccomtype c1qccomtype = db.C1qccomtype.Find(id);
            if (c1qccomtype == null)
            {
                return HttpNotFound();
            }
            return View(c1qccomtype);
        }

        // POST: C1qccomtype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1qccomtype c1qccomtype = db.C1qccomtype.Find(id);
            //
            // block deletion if this type in use
            //
            var s = db.C1qccom.Where(c => c.idCategory == id);
            if (s.Any())
            {
                @ViewBag.ResultMessage = "Cannot delete as this comment category is in-use";
                return View();
            }
            db.C1qccomtype.Remove(c1qccomtype);
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
