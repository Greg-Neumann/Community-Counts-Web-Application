using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;


namespace CommunityCounts.Controllers.Master
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private ccMaster db = new ccMaster(null);
        // GET: Attendance
        public ActionResult Index()
        {
            var activities = from a in db.C1servicetypes.OrderBy(s => s.ServiceType)
                             where ((a.refdata.RefCodeValue != "AMNP")                                    // suppress "attendance mark not permitted'
                             && ((a.EndedDate == null) || (a.EndedDate > System.DateTime.Now))) select a; // dont list ended Activities past their end date
            return View(activities.ToList());
        }

        // GET: Attendance/PrintAttendance/id
        public ActionResult PrintAttendance(int id) // id passed is idservicetype
        {
            var bookings = db.C1bookings.Where(b => b.idServiceType == id).Where(b => b.StartDate >= System.DateTime.Today).OrderBy(b => b.StartDate);
            var getActivityName = from a in db.C1servicetypes where (a.idServiceType==id) select new {a.ServiceType};
            ViewBag.ActivityName = getActivityName.First().ServiceType; // get activity name (Service type name)
            return View(bookings.ToList());
        }

        // GET: Attendance/PrintAList/id
        public ActionResult PrintAList (int id) // id passed is idBooking
        {
            List<C1client> cl = new List<C1client>();
            var st = from a in db.C1bookings where (a.idBookings == id) select a;
            var servicetype = st.First().idServiceType;
            var sessionStartDate = st.First().StartDate;
            @ViewBag.StartDate = st.First().StartDate.ToString("ddd dd MMM yy");
            @ViewBag.StartTime = st.First().StartTime.ToString("hh':'mm");
            var activityName = from b in db.C1servicetypes where (b.idServiceType == servicetype) select b;
            @ViewBag.ActivityName = activityName.First().ServiceType;
            //
            // what type Attendance List ?
            //
            String listFormat = "_______________________";
            String listHeader = "Signature";
            var attendanceType = db.C1servicetypes.Where(s => s.idServiceType == servicetype).First();
            var markFormat = from c in db.refdatas.Where(c => c.idRefData == attendanceType.AttendanceType) select new { c.RefCodeValue };
            if (markFormat.First().RefCodeValue == "AMUT")
            {
                listFormat = "[_] [_] [_] [_] [_] [_] [_] [_] [_] [_] _______________________";
                listHeader = "Tick a box to mark attendance and sign once";
            }
            @ViewBag.ListFormat = listFormat;
            @ViewBag.ListHeader = listHeader;
            var client = from b in db.C1service
                         where ((b.idServiceType == servicetype) && ((b.StartedDate <= sessionStartDate) && ((b.EndedDate == null) || (b.EndedDate > sessionStartDate))))
                         select new { b.C1client.FirstName, b.C1client.LastName, b.C1client.scramble }; // now have all idClients who need to have their attendance list printed
            foreach (var c in client.ToList())
            {
                var fname = CS.unscramble(c.FirstName, c.scramble);
                var lname = CS.unscramble(c.LastName, c.scramble);
                cl.Add(new C1client() { FirstName = fname, LastName = lname });
            }
            if (markFormat.First().RefCodeValue == "AMTT") // times to be printed
            {
                return View("PrintAListT", cl.OrderBy(l => l.FirstName).ThenBy(l => l.LastName));
            }
            else
            {
                return View("PrintaList", cl.OrderBy(l => l.FirstName).ThenBy(l => l.LastName));
            }
        }

        public ActionResult MarkAttendance(int id) // id passed is idservicetype
            {
                var  BookingsList = new List<BookingsList>();
                Boolean marked;
                string resourceName;    
                var Bookings = db.C1bookings.Where(b => b.idServiceType == id).Where(b => b.StartDate <= System.DateTime.Today).OrderByDescending(b => b.StartDate);
                foreach (var bookings in Bookings.ToList())
                {
                    //
                    // Any attendance records for this booking and session date/time
                    //
                    marked = db.C1attendance.Where(a => a.idSchedules == bookings.idSchedules).Where(a => a.SessionDate == bookings.StartDate).Where(a => a.SessionTime == bookings.StartTime).Any();
                    resourceName = db.C1resources.Find(bookings.idResource).ResourceName;
                    BookingsList.Add(new BookingsList()
                    {
                        EndDate = bookings.EndDate,
                        EndTime = bookings.EndTime,
                        idBookings = bookings.idBookings,
                        Resource = resourceName,
                        idSchedules = bookings.idSchedules,
                        idServiceType = bookings.idServiceType,
                        StartDate = bookings.StartDate,
                        StartTime = bookings.StartTime,
                        Marked = marked
                    });
                }
                var getActivityName = from a in db.C1servicetypes where (a.idServiceType == id) select new { a.ServiceType };
                ViewBag.ActivityName = getActivityName.First().ServiceType; // get activity name (Service type name)
                return View(BookingsList.ToList());
            }
        [Authorize(Roles = "systemAdmin,superAdmin,canMarkAttendance")]
        public ActionResult MarkAList (int id) // id passed is idBooking,  Mark a list with default as not present
        {
            var st = from a in db.C1bookings where (a.idBookings == id) select a;
            var resource = st.First().idResource;
            var servicetype = st.First().idServiceType;
            var schedule = st.First().idSchedules;
            var sessiondate = st.First().StartDate;
            var sessiontime = st.First().StartTime;
            var activityName = from b in db.C1servicetypes where (b.idServiceType == servicetype) select b;
            var attendanceType = db.C1servicetypes.Where(s => s.idServiceType == servicetype).First();
            var markFormat = from c in db.refdatas.Where(c => c.idRefData == attendanceType.AttendanceType) select new { c.RefCodeValue };
            String markType=markFormat.First().RefCodeValue;
            var client = from b in db.C1service where ((b.idServiceType == servicetype)  && ((b.StartedDate <= sessiondate) && ((b.EndedDate == null) || (b.EndedDate > sessiondate)))) select new { b.idClient}; // now have all idClients who need to have their attendance list marked
                //
                // generate Attendance records (marked not present) for this idBooking and idServiceType 
                //
            var alreadyMarked = db.C1attendance.Where(a => a.idResource == resource).Where(a => a.idServiceType == servicetype).Where(a => a.idSchedules == schedule).Count()>0;
                
                // Loop through all this clients currently enrolled for this session and Add an attendance record (marked absent) if a record is not present
                // deletions are blocked by checking, upon activity enrollment (or modificaton of enrollment), whether attendance marks already exist
            foreach (var clients in client.ToList())
            {
                var existingRec = db.C1attendance.Where(a => a.idResource == resource).Where(a => a.idServiceType == servicetype).Where(a => a.idSchedules == schedule).Where(a => a.idClient == clients.idClient).Where(a => a.SessionDate == sessiondate).Where(a => a.SessionTime == sessiontime);
                if (!existingRec.Any())
                {
                    db.C1attendance.Add(new C1attendance() { 
                        idResource = resource, 
                        idServiceType = servicetype, 
                        idSchedules = schedule, 
                        idClient = clients.idClient, 
                        SessionDate = sessiondate, 
                        SessionTime = sessiontime, 
                        AttendedCount = 0, 
                        AttendedTime=TimeSpan.Zero,
                        SignInTime=TimeSpan.Zero,
                        SignOutTime=TimeSpan.Zero,
                    });
                }
            }
            db.SaveChanges();
                //
                // now read-back those attendance records for processing. The Clientid will need turning to text and unscrambling
                //
            var attendanceList = db.C1attendance.Where(a => a.idResource == resource).Where(a => a.idServiceType == servicetype).Where(a => a.idSchedules == schedule).Where(a => a.SessionDate == sessiondate).Where(a => a.SessionTime == sessiontime);
            List<AttendanceMark> al = new List<AttendanceMark>();
            bool pr;
            foreach (var a in attendanceList.ToList())
            {             
               var d = db.C1client.Where(c=>c.idClient==a.idClient).First();
               pr = (a.AttendedCount > 0);
               al.Add(new AttendanceMark() {
                   idAttendance=a.idAttendance, 
                   FirstName=CS.unscramble(d.FirstName,d.scramble),
                   LastName=CS.unscramble(d.LastName,d.scramble),
                   idResource=resource,idServiceType=servicetype,
                   idSchedules=schedule,
                   SessionDate=sessiondate,
                   SessionTime=sessiontime,
                   AttendedCount=a.AttendedCount,
                   SignInTime = a.SignInTime,
                   SignOutTime = a.SignOutTime,
                   Present=pr,
                   idClient=d.idClient});
            }
            //
            // Stuff the ViewBag with header information defining the session that is being marked
            //
            var ResourceName = from r in db.C1resources where (r.idResource == resource) select new { r.ResourceName };
            @ViewBag.ResourceName = ResourceName.First().ResourceName;
            @ViewBag.StartDate = st.First().StartDate.ToString("ddd dd MMM yy");
            @ViewBag.StartTime = st.First().StartTime.ToString("hh':'mm");
            @ViewBag.ActivityName = activityName.First().ServiceType;
            switch (markType)
            {
                case "AMUN":
                    return View("MarkAList",al.OrderBy(a=>a.FirstName).ThenBy(a=>a.LastName).ToList()); // normal present / absent marking
                case "AMUT":
                    return View("MarkAListC",al.OrderBy(a=>a.FirstName).ThenBy(a=>a.LastName).ToList()); // Tally count marking
                case "AMTT":
                    return View("MarkAListT",al.OrderBy(a=>a.FirstName).ThenBy(a=>a.LastName).ToList()); // Timed marking
                default :
                    throw new ArgumentOutOfRangeException("Attendance mark type not recognised"); 
            }       
        }
        // POST: MarkAList
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "systemAdmin,superAdmin,canMarkAttendance")]
        public ActionResult MarkAList(List<AttendanceMark> aMark)
        {
            
            var attendanceType = db.C1servicetypes.Find(aMark.First().idServiceType).AttendanceType;
            var markFormat = from c in db.refdatas.Where(c => c.idRefData == attendanceType ) select new { c.RefCodeValue };
            string markType = markFormat.First().RefCodeValue;
            //
            // for Tally counting & Timed attendance, validate input data (Present/Absent marking does not need validating)
            //
            switch (markType)
            {
                case "AMUN":
                    break;
                case "AMUT":
                    foreach (var mark in aMark.ToList())
                    {
                        if (mark.AttendedCount < 0 || mark.AttendedCount > 100)
                        {
                            ModelState.AddModelError("", "All marks must be between 0 and 100. Please correct the incorrect values below");
                            break;
                        }
                    }
                    break;
                case "AMTT":
                    foreach (var mark in aMark.ToList())
                    {
                        if ((mark.SignOutTime <= mark.SignInTime) && (mark.SignInTime>TimeSpan.Zero))
                        {
                            ModelState.AddModelError("", "Signout times must be later than signin times. Please correct the values below");
                            break;
                        }
                    }
                    break;
            }
            if (ModelState.IsValid)
            {
                foreach (var mark in aMark.ToList())
                {
                    C1attendance rec = db.C1attendance.Find(mark.idAttendance); // get attendance record by key
                    switch (markType)
                    {
                        case "AMUN": // present/absent marking
                            if (mark.Present)
                            {
                                rec.AttendedCount = 1;
                            }
                            else
                            {
                                rec.AttendedCount = 0;
                            }
                            break;
                        case "AMUT": // Tally count marking
                            rec.AttendedCount = mark.AttendedCount;
                            break;
                        case "AMTT":
                            rec.AttendedTime = mark.SignOutTime - mark.SignInTime;
                            rec.SignInTime = mark.SignInTime;
                            rec.SignOutTime = mark.SignOutTime;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Attendance mark type not recognised");
                    }
                    db.Entry(rec).State = EntityState.Modified;
                }

                db.SaveChanges(); // commit all attendance changes in one go
                return RedirectToAction("MarkAttendance/" + aMark.First().idServiceType);
            }
            else
            {
                //
                // Rebuild the list screen for display
                //
                var resource = aMark.First().idResource;
                var servicetype = aMark.First().idServiceType;
                var schedule = aMark.First().idSchedules;
                var sessiondate = aMark.First().SessionDate;
                var sessiontime = aMark.First().SessionTime;
                var activityName = from b in db.C1servicetypes where (b.idServiceType == servicetype) select b;
                var attendanceList = db.C1attendance.Where(a => a.idResource == resource).Where(a => a.idServiceType == servicetype).Where(a => a.idSchedules == schedule).Where(a => a.SessionDate == sessiondate).Where(a => a.SessionTime == sessiontime);
                List<AttendanceMark> al = new List<AttendanceMark>();
                bool pr;
                foreach (var a in attendanceList.ToList())
                {

                    var d = db.C1client.Where(c => c.idClient == a.idClient).First();
                    pr = (a.AttendedCount > 0);
                    al.Add(new AttendanceMark() { 
                        idAttendance = a.idAttendance, 
                        FirstName = CS.unscramble(d.FirstName, d.scramble), 
                        LastName = CS.unscramble(d.LastName, d.scramble), 
                        idResource = resource, 
                        idServiceType = servicetype, 
                        idSchedules = schedule, 
                        SessionDate = sessiondate, 
                        SessionTime = sessiontime, 
                        AttendedCount = a.AttendedCount, 
                        SignInTime = a.SignInTime,
                        SignOutTime = a.SignOutTime,
                        Present = pr, 
                        idClient = d.idClient });
                }
                //
                // Stuff the ViewBag with header information defining the session that is being marked
                //
                var ResourceName = from r in db.C1resources where (r.idResource == resource) select new { r.ResourceName };
                @ViewBag.ResourceName = ResourceName.First().ResourceName;
                @ViewBag.StartDate = aMark.First().SessionDate.ToLongDateString();
                @ViewBag.StartTime = aMark.First().SessionTime.ToString();
                @ViewBag.ActivityName = activityName.First().ServiceType;
                return View("MarkAListT", al.OrderBy(a => a.FirstName).ThenBy(a=>a.LastName).ToList()); // Tally count marking
            }

        }
    // GET: UNmark attendance marks
    [Authorize(Roles = "systemAdmin,superAdmin,canMarkAttendance")]
    public ActionResult UnmarkAList(int id) // idpassed is idbooking
    {
        C1bookings bookings = db.C1bookings.Find(id);
        return View(bookings);    
    }
        // Post: Unmark attendance marks
    [Authorize(Roles = "systemAdmin,superAdmin,canMarkAttendance")]
    [HttpPost, ActionName("UnmarkAList")]
    [ValidateAntiForgeryToken]
    public ActionResult UnmarkConfirmed(int id)
    {
        C1bookings bookings = db.C1bookings.Find(id);
        var attendances = db.C1attendance.Where(a=>a.idSchedules==bookings.idSchedules).Where(a=>a.SessionDate==bookings.StartDate).Where(a=>a.SessionTime==bookings.StartTime);
        var idserviceType = bookings.idServiceType;
        db.C1attendance.RemoveRange(attendances);
        db.SaveChanges();
        return RedirectToAction("MarkAttendance/" + idserviceType );
    }
    }
    
}
