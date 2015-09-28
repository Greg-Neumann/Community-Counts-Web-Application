using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using Microsoft.AspNet.Identity;
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
          
            var c1client = from c in db.C1client.Where(c=>c.idClient>1) select c; // idClient 1 is reserved for system use (surveys etc as anonymous)
            //               select new { c.FirstName, c.LastName, c.HouseNumber, c.AddressLine1, c.citylist, c.countylist, c.postcode, c.Phone };
            var clientsList = c1client.ToList(); // execute the SQL call - get all data.
            //
            // need to parse all data returned and unscramble already scrambled data
            // The .ToList() method in the foreach is called on the original clientsList so that the ensuing .Remove() method 
            // is not processing the original list which is invalid!
            //
            
            foreach (var c1clients in clientsList.ToList())
            {
                c1clients.FirstName = CC.unscramble(c1clients.FirstName, c1clients.scramble);
                c1clients.LastName = CC.unscramble(c1clients.LastName, c1clients.scramble);
                if (c1clients.HouseNumber != null)
                { c1clients.HouseNumber = CC.unscramble(c1clients.HouseNumber,c1clients.scramble); }
                c1clients.AddressLine1 = CC.unscramble(c1clients.AddressLine1,c1clients.scramble);
                if (c1clients.AddressLine2 != null)
                { c1clients.AddressLine2 = CC.unscramble(c1clients.AddressLine2,c1clients.scramble); }
                if (c1clients.Email != null)
                { c1clients.Email = CC.unscramble(c1clients.Email,c1clients.scramble); }
                if (c1clients.Phone != null)
                { c1clients.Phone = CC.unscramble(c1clients.Phone,c1clients.scramble); }
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
             
            @ViewBag.ClientCount = clientsList.Count().ToString();
            return View(clientsList.Take(30).OrderBy(c=>c.LastName)); // limit i/o to the user to max 50 rows
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
            c1client.FirstName = CC.unscramble(c1client.FirstName,c1client.scramble);
            c1client.LastName = CC.unscramble(c1client.LastName,c1client.scramble);
            if (c1client.HouseNumber != null)
               { c1client.HouseNumber = CC.unscramble(c1client.HouseNumber,c1client.scramble); }
            c1client.AddressLine1 = CC.unscramble(c1client.AddressLine1,c1client.scramble);
            if (c1client.AddressLine2 != null)
               { c1client.AddressLine2 = CC.unscramble(c1client.AddressLine2,c1client.scramble); }
            if (c1client.Email != null)
               { c1client.Email = CC.unscramble(c1client.Email,c1client.scramble); }
            if (c1client.Phone != null)
               { c1client.Phone = CC.unscramble(c1client.Phone,c1client.scramble); }

            if (c1client == null)
            {
                return HttpNotFound();
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
        public ActionResult Create([Bind(Include = "idClient,FirstName,LastName,idPostcode,HouseNumber,AddressLine1,AddressLine2,idCity,idCounty,Phone,Email,idGender,idAgeRange,idEthnicity,Ethnicity_Other,idOccupation,Occupation_Other,idDisability,idBenefits,idTravelMethod,idHearOfServices,HearOther,AttainmentTracked,MemoryStickIssued,ConfirmSigned,idFirstLanguage,FirstLanguageOther,idHousingStatus,idTenantStatus,ArmedServCur,ArmedSerPre")] C1client c1client, string postcodeText)
        {
            var getRegYear = from a in db.regyears where (a.RegYear1=="2015") select a; // fix registration year always as 2015 - will need changing!
            c1client.idRegYear= getRegYear.First().idRegYear;
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
            // Housing Status / Landlord checking
            // idHousingStatus of 106 = rented accommodation
            // idTenantStatus of 121 = not a tennant
            Boolean Tennant;
            Boolean validTennat;
            Boolean validAccomm;
            Tennant = (c1client.idHousingStatus == 106);
            validTennat = (c1client.idTenantStatus != 121);
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
                c1client.FirstName = CC.scramble(c1client.FirstName,true); 
                c1client.LastName = CC.scramble(c1client.LastName,true);
                if (c1client.HouseNumber != null)
                   {c1client.HouseNumber = CC.scramble(c1client.HouseNumber,true);}
                c1client.AddressLine1 = CC.scramble(c1client.AddressLine1,true);
                if (c1client.AddressLine2 != null)
                { c1client.AddressLine2 = CC.scramble(c1client.AddressLine2,true); }
                if (c1client.Email != null)
                { c1client.Email = CC.scramble(c1client.Email,true); }
                if (c1client.Phone != null)
                { c1client.Phone = CC.scramble(c1client.Phone,true); }
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
            c1client.FirstName = CC.unscramble(c1client.FirstName,c1client.scramble);
            c1client.LastName = CC.unscramble(c1client.LastName,c1client.scramble);
            if (c1client.HouseNumber != null)
            { c1client.HouseNumber = CC.unscramble(c1client.HouseNumber,c1client.scramble); }
            c1client.AddressLine1 = CC.unscramble(c1client.AddressLine1,c1client.scramble);
            if (c1client.AddressLine2 != null)
            { c1client.AddressLine2 = CC.unscramble(c1client.AddressLine2,c1client.scramble); }
            if (c1client.Email != null)
            { c1client.Email = CC.unscramble(c1client.Email,c1client.scramble); }
            if (c1client.Phone != null)
            { c1client.Phone = CC.unscramble(c1client.Phone,c1client.scramble); }
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
        public ActionResult Edit([Bind(Include = "idClient,FirstName,LastName,idPostcode,HouseNumber,AddressLine1,AddressLine2,idCity,idCounty,Phone,Email,idGender,idAgeRange,idEthnicity,Ethnicity_Other,idOccupation,Occupation_Other,idDisability,idBenefits,idTravelMethod,idHearOfServices,HearOther,AttainmentTracked,MemoryStickIssued,CreatedDateTime,ChangedDateTime,ConfirmSigned,idFirstLanguage,FirstLanguageOther,idHousingStatus,idTenantStatus,ArmedServCur,ArmedSerPre")] C1client c1client, string postcodeText)
        {
            var getRegYear = from a in db.regyears where (a.RegYear1 == "2015") select a; // fix registration year always as 2015 - will need changing!
            c1client.idRegYear = getRegYear.First().idRegYear;
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
            // Housing Status / Landlord checking
            // idHousingStatus of 106 = rented accommodation
            // idTenantStatus of 121 = not a tennant
            Boolean Tennant;
            Boolean validTennat;
            Boolean validAccomm;
            Tennant = (c1client.idHousingStatus == 106);
            validTennat = (c1client.idTenantStatus != 121);       
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
                c1client.FirstName = CC.scramble(c1client.FirstName, c1client.scramble);
                c1client.LastName = CC.scramble(c1client.LastName, c1client.scramble);
                if (c1client.HouseNumber != null)
                { c1client.HouseNumber = CC.scramble(c1client.HouseNumber, c1client.scramble); }
                c1client.AddressLine1 = CC.scramble(c1client.AddressLine1, c1client.scramble);
                if (c1client.AddressLine2 != null)
                { c1client.AddressLine2 = CC.scramble(c1client.AddressLine2, c1client.scramble); }
                if (c1client.Email != null)
                { c1client.Email = CC.scramble(c1client.Email, c1client.scramble); }
                if (c1client.Phone != null)
                { c1client.Phone = CC.scramble(c1client.Phone, c1client.scramble); }

                
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
            c1client.FirstName = CC.unscramble(c1client.FirstName, c1client.scramble);
            c1client.LastName = CC.unscramble(c1client.LastName, c1client.scramble);
            if (c1client.HouseNumber != null)
            { c1client.HouseNumber = CC.unscramble(c1client.HouseNumber, c1client.scramble); }
            c1client.AddressLine1 = CC.unscramble(c1client.AddressLine1, c1client.scramble);
            if (c1client.AddressLine2 != null)
            { c1client.AddressLine2 = CC.unscramble(c1client.AddressLine2, c1client.scramble); }
            if (c1client.Email != null)
            { c1client.Email = CC.unscramble(c1client.Email, c1client.scramble); }
            if (c1client.Phone != null)
            { c1client.Phone = CC.unscramble(c1client.Phone, c1client.scramble); }
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
