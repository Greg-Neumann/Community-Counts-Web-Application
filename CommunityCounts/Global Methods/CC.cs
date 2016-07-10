using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using CommunityCounts.Models.Master;


namespace CommunityCounts.Global_Methods
{
    public static class CS
    {
        public static string getRegYear(ccMaster db, Boolean fullDate)
        {
            //
            // set up the business registration year for this user. Defaults to current business year if none specified or orverrided
            //
            var userName = HttpContext.Current.User.Identity.Name;
            var us = db.users.Where(u => u.Email == userName);
            if (us.Any())
            {
                if (fullDate)
                {
                    return db.regyears.Find((us.First().idRegYear)).EndDate.ToShortDateString();
                }
                else
                {
                    return db.regyears.Find((us.First().idRegYear)).RegYear1;
                }
            }
            var currentDate = DateTime.Today;
            var reg = db.regyears.Where(r => r.StartDate <= currentDate).Where(r => r.EndDate >= currentDate);
            if (!reg.Any())
            {
                throw new Exception("No control data in RegYears table for todays date");
            }
            if (fullDate)
            {
                return reg.First().EndDate.ToShortDateString();
            }
            else
            {
                return reg.First().RegYear1;
            }
        }
        public static int getRegYearId(ccMaster db)
        {
            //
            // set up the business registration year id for this user. Defaults to current business year if none specified or orverrided
            //
            var userName = HttpContext.Current.User.Identity.Name;
            var us = db.users.Where(u => u.Email == userName);
            if (us.Any())
            {
                return us.First().idRegYear;
            }
            var currentDate = DateTime.Today;
            var reg = db.regyears.Where(r => r.StartDate <= currentDate).Where(r => r.EndDate >= currentDate);
            if (!reg.Any())
            {
                throw new Exception("No control data in RegYears table for todays date");
            }
            return reg.First().idRegYear;
        }
        public static void addJourneyChild(List<activityList> servicesList, int idClient, int idService, DateTime StartedDate, int journeyDepth, ccMaster db)
        {
            var childList = from j in db.C1journeys.Where(j => j.OrigidService == idService) select new { j.JourneyedidService };
            string JrnyCat;
            List<clist> childunsorted = new List<clist>();
            foreach (var i in childList.ToList()) // build up names of service type for sort.
            {
                var ServiceType = db.C1servicetypes.Find(db.C1service.Find(i.JourneyedidService).idServiceType).ServiceType;
                childunsorted.Add(new clist
                {
                    journeydedidService = i.JourneyedidService, 
                    serviceTypeName = ServiceType
                });
            }
            var child = childunsorted.OrderBy(c => c.serviceTypeName); // sort records by text service name.
            activityList childItem = new activityList();
            string indentString = "=>";
            for (int j = 0; j <= journeyDepth; j++ )
            {
                indentString = "&nbsp;&nbsp;&nbsp;&nbsp;" + indentString;
            }
                foreach (var item in child.ToList())
                {
                    var serviceRec = db.C1service.Find(item.journeydedidService); // get child service record
                    if (serviceRec.JourneyedidCategory == 0) // system reserved value
                    {
                        JrnyCat = "";
                    }
                    else
                    {
                        JrnyCat = serviceRec.C1journeycat.CatName;
                    }
                    servicesList.Add(new activityList { 
                        idClient= idClient,
                        idService=serviceRec.idService,
                        idServiceType=serviceRec.idServiceType,
                        journeyDepth = journeyDepth + 1, // nested one level deeper
                        CreatedDate = serviceRec.CreateDate,
                        ServiceType = indentString + db.C1servicetypes.Find(serviceRec.idServiceType).ServiceType,
                        StartedDate = serviceRec.StartedDate,
                        EndedDate = serviceRec.EndedDate,
                        JrnyCatName = JrnyCat
                    });
                    addJourneyChild(servicesList, idClient, serviceRec.idService, serviceRec.StartedDate, journeyDepth+1, db); // use recursion to dig deeper for this service!
                }
        }
        public static string IntToLetters(int value)
        {
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }

        public static bool userHasNews(ccMaster  db) // passed the database context for the Customer
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var us = db.users.Where(u => u.Email == userName);
            bool nameIsPresent = us.Any();
            if (nameIsPresent)
            {
                return !us.First().readNews; // returns user declared status for this new stack
            }
            else
            {
                user newRec = new user();
                newRec.Email = userName;
                newRec.UserShortName = " ";
                newRec.readNews = false;
                int idYear = CS.getRegYearId(db);
                newRec.idRegYear = idYear;
                db.users.Add(newRec); // insert username into users table (for next time) and set read to false
                db.SaveChanges();
                return true;
            }
        }
        public static string simple_scramble (string input)
        {
            char[] forwards_table = new char[] { 
                (char)64,(char)89,(char)76,(char)41,(char)122,(char)35,(char)119,(char)32,(char)51,(char)67,(char)101,(char)94,(char)59,(char)37,(char)53,
                (char)126,(char)33,(char)105,(char)57,(char)81,(char)79,(char)38,(char)83,(char)113,(char)46,(char)97,(char)110,(char)124,(char)43,(char)86,
                (char)62,(char)34,(char)73,(char)49,(char)123,(char)108,(char)92,(char)47,(char)116,(char)120,(char)36,(char)103,(char)82,(char)42,(char)56,
                (char)74,(char)115,(char)48,(char)87,(char)75,(char)39,(char)117,(char)71,(char)50,(char)125,(char)40,(char)72,(char)80,(char)121,(char)44,
                (char)77,(char)61,(char)104,(char)96,(char)90,(char)45,(char)69,(char)93,(char)60,(char)99,(char)112,(char)118,(char)98,(char)55,(char)95,
                (char)100,(char)106,(char)63,(char)111,(char)66,(char)65,(char)70,(char)85,(char)78,(char)52,(char)102,(char)109,(char)58,(char)114,(char)68,
                (char)54,(char)88,(char)91,(char)107,(char)84};
            int l = input.Length - 1;
            StringBuilder i_String = new StringBuilder(input);
            StringBuilder o = new StringBuilder();
            for (int c=l;c>=0;c--)
            {
                int i = (int)i_String[c] - 32; // cast ordinal (ASCII) value and adjust for array index lookup such that ascii 32 (space) is first element of array   
                if ((i<0) || (i>94))
                {
                    var omsg = String.Format("Data scrambling illegal index for ASCII value {0} (DEC) on input '{1}'", i, input);
                    throw new InvalidConstraintException(omsg);
                }
                o = o.Append(forwards_table[i]);
            }
            return o.ToString();
        }
        public static string simple_unscramble (string input)
        {
            char[] reverse_table = new char[] {
                (char)39,(char)48,(char)63,(char)37,(char)72,(char)45,(char)53,(char)82,(char)87,(char)35,(char)75,(char)60,(char)91,(char)97,(char)56,
                (char)69,(char)79,(char)65,(char)85,(char)40,(char)116,(char)46,(char)122,(char)105,(char)76,(char)50,(char)119,(char)44,(char)100,(char)93,
                (char)62,(char)109,(char)32,(char)112,(char)111,(char)41,(char)121,(char)98,(char)113,(char)84,(char)88,(char)64,(char)77,(char)81,(char)34,
                (char)92,(char)115,(char)52,(char)89,(char)51,(char)74,(char)54,(char)126,(char)114,(char)61,(char)80,(char)123,(char)33,(char)96,(char)124,
                (char)68,(char)99,(char)43,(char)106,(char)95,(char)57,(char)104,(char)101,(char)107,(char)42,(char)117,(char)73,(char)94,(char)49,(char)108,
                (char)125,(char)67,(char)118,(char)58,(char)110,(char)102,(char)55,(char)120,(char)78,(char)70,(char)83,(char)103,(char)38,(char)71,(char)90,
                (char)36,(char)66,(char)59,(char)86,(char)47};
            int l = input.Length - 1;
            StringBuilder i_String = new StringBuilder(input);
            StringBuilder o = new StringBuilder();
            for (int c = l; c >= 0; c--)
            {
                int i = (int)i_String[c] - 32; // cast ordinal (ASCII) value and adjust for array index lookup such that ascii 32 (space) is first element of array   
                if ((i < 0) || (i > 94))
                {
                    var omsg = String.Format("Data Unscrambling illegal index for ASCII value {0} (DEC) on input '{1}'", i, input);
                    throw new InvalidConstraintException(omsg);
                }
                o = o.Append(reverse_table[i]);
            }
            return o.ToString();
        }
        public static string scramble (string input, Boolean scramble)
        {
            //
            // encrypt or scramble?
            //
            if (scramble)
            {
                return simple_scramble(input);
            }
            else
            {
                return Encrypt.scrambledText(input);
            }
        }
        public static string unscramble (string input, Boolean scramble)
        {
            //
            // decrypt or unscramble?
            //
            if (scramble)
            {
                return simple_unscramble(input);
            }
            else
            {
                return Encrypt.unscrambleText(input);
            }
        }
    }

}