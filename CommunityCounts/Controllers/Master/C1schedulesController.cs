using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class C1schedulesController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1schedules
        public ActionResult Index()
        {
            //var c1schedules = db.C1schedules.Include(c => c.C1resources).Include(c => c.C1servicetypes).Include(c => c.refdata);
            var numSchedulesToGenerate = db.C1schedules.Where(s => s.Generated == false).Count();
            @ViewBag.numSchedulesToGenerate = numSchedulesToGenerate;
            var c1schedules = db.C1schedules.ToList();
            return View(c1schedules.ToList());
        }

        // GET: C1schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1schedules c1schedules = db.C1schedules.Find(id);
            if (c1schedules == null)
            {
                return HttpNotFound();
            }
            //
            // stack original schedule values in ViewBag to allow comparison if they are present
            //
            C1schedulesorig c1schedulesorig = db.C1schedulesorig.Find(id);
            if (c1schedulesorig != null)
            {
                var ResourceName = db.C1resources.Find(c1schedulesorig.idResource).ResourceName;
                if (c1schedulesorig.idResource!= c1schedules.idResource)
                {
                    @ViewBag.ResourceName = "==> was " + ResourceName;
                }
                var ServiceName = db.C1servicetypes.Find(c1schedulesorig.idServiceType).ServiceType;
                if (c1schedulesorig.idServiceType != c1schedules.idServiceType)
                {
                    @ViewBag.ServiceType = "==> was " + ServiceName;
                }
                var Type = db.refdatas.Find(c1schedulesorig.idScheduleType).RefCodeDesc;
                if (c1schedulesorig.idScheduleType != c1schedules.idScheduleType)
                {
                    @ViewBag.Type = "==> was " + Type;
                }
                if (c1schedulesorig.StartDate != c1schedules.StartDate)
                {
                    @ViewBag.StartDate = "==> was " + c1schedulesorig.StartDate.ToString("yyyy-MM-dd");
                }
                if (c1schedulesorig.EndDate != c1schedules.EndDate)
                {
                    @ViewBag.EndDate = "==> was " + c1schedulesorig.EndDate.ToString("yyyy-MM-dd");
                }
                if (c1schedulesorig.StartTime != c1schedules.StartTime)
                {
                    @ViewBag.StartTime = "==> was " + c1schedulesorig.StartTime.ToString("hh\\:mm");
                }
                if (c1schedulesorig.EndTime != c1schedules.EndTime)
                {
                    @ViewBag.EndTime = "==> was " + c1schedulesorig.EndTime.ToString("hh\\:mm");
                }
                if (c1schedulesorig.Repetition != c1schedules.Repetition)
                {
                    if (c1schedulesorig.Repetition==null)
                    {
                        @ViewBag.Repetition = "==> was zero";
                    }
                    else
                    {
                        @ViewBag.Repetition = "==> was " + c1schedulesorig.Repetition;
                    }
                }
            }
            return View(c1schedules);
        }

        // GET: C1schedules/Create
        public ActionResult Create()
        {
            ViewBag.idResource = new SelectList(db.C1resources, "idResource", "ResourceName");
            ViewBag.idServiceType = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType");
            ViewBag.idScheduleType = new SelectList(db.refdatas.Where(a => a.RefCode == "Sche"), "idRefData", "RefCodeDesc"); // only filter scheduling types
            return View();
        }

        // POST: C1schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idResource,idServiceType,StartDate,StartTime,EndDate,EndTime,idScheduleType,Repetition,Notes")] C1schedules c1schedules)
        {
            if (c1schedules.StartDate > c1schedules.EndDate)
            {
                ModelState.AddModelError("StartDate", "Schedules must end on, of after, the start date");
            }
            if (c1schedules.StartDate == c1schedules.EndDate)
            {
                if (c1schedules.StartTime >= c1schedules.EndTime)
                {
                    ModelState.AddModelError("StartTime", "Schedules must have an end-time after the start time");
                }
            }
            if (ModelState.IsValid)
            {
                var getRegYear = from a in db.regyears where (a.RegYear1 == "2015") select a; // fix registration year always as 2015 - will need changing!
                c1schedules.idRegYear = getRegYear.First().idRegYear;
                c1schedules.CreatedUser = User.Identity.Name;
                c1schedules.CreatedDateTime = System.DateTime.Now;
                c1schedules.Generated = false;
                db.C1schedules.Add(c1schedules);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idResource = new SelectList(db.C1resources, "idResource", "ResourceName", c1schedules.idResource);
            ViewBag.idServiceType = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType", c1schedules.idServiceType);
            ViewBag.idScheduleType = new SelectList(db.refdatas.Where(a => a.RefCode == "Sche"), "idRefData", "RefCodeDesc",c1schedules.idScheduleType); // only filter scheduling types
            return View(c1schedules);
        }

        // GET: C1schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1schedules c1schedules = db.C1schedules.Find(id);
            if (c1schedules == null)
            {
                return HttpNotFound();
            }
            //
            // spool the original schedule record to it's clone table - SchedulesOrig - if not already there. This clone is purged upon successful schedule generation and exists to enable the User to revert to the original version by request
            //
            if (db.C1schedulesorig.Find(id)==null)
            {
                C1schedulesorig c1schedulesorig = new C1schedulesorig();
                c1schedulesorig.idSchedules = c1schedules.idSchedules;
                c1schedulesorig.idRegYear = c1schedules.idRegYear;
                c1schedulesorig.idResource = c1schedules.idResource;
                c1schedulesorig.idServiceType = c1schedules.idServiceType;
                c1schedulesorig.StartDate = c1schedules.StartDate;
                c1schedulesorig.EndDate = c1schedules.EndDate;
                c1schedulesorig.StartTime = c1schedules.StartTime;
                c1schedulesorig.EndTime = c1schedules.EndTime;
                c1schedulesorig.idScheduleType = c1schedules.idScheduleType;
                c1schedulesorig.Repetition = c1schedules.Repetition;
                c1schedulesorig.Notes = c1schedules.Notes;
                c1schedulesorig.UpdatedDateTime = c1schedules.UpdatedDateTime;
                c1schedulesorig.UpdatedUser = c1schedules.UpdatedUser;
                c1schedulesorig.CreatedDateTime = c1schedules.CreatedDateTime;
                c1schedulesorig.CreatedUser = c1schedules.CreatedUser;
                db.C1schedulesorig.Add(c1schedulesorig);
                db.SaveChanges();
            }
            ViewBag.idResource = new SelectList(db.C1resources, "idResource", "ResourceName", c1schedules.idResource);
            ViewBag.idServiceType = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType", c1schedules.idServiceType);
            ViewBag.idScheduleType = new SelectList(db.refdatas.Where(a => a.RefCode == "Sche"), "idRefData", "RefCodeDesc",c1schedules.idScheduleType);
            return View(c1schedules);
        }

        // POST: C1schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSchedules,idRegYear,idResource,idServiceType,StartDate,StartTime,EndDate,EndTime,idScheduleType,Repetition,Notes,CreatedDateTime,CreatedUser")] C1schedules c1schedules)
        {
            if (c1schedules.StartDate > c1schedules.EndDate)
            {
                ModelState.AddModelError("StartDate", "Schedules must end on, of after, the start date");
            }
            if (c1schedules.StartDate==c1schedules.EndDate)
            {
                if (c1schedules.StartTime>=c1schedules.EndTime)
                {
                    ModelState.AddModelError("StartTime", "Schedules must have an end-time after the start time");
                }
            }
            if (ModelState.IsValid)
            {
                c1schedules.UpdatedDateTime = System.DateTime.Now;
                c1schedules.UpdatedUser = User.Identity.Name;
                c1schedules.Generated = false;
                db.Entry(c1schedules).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idResource = new SelectList(db.C1resources, "idResource", "ResourceName", c1schedules.idResource);
            ViewBag.idServiceType = new SelectList(db.C1servicetypes, "idServiceType", "ServiceType", c1schedules.idServiceType);
            ViewBag.idScheduleType = new SelectList(db.refdatas.Where(a => a.RefCode == "Sche"), "idRefData", "RefCodeDesc",c1schedules.idScheduleType);
            return View(c1schedules);
        }

        // GET: C1schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1schedules c1schedules = db.C1schedules.Find(id);
            if (c1schedules == null)
            {
                return HttpNotFound();
            }
            return View(c1schedules);
        }

        // POST: C1schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1schedules c1schedules = db.C1schedules.Find(id);
            db.C1schedules.Remove(c1schedules);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Generate()
        {

            var c1schedules = db.C1schedules.Where(s => s.Generated == false).ToList(); // list all schedules that need generating
            List<generateList> glist = new List<generateList>();
            string lastAttendedD, lastAttendedT;
            DateTime ld;
            TimeSpan lt;
            bool validAttendance, attendanceMsg;
            attendanceMsg = false;
            foreach (var schedule in c1schedules)
            {
                //
                // look up last masked attendance for this schedule?
                //
                var lastAttendedList = db.C1attendance.Where(a=>a.idSchedules==schedule.idSchedules).OrderByDescending(a=>a.SessionDate).ThenByDescending(a=>a.SessionTime);
                if (lastAttendedList.Any())
                {
                    ld = lastAttendedList.First().SessionDate;
                    lt = lastAttendedList.First().SessionTime;
                    lastAttendedD = lastAttendedList.First().SessionDate.ToString("yyyy-MM-dd");
                    lastAttendedT = lastAttendedList.First().SessionTime.ToString("hh\\:mm");
                    validAttendance = (ld<schedule.StartDate) | (ld==schedule.StartDate && (lt < schedule.StartTime));
                }
                else
                {
                    // no attendance records
                    lastAttendedD = null;
                    lastAttendedT = null;
                    validAttendance = true;
                }
                var resourceName = db.C1resources.Find(schedule.idResource).ResourceName;
                var serviceTypeName = db.C1servicetypes.Find(schedule.idServiceType).ServiceType;
                var scheduleTypeName = db.refdatas.Find(schedule.idScheduleType).RefCodeDesc;
                attendanceMsg = attendanceMsg | !validAttendance;

                glist.Add(new generateList()
                    {
                        idSchedules = schedule.idSchedules,
                        Resource = resourceName,
                        ServiceType = serviceTypeName,
                        ScheduleType = scheduleTypeName,
                        StartDate = schedule.StartDate,
                        EndDate = schedule.EndDate,
                        StartTime = schedule.StartTime,
                        EndTime = schedule.EndTime,
                        Repetition = schedule.Repetition,
                        lastAttendedDate = lastAttendedD,
                        lastAttendedTime = lastAttendedT,
                        valid = validAttendance
                    });
            }
            //
            // go the list with last attended date/times, display it all.
            // 
            @ViewBag.attendanceMsg = attendanceMsg;
            @ViewBag.numSchedulesToGenerate = c1schedules.Count();
            return View(glist);
        }
        [HttpPost, ActionName("Generate")]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateConfirmed()
        {
            // 
            // generation has been confirmed. Loop over all schedules that have not been generated and do them
            //
            var c1schedules = db.C1schedules.Where(s => s.Generated == false).ToList(); // list all schedules that need generating
            var getRegYear = from a in db.regyears where (a.RegYear1=="2015") select a; // fix registration year always as 2015 - will need changing!
            DateTime endOfYear = getRegYear.First().EndDate;                            // this is calendar date of the end of the current service year (business year)
            DateTime? ld;                                                               // last attendance date for this activity/schedule
            TimeSpan? lt;                                                               // last attendance time ....
            DateTime sessionEndDate, sessionStartDate;                                  // calculated during schedule loop processing as the calendar date of the booking end/start
            Boolean newScheduleNotComplete;                                             // designates if the generation of the current schedule has finished
            string scheduleType;                                                        // looked-up schedule type string for SWITCH statement below
            int? everyN;                                                                // integer value of n in 'every n weeks'
            foreach (var schedule in c1schedules.ToList())
            {
                //
                // look up last masked attendance for this schedule?
                //
                var lastAttendedList = db.C1attendance.Where(a => a.idSchedules == schedule.idSchedules).OrderByDescending(a => a.SessionDate).ThenByDescending(a => a.SessionTime);
                if (lastAttendedList.Any())
                {
                    ld = lastAttendedList.First().SessionDate;
                    lt = lastAttendedList.First().SessionTime;
                }
                else
                {
                    // no attendance records
                    ld = null;
                    lt = null;
                }
                //
                // if no attendance records for this schedule at all, simple delete the entire schedule from the bookings table.
                // If there are attendance records, delete the schedule from immediately after the last attendance date/time.
                //
                if (ld==null)
                {
                    var c1bookings = db.C1bookings.Where(b=>b.idSchedules==schedule.idSchedules); // build up delete list
                    db.C1bookings.RemoveRange(c1bookings);
                    db.SaveChanges();
                }
                else
                {
                    var c1bookings = db.C1bookings
                        .Where(b => b.idSchedules == schedule.idSchedules)
                        .Where(b => b.StartDate > ld);                           // any booking on a later date than the schedule date
                    db.C1bookings.RemoveRange(c1bookings);
                    c1bookings = db.C1bookings
                        .Where(b => b.idSchedules == schedule.idSchedules)
                        .Where(b => b.StartDate == ld)
                        .Where(b => b.StartTime > lt);                           // any booking on same date but later in time than schedule date/time
                    db.C1bookings.RemoveRange(c1bookings);
                    db.SaveChanges();
                }
                //
                // now generate the new bookings records for the new schedule until the end of current year
                //
                TimeSpan bookingDuration = schedule.EndDate - schedule.StartDate;   // the elapsed time of each session
                sessionStartDate = schedule.StartDate;
                newScheduleNotComplete = true;
                scheduleType = db.refdatas.Find(schedule.idScheduleType).RefCodeValue ;   // type of schedule desired
                if (schedule.Repetition != null)
                {
                    everyN = Convert.ToInt32(schedule.Repetition);
                }
                else
                {
                    everyN = null;
                }
                sessionEndDate = sessionStartDate + bookingDuration;
                do
                {
                    C1bookings sessionRec = new C1bookings();                       // build new bookings record
                    sessionRec.idSchedules = schedule.idSchedules;
                    sessionRec.idResource = schedule.idResource;
                    sessionRec.idServiceType = schedule.idServiceType;
                    sessionRec.StartDate = sessionStartDate;
                    sessionRec.StartTime = schedule.StartTime;
                    sessionRec.EndDate = sessionEndDate;
                    sessionRec.EndTime = schedule.EndTime;
                    sessionRec.UpdateDateTime = System.DateTime.Now;
                    db.C1bookings.Add(sessionRec);                                  // add the session record to the booking table
                    //
                    // Now advance the session dates by the schedule type
                    //
                    switch (scheduleType)
                    {
                        case "ScDa":                                                // Daily (weekdays)
                            if (sessionStartDate.DayOfWeek == DayOfWeek.Friday)
                            {
                                sessionStartDate = sessionStartDate.AddDays(3);     // skip the weekend
                            }
                            else
                            {
                                sessionStartDate = sessionStartDate.AddDays(1);
                            };
                            break;
                        case "ScD7":                                                // Daily (all 7 days)
                            sessionStartDate = sessionStartDate.AddDays(1);
                            break;
                        case "ScDW":                                                // Daily (weekends)
                            if (sessionStartDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                sessionStartDate = sessionStartDate.AddDays(6);     // Skip the week
                            }
                            else
                            {
                                sessionStartDate = sessionStartDate.AddDays(1);
                            }
                            break;
                        case "ScWe":                                                // Weekly
                            sessionStartDate = sessionStartDate.AddDays(7);
                            break;
                        case "ScN":                                                 // every N weeks
                            sessionStartDate = sessionStartDate.AddDays(Convert.ToDouble(7 * everyN));
                            break;
                        case "ScA":                                                 // Annually
                            sessionStartDate = sessionStartDate.AddYears(1);
                            break;
                        default:
                            throw new ArgumentException("Schedule type of {0} invalid for scheduling", scheduleType);
                    }
                    //
                    // is the newly calculated session end date AFTER the current business year (service Year)? If so, then done for this schedule
                    //
                    sessionEndDate = sessionStartDate + bookingDuration;
                    newScheduleNotComplete = sessionEndDate <= endOfYear;           // done this schedule. Go for the next one
                }
                while (newScheduleNotComplete);
                C1schedules srec = db.C1schedules.Find(schedule.idSchedules);           // get the original schedule
                srec.Generated = true;
                db.Entry(srec).State = EntityState.Modified;                             // we have generated this schedule so update flag
                C1schedulesorig orig = db.C1schedulesorig.Find(schedule.idSchedules);   // get the saved (original) schedule if it exists
                if (orig != null)
                {
                    db.C1schedulesorig.Remove(orig);                                        // delete it    
                }
                db.SaveChanges();                                                       // Whole schedule done - commit records.
            }
            return RedirectToAction("Index");                                  // job done - display schedules list screen
        }

        // GET: C1schedules/Revert/5
        public ActionResult Revert(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1schedules c1schedules = db.C1schedules.Find(id);
            if (c1schedules == null)
            {
                return HttpNotFound();
            }
            //
            // stack original schedule values in ViewBag to allow comparison if they are present
            //
            C1schedulesorig c1schedulesorig = db.C1schedulesorig.Find(id);
            bool revertValid = false;
            if (c1schedulesorig != null)
            {
                var ResourceName = db.C1resources.Find(c1schedulesorig.idResource).ResourceName;
                if (c1schedulesorig.idResource != c1schedules.idResource)
                {
                    @ViewBag.ResourceName = "==> was " + ResourceName;
                    revertValid = true;
                }
                var ServiceName = db.C1servicetypes.Find(c1schedulesorig.idServiceType).ServiceType;
                if (c1schedulesorig.idServiceType != c1schedules.idServiceType)
                {
                    @ViewBag.ServiceType = "==> was " + ServiceName;
                    revertValid = true;
                }
                var Type = db.refdatas.Find(c1schedulesorig.idScheduleType).RefCodeDesc;
                if (c1schedulesorig.idScheduleType != c1schedules.idScheduleType)
                {
                    @ViewBag.Type = "==> was " + Type;
                    revertValid = true;

                }
                if (c1schedulesorig.StartDate != c1schedules.StartDate)
                {
                    @ViewBag.StartDate = "==> was " + c1schedulesorig.StartDate.ToString("yyyy-MM-dd");
                    revertValid = true;

                }
                if (c1schedulesorig.EndDate != c1schedules.EndDate)
                {
                    @ViewBag.EndDate = "==> was " + c1schedulesorig.EndDate.ToString("yyyy-MM-dd");
                    revertValid = true;
                }
                if (c1schedulesorig.StartTime != c1schedules.StartTime)
                {
                    @ViewBag.StartTime = "==> was " + c1schedulesorig.StartTime.ToString("hh\\:mm");
                    revertValid = true;
                }
                if (c1schedulesorig.EndTime != c1schedules.EndTime)
                {
                    @ViewBag.EndTime = "==> was " + c1schedulesorig.EndTime.ToString("hh\\:mm");
                    revertValid = true;
                }
                if (c1schedulesorig.Repetition != c1schedules.Repetition)
                {
                    revertValid = true;
                    if (c1schedulesorig.Repetition == null)
                    {
                        @ViewBag.Repetition = "==> was zero";
                    }
                    else
                    {
                        @ViewBag.Repetition = "==> was " + c1schedulesorig.Repetition;
                    }
                }

            }
           
            @ViewBag.revertValid = revertValid;
            return View(c1schedules);
        }
        

        // POST: C1schedules/Revert/5
        [HttpPost, ActionName("Revert")]
        [ValidateAntiForgeryToken]
        public ActionResult RevertConfirmed(int id)
        {
            C1schedules c1schedules = db.C1schedules.Find(id);
            C1schedulesorig c1schedulesorig = db.C1schedulesorig.Find(id);
            c1schedules.idSchedules = c1schedulesorig.idSchedules;
            c1schedules.idRegYear = c1schedulesorig.idRegYear;
            c1schedules.idResource = c1schedulesorig.idResource;
            c1schedules.idServiceType = c1schedulesorig.idServiceType;
            c1schedules.StartDate = c1schedulesorig.StartDate;
            c1schedules.EndDate = c1schedulesorig.EndDate;
            c1schedules.StartTime = c1schedulesorig.StartTime;
            c1schedules.EndTime = c1schedulesorig.EndTime;
            c1schedules.idScheduleType = c1schedulesorig.idScheduleType;
            c1schedules.Repetition = c1schedulesorig.Repetition;
            c1schedules.Notes = c1schedulesorig.Notes;
            c1schedules.UpdatedDateTime = c1schedulesorig.UpdatedDateTime;
            c1schedules.UpdatedUser = c1schedulesorig.UpdatedUser;
            c1schedules.CreatedDateTime = c1schedulesorig.CreatedDateTime;
            c1schedules.CreatedUser = c1schedulesorig.CreatedUser;
            db.Entry(c1schedules).State = EntityState.Modified;
            db.SaveChanges();
            db.C1schedulesorig.Remove(c1schedulesorig); // remove original schedule information as User has reverted the record
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
