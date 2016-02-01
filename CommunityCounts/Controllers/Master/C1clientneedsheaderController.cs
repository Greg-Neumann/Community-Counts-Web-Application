using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers
{
    public class C1clientneedsheaderController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1clientneedsheader
        public ActionResult Index(int id) // list all needs records for this client id
        {
            var c1clientneedsheader = db.C1clientneedsheader.Include(c => c.C1client).Where(c=>c.idClient== id).OrderByDescending(c=>c.ClientNeedsDate);
            List<clientNeedsList> nl = new List<clientNeedsList>();
            foreach (var i in c1clientneedsheader.ToList())
            {
                var numOfNeeds = db.C1clientneedsdetail.Where(a => a.idClientNeeds == i.idClientNeeds).Where(a=>a.hasThisNeed==true).Count();
                nl.Add(new clientNeedsList
                {
                    idClientNeeds = i.idClientNeeds,
                    idClient = i.idClient,
                    ClientNeedsDate = i.ClientNeedsDate,
                    ClientNeedsNotes = i.ClientNeedsNotes,
                    numOfNeeds = numOfNeeds
                });
            }
            var getClientName = from c in db.C1client where (c.idClient == id) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = id;
            return View(nl);
        }
        
        // GET: C1clientneedsheader/Create
        public ActionResult Create(int id)
        {
            ViewBag.idClient = id;
            var getClientName = from c in db.C1client where (c.idClient == id) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            return View();
        }

        // POST: C1clientneedsheader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idClient,ClientNeedsDate,ClientNeedsNotes")] C1clientneedsheader c1clientneedsheader)
        {
            
            //
            // Needs date must be within the registration year
            //
            var idRegYear = CS.getRegYearId(db);
            var regyear = db.regyears.Find(idRegYear);
            var idclient = c1clientneedsheader.idClient;
            var needDate = c1clientneedsheader.ClientNeedsDate;
            if ((c1clientneedsheader.ClientNeedsDate > regyear.EndDate) || (c1clientneedsheader.ClientNeedsDate < regyear.StartDate))
            {
                ModelState.AddModelError("ClientNeedsDate", "This Needs date is not within the current registration year (" + regyear.StartDate.ToShortDateString() + "-" + regyear.EndDate.ToShortDateString() + ")");
            }
            if (db.C1clientneedsheader.Where(c => c.idClient == idclient).Where(c => c.ClientNeedsDate == needDate).Any())
            {
                ModelState.AddModelError("ClientNeedsDate", "There is already a Need entered for this client on this date; Edit that one instead");
            }
            if (ModelState.IsValid)
            {
                db.C1clientneedsheader.Add(c1clientneedsheader);
                db.SaveChanges();
                c1clientneedsheader = db.C1clientneedsheader.Where(c => c.idClient == idclient).Where(c => c.ClientNeedsDate == needDate).First();
                //
                // now go to manage the detailed Needs records for this clientid
                //
                return RedirectToAction("MarkNeeds",new { idclient = c1clientneedsheader.idClient, idclientneeds = c1clientneedsheader.idClientNeeds }); // pass keys of idClient & idClientNeeds
            }

            ViewBag.idClient = idclient;
            var getClientName = from c in db.C1client where (c.idClient == idclient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            return View(c1clientneedsheader);
        }

        // GET: C1clientneedsheader/Notes/5
        public ActionResult Notes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientneedsheader c1clientneedsheader = db.C1clientneedsheader.Find(id);
            if (c1clientneedsheader == null)
            {
                return HttpNotFound();
            }
            ViewBag.idClient = c1clientneedsheader.idClient;
            var getClientName = from c in db.C1client where (c.idClient == c1clientneedsheader.idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.NeedsDate = c1clientneedsheader.ClientNeedsDate.ToLongDateString();
            ViewBag.idClient = c1clientneedsheader.idClient;
            ViewBag.Needs = c1clientneedsheader.ClientNeedsNotes;
            return View(c1clientneedsheader);
        }

        // POST: C1clientneedsheader/Notes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Notes([Bind(Include = "idClientNeeds,idClient,ClientNeedsDate,ClientNeedsNotes")] C1clientneedsheader c1clientneedsheader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1clientneedsheader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index/"+c1clientneedsheader.idClient.ToString());
            }
            ViewBag.idClient = c1clientneedsheader.idClient;
            var getClientName = from c in db.C1client where (c.idClient == c1clientneedsheader.idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.NeedsDate = c1clientneedsheader.ClientNeedsDate.ToLongDateString();
            ViewBag.idClient = c1clientneedsheader.idClient;
            ViewBag.Needs = c1clientneedsheader.ClientNeedsNotes;
            return View(c1clientneedsheader);
        }

        // GET: C1clientneedsheader/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientneedsheader c1clientneedsheader = db.C1clientneedsheader.Find(id);
            if (c1clientneedsheader == null)
            {
                return HttpNotFound();
            }
            var getClientName = from c in db.C1client where (c.idClient == c1clientneedsheader.idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.NeedsDate = c1clientneedsheader.ClientNeedsDate.ToLongDateString();
            ViewBag.idClient = c1clientneedsheader.idClient;
            ViewBag.Needs = c1clientneedsheader.ClientNeedsNotes;
            var c1clientneedsdetail = db.C1clientneedsdetail.Where(a => a.idClientNeeds == id);
            return View(c1clientneedsdetail.ToList());
        }

        // POST: C1clientneedsheader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1clientneedsheader c1clientneedsheader = db.C1clientneedsheader.Find(id);
            db.C1clientneedsheader.Remove(c1clientneedsheader);                         // delete header, cascade deletes details
            db.SaveChanges();
            return RedirectToAction("Index/"+c1clientneedsheader.idClient.ToString());
        }
        // GET: C1clientneedsheader/MarkNeeds/idClient/idClientNeeds
        public ActionResult MarkNeeds(int idClient, int? idClientNeeds)
        {
            //
            // if needs for this needs id already present, display them. Merge in any new needs Categories if found
            // else simply populate a needs table using the defined categories and set hasThisNeeds to false
            //
            var needsCategories = db.C1clientneedscat;
            foreach (var need in needsCategories.ToList())
            {
                if (db.C1clientneedsdetail.Where(c => c.idClientNeeds == idClientNeeds).Where(c => c.idClientNeedsCat == need.idNeedsCat).Any())
                {

                }
                else
                {
                    db.C1clientneedsdetail.Add(new C1clientneedsdetail
                    {
                        idClientNeedsCat = need.idNeedsCat,
                        idClientNeeds = (int)idClientNeeds,
                        hasThisNeed = false
                    });
                }
            }
            db.SaveChanges();
            var needslist = db.C1clientneedsdetail.Where(c => c.idClientNeeds == idClientNeeds).OrderBy(a=>a.C1clientneedscat.Category);
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            C1clientneedsheader c1clientneedsheader = db.C1clientneedsheader.Find(needslist.First().idClientNeeds);
            ViewBag.NeedsDate = c1clientneedsheader.ClientNeedsDate.ToLongDateString();
            ViewBag.idClient = c1clientneedsheader.idClient;
            ViewBag.Needs = c1clientneedsheader.ClientNeedsNotes;
            return View(needslist);
        }
        // POST: C1clientneedsheader/MarkNeeds
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkNeeds([Bind(Include = "hasThisNeed,idClientNeeds,idClientNeedsCat")] List<C1clientneedsdetail> details)
        {

            int idClientNeeds = details.First().idClientNeeds;
            C1clientneedsheader c1clientneedsheader = db.C1clientneedsheader.Find(idClientNeeds);
            //
            // database methodology is to delete all ClientNeedsDetails for this idClientneeds and re-add the POST values held in List details
            //
            var removeList = db.C1clientneedsdetail.Where(a => a.idClientNeeds == idClientNeeds);
            db.C1clientneedsdetail.RemoveRange(removeList);
            foreach (var addItem in details)
            {
                db.C1clientneedsdetail.Add(new C1clientneedsdetail
                {
                    idClientNeedsCat = addItem.idClientNeedsCat,
                    idClientNeeds = addItem.idClientNeeds,
                    hasThisNeed = addItem.hasThisNeed
                });
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { id = c1clientneedsheader.idClient });
        }

       
        public ActionResult DetailsNeeds(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientneedsheader c1clientneedsheader = db.C1clientneedsheader.Find(id);
            if (c1clientneedsheader == null)
            {
                return HttpNotFound();
            }
            var getClientName = from c in db.C1client where (c.idClient == c1clientneedsheader.idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = c1clientneedsheader.idClient;
            ViewBag.idClientNeeds = c1clientneedsheader.idClientNeeds;
            ViewBag.NeedsDate = c1clientneedsheader.ClientNeedsDate.ToLongDateString();
            ViewBag.Needs = c1clientneedsheader.ClientNeedsNotes;
            var c1clientneedsdetail = db.C1clientneedsdetail.Where(a => a.idClientNeeds == id);
            return View(c1clientneedsdetail.ToList());
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
