using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "systemAdmin")] 
    public class FrigController : Controller
    {
        // GET: Frig
        public ActionResult Index()
        {
            var beginFrig = System.DateTime.Now;
            ccMaster db = new ccMaster(null);
            var clients = db.C1client.Where(cs=>cs.idClient>1);
            
            var c = 0;
            foreach (var client in clients.ToList())
            {
                client.FirstName = CS.unscramble(client.FirstName, client.scramble);   
                client.LastName = CS.unscramble(client.LastName, client.scramble);
                if (client.HouseNumber != null)
                { client.HouseNumber = CS.unscramble(client.HouseNumber, client.scramble); }
                client.AddressLine1 = CS.unscramble(client.AddressLine1, client.scramble);
                if (client.AddressLine2 != null)
                { client.AddressLine2 = CS.unscramble(client.AddressLine2, client.scramble); }
                if (client.Email != null)
                { client.Email = CS.unscramble(client.Email, client.scramble); }
                if (client.Phone != null)
                { client.Phone = CS.unscramble(client.Phone, client.scramble); }
                //
                // encrypt certain fields on client record
                //
                client.FirstName = CS.scramble(client.FirstName, true);
                client.LastName = CS.scramble(client.LastName, true);
                if (client.HouseNumber != null)
                { client.HouseNumber = CS.scramble(client.HouseNumber, true); }
                client.AddressLine1 = CS.scramble(client.AddressLine1, true);
                if (client.AddressLine2 != null)
                { client.AddressLine2 = CS.scramble(client.AddressLine2, true); }
                if (client.Email != null)
                { client.Email = CS.scramble(client.Email, true); }
                if (client.Phone != null)
                { client.Phone = CS.scramble(client.Phone, true); }
                //
                client.scramble = true;                       // always use scrambling instead of encryption (performance)

                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                c++;
            }
            var endFrig = System.DateTime.Now;
            @ViewBag.elapsedTime = endFrig - beginFrig;
            @ViewBag.numRecs = c;
   
            return View();
        }
    }
}