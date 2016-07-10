using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers.Master
{
    [Authorize]
    public class C1clientController : Controller
    {
       
        private ccMaster db = new ccMaster(null);
        
        // GET: C1client
        public ActionResult Index(string searchStringName)
        {
            //var c1client = db.C1client.Include(c => c.citylist).Include(c => c.countylist).Include(c => c.customer).Include(c => c.postcode).Include(c => c.refdata).Include(c => c.refdata1).Include(c => c.refdata10).Include(c => c.refdata2).Include(c => c.refdata3).Include(c => c.refdata4).Include(c => c.refdata5).Include(c => c.refdata6).Include(c => c.refdata7).Include(c => c.refdata8).Include(c => c.refdata9).Take(50);
            //
            //
            int idYear = CS.getRegYearId(db);
            var c1client = from c in db.C1client.Where(c=>c.idClient>1).Where(c=>c.idRegYear==idYear) select c; // idClient 1 is reserved for system use (surveys etc as anonymous)
            //               select new { c.FirstName, c.LastName, c.HouseNumber, c.AddressLine1, c.citylist, c.countylist, c.postcode, c.Phone };
            var clientsList = c1client.ToList(); // execute the SQL call - get all data.
            //
            // need to parse all data returned and unscramble already scrambled data
            // The .ToList() method in the foreach is called on the original clientsList so that the ensuing .Remove() method 
            // is not processing the original list which is invalid!
            //
            
            foreach (var c1clients in clientsList.ToList())
            {
                c1clients.FirstName = CS.unscramble(c1clients.FirstName, c1clients.scramble);
                c1clients.LastName = CS.unscramble(c1clients.LastName, c1clients.scramble);
                if (c1clients.HouseNumber != null)
                { c1clients.HouseNumber = CS.unscramble(c1clients.HouseNumber,c1clients.scramble); }
                c1clients.AddressLine1 = CS.unscramble(c1clients.AddressLine1,c1clients.scramble);
                if (c1clients.AddressLine2 != null)
                { c1clients.AddressLine2 = CS.unscramble(c1clients.AddressLine2,c1clients.scramble); }
                if (c1clients.Email != null)
                { c1clients.Email = CS.unscramble(c1clients.Email,c1clients.scramble); }
                if (c1clients.Phone != null)
                { c1clients.Phone = CS.unscramble(c1clients.Phone,c1clients.scramble); }
                //
                // shall we keep the row?
                //
                if (!String.IsNullOrEmpty(searchStringName))
                {
                    if (!(c1clients.LastName.ToUpper().Contains(searchStringName.ToUpper()) || c1clients.FirstName.ToUpper().Contains(searchStringName.ToUpper()) || c1clients.idClient.ToString()==searchStringName.ToString()))
                    { 
                        clientsList.Remove(c1clients); // name search string specified and row does not match
                    }
                }
            }
            var displayList = clientsList.OrderByDescending(c => c.CreatedDateTime);
            @ViewBag.userHasNews = CS.userHasNews(db);
            @ViewBag.ClientCount = clientsList.Count().ToString();
            @ViewBag.RegYear = CS.getRegYear(db,false);
            return View(displayList.Take(10).OrderBy(c=>c.LastName)); // limit i/o to the user 
        }

        // GET: C1client/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1client c1client = db.C1client.Find(id);
            // unencrypt already encrypted fields for display.
            //
            c1client.FirstName = CS.unscramble(c1client.FirstName,c1client.scramble);
            c1client.LastName = CS.unscramble(c1client.LastName,c1client.scramble);
            if (c1client.HouseNumber != null)
               { c1client.HouseNumber = CS.unscramble(c1client.HouseNumber,c1client.scramble); }
            c1client.AddressLine1 = CS.unscramble(c1client.AddressLine1,c1client.scramble);
            if (c1client.AddressLine2 != null)
               { c1client.AddressLine2 = CS.unscramble(c1client.AddressLine2,c1client.scramble); }
            if (c1client.Email != null)
               { c1client.Email = CS.unscramble(c1client.Email,c1client.scramble); }
            if (c1client.Phone != null)
               { c1client.Phone = CS.unscramble(c1client.Phone,c1client.scramble); }

            if (c1client == null)
            {
                return HttpNotFound();
            }
            ViewBag.PrevYear = "";
            if (c1client.idClientPrev != null)
            {
                ViewBag.PrevYear =" In Year " +  db.regyears.Find(db.C1client.Find(c1client.idClientPrev).idRegYear).RegYear1;
            }
            return View(c1client);
        }

        // GET: C1client/Create
        public ActionResult Create()
        {
            ViewBag.idCity = new SelectList(db.citylists.OrderBy(a=>a.City), "Cityid", "City");
            ViewBag.idCounty = new SelectList(db.countylists.OrderBy(a=>a.County), "idCountyList", "County");
            ViewBag.idCust = new SelectList(db.customers, "idCust", "CustName");
            ViewBag.postcodeText = string.Empty; // populate text value to avoid dropdown box
            ViewBag.idAgeRange = new SelectList(db.refdatas.Where(a =>a.RefCode=="AgeR").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue"); // only filter age-ranges
            ViewBag.idOccupation = new SelectList(db.refdatas.Where(a => a.RefCode == "Empl").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idTenantStatus = new SelectList(db.refdatas.Where(a => a.RefCode == "TS").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idTravelMethod = new SelectList(db.refdatas.Where(a => a.RefCode == "Trav").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idGender = new SelectList(db.refdatas.Where(a => a.RefCode == "Gen").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue");
            ViewBag.idEthnicity = new SelectList(db.refdatas.Where(a => a.RefCode == "Ethn").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue");
            ViewBag.idHearOfServices = new SelectList(db.refdatas.Where(a => a.RefCode == "Hos").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue");
            ViewBag.idDisability = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idBenefits = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idFirstLanguage = new SelectList(db.refdatas.Where(a => a.RefCode == "Lang").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idHousingStatus = new SelectList(db.refdatas.OrderBy(a=>a.RefCodeValue).Where(a => a.RefCode == "Hous"), "idRefData", "RefCodeValue");
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1");
            return View();
        }

        // POST: C1client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idClient,FirstName,LastName,idPostcode,HouseNumber,AddressLine1,AddressLine2,idCity,idCounty,Phone,Email,idGender,idAgeRange,idEthnicity,Ethnicity_Other,idOccupation,Occupation_Other,idDisability,idBenefits,idTravelMethod,idHearOfServices,HearOther,AttainmentTracked,MemoryStickIssued,ConfirmSigned,idFirstLanguage,FirstLanguageOther,idHousingStatus,idTenantStatus,ArmedServCur,ArmedSerPre,isCaseWorked,hasNeedsAnalysed")] C1client c1client, string postcodeText)
        {
            c1client.idRegYear = CS.getRegYearId(db);
            c1client.idCust = 1;                                                          // fix customer as 1; will need database edit for merging multi-customer records!
            //
            // TextBox used for Postcode as string parameter postcodeText for performance reasons. Get key value (and validate value)
            // Cannot rely on ModelState at this point as postcodeText field not part of the database model
            //
            var pcode = postcodeText.Trim();                                            // postcode value entered by User in web form
            var getid = from b in db.postcodes where (b.PostCode1 == pcode) select b;   // is the entered postcode in the database?
            var valid = getid.Any();                                                    // is there anything in the collection?
            if (!valid)
            {
                ModelState.AddModelError("postcode", "The postcode is not a recognized UK postcode with a space between the two parts");
            }
            else
            {
                c1client.idPostcode = getid.First().idPostCode;                         // set the idPostCode into the Database field             
                ModelState.Remove("postcode");                                          // reset the error message affecting the model state                             
            }
            // 
            // Create pseudo address for special postcodes relating to Homeless or No Fixed Abode
            //
            switch (pcode.ToUpper())
            {
                case "CC1 1CC":
                case "CC1 2CC":
                    //
                    c1client.HouseNumber = "1";
                    c1client.AddressLine1 = "Hope Way";
                    c1client.idCity = db.citylists.Where(t => t.City == "Town").First().Cityid;
                    c1client.idCounty = db.countylists.Where(c => c.County == "United Kingdom").First().idCountyList;
                    c1client.idHousingStatus = db.refdatas.Where(r => r.RefCodeValue == "Homeless/No fixed abode").First().idRefData;
                    c1client.idTenantStatus = db.refdatas.Where(r => r.RefCodeValue == "TSb").First().idRefData;
                    break;
                default:
                    if (c1client.AddressLine1 == null)
                    {
                        ModelState.AddModelError("AddressLine1", "Please enter a valid street name");
                    }
                    break;
            }
            // Housing Status / Landlord checking
            // idHousingStatus of 106 = rented accommodation
            // idTenantStatus of 121 = not a tennant
            Boolean Tennant;
            Boolean validTennat;
            Boolean validAccomm;
            Tennant = (c1client.idHousingStatus == db.refdatas.Where(r => r.RefCodeValue == "Rented Accommodation").First().idRefData);
            validTennat = (c1client.idTenantStatus != db.refdatas.Where(r => r.RefCodeValue == "TSb").First().idRefData); // Tennant status of 'none'
            if (!Tennant)
            {
                validAccomm = !validTennat;
                ModelState.AddModelError("idHousingStatus", "Use Landlord of 'none' if not rented accommodation ");
                ViewBag.ResultMessage = "Please correct the errors further down on this screen";
            }
            else
            {
                validAccomm = validTennat;
                ModelState.AddModelError("idHousingStatus", "Select a valid Landlord if rented accommodation (do not use 'none')");
                ViewBag.ResultMessage = "Please correct the errors further down on this screen";
            }
            if (validAccomm)
            {
                ModelState.Remove("idHousingStatus");
            }
            
            if (ModelState.IsValid)
            {
                c1client.CreatedDateTime = System.DateTime.Now;                      // successful creation to record logged.
                c1client.ChangedDateTime = System.DateTime.Now;
                //
                // encrypt certain fields on client record
                //
                c1client.FirstName = CS.scramble(c1client.FirstName,true); 
                c1client.LastName = CS.scramble(c1client.LastName,true);
                if (c1client.HouseNumber != null)
                   {c1client.HouseNumber = CS.scramble(c1client.HouseNumber,true);}
                c1client.AddressLine1 = CS.scramble(c1client.AddressLine1,true);
                if (c1client.AddressLine2 != null)
                { c1client.AddressLine2 = CS.scramble(c1client.AddressLine2,true); }
                if (c1client.Email != null)
                { c1client.Email = CS.scramble(c1client.Email,true); }
                if (c1client.Phone != null)
                { c1client.Phone = CS.scramble(c1client.Phone,true); }
                //
                c1client.CreatedDateTime = System.DateTime.Now;
                c1client.scramble = true;                       // always use scrambling instead of encryption (performance)
                db.C1client.Add(c1client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCity = new SelectList(db.citylists.OrderBy(a=>a.City), "Cityid", "City");
            ViewBag.idCounty = new SelectList(db.countylists.OrderBy(a=>a.County), "idCountyList", "County");
            ViewBag.idCust = new SelectList(db.customers, "idCust", "CustName");
            ViewBag.postcodeText = pcode; // populate text value to avoid dropdown box
            ViewBag.idAgeRange = new SelectList(db.refdatas.Where(a => a.RefCode == "AgeR").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue"); // only filter age-ranges
            ViewBag.idOccupation = new SelectList(db.refdatas.Where(a => a.RefCode == "Empl").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idTenantStatus = new SelectList(db.refdatas.Where(a => a.RefCode == "TS").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idTravelMethod = new SelectList(db.refdatas.Where(a => a.RefCode == "Trav").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idGender = new SelectList(db.refdatas.Where(a => a.RefCode == "Gen").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue");
            ViewBag.idEthnicity = new SelectList(db.refdatas.Where(a => a.RefCode == "Ethn").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue");
            ViewBag.idHearOfServices = new SelectList(db.refdatas.Where(a => a.RefCode == "Hos").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue");
            ViewBag.idDisability = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idBenefits = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idFirstLanguage = new SelectList(db.refdatas.Where(a => a.RefCode == "Lang").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc");
            ViewBag.idHousingStatus = new SelectList(db.refdatas.OrderBy(a=>a.RefCodeValue).Where(a => a.RefCode == "Hous"), "idRefData", "RefCodeValue");
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1");
            return View(c1client);
        }

        // GET: C1client/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1client c1client = db.C1client.Find(id);
            if (c1client == null)
            {
                return HttpNotFound();
            }
            // unencrypt already encrypted data
            c1client.FirstName = CS.unscramble(c1client.FirstName,c1client.scramble);
            c1client.LastName = CS.unscramble(c1client.LastName,c1client.scramble);
            if (c1client.HouseNumber != null)
            { c1client.HouseNumber = CS.unscramble(c1client.HouseNumber,c1client.scramble); }
            c1client.AddressLine1 = CS.unscramble(c1client.AddressLine1,c1client.scramble);
            if (c1client.AddressLine2 != null)
            { c1client.AddressLine2 = CS.unscramble(c1client.AddressLine2,c1client.scramble); }
            if (c1client.Email != null)
            { c1client.Email = CS.unscramble(c1client.Email,c1client.scramble); }
            if (c1client.Phone != null)
            { c1client.Phone = CS.unscramble(c1client.Phone,c1client.scramble); }
            ViewBag.postcodeText = c1client.postcode.PostCode1; // populate text value to avoid dropdown box
            ViewBag.idCity = new SelectList(db.citylists.OrderBy(a=>a.City), "Cityid", "City",c1client.idCity);
            ViewBag.idCounty = new SelectList(db.countylists.OrderBy(a=>a.County), "idCountyList", "County",c1client.idCounty);
            ViewBag.idCust = new SelectList(db.customers, "idCust", "CustName",c1client.idCust);
            ViewBag.idAgeRange = new SelectList(db.refdatas.Where(a => a.RefCode == "AgeR").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue", c1client.idAgeRange); // only filter age-ranges
            ViewBag.idOccupation = new SelectList(db.refdatas.Where(a => a.RefCode == "Empl").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc",c1client.idOccupation);
            ViewBag.idTenantStatus = new SelectList(db.refdatas.Where(a => a.RefCode == "TS").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc",c1client.idTenantStatus);
            ViewBag.idTravelMethod = new SelectList(db.refdatas.Where(a => a.RefCode == "Trav").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc",c1client.idTravelMethod);
            ViewBag.idGender = new SelectList(db.refdatas.Where(a => a.RefCode == "Gen").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue",c1client.idGender);
            ViewBag.idEthnicity = new SelectList(db.refdatas.Where(a => a.RefCode == "Ethn").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue",c1client.idEthnicity);
            ViewBag.idHearOfServices = new SelectList(db.refdatas.Where(a => a.RefCode == "Hos").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue",c1client.idHearOfServices);
            ViewBag.idDisability = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc",c1client.idDisability);
            ViewBag.idBenefits = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc",c1client.idBenefits);
            ViewBag.idFirstLanguage = new SelectList(db.refdatas.Where(a => a.RefCode == "Lang").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc",c1client.idFirstLanguage);
            ViewBag.idHousingStatus = new SelectList(db.refdatas.OrderBy(a=>a.RefCodeValue).Where(a => a.RefCode == "Hous"), "idRefData", "RefCodeValue",c1client.idHousingStatus);
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1");
            return View(c1client);
        }

        // POST: C1client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idClient,idClientPrev,idRegYear,FirstName,LastName,idPostcode,HouseNumber,AddressLine1,AddressLine2,idCity,idCounty,Phone,Email,idGender,idAgeRange,idEthnicity,Ethnicity_Other,idOccupation,Occupation_Other,idDisability,idBenefits,idTravelMethod,idHearOfServices,HearOther,AttainmentTracked,MemoryStickIssued,CreatedDateTime,ChangedDateTime,ConfirmSigned,idFirstLanguage,FirstLanguageOther,idHousingStatus,idTenantStatus,ArmedServCur,ArmedSerPre,isCaseWorked,hasNeedsAnalysed")] C1client c1client, string postcodeText)
        {
            c1client.idCust = 1;                                                          // fix customer as 1; will need database edit for merging multi-customer records!
            //
            // TextBox used for Postcode as string parameter postcodeText for performance reasons. Get key value (and validate value)
            // Cannot rely on ModelState at this point as postcodeText field not part of the database model
            //
            var pcode = postcodeText.Trim();                                            // postcode value entered by User in web form
            var getid = from b in db.postcodes where (b.PostCode1 == pcode) select b;   // is the entered postcode in the database?
            var valid = getid.Any();                                                    // is there anything in the collection?
            if (!valid)
            {
                ModelState.AddModelError("postcode", "The postcode is not a recognized UK postcode with a space between the two parts");
                ViewBag.ResultMessage = "There are errors furher down on this screen !";
            }
            else
            {
                c1client.idPostcode = getid.First().idPostCode;                         // set the idPostCode into the Database field  
                ModelState.Remove("postcode");                                          // reset the error message affecting the model state                             
            }
            // 
            // Create pseudo address for special postcodes relating to Homeless or No Fixed Abode
            //
            switch (pcode.ToUpper())
            {
                case "CC1 1CC":
                case "CC1 2CC":
                    //
                    c1client.HouseNumber = "1";
                    c1client.AddressLine1 = "Hope Way";
                    c1client.idCity = db.citylists.Where(t => t.City == "Town").First().Cityid;
                    c1client.idCounty = db.countylists.Where(c => c.County == "United Kingdom").First().idCountyList;
                    c1client.idHousingStatus = db.refdatas.Where(r => r.RefCodeValue == "Homeless/No fixed abode").First().idRefData;
                    c1client.idTenantStatus = db.refdatas.Where(r => r.RefCodeValue == "TSb").First().idRefData;
                    break;
                default:
                    if (c1client.AddressLine1 == null)
                    {
                        ModelState.AddModelError("AddressLine1", "Please enter a valid street name");
                    }
                    break;
            }
            // Housing Status / Landlord checking
            // idHousingStatus of 106 = rented accommodation
            // idTenantStatus of 121 = not a tennant
            Boolean Tennant;
            Boolean validTennat;
            Boolean validAccomm;
            Tennant = (c1client.idHousingStatus == db.refdatas.Where(r => r.RefCodeValue == "Rented Accommodation").First().idRefData);
            validTennat = (c1client.idTenantStatus != db.refdatas.Where(r => r.RefCodeValue == "TSb").First().idRefData); // Tennant status of 'none'       
            if (!Tennant)
            {
                validAccomm = !validTennat;
                ModelState.AddModelError("idHousingStatus", "Use Landlord of 'none' if not rented accommodation ");
                ViewBag.ResultMessage = "Please correct the errors further down on this screen";
            }
            else
            {
                validAccomm = validTennat;
                ModelState.AddModelError("idHousingStatus", "Select a valid Landlord if rented accommodation (do not use 'none')");
                ViewBag.ResultMessage = "Please correct the errors further down on this screen";
            }
            if (validAccomm)
            {
                ModelState.Remove("idHousingStatus");
            }
            if (ModelState.IsValid)
            {
                c1client.ChangedDateTime = System.DateTime.Now;                      // successful change to record logged.
                //
                // encrypt certain fields on client record
                //
                c1client.scramble = true;                                         // always use scrambling upon write (performance)
                c1client.FirstName = CS.scramble(c1client.FirstName, c1client.scramble);
                c1client.LastName = CS.scramble(c1client.LastName, c1client.scramble);
                if (c1client.HouseNumber != null)
                { c1client.HouseNumber = CS.scramble(c1client.HouseNumber, c1client.scramble); }
                c1client.AddressLine1 = CS.scramble(c1client.AddressLine1, c1client.scramble);
                if (c1client.AddressLine2 != null)
                { c1client.AddressLine2 = CS.scramble(c1client.AddressLine2, c1client.scramble); }
                if (c1client.Email != null)
                { c1client.Email = CS.scramble(c1client.Email, c1client.scramble); }
                if (c1client.Phone != null)
                { c1client.Phone = CS.scramble(c1client.Phone, c1client.scramble); }

                
                db.Entry(c1client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.postcodeText = pcode;
            ViewBag.idCity = new SelectList(db.citylists.OrderBy(a=>a.City), "Cityid", "City", c1client.idCity);
            ViewBag.idCounty = new SelectList(db.countylists.OrderBy(a=>a.County), "idCountyList", "County", c1client.idCounty);
            ViewBag.idCust = new SelectList(db.customers, "idCust", "CustName", c1client.idCust);
            ViewBag.idAgeRange = new SelectList(db.refdatas.Where(a => a.RefCode == "AgeR").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue", c1client.idAgeRange); // only filter age-ranges
            ViewBag.idOccupation = new SelectList(db.refdatas.Where(a => a.RefCode == "Empl").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc", c1client.idOccupation);
            ViewBag.idTenantStatus = new SelectList(db.refdatas.OrderBy(a=>a.RefCodeDesc).Where(a => a.RefCode == "TS"), "idRefData", "RefCodeDesc", c1client.idTenantStatus);
            ViewBag.idTravelMethod = new SelectList(db.refdatas.Where(a => a.RefCode == "Trav").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc", c1client.idTravelMethod);
            ViewBag.idGender = new SelectList(db.refdatas.Where(a => a.RefCode == "Gen").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue", c1client.idGender);
            ViewBag.idEthnicity = new SelectList(db.refdatas.Where(a => a.RefCode == "Ethn").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue", c1client.idEthnicity);
            ViewBag.idHearOfServices = new SelectList(db.refdatas.Where(a => a.RefCode == "Hos").OrderBy(a=>a.RefCodeValue), "idRefData", "RefCodeValue", c1client.idHearOfServices);
            ViewBag.idDisability = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc", c1client.idDisability);
            ViewBag.idBenefits = new SelectList(db.refdatas.Where(a => a.RefCode == "YNP").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc", c1client.idBenefits);
            ViewBag.idFirstLanguage = new SelectList(db.refdatas.Where(a => a.RefCode == "Lang").OrderBy(a=>a.RefCodeDesc), "idRefData", "RefCodeDesc", c1client.idFirstLanguage);
            ViewBag.idHousingStatus = new SelectList(db.refdatas.OrderBy(a=>a.RefCodeValue).Where(a => a.RefCode == "Hous"), "idRefData", "RefCodeValue", c1client.idHousingStatus);
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1");
            return View(c1client);
        }

        // GET: C1client/Delete/5
        [Authorize(Roles = "canDeleteClient,superAdmin,systemAdmin")] 
        public ActionResult Delete(int? id)
            
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1client c1client = db.C1client.Find(id);
            if (c1client == null)
            {
                return HttpNotFound();
            }
            // unencrypt already encrypted fields for display.
            //
            c1client.FirstName = CS.unscramble(c1client.FirstName, c1client.scramble);
            c1client.LastName = CS.unscramble(c1client.LastName, c1client.scramble);
            if (c1client.HouseNumber != null)
            { c1client.HouseNumber = CS.unscramble(c1client.HouseNumber, c1client.scramble); }
            c1client.AddressLine1 = CS.unscramble(c1client.AddressLine1, c1client.scramble);
            if (c1client.AddressLine2 != null)
            { c1client.AddressLine2 = CS.unscramble(c1client.AddressLine2, c1client.scramble); }
            if (c1client.Email != null)
            { c1client.Email = CS.unscramble(c1client.Email, c1client.scramble); }
            if (c1client.Phone != null)
            { c1client.Phone = CS.unscramble(c1client.Phone, c1client.scramble); }
            return View(c1client);
        }

        // POST: C1client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canDeleteClient,superAdmin,systemAdmin")] 
        public ActionResult DeleteConfirmed(int id)
        {
            C1client c1client = db.C1client.Find(id);
            db.C1client.Remove(c1client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: C1clients/Copy
        public ActionResult Copy(int? id)
        {
            //
            // copy forward selected idClient to another registration year.
            //
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var c1client = db.C1client.Find(id);
            ViewBag.FullName = CS.unscramble(c1client.FirstName, true) + " "
                + CS.unscramble(c1client.LastName, true)
                + " of " + c1client.postcode.PostCode1;
            ViewBag.idRegYear = new SelectList(db.regyears,"idRegYear", "RegYear1",null, CS.getRegYearId(db));

            return View();
        }
        // POST: C1client/Copy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy([Bind(Include = "idRegYear")] int id, int idRegYear)
        {
            var prevClient = db.C1client.Find(id);
            ViewBag.Msg = "";
            if (prevClient.idRegYear==idRegYear)
            {
                ViewBag.Msg="Year cannot be the same as this currrent year; choose another";
            }
            if (ViewBag.Msg == "")
            {
                Boolean alreadyCopied = db.C1client
                    .Where(c => c.FirstName == prevClient.FirstName)
                    .Where(c => c.LastName == prevClient.LastName)
                    .Where(c => c.idPostcode == prevClient.idPostcode)
                    .Where(c => c.idRegYear == idRegYear).Any();
                if (alreadyCopied)
                {
                    ViewBag.Msg = "A Client of this name and postcode already exists for this postcode and year " + db.regyears.Find(idRegYear).RegYear1.ToString();
                }
            }
            if (ViewBag.Msg=="")
            {
                DateTime rightNow = System.DateTime.Now;
                db.C1client.Add(new C1client
                {
                    AddressLine1 = prevClient.AddressLine1,
                    AddressLine2 = prevClient.AddressLine2,
                    ArmedSerPre = prevClient.ArmedSerPre,
                    ArmedServCur = prevClient.ArmedServCur,
                    AttainmentTracked = prevClient.AttainmentTracked,
                    ChangedDateTime = rightNow,
                    citylist = prevClient.citylist,
                    ConfirmSigned = false,                  // override
                    countylist = prevClient.countylist,
                    CreatedDateTime = rightNow,
                    customer = prevClient.customer,
                    Email = prevClient.Email,
                    Ethnicity_Other = prevClient.Ethnicity_Other,
                    FirstLanguageOther = prevClient.FirstLanguageOther,
                    FirstName = prevClient.FirstName,
                    HearOther = prevClient.HearOther,
                    HouseNumber = prevClient.HouseNumber,
                    idAgeRange = prevClient.idAgeRange,
                    idBenefits = prevClient.idBenefits,
                    idCity = prevClient.idCity,
                    idClientPrev = prevClient.idClient,      // watch swop of id's
                    idCounty = prevClient.idCounty,
                    idCust = prevClient.idCust,
                    idDisability = prevClient.idDisability,
                    idEthnicity = prevClient.idEthnicity,
                    idFirstLanguage = prevClient.idFirstLanguage,
                    idGender = prevClient.idGender,
                    idHearOfServices = prevClient.idHearOfServices,
                    idHousingStatus = prevClient.idHousingStatus,
                    idOccupation = prevClient.idOccupation,
                    idPostcode = prevClient.idPostcode,
                    idRegYear = idRegYear,
                    idTenantStatus = prevClient.idTenantStatus,
                    idTravelMethod = prevClient.idTravelMethod,
                    LastName = prevClient.LastName,
                    MemoryStickIssued = prevClient.MemoryStickIssued,
                    Occupation_Other = prevClient.Occupation_Other,
                    Phone = prevClient.Phone,
                    scramble = prevClient.scramble
                });

                db.SaveChanges();   // write the new client record
                                    //
                                    // read new client record back to get it's key
                                    //
                var newClient = db.C1client.Where(c => c.idClientPrev==prevClient.idClient).OrderByDescending(c=>c.ChangedDateTime);
                if (!newClient.Any())
                {
                    throw new Exception("Program Logic Error : cannot find new Client created at " + rightNow.ToString() + " for idPostcode " + prevClient.idPostcode.ToString());
                }
                return RedirectToAction("Edit/" + newClient.First().idClient.ToString());
            }
            ViewBag.FullName = CS.unscramble(prevClient.FirstName, true) + " "
             + CS.unscramble(prevClient.LastName, true)
             + " of " + prevClient.postcode.PostCode1;
            ViewBag.idRegYear = new SelectList(db.regyears, "idRegYear", "RegYear1", null, CS.getRegYearId(db));
            return View();
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
