using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers
{
    [Authorize]
    public class C1serviceController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1service for idClient
        public ActionResult Index(int id)
        {
            List<activityList> servicesList = new List<activityList>();
            string JrnyCat;
            //
            // start the list (for display) by simply browsing all the non-journeyed services for this client
            //
            var services = db.C1service.Where(c => c.idClient == id).Where(c => c.JourneyedServices==false).OrderBy(c=>c.C1servicetypes.ServiceType).ThenBy(c=>c.CreateDate); // if journey id is null, then this is a simple service for the client (not journeyed)
            foreach (var item in services.ToList())
            { 
                if (item.JourneyedidCategory == 0) // system reserved value
                {
                    JrnyCat = "";
                }
                else
                {
                    JrnyCat = item.C1journeycat.CatName;
                }
                servicesList.Add(new activityList { 
                    idClient= id,
                    idService = item.idService,
                    idServiceType = item.idServiceType,
                    ServiceType = item.C1servicetypes.ServiceType,
                    journeyDepth = 0,
                    StartedDate = item.StartedDate,
                    EndedDate = item.EndedDate,
                    CreatedDate = item.CreateDate,
                    JrnyCatName = JrnyCat
                });
                
                //
                // now, for each original item in the services list, build up the linked-list of journeyed services under each
                //
                CommunityCounts.Global_Methods.CS.addJourneyChild(servicesList, id, item.idService, item.StartedDate,0, db);
            }
            //
            ViewBag.idClient = id;
            var getClientName = from c in db.C1client where (c.idClient==id) select new {c.FirstName, c.LastName, c.scramble};
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName,getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName,getClientName.First().scramble);
            return View(servicesList); 
        }
        // GET: c1service/Journey
        public ActionResult Journey(int id) // key passed is idService
        {
            
            C1service c1service = db.C1service.Find(id);
            journeyItem j = new journeyItem();
            j.idclient = c1service.idClient;
            j.origidService = id;
            j.origidServiceType = c1service.idServiceType;
            j.StartedDate = System.DateTime.Today;
            var currentdepth = db.C1journeys.Find(id);
            if (currentdepth==null)
            {
                j.JourneyDepth = 0; // no parent record, so must be first child.
            }
            else
            {
                j.JourneyDepth = currentdepth.JourneyDepth;
            }
            C1client c1client = db.C1client.Find(c1service.idClient);
            ViewBag.FirstName = CS.unscramble(c1client.FirstName, c1client.scramble);
            ViewBag.LastName = CS.unscramble(c1client.LastName, c1client.scramble);
            ViewBag.ServiceType = db.C1servicetypes.Find(c1service.idServiceType).ServiceType;
            ViewBag.idClient = c1service.idClient;
            //
            // Only permit selection of service types not already in use
            //
            var fulllist = db.C1servicetypes.ToList();
            var alreadyInUse = from s in db.C1service where ((s.idClient == c1service.idClient) & ((s.EndedDate == null) || s.EndedDate > System.DateTime.Today)) select new {s.idServiceType};
            foreach (var item in fulllist.ToList())
            {
                foreach (var item2 in alreadyInUse)
                {
                    if ((item.idServiceType==item2.idServiceType) || item.EndedDate<=System.DateTime.Now)
                    {
                        fulllist.Remove(item);
                    }
                }
            }
            ViewBag.idServiceType = new SelectList(fulllist, "idServiceType", "ServiceType");
            return View(j);
        }
        // POST: c1service/Journey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Journey([Bind(Include = "idServiceType,StartedDate,origidService,origidServiceType,idclient,JourneyDepth")] journeyItem j,int id) 
        {
            
            C1service c1service = db.C1service.Find(j.origidService);
            c1service.CreateDate = System.DateTime.Today;
            c1service.idClient = j.idclient;
            journeyItem journeyItem = new journeyItem();
            journeyItem.idclient = j.idclient;
            journeyItem.idServiceType = j.idServiceType;
            journeyItem.JourneyDepth = j.JourneyDepth;
            journeyItem.origidService = j.origidService;
            journeyItem.origidServiceType = j.origidServiceType;
            journeyItem.StartedDate = j.StartedDate;
            C1client c1client = db.C1client.Find(c1service.idClient);
            ViewBag.FirstName = CS.unscramble(c1client.FirstName, c1client.scramble);
            ViewBag.LastName = CS.unscramble(c1client.LastName, c1client.scramble);
            ViewBag.ServiceType = db.C1servicetypes.Find(c1service.idServiceType).ServiceType;
            //
            // check that this Client isn;t already allocated to this activity at this date?
            //
            var serviceCheck = from s in db.C1service where ((s.idServiceType == j.idServiceType) & (s.idClient == j.idclient) & (s.EndedDate > j.StartedDate)) select s;
            var alreadyPresent = serviceCheck.Any();
            if (alreadyPresent)
            {
                ModelState.AddModelError("idServiceType", "This client is already allocated to this activity on this date");
            }
            // 
            // this journeyed activity start date must be not before the start date of the parent item.
            //
            var parentStartedDate = db.C1service.Find(j.origidService).StartedDate;
            if (parentStartedDate> j.StartedDate)
            {
                string msg = String.Format("Enrolled Date cannot be earlier than {0:d} as this is when the originating Activity was started",parentStartedDate);
                ModelState.AddModelError("StartedDate", msg);
            }
            //
            // Enrollment date must we within the registration year
            //
            var idRegYear = CS.getRegYearId(db);
            var regyear = db.regyears.Find(idRegYear);
            if ((journeyItem.StartedDate > regyear.EndDate) || (journeyItem.StartedDate < regyear.StartDate))
            {
                ModelState.AddModelError("StartedDate", "This enrollment date is not within the current registration year (" + regyear.StartDate.ToShortDateString() + "-" + regyear.EndDate.ToShortDateString() + ")");
            }
            if (ModelState.IsValid)
            {
                // create new service record for new service for this client
                c1service.EndedDate = null;
                c1service.idServiceType = j.idServiceType;
                c1service.StartedDate = j.StartedDate;
                c1service.CreateDate = System.DateTime.Today;
                c1service.JourneyedServices = true;
                db.C1service.Add(c1service);    // create new Activity (service)
                db.SaveChanges();               // need to save record for Auto Increment on idService to be valid on next read
                // re-read back this service (activity) record to get it's key value
                var newrec = from s in db.C1service where ((s.idClient == j.idclient) & (s.idServiceType == j.idServiceType) & (s.StartedDate == j.StartedDate) & (s.CreateDate == System.DateTime.Today)) select s;
                if (!newrec.Any())
                {
                    throw new ArgumentOutOfRangeException("Cannot re-find newly created service record when Journeying");
                }
                // create linking information in Journey table for this pair of services
                C1journeys c1journeys = new C1journeys();
                c1journeys.OrigidService = j.origidService;
                c1journeys.JourneyedidService = newrec.First().idService;
                c1journeys.JourneyDepth=j.JourneyDepth+1;
                db.C1journeys.Add(c1journeys);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = j.idclient }); // pass client id back to index (list) screen
            }
            ViewBag.idClient = id; // pass idClient to add screen from ViewBag
            ViewBag.idServiceType = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType");
            ViewBag.DefStartDate = c1service.StartedDate.ToShortDateString();
            return View(journeyItem);     
        }
        // GET: c1service/Add
        public ActionResult Add(int id) // id is idClient
        {
            //
            // Only permit selection of service types not already in use
            //
          
            var fulllist = db.C1servicetypes.ToList();
            var alreadyInUse = from s in db.C1service where ((s.idClient ==id) & ((s.EndedDate == null) || s.EndedDate > System.DateTime.Today)) select new { s.idServiceType };
            foreach (var item in fulllist.ToList())
            {
                foreach (var item2 in alreadyInUse)
                {
                    if ((item.idServiceType == item2.idServiceType)  || item.EndedDate<=System.DateTime.Now)
                    {
                        fulllist.Remove(item);
                    }
                }
            }
            ViewBag.idServiceType = new SelectList(fulllist, "idServiceType", "ServiceType");
            ViewBag.DefStartDate = System.DateTime.Now.ToShortDateString();
            ViewBag.idClient = id; // pass idClient to add screen from ViewBag
            return View();
        }

        // POST: c1service/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "idServiceType,StartedDate")] C1service c1service,int id)
        {
            c1service.CreateDate = System.DateTime.Now;
            c1service.idClient = id;
            //
            // check that this Client isn;t already allocated to this activity?
            //
            var serviceCheck = from s in db.C1service where ((s.idServiceType == c1service.idServiceType) & (s.idClient == id) & (s.EndedDate > c1service.StartedDate)) select s;
            var alreadyPresent = serviceCheck.Any();
            if (alreadyPresent)
            {
                ModelState.AddModelError("idServiceType", "This client is already allocated to this activity on this date");
            }
            //
            // Enrollment date must we within the registration year
            //
            var idRegYear = CS.getRegYearId(db);
            var regyear = db.regyears.Find(idRegYear);
            if ((c1service.StartedDate > regyear.EndDate) || (c1service.StartedDate<regyear.StartDate))
            {
                ModelState.AddModelError("StartedDate", "This enrollment date is not within the current registration year (" + regyear.StartDate.ToShortDateString() + "-" + regyear.EndDate.ToShortDateString()+")");
            }
            if (ModelState.IsValid)
            {
                c1service.EndedDate = null;
                db.C1service.Add(c1service);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id}); // pass client id back to index (list) screen
            }
            ViewBag.idClient = id; // pass idClient to add screen from ViewBag
            ViewBag.idServiceType = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType");
            ViewBag.DefStartDate = c1service.StartedDate.ToShortDateString();
            return View(c1service);     
        }
        // GET: C1Service/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1service c1service = db.C1service.Find(id);
            var services = db.C1service.Where(c => c.idService == id).ToList();
            var getClientName = from c in db.C1client where (c.idClient == c1service.idClient) select new { c.FirstName, c.LastName, c.scramble };
            var getActivityName = from a in db.C1servicetypes where (a.idServiceType == c1service.idServiceType) select new { a.ServiceType };
            ViewBag.idClient = c1service.idClient;
            ViewBag.Activity = getActivityName.First().ServiceType;
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName,getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName,getClientName.First().scramble);
            ViewBag.JourneyedidCategory = new SelectList(db.C1journeycat.OrderBy(a=>a.CatName), "idJourneyCat", "CatName", c1service.JourneyedidCategory);
            ViewBag.OrigEnrollmentDate = c1service.StartedDate; 
        
            if (c1service == null)
            {
                return HttpNotFound();
            }
            return View(c1service);
        }
        // POST: c1Service/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idService,idServiceType,idClient,EndedDate,CreateDate,StartedDate,JourneyedServices,JourneyedidCategory")] C1service c1service, DateTime OrigEnrollmentDate)
        {
            if (c1service.EndedDate!=null)
            {
                if (c1service.EndedDate<=c1service.StartedDate)
                {
                    ModelState.AddModelError("EndedDate", "Any unenrolment date must be later than any enrolment date");
                }
            }
            //
            // Enrollment date must we within the registration year
            //
            var idRegYear = CS.getRegYearId(db);
            var regyear = db.regyears.Find(idRegYear);
            if ((c1service.StartedDate > regyear.EndDate) || (c1service.StartedDate < regyear.StartDate))
            {
                ModelState.AddModelError("StartedDate", "This enrollment date is not within the current registration year (" + regyear.StartDate.ToShortDateString() + "-" + regyear.EndDate.ToShortDateString() + ")");
            }
            // any earlier enrollment to this same activity with an unenrollment date later than this start date?
            var earlyChk = from s in db.C1service where (
                               (s.idClient==c1service.idClient) &&
                               (s.idServiceType == c1service.idServiceType) &&
                               (s.EndedDate > c1service.StartedDate) &&
                               (s.idService != c1service.idService)  // don't count the same record twice!
                               ) select s;
            if (earlyChk.Any())
            {
                ModelState.AddModelError("StartedDate", "Too early. This client was already enrolled in this activity at this date. Make the started date later.");
            }
            //
            // check no attendance records exist already and start date/endeddate invalidates this
            //
            TimeSpan zero = new TimeSpan(0);
            var aMarkedChk = from a in db.C1attendance
                          where (
                          (a.idClient == c1service.idClient) &&
                          (a.idServiceType == c1service.idServiceType) &&
                          (a.SessionDate < c1service.StartedDate) &&
                          (a.SessionDate>=OrigEnrollmentDate) &&           
                          ((a.AttendedCount>0) || (a.AttendedTime > zero)
                          ))
                          select a;
            var aMarked = aMarkedChk.Any();
            //var aMarked = db.C1attendance
            //    .Where(a => a.idClient == c1service.idClient)
            //    .Where(a => a.idServiceType == c1service.idServiceType)
            //    .Where(a => a.SessionDate < c1service.StartedDate)
            //    .Where((a=>a.AttendedCount>0))
            //    .Any();
            if (aMarked)
            {
                ModelState.AddModelError("StartedDate", "This Client has already been marked as having attended for this Activity before this enrollment date. Make the enrollment date earlier");
            }
            if (c1service.EndedDate != null)
            {
                aMarkedChk = from a in db.C1attendance
                                 where (
                                 (a.idClient == c1service.idClient) &&
                                 (a.idServiceType == c1service.idServiceType) &&
                                 (a.SessionDate >= c1service.EndedDate) &&
                                 ((a.AttendedCount > 0) || (a.AttendedTime > zero)
                                 ))
                                 select a;
                aMarked = aMarkedChk.Any();
                //aMarked = db.C1attendance
                //    .Where(a => a.idClient == c1service.idClient)
                //    .Where(a => a.idServiceType == c1service.idServiceType)
                //    .Where(a => a.SessionDate >= c1service.EndedDate)
                //    .Any();
                if (aMarked)
                {
                    ModelState.AddModelError("EndedDate", "This Client has already been marked as having attended for this Activity after this unenrollment date. Make the unenrollment date later");
                }
            }
            //
            // If this activity has a journey (child) record - ensure that start date not later than child dates
            //
            var child = db.C1journeys.Where(j => j.OrigidService == c1service.idService);
            DateTime? childStartDate, earliestChildStartDate;
            earliestChildStartDate = null;
            if (child.Any())
            {
                foreach (var c in child.ToList())
                {
                    childStartDate = db.C1service.Find(c.JourneyedidService).StartedDate;
                    if (earliestChildStartDate == null)
                    {
                        earliestChildStartDate = childStartDate;
                    }
                    else
                    {
                        if (childStartDate < earliestChildStartDate)
                        {
                            earliestChildStartDate = childStartDate;
                        }
                    }
                }
                if (earliestChildStartDate < c1service.StartedDate)
                {
                    var msg = String.Format("This Activity has journeyed Activities (Activities dependent upon this one) that start on {0:d}. This date cannot be later than that.", earliestChildStartDate);
                    ModelState.AddModelError("StartedDate", msg);
                }
            }
            // 
            // this journeyed activity start date must be not before the start date of the parent item.
            //
            DateTime? parentStartedDate;
            parentStartedDate=null;
            var parent = db.C1journeys.Where(j=>j.JourneyedidService == c1service.idService);
            if (parent.Any())
            {
                parentStartedDate = db.C1service.Find(parent.First().OrigidService).StartedDate;
                if (parentStartedDate > c1service.StartedDate)
                {
                    string msg = String.Format("Enrolled Date cannot be earlier than {0:d} as this is when the originating Activity was started", parentStartedDate);
                    ModelState.AddModelError("StartedDate", msg);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(c1service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = c1service.idClient});
            }
            var services = db.C1service.Where(c => c.idService == c1service.idClient).ToList();
            var getClientName = from c in db.C1client where (c.idClient == c1service.idClient) select new { c.FirstName, c.LastName, c.scramble };
            var getActivityName = from a in db.C1servicetypes where (a.idServiceType == c1service.idServiceType) select new { a.ServiceType };
            ViewBag.idClient = c1service.idClient;
            ViewBag.Activity = getActivityName.First().ServiceType;
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
            ViewBag.JourneyedidCategory = new SelectList(db.C1journeycat.OrderBy(a => a.CatName), "idJourneyCat", "CatName", c1service.JourneyedidCategory);
            ViewBag.OrigEnrollmentDate = OrigEnrollmentDate; 
            return View(c1service);
        }
        // GET: C1service/Delete/5
        [Authorize(Roles = "canDeleteClient,superAdmin,systemAdmin")] 
        public ActionResult Delete(int? id)    
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1service c1service = db.C1service.Find(id);
            var getClientName = from c in db.C1client where (c.idClient == c1service.idClient) select new { c.FirstName, c.LastName, c.scramble };
            ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName,getClientName.First().scramble);
            ViewBag.LastName = CS.unscramble(getClientName.First().LastName,getClientName.First().scramble);
            ViewBag.idClient = c1service.idClient;
            if (c1service  == null)
            {
                return HttpNotFound();
            }
            return View (c1service);
       }
        // POST: C1service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1service c1service = db.C1service.Find(id);
            var child = db.C1journeys.Where(j => j.OrigidService == id);
            if (child.Any())
            {
                ModelState.AddModelError("", "This Activity cannot be deleted as it has journeyed Activities (Activities dependent upon this one). Delete those first.");
                var getClientName = from c in db.C1client where (c.idClient == c1service.idClient) select new { c.FirstName, c.LastName, c.scramble };
                ViewBag.FirstName = CS.unscramble(getClientName.First().FirstName, getClientName.First().scramble);
                ViewBag.LastName = CS.unscramble(getClientName.First().LastName, getClientName.First().scramble);
                ViewBag.idClient = c1service.idClient;
                return View(c1service);
            }

            int Clientid = c1service.idClient;
            db.C1service.Remove(c1service);
            var jny = db.C1journeys.Where(j => j.JourneyedidService == id);
            if (jny.Any())
            {
                C1journeys c1journey = db.C1journeys.Find(jny.First().idJourneys);
                db.C1journeys.Remove(c1journey);
            }
                db.SaveChanges();
            return RedirectToAction("Index", new { id = Clientid });
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