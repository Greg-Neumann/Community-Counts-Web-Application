using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;



namespace CommunityCounts.Controllers
{
    [Authorize(Roles = "superAdmin,systemAdmin,canManageCase")]
    public class ClientCaseWorkController : Controller
    {
        private ccMaster db = new ccMaster(null); // open the database
        // GET: ClientCaseWork
        public ActionResult CaseWorkSummary(int? id)  // id passed is idClient
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var getClientName = from c in db.C1client where (c.idClient == id) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = id;
            var ClientCaseHeader = db.C1clientcaseheader.Where(a => a.idClient == id); // Get any? exisiting casework records for this idClient
            //
            List<clientCaseWorkList> caseList = new List<clientCaseWorkList>();
            TimeSpan totalTime = new TimeSpan(0, 0, 0); // hours minutes seconds integers
            string serviceName,staffName;
            //char[] splitChar;
            //
            if (ClientCaseHeader.Any())
            {

                ViewBag.startCaseWorking = false;
                var idClientCaseHeader = ClientCaseHeader.First().idClientCaseHeader;
                ViewBag.idClientCaseHeader = idClientCaseHeader;
                var caseService = db.C1clientcaseservice.Where(a => a.idClientCaseHeader==idClientCaseHeader);
                foreach (var serviceRec in caseService.ToList())
                {
                    int idClientCaseDetail = serviceRec.idClientCaseDetail; // grab the key of all children for this Service
                    //
                    // Loop over all detail recs to add up time - cannot do this by aggregate function
                    //
                    var caseservicedetails = db.C1clientcaseservicedetail.Where(a => a.idClientCaseDetail == idClientCaseDetail).OrderByDescending(a => a.CaseServiceDate);
                    long totalTimeToDate = 0;
                    int? lastStaff = 0;
                    int numDetailRecs = 0;
                    foreach (var details in caseservicedetails)
                    {
                        totalTimeToDate = totalTimeToDate + details.CaseServiceTime.Ticks;
                        lastStaff = details.CaseServiceStaffid;
                        numDetailRecs++;
                    }
                    if (lastStaff!=0)
                    {
                        staffName = db.users.Find(lastStaff).Email;
                        //splitChar = "@".ToCharArray();
                        //var logonParts = staffName.Split(splitChar[0]); // Email domain will be in 2nd part (index=1)
                        //staffName = logonParts[0]; // gets username from email address
                    }
                    else
                    {
                        staffName = null;
                    }
                    totalTime = new TimeSpan(new DateTime(totalTimeToDate).Hour, new DateTime(totalTimeToDate).Minute, new DateTime(totalTimeToDate).Second);
                    serviceName = db.C1servicetypes.Find(serviceRec.ServiceTypesid).ServiceType; // service name
                    //
                    caseList.Add(new clientCaseWorkList
                    {
                        numDetailRecs = numDetailRecs,
                        staffName = staffName,
                        totalTimeToDate = totalTime,
                        ServiceName = serviceName,
                        isCaseWorked = false,
                        idClientCaseDetail = serviceRec.idClientCaseDetail
                    });
                }
            }
            else
            {
                ViewBag.startCaseWorking = true;
            }
            var sortedList = caseList.OrderBy(a => a.ServiceName);
            return View(caseList);
        }
        //Get ClientCaseWork/CreateCaseService/id
        public ActionResult CreateCaseService (int idClientCaseHeader)
        {
            List<clientCaseWorkSelList> selList = new List<clientCaseWorkSelList>();
            //
            // Build Activity selection list from any previous casework records AND any new activity enrollments (since casework started)
            //
            int idClient = db.C1clientcaseheader.Find(idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            var existingActivities = from a in db.C1service where (a.idClient == idClient & ((a.EndedDate == null) || (a.EndedDate > System.DateTime.Now))) select new { a.idServiceType, a.StartedDate };
            //
            foreach (var activityRec in existingActivities.ToList())
            {
                string serviceName = db.C1servicetypes.Find(activityRec.idServiceType).ServiceType;
                bool isCaseWorked = db.C1clientcaseservice.Where(a => a.ServiceTypesid == activityRec.idServiceType).Any();
                selList.Add(new clientCaseWorkSelList
                {
                    idClientCaseHeader = idClientCaseHeader,
                    idServiceType = activityRec.idServiceType,
                    startedDate = activityRec.StartedDate,
                    ServiceName = serviceName,
                    isCaseWorked = isCaseWorked
                });
            }
            return View(selList);
        }
        // POST: ClientCaseWork/CreateCaseService/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCaseService([Bind(Include = "isCaseWorked,idServiceType,idClientCaseHeader")] List<clientCaseWorkSelList> selList)
        {
            //
            if (selList.Where(a=>a.isCaseWorked==true).Count() == 0)
            {
                ModelState.AddModelError("", "Please tick at least one Activity to be Caseworked or press 'Back to List' to cancel");
            }
            //
            // check that records previous marked for Caseworking are now not-unselected (cannot use readonly attr on <input> to stop this)
            //
            foreach (var i in selList)
            {
                var rec = db.C1clientcaseservice.Where(a => a.idClientCaseHeader == i.idClientCaseHeader).Where(a => a.ServiceTypesid == i.idServiceType);
                if (rec.Any())
                {
                    if (!i.isCaseWorked)
                    {
                        ModelState.AddModelError("", "You cannot untick already marked records as Casework details exist. Use the Delete action on the Client Casework Summary Screen to remove all CaseWorking activity for this client/Activity after pressing the 'Back to List' option.");
                    }
                }
            }
            int idClient = db.C1clientcaseheader.Find(selList[0].idClientCaseHeader).idClient;
            if (ModelState.IsValid)
            {   
                foreach (var i in selList)
                {
                    //
                    // Does this casework detail record already exist? If not,add it.
                    //
                    if (!db.C1clientcaseservice.Where(a => a.idClientCaseHeader == i.idClientCaseHeader).Where(a => a.ServiceTypesid == i.idServiceType).Any())
                    {
                        if (i.isCaseWorked)
                        {
                            db.C1clientcaseservice.Add(new C1clientcaseservice
                            {
                                idClientCaseHeader = i.idClientCaseHeader,
                                ServiceTypesid = i.idServiceType,
                                isCaseWorked = i.isCaseWorked
                            });
                        }
                    }

                }
                db.SaveChanges();
                return RedirectToAction("CaseWorkSummary", new { id = idClient });
            }
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.idClient = idClient;
            foreach (var i in selList)
            {
                i.ServiceName= db.C1servicetypes.Find(i.idServiceType).ServiceType;
            }
            return View(selList);
        }
        public ActionResult StartCaseService(int idClient)
        {
            //
            // set up caseworking tables for idClient
            //
          
            if (db.C1clientcaseheader.Where(a=>a.idClient==idClient).Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            db.C1clientcaseheader.Add(new C1clientcaseheader
            {
                idClient = (int)idClient
            });
            db.SaveChanges();
            int idClientCaseHeader = db.C1clientcaseheader.Where(a => a.idClient == idClient).First().idClientCaseHeader;
            return RedirectToAction("CreateCaseService", new { idClientCaseHeader = idClientCaseHeader });
        }
        [Authorize(Roles = "canDeleteCaseWork,superAdmin,systemAdmin")]
        public ActionResult DeleteCaseWork(int? id) // id is idclientcasedetail

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1clientcaseservice c1clientcaseservice = db.C1clientcaseservice.Find(id);
            if (c1clientcaseservice == null)
            {
                return HttpNotFound();
            }
            int idClient = db.C1clientcaseheader.Find(c1clientcaseservice.idClientCaseHeader).idClient;
            var getClientName = from c in db.C1client where (c.idClient == idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            int idServiceType = db.C1clientcaseservice.Find(id).ServiceTypesid;
            ViewBag.ActivityName = db.C1servicetypes.Find(idServiceType).ServiceType;
            ViewBag.idClient = idClient;
            return View();
        }
        [HttpPost, ActionName("DeleteCaseWork")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canDeleteCaseWork,superAdmin,systemAdmin")]
        public ActionResult DeleteCaseWorkConfirmed(int id)
        {
            C1clientcaseservice c1clientcaseservice = db.C1clientcaseservice.Find(id);
            db.C1clientcaseservice.Remove(c1clientcaseservice);
            db.SaveChanges();
            int idClient = db.C1clientcaseheader.Find(c1clientcaseservice.idClientCaseHeader).idClient;
            return RedirectToAction("CaseWorkSummary/"+idClient.ToString());
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