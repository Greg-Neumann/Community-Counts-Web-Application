using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using CommunityCounts.Models.Master;


namespace CommunityCounts.Controllers.Master
{
    public class C1CalDatController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: C1CalDat
        public ActionResult Index()
        {
            List<caldatList> l = new List<caldatList>();
            var caldata = db.C1caldat.GroupBy(p => p.ccCalYear);
            foreach (var c in caldata.ToList())
            {
                // lookup start and end dates for this registration year
                var dates = db.regyears.Where(r=>r.RegYear1 == c.Key).First();
                var sdate = dates.StartDate;
                var edate = dates.EndDate;
                l.Add(new caldatList{
                RegYear = c.Key.ToString(),
                RegYearStartDate = sdate,
                RegYearEndDate = edate
                });
            }
            return View(l.ToList().OrderByDescending(m=>m.RegYear));
        }
        public ActionResult Create()
        {
            return View();
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult Create(caldatList clist)
         {
             //
             // does desired registratiuon year exist?
             //
             Boolean alreadyExist = db.C1caldat.Where(r => r.ccCalYear == clist.RegYear).Any();
             if (alreadyExist)
             {
                 ModelState.AddModelError("RegYear","This registration year already exists on the database");
             }
             int reg;
             if (!int.TryParse(clist.RegYear, out reg))
             {
                 ModelState.AddModelError("RegYear", "This registration year is not valid");

             }
             if ((reg>2050)||(reg<2000))
             {
                 ModelState.AddModelError("RegYear", "This registration year is not in a valid range");
             }
             if (clist.RegYearEndDate<=clist.RegYearStartDate)
             {
                 ModelState.AddModelError("RegYearEndDate", "The stated end date must be after the stated start date");
             }
             if (clist.RegYearEndDate.Ticks - clist.RegYearStartDate.Ticks <=10000000E0*3600E0*24E0*360E0) // 10,000,000 ticks in a second
             {
                 ModelState.AddModelError("RegYearEndDate", "This end date does not appear to be a full year after the start date");
             }
             if (Math.Abs(clist.RegYearStartDate.Year-reg)>0)
             {
                 ModelState.AddModelError("RegYearStartDate", "This start date does not seem to be valid for this registration year");
             }
             if (Math.Abs(clist.RegYearEndDate.Year - reg) > 1)
             {
                 ModelState.AddModelError("RegYearEndDate", "This end date does not seem to be valid for this registration year");
             }

             if (ModelState.IsValid)
             {
                 //
                 // create all rows on Catdat table by  inserting from 1st date forwards and deriving related field
                 //
                 Boolean endOfYear = false;
                 var loopDate = clist.RegYearStartDate.Date;
                 var qtr = 1;
                 var half = 1;
                 var monthCtr = 0;
                 while (!endOfYear)
                 {
                     db.C1caldat.Add(new C1caldat
                     {
                         ccCalYear = clist.RegYear,
                         ccCalDate = loopDate,
                         ccCalYearMonth = loopDate.Year.ToString() + loopDate.ToString("-MM")+ loopDate.ToString("MMM"),
                         ccCalQtr = clist.RegYear.ToString() + "Q" + qtr.ToString(),
                         ccCalHalf = clist.RegYear.ToString() + "H" + half.ToString()
                     });
                     var newLoopDate = loopDate.AddDays(1);
                     if ((newLoopDate.Month>loopDate.Month) || (newLoopDate.Year>loopDate.Year))
                     {
                         monthCtr++;
                         if (monthCtr % 3.0 == 0.0)
                         {
                             qtr++;
                         }
                         if (monthCtr % 6.0 == 0.0)
                         {
                             half++;
                         }
                     }
                     endOfYear = (newLoopDate > clist.RegYearEndDate);
                     loopDate = newLoopDate;
                 }
                 //
                 // now insert into the RegYear table if not already present
                 //
                 Boolean alreadyPresent = db.regyears.Where(y => y.RegYear1 == clist.RegYear.ToString()).Any();
                 if (!alreadyPresent)
                 {
                     db.regyears.Add(new regyear
                     {
                         RegYear1 = clist.RegYear.ToString(),
                         StartDate = clist.RegYearStartDate,
                         EndDate = clist.RegYearEndDate
                     });
                 }
                 db.SaveChanges();
                 return RedirectToAction("index");
             }
             return View();
         }
    }
}