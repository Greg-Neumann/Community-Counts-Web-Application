using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers.Master
{
    public class ClientCaseWorkDetailController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: ClientCaseWorkDetail
        public ActionResult Index(int id) // id passed is idClientCaseDetail
        {
            var c1clientcaseservicedetail = db.C1clientcaseservicedetail.Where(a => a.idClientCaseDetail == id).OrderByDescending(x=>x.CaseServiceDate);
            List<clientcaseWorkDetailList> detailList = new List<clientcaseWorkDetailList> { };
            char[] splitChar;
            string staffName;
            foreach (var i in c1clientcaseservicedetail.ToList())
            {
                staffName = db.users.Find(i.CaseServiceStaffid).Email;
                splitChar = "@".ToCharArray();
                var logonParts = staffName.Split(splitChar[0]); // Email domain will be in 2nd part (index=1)
                staffName = logonParts[0]; // gets username from email address
                //
                TimeSpan t = i.CaseServiceTime; // convert ticks to timespan
                int totalSeconds = t.Seconds;
                int totalMinutes = t.Minutes;
                int totalDays = t.Days;
                int totalHours = t.Hours + totalDays * 24;
                string formattedTime = totalHours.ToString("#,###") + ":" + totalMinutes.ToString("00");
                detailList.Add(new clientcaseWorkDetailList
                {
                    CaseServiceDate = i.CaseServiceDate,
                    CaseServiceEditDate = i.CaseServiceEditDate,
                    CaseServiceNotes = i.CaseServiceNotes,
                    CaseServiceTime = formattedTime,
                    name = staffName,
                    idClientCaseServiceDetail = i.idClientCaseServiceDetail
                });
            }
            var ClientCaseHeader = db.C1clientcaseservice.Find(id);
            int idClientCaseHeader = ClientCaseHeader.idClientCaseHeader;          
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            ViewBag.idClientCaseDetail = id;
            int ServiceTypesID = ClientCaseHeader.ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(ServiceTypesID).ServiceType;

            return View(detailList);
        }

        // GET: ClientCaseWorkDetail/Create
        public ActionResult Create(int id) // id is idClientCaseDetail
        {
            var c1clientcaseservicedetail = db.C1clientcaseservicedetail.Where(a => a.idClientCaseDetail == id);
            var ClientCaseHeader = db.C1clientcaseservice.Find(id);
            int idClientCaseHeader = ClientCaseHeader.idClientCaseHeader;
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            ViewBag.idClientCaseDetail = id;
            int ServiceTypesID = ClientCaseHeader.ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(ServiceTypesID).ServiceType;
            return View();
        }

        // POST: ClientCaseWorkDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idClientCaseDetail,CaseServiceTime,CaseServiceNotes")] C1clientcaseservicedetail c1clientcaseservicedetail)
        {
            if (ModelState.IsValid)
            {
                c1clientcaseservicedetail.CaseServiceDate = DateTime.Now;
                c1clientcaseservicedetail.CaseServiceEditDate = DateTime.Now;
                c1clientcaseservicedetail.CaseServiceStaffid = db.users.Where(u => u.Email == HttpContext.User.Identity.Name).First().idUsers;
                db.C1clientcaseservicedetail.Add(c1clientcaseservicedetail);
                db.SaveChanges();
                return RedirectToAction("Index",new { id = c1clientcaseservicedetail.idClientCaseDetail });
            }
            var ClientCaseHeader = db.C1clientcaseservice.Find(c1clientcaseservicedetail.idClientCaseDetail);
            int idClientCaseHeader = ClientCaseHeader.idClientCaseHeader;
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            ViewBag.idClientCaseDetail = c1clientcaseservicedetail.idClientCaseDetail;
            int ServiceTypesID = ClientCaseHeader.ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(ServiceTypesID).ServiceType;
            return View(c1clientcaseservicedetail);
        }

        // GET: ClientCaseWorkDetail/Edit/5
        public ActionResult Edit(int? id)  // key passed is idClientCaseServiceDetail
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientcaseservicedetail c1clientcaseservicedetail = db.C1clientcaseservicedetail.Find(id);
            if (c1clientcaseservicedetail == null)
            {
                return HttpNotFound();
            }
            
            var ClientCaseHeader = db.C1clientcaseservice.Find(c1clientcaseservicedetail.idClientCaseDetail);
            int idClientCaseHeader = ClientCaseHeader.idClientCaseHeader;
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            ViewBag.idClientCaseDetail = c1clientcaseservicedetail.idClientCaseDetail;
            int ServiceTypesID = ClientCaseHeader.ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(ServiceTypesID).ServiceType;
            return View(c1clientcaseservicedetail);
        }

        // POST: ClientCaseWorkDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idClientCaseServiceDetail,CaseServiceDate,idClientCaseDetail,CaseServiceTime,CaseServiceNotes")] C1clientcaseservicedetail c1clientcaseservicedetail)
        {
            if (ModelState.IsValid)
            {
                c1clientcaseservicedetail.CaseServiceEditDate = DateTime.Now;
                c1clientcaseservicedetail.CaseServiceStaffid = db.users.Where(u => u.Email == HttpContext.User.Identity.Name).First().idUsers;
                db.Entry(c1clientcaseservicedetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = c1clientcaseservicedetail.idClientCaseDetail });
            }
            var ClientCaseHeader = db.C1clientcaseservice.Find(c1clientcaseservicedetail.idClientCaseDetail);
            int idClientCaseHeader = ClientCaseHeader.idClientCaseHeader;
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            ViewBag.idClientCaseDetail = c1clientcaseservicedetail.idClientCaseDetail;
            int ServiceTypesID = ClientCaseHeader.ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(ServiceTypesID).ServiceType;
            return View(c1clientcaseservicedetail);
        }

        // GET: ClientCaseWorkDetail/Delete/5
        [Authorize(Roles = "canDeleteCaseWork,superAdmin,systemAdmin")]
        public ActionResult Delete(int? id) // id is idClientCaseServiceDetail
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientcaseservicedetail c1clientcaseservicedetail = db.C1clientcaseservicedetail.Find(id);
            if (c1clientcaseservicedetail == null)
            {
                return HttpNotFound();
            }
            var ClientCaseHeader = db.C1clientcaseservice.Find(c1clientcaseservicedetail.idClientCaseDetail);
            int idClientCaseHeader = ClientCaseHeader.idClientCaseHeader;
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            ViewBag.idClientCaseDetail = c1clientcaseservicedetail.idClientCaseDetail;
            int ServiceTypesID = ClientCaseHeader.ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(ServiceTypesID).ServiceType;
            return View(c1clientcaseservicedetail);
        }

        // POST: ClientCaseWorkDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1clientcaseservicedetail c1clientcaseservicedetail = db.C1clientcaseservicedetail.Find(id);
            db.C1clientcaseservicedetail.Remove(c1clientcaseservicedetail);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = c1clientcaseservicedetail.idClientCaseDetail });
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
