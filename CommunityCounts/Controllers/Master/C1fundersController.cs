using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1fundersController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1funders
        public ActionResult Index()
        {
            return View(db.C1funders.ToList().OrderBy(f=>f.FunderCode));
        }

        // GET: C1funders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1funders c1funders = db.C1funders.Find(id);
            if (c1funders == null)
            {
                return HttpNotFound();
            }
            return View(c1funders);
        }

        // GET: C1funders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: C1funders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FunderCode,FunderName")] C1funders c1funders)
        {
            if (ModelState.IsValid)
            {
                db.C1funders.Add(c1funders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(c1funders);
        }

        // GET: C1funders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1funders c1funders = db.C1funders.Find(id);
            if (c1funders == null)
            {
                return HttpNotFound();
            }
            return View(c1funders);
        }

        // POST: C1funders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FunderCode,FunderName")] C1funders c1funders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1funders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c1funders);
        }

        // GET: C1funders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1funders c1funders = db.C1funders.Find(id);
            if (c1funders == null)
            {
                return HttpNotFound();
            }
            return View(c1funders);
        }

        // POST: C1funders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            C1funders c1funders = db.C1funders.Find(id);
            //
            // check for no ServiceTypes using this FunderCode
            //
            var serviceTypes = from s in db.C1servicetypes where (s.FunderCode == id) select s;
            var invalid = serviceTypes.Any();   
            if (invalid)
            {
                ViewBag.ResultMessage = "Please delete all Activities for this Funder before deleting Funder";
                return View(c1funders);
            }
            db.C1funders.Remove(c1funders);
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
