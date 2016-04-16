using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "superAdmin,systemAdmin")]
    public class ClientBulkCopyController : Controller
    {
        private ccMaster db = new ccMaster(null);   // open database
        // GET: ClientBulkCopy
        public ActionResult Start()
        {
            string regYear = CS.getRegYear(db, true);  // current registration year string
            int idYear = CS.getRegYearId(db);
            int current_count = db.C1client.Where(c => c.idRegYear == idYear).Count();
            ViewBag.regYear = regYear;
            ViewBag.current_count = current_count;

            return View();
        }
        public ActionResult Year()
        {
            string regYear = CS.getRegYear(db, true);  // current registration year string
            int idYear = CS.getRegYearId(db);
            int current_count = db.C1client.Where(c => c.idRegYear == idYear).Count();
            ViewBag.regYear = regYear;
            ViewBag.current_count = current_count;
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1", null, idYear);
            return View();

        }
        // POST: C1client/Year
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Year([Bind(Include = "idRegYear")] int idRegYear)
        {
            int idYear = CS.getRegYearId(db);
            int will_be_copied_count = 0;
            int already_copied_count = 0;
            int current_count;
            string regYear;
            ViewBag.Msg = "";
            if (idYear == idRegYear)
            {
                ViewBag.Msg = "Year cannot be the same as this currrent year; choose another";
            }
            if (ViewBag.Msg == "")
            {
                foreach (var oldClient in db.C1client.Where(c => c.idRegYear == idYear).ToList())
                {
                    //
                    // first loop is to count how many copies will take place and how many will skip
                    //
                    Boolean alreadyCopied = db.C1client
                   .Where(c => c.FirstName == oldClient.FirstName)
                   .Where(c => c.LastName == oldClient.LastName)
                   .Where(c => c.idPostcode == oldClient.idPostcode)
                   .Where(c => c.HouseNumber == oldClient.HouseNumber)
                   .Where(c => c.AddressLine1 == oldClient.AddressLine1)
                   .Where(c => c.AddressLine2 == oldClient.AddressLine2)
                   .Where(c => c.idCity == oldClient.idCity)
                   .Where(c => c.idRegYear == idRegYear).Any();
                    if (alreadyCopied)
                    {
                        already_copied_count++;
                    }
                    else
                    {
                        will_be_copied_count++;
                    }
                }
                regYear = CS.getRegYear(db, true);  // current registration year string

                current_count = db.C1client.Where(c => c.idRegYear == idYear).Count();
                ViewBag.regYear = regYear;
                ViewBag.current_count = current_count;
                ViewBag.already_copied_count = already_copied_count;
                ViewBag.will_be_copied_count = will_be_copied_count;
                ViewBag.NewYearStarts = db.regyears.Find(idRegYear).StartDate.ToShortDateString();
                ViewBag.idRegYear = idRegYear;
                return View("YearConfirm");
            }

            regYear = CS.getRegYear(db, true);  // current registration year string
            current_count = db.C1client.Where(c => c.idRegYear == idYear).Count();
            ViewBag.regYear = regYear;
            ViewBag.current_count = current_count;
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1", null, idYear);
            return View();
        }
        public ActionResult Copy(int id)
        {
            // do the actual copy after confirmation with the user
            int idYear = CS.getRegYearId(db);
            int was_copied_count = 0;
            int already_copied_count = 0;
            foreach (var oldClient in db.C1client.Where(c => c.idRegYear == idYear).ToList())
            {
                //
                // Second loop is to actually do the copies
                //
                Boolean alreadyCopied = db.C1client
               .Where(c => c.FirstName == oldClient.FirstName)
               .Where(c => c.LastName == oldClient.LastName)
               .Where(c => c.idPostcode == oldClient.idPostcode)
               .Where(c => c.HouseNumber == oldClient.HouseNumber)
               .Where(c => c.AddressLine1 == oldClient.AddressLine1)
               .Where(c => c.AddressLine2 == oldClient.AddressLine2)
               .Where(c => c.idCity == oldClient.idCity)
               .Where(c => c.idRegYear == id).Any();
                if (alreadyCopied)
                {
                    already_copied_count++;
                }
                else
                {
                    // copy over the details and link to two client records by id
                    was_copied_count++;
                    DateTime rightNow = System.DateTime.Now;
                    db.C1client.Add(new C1client
                    {
                        AddressLine1 = oldClient.AddressLine1,
                        AddressLine2 = oldClient.AddressLine2,
                        ArmedSerPre = oldClient.ArmedSerPre,
                        ArmedServCur = oldClient.ArmedServCur,
                        AttainmentTracked = oldClient.AttainmentTracked,
                        ChangedDateTime = rightNow,
                        citylist = oldClient.citylist,
                        ConfirmSigned = false,                  // override
                        countylist = oldClient.countylist,
                        CreatedDateTime = rightNow,
                        customer = oldClient.customer,
                        Email = oldClient.Email,
                        Ethnicity_Other = oldClient.Ethnicity_Other,
                        FirstLanguageOther = oldClient.FirstLanguageOther,
                        FirstName = oldClient.FirstName,
                        HearOther = oldClient.HearOther,
                        HouseNumber = oldClient.HouseNumber,
                        idAgeRange = oldClient.idAgeRange,
                        idBenefits = oldClient.idBenefits,
                        idCity = oldClient.idCity,
                        idClientPrev = oldClient.idClient,      // watch swop of id's
                        idCounty = oldClient.idCounty,
                        idCust = oldClient.idCust,
                        idDisability = oldClient.idDisability,
                        idEthnicity = oldClient.idEthnicity,
                        idFirstLanguage = oldClient.idFirstLanguage,
                        idGender = oldClient.idGender,
                        idHearOfServices = oldClient.idHearOfServices,
                        idHousingStatus = oldClient.idHousingStatus,
                        idOccupation = oldClient.idOccupation,
                        idPostcode = oldClient.idPostcode,
                        idRegYear = id,
                        idTenantStatus = oldClient.idTenantStatus,
                        idTravelMethod = oldClient.idTravelMethod,
                        LastName = oldClient.LastName,
                        MemoryStickIssued = oldClient.MemoryStickIssued,
                        Occupation_Other = oldClient.Occupation_Other,
                        Phone = oldClient.Phone,
                        scramble = oldClient.scramble
                    });
                }
            }
            db.SaveChanges(); //commit the whole lot in one go....
            ViewBag.was_copied_count = was_copied_count;
            ViewBag.already_copied_count = already_copied_count;
            return View();
        }
    }
}
