using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "systemAdmin")]
    public class refdatasController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: refdatas
        public ActionResult Index()
        {
            var refdatas = db.refdatas.Include(r => r.refdatatype).OrderBy(r=>r.refdatatype.TypeName).ThenBy(r=>r.RefCodeValue);
            return View(refdatas.ToList());
        }

        // GET: refdatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            refdata refdata = db.refdatas.Find(id);
            if (refdata == null)
            {
                return HttpNotFound();
            }
            return View(refdata);
        }

        // GET: refdatas/Create
        public ActionResult Create()
        {
            ViewBag.RefCode = new SelectList(db.refdatatypes, "TypeCode", "TypeName");
            return View();
        }

        // POST: refdatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRefData,RefCode,RefCodeValue,RefCodeDesc")] refdata refdata)
        {
            if (ModelState.IsValid)
            {
                db.refdatas.Add(refdata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RefCode = new SelectList(db.refdatatypes, "TypeCode", "TypeName", refdata.RefCode);
            return View(refdata);
        }

        // GET: refdatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            refdata refdata = db.refdatas.Find(id);
            if (refdata == null)
            {
                return HttpNotFound();
            }
            ViewBag.RefCode = new SelectList(db.refdatatypes, "TypeCode", "TypeName", refdata.RefCode);
            return View(refdata);
        }

        // POST: refdatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRefData,RefCode,RefCodeValue,RefCodeDesc")] refdata refdata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(refdata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RefCode = new SelectList(db.refdatatypes, "TypeCode", "TypeName", refdata.RefCode);
            return View(refdata);
        }

        // GET: refdatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            refdata refdata = db.refdatas.Find(id);
            if (refdata == null)
            {
                return HttpNotFound();
            }
            return View(refdata);
        }

        // POST: refdatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            refdata refdata = db.refdatas.Find(id);
            db.refdatas.Remove(refdata);
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
