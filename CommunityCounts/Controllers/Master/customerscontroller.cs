using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "systemAdmin")] 
    public class customersController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // GET: customers
        public ActionResult Index()
        {
            var customers = db.customers.Include(c => c.citylist).Include(c => c.countylist).Include(c => c.postcode);
            return View(customers.ToList());
        }

        // GET: customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: customers/Create
        public ActionResult Create()
        {
            ViewBag.idCity = new SelectList(db.citylists, "Cityid", "City");
            ViewBag.idCounty = new SelectList(db.countylists, "idCountyList", "County");
            ViewBag.postcodeText = string.Empty; // populate text value to avoid dropdown box
            return View();
        }

        // POST: customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCust,CustName,CustShortName,StartDate,EndDate,Number,AddressLine1,AddressLine2,idCity,idCounty,idPostCode")] customer customer, string postcodeText)
        {
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
                customer.idPostCode = getid.First().idPostCode;                         // set the idPostCode into the Database field             
                ModelState.Remove("postcode");                                          // reset the error message affecting the model state                             
            }
            //
            // Startdate - Enddate checking to overlap
            //
            valid = (customer.StartDate <= customer.EndDate) & (customer.EndDate > DateTime.Now);
            if (!valid)
            {
                ModelState.AddModelError("StartDate", "Start date after end date or end date too soon");
            }
            else
            {
                ModelState.Remove("StartDate");
            }
            if (ModelState.IsValid)
            {
                db.customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCity = new SelectList(db.citylists, "Cityid", "City", customer.idCity);  // rebuild select list
            ViewBag.idCounty = new SelectList(db.countylists, "idCountyList", "County", customer.idCounty);  // rebuild select list
            return View(customer);
        }

        // GET: customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCity = new SelectList(db.citylists, "Cityid", "City", customer.idCity);
            ViewBag.idCounty = new SelectList(db.countylists, "idCountyList", "County", customer.idCounty);
            ViewBag.postcodeText = customer.postcode.PostCode1; // populate text value to avoid dropdown box
           // ViewBag.idPostCode = new SelectList(db.postcodes, "idPostCode", "PostCode1", customer.idPostCode); 
            return View(customer);
        }

        // POST: customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCust,CustName,CustShortName,StartDate,EndDate,Number,AddressLine1,AddressLine2,idCity,idCounty")] customer customer,string postcodeText)
        {
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
                customer.idPostCode = getid.First().idPostCode;                         // set the idPostCode into the Database field             
                ModelState.Remove("postcode");                                          // reset the error message affecting the model state                             
            }
            //
            // Startdate - Enddate checking to overlap
            //
            valid = (customer.StartDate <= customer.EndDate) & (customer.EndDate > DateTime.Now);
            if(!valid)
            {
                ModelState.AddModelError("StartDate", "Start date after end date or end date too soon");
            }
            else{
                ModelState.Remove("StartDate");
            }
            if (ModelState.IsValid)                                                     // No changes to model state - so save the record and exit back to list
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCity = new SelectList(db.citylists, "Cityid", "City", customer.idCity); // rebuild select list
            ViewBag.idCounty = new SelectList(db.countylists, "idCountyList", "County", customer.idCounty); // rebuild select list
            return View(customer);
        }

        // GET: customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
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
