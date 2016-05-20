using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CommunityCounts.Models.Master;
using CommunityCounts.Global_Methods;

namespace CommunityCounts.Controllers.Master
{
    [Authorize(Roles = "systemAdmin,superAdmin,canManageSurveyResults")]
    public class C1surveysEnterController : Controller
    {
        private ccMaster db = new ccMaster(null);

        // 
        public ActionResult Index()
        {
            var c1surveys = from s in db.C1surveys where (s.active == true) && ((s.C1servicetypes.EndedDate == null) || (s.C1servicetypes.EndedDate >= System.DateTime.Now)) select s;
            return View(c1surveys.ToList().OrderByDescending(s=>s.idSurvey));
        }
        // 
        public ActionResult Clients(int id) // id passed is idSurvey
        {
            List<SurveyEnterList> sle = new List<SurveyEnterList>();
            string fname, lname, textQ, numericQ;

            var survey = db.C1surveys.Find(id);
            @ViewBag.SurveyName = survey.SurveyName;
            @ViewBag.SurveyID = id.ToString();
            @ViewBag.Activity = db.C1servicetypes.Find(survey.idServiceype).ServiceType;
            int idYear = CS.getRegYearId(db);   // get signed-in year to work with
            if (survey.forAllClients)
            {
                // for all clients
                var cl = db.C1client.Where(c=>c.idClient>1).Where(c=>c.idRegYear==idYear); // omit anonymous client initially 
                foreach (var c in cl.ToList())
                {
                    var scaRes = db.C1surressca.Where(s => s.idSurvey == survey.idSurvey).Where(s => s.idClient == c.idClient);
                    var scaTxt = db.C1surrestxt.Where(s => s.idSurvey == survey.idSurvey).Where(s => s.idClient == c.idClient);
                    fname = CS.unscramble(c.FirstName,c.scramble);
                    lname = CS.unscramble(c.LastName,c.scramble);
                    textQ = scaTxt.Count().ToString() + "/" + survey.numTxtQ.ToString();
                    numericQ = scaRes.Count().ToString() + "/" + survey.numScaQ.ToString();
                    sle.Add(new SurveyEnterList() { 
                        FirstName = fname, 
                        LastName = lname, 
                        PostCode = c.postcode.PostCode1 ,
                        idClient=c.idClient, 
                        idSurvey=survey.idSurvey,
                        numNQ = numericQ,
                        numTQ = textQ,
                        gotResults = ((scaTxt.Count()>0) || (scaRes.Count()>0)),
                        numericReq=(survey.numScaQ>0),
                        textReq=(survey.numTxtQ>0)
                    });
                }
            }
            else
            {
                // for all clients enrolled in activity idservicetype (despite typos on database schema!)
                var sl = db.C1service.Where(s => s.idServiceType == survey.idServiceype).Where(s=>s.C1client.idRegYear==idYear);
                foreach (var t in sl.ToList())
                {
                    var c = db.C1client.Find(t.idClient);
                    var scaRes = db.C1surressca.Where(s => s.idSurvey == survey.idSurvey).Where(s => s.idClient == c.idClient);
                    var scaTxt = db.C1surrestxt.Where(s => s.idSurvey == survey.idSurvey).Where(s => s.idClient == c.idClient);
                    fname = CS.unscramble(c.FirstName,c.scramble);
                    lname = CS.unscramble(c.LastName,c.scramble);
                    textQ = scaTxt.Count().ToString() + "/" + survey.numTxtQ.ToString();
                    numericQ = scaRes.Count().ToString() + "/" + survey.numScaQ.ToString();
                    sle.Add(new SurveyEnterList()
                    {
                        FirstName = fname,
                        LastName = lname,
                        PostCode = c.postcode.PostCode1,
                        idClient = c.idClient,
                        idSurvey = survey.idSurvey,
                        numNQ = numericQ,
                        numTQ = textQ,
                        gotResults = ((scaTxt.Count() > 0) || (scaRes.Count() > 0)),
                        numericReq = (survey.numScaQ > 0),
                        textReq = (survey.numTxtQ > 0)
                    });

                }

            }
            // do we need to add slot for anonymous clients responses?
            if (survey.forAnonymousClients)
            {
                var c = db.C1client.Find(1); // anonymous client is always id 1
                var scaRes = db.C1surressca.Where(s => s.idSurvey == survey.idSurvey).Where(s => s.idClient == c.idClient);
                var scaTxt = db.C1surrestxt.Where(s => s.idSurvey == survey.idSurvey).Where(s => s.idClient == c.idClient);
                fname = CS.unscramble(c.FirstName, c.scramble);
                lname = CS.unscramble(c.LastName, c.scramble);

                var tQ = 0;
                if (survey.numTxtQ > 0)
                {
                    tQ = scaTxt.Count() / survey.numTxtQ;
                }
                var nQ = 0;
                if (survey.numScaQ > 0)
                {
                    nQ = scaRes.Count() / survey.numScaQ;
                }
                textQ = "{" + tQ.ToString() + "}";
                numericQ = "{" + nQ.ToString() + "}";
                sle.Add(new SurveyEnterList()
                {
                    FirstName = fname,
                    LastName = lname,
                    PostCode = c.postcode.PostCode1,
                    idClient = c.idClient,
                    idSurvey = survey.idSurvey,
                    numNQ = numericQ,
                    numTQ = textQ,
                    gotResults = ((scaTxt.Count() > 0) || (scaRes.Count() > 0)),
                    numericReq = (survey.numScaQ > 0),
                    textReq = (survey.numTxtQ > 0)
                });
            }
            return View(sle.OrderBy(s=>s.LastName));
        }
        public ActionResult Results(int id, int clientid) // first id is idSurvey
        {
            var survey = db.C1surveys.Find(id);
            var client = db.C1client.Find(clientid);
            @ViewBag.Name = CS.unscramble(client.FirstName,client.scramble) + " " + CS.unscramble(client.LastName,client.scramble);
            @ViewBag.SurveyName = survey.SurveyName;
            List<SurveyResultsN> resultsList = new List<SurveyResultsN>();
            var scaledResponses = db.C1surressca.Where(s=>s.idSurvey==id).Where(s=>s.idClient==clientid).OrderBy(s=>s.ScaledQ);
            //
            // firstly, if this is not the special case of Anonymous persons, assign any responses already entered into the view list
            // Then, go back over the list to insert any questions not yet answered
            //
            if (clientid!=1)
            {
                foreach (var r in scaledResponses.ToList())
                {
                    var responseValue = db.refdatas.Find(r.IDResponse).RefCodeValue; // get the previous response value   
                    resultsList.Add(new SurveyResultsN()
                    {
                        idSurResSca = r.idSurResSca, // retrieve existing key
                        idClient = clientid,
                        idSurvey = id,
                        questionNum = r.ScaledQ,
                        response = responseValue
                    }); // add the child row !
                }
            }
            // thus, selecting to enter responses for Anonymous person always advances to a new record
            for (int i = 1; i <= survey.numScaQ;i++ )
            {
                var alreadyPresent = resultsList.Where(r=>r.questionNum==i).Count();
                if (alreadyPresent==0)
                {
                    resultsList.Add(new SurveyResultsN()    // key (idSurResSca) not specified so as to auto increment to support duplicates
                        {
                            idClient = clientid,
                            idSurvey = id,
                            questionNum = i,
                            response = ""
                        });
                }

            }
                return View(resultsList);
        }
        // POST: Results
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Results([Bind(Exclude="")] List<SurveyResultsN> sc)
        {
            int n;
            Boolean isnumeric;
            // validate the list of entered responses as a whole lot
            foreach (var i in sc)
            {
                isnumeric = int.TryParse(i.response, out n);
                if (!isnumeric)
                {
                    @ViewBag.ResultMessage = "All responses must be numbers";
                    return View(sc);
                }
                else
                {
                    if (n<1 | n > 7)
                    {
                        @ViewBag.ResultMessage = "All responses must be between 1 and 7 (lol!)";
                        return View(sc);
                    }
                }
            }
            // responses okay, save to database
            // Anonymous Client processing requires counter responseSeqNo
            int maxSeqNo = 0;
            int cl = sc.First().idClient;
            int su = sc.First().idSurvey;
            if (sc.First().idClient == 1) // anonymous client
            {
                var lastGroup = db.C1surressca.Where(c => c.idClient == cl).Where(c => c.idSurvey == su).OrderByDescending(c => c.responseSeqNo);
                if (lastGroup.Any())
                {
                    maxSeqNo = lastGroup.First().responseSeqNo + 1;
                }
                else
                {
                    maxSeqNo = 0;
                }
            }
            foreach (var i in sc)
            {
                var id = db.refdatas.Where(r=>r.RefCodeValue==i.response).Where(r=>r.RefCode=="SuGr").First().idRefData;
                if (i.idSurResSca==0)                // new result to be added, therefor autonum idSurResSca
                {
                    
                    db.C1surressca.Add(new C1surressca()
                    {
                        idSurvey = i.idSurvey,
                        idClient = i.idClient,
                        IDResponse = id,
                        ScaledQ = i.questionNum,
                        responseSeqNo = maxSeqNo
                    });
                }
                else
                 {
                     C1surressca row = db.C1surressca.Find(i.idSurResSca);
                     row.IDResponse=id;
                     db.Entry(row).State = EntityState.Modified;
                 }

                }
            db.SaveChanges();
            return RedirectToAction("Clients/" + sc.First().idSurvey); // pass idSurvey as key to list clients for survey screen
        }
        public ActionResult Text(int id, int clientid) // first id is idSurvey
        {
            var survey = db.C1surveys.Find(id);
            var client = db.C1client.Find(clientid);
            @ViewBag.Name = CS.unscramble(client.FirstName,client.scramble) + " " + CS.unscramble(client.LastName,client.scramble);
            @ViewBag.SurveyName = survey.SurveyName;
            //var resultsList = new SurveyResults() { idClient = clientid, idSurvey = id, results=new List<SurveyResultsN>()}; // create header row!
            List<SurveyResultsT> resultsList = new List<SurveyResultsT>();
            var Responses = db.C1surrestxt.Where(s => s.idSurvey == id).Where(s => s.idClient == clientid).OrderBy(s => s.TextQ);
            //
            // firstly, if this is not the special case of Anonymous persons, assign any responses already entered into the view list
            // Then, go back over the list to insert any questions not yet answered
            //
            if (clientid != 1)
            {
                foreach (var r in Responses.ToList())
                {
                    resultsList.Add(new SurveyResultsT()
                    {
                        idSurResTxt = r.idSurResTxt, // retrieve existing key
                        idClient = clientid,
                        idSurvey = id,
                        questionNum = r.TextQ,
                        response = r.Response
                    }); // add the child row !
                }
            }
            // thus, selecting to enter responses for Anonymous person always advances to a new record
            for (int i = 1; i <= survey.numTxtQ; i++)
            {
                var alreadyPresent = resultsList.Where(r => r.questionNum == CS.IntToLetters(i)).Count();
                if (alreadyPresent == 0)
                {
                    resultsList.Add(new SurveyResultsT()    // key (idSurResTxt) not specified so as to auto increment to support duplicates
                    {
                        idClient = clientid,
                        idSurvey = id,
                        questionNum = CS.IntToLetters(i), // scaled question responses are alphamumeric
                        response = ""
                    });
                }

            }
            return View(resultsList);
        }
        // POST: Text
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Text([Bind(Exclude = "")] List<SurveyResultsT> sc)
        {
            string r;
            int ctr = 1;
            // Anonymous Client processing requires counter responseSeqNo
            int maxSeqNo = 0;
            int cl = sc.First().idClient;
            int su = sc.First().idSurvey;
            //
            // check string length returned as not using entity framework model for the POST
            //
            foreach (var t in sc)
            {
                if ((t.response != null) && (t.response.Length> 255))
                {
                    ModelState.AddModelError("", "The length of the entered text in response " + CS.IntToLetters(ctr) + " exceeds the limit of 255 characters by " + (t.response.Length - 255).ToString() + " characters. Please shorten this.");
                    break;
                }
                else { ctr++; }
            }
            if (ModelState.IsValid)
            {
               
                if (sc.First().idClient == 1) // anonymous client
                {
                    var lastGroup = db.C1surrestxt.Where(c => c.idClient == cl).Where(c => c.idSurvey == su).OrderByDescending(c => c.responseSeqNo);
                    if (lastGroup.Any())
                    {
                        maxSeqNo = lastGroup.First().responseSeqNo + 1;
                    }
                    else
                    {
                        maxSeqNo = 0;
                    }
                }
                foreach (var i in sc)
                {
                    if (String.IsNullOrEmpty(i.response))
                    {
                        r = "No Comment";
                    }
                    else
                    {
                        r = i.response;
                    }
                    if (i.idSurResTxt == 0)                // new result to be added, therefor autonum idSurResSca
                    {
                        // text st
                        db.C1surrestxt.Add(new C1surrestxt()
                        {
                            idSurvey = i.idSurvey,
                            idClient = i.idClient,
                            Response = r,
                            TextQ = i.questionNum,
                            responseSeqNo = maxSeqNo
                        });
                    }
                    else
                    {
                        C1surrestxt row = db.C1surrestxt.Find(i.idSurResTxt);
                        row.Response = r;
                        db.Entry(row).State = EntityState.Modified;
                    }

                }
                db.SaveChanges();
                return RedirectToAction("Clients/" + sc.First().idSurvey); // pass idSurvey as key to list clients for survey screen
            }
            else
            {
                //
                // error re-display
                //
                var survey = db.C1surveys.Find(su);
                var client = db.C1client.Find(cl);
                @ViewBag.Name = CS.unscramble(client.FirstName, client.scramble) + " " + CS.unscramble(client.LastName, client.scramble);
                @ViewBag.SurveyName = survey.SurveyName;
                //var resultsList = new SurveyResults() { idClient = clientid, idSurvey = id, results=new List<SurveyResultsN>()}; // create header row!
                List<SurveyResultsT> resultsList = new List<SurveyResultsT>();
                var Responses = db.C1surrestxt.Where(s => s.idSurvey == su).Where(s => s.idClient == cl).OrderBy(s => s.TextQ);
                //
                // firstly, if this is not the special case of Anonymous persons, assign any responses already entered into the view list
                // Then, go back over the list to insert any questions not yet answered
                //
                if (cl != 1)
                {
                    foreach (var res in Responses.ToList())
                    {
                        resultsList.Add(new SurveyResultsT()
                        {
                            idSurResTxt = res.idSurResTxt, // retrieve existing key
                            idClient = cl,
                            idSurvey = su,
                            questionNum = res.TextQ,
                            response = res.Response
                        }); // add the child row !
                    }
                }
                // thus, selecting to enter responses for Anonymous person always advances to a new record
                for (int i = 1; i <= survey.numTxtQ; i++)
                {
                    var alreadyPresent = resultsList.Where(res => res.questionNum == CS.IntToLetters(i)).Count();
                    if (alreadyPresent == 0)
                    {
                        resultsList.Add(new SurveyResultsT()    // key (idSurResTxt) not specified so as to auto increment to support duplicates
                        {
                            idClient = cl,
                            idSurvey = su,
                            questionNum = CS.IntToLetters(i), // scaled question responses are alphamumeric
                            response = ""
                        });
                    }

                }
                return View(resultsList);
            } 
        }
        // GET: C1surveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1surveys c1surveys = db.C1surveys.Find(id);
            if (c1surveys == null)
            {
                return HttpNotFound();
            }
            return View(c1surveys);
        }
           // GET: Backup! - delete the most recently entered entry
    
    public ActionResult Delete(int id, int clientid) // idpassed is idsurvey and idclient
    {
        var surResSca = db.C1surressca.Where(s=>s.idSurvey==id).Where(s=>s.idClient==clientid).OrderByDescending(s=>s.responseSeqNo); // gets latest results group row off stack
        var surResTxt = db.C1surrestxt.Where(s=>s.idSurvey==id).Where(s=>s.idClient==clientid).OrderByDescending(s=>s.responseSeqNo); // gets latest results group row off stack
        var lastResSca = 0;
        var lastResTxt = 0;
        if (surResSca.Any())
        {
            lastResSca = surResSca.First().responseSeqNo;
        }
        if (surResTxt.Any())
        {
            lastResTxt = surResTxt.First().responseSeqNo;
        }
        var Sca = db.C1surressca.Where(s => s.idSurvey == id).Where(s => s.idClient == clientid).Where(s => s.responseSeqNo == lastResSca); // just this response block now
        List<surveyCombinedItems> sci = new List<surveyCombinedItems>();
        foreach (var i in Sca.ToList())
        {
            sci.Add(new surveyCombinedItems()
                {
                    idClient = i.idClient,
                    idSurvey = i.idSurvey,
                    idSurResNum = i.idSurResSca,
                    seqNo = i.ScaledQ.ToString(),
                    idSurResTxt = 0, // not a text response
                    value = db.refdatas.Find(i.IDResponse).RefCodeValue // actual response value
                });
        }
        var Txt = db.C1surrestxt.Where(s => s.idSurvey == id).Where(s => s.idClient == clientid).Where(s => s.responseSeqNo == lastResTxt); // just this response block now
        foreach (var i in Txt.ToList())
        {
            sci.Add(new surveyCombinedItems()
                { 
                    idClient = i.idClient,
                    idSurvey = i.idSurvey,
                    idSurResNum = 0, 
                    seqNo = i.TextQ,
                    idSurResTxt = i.idSurResTxt,
                    value = i.Response
        });
        }
        C1client c = db.C1client.Find(clientid);
        @ViewBag.FirstName = CS.unscramble(c.FirstName, c.scramble);
        @ViewBag.LastName = CS.unscramble(c.LastName, c.scramble);
        return View(sci.OrderBy(s=>s.seqNo).ToList());    
    }
        // Post: Backup! - delete the most recently entered entry
   
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult BackupConfirmed(List<surveyCombinedItems> s)
    {
        string surveyNum = s.First().idSurvey.ToString();
        foreach (var i in s)
        {
            if (i.idSurResTxt>0)
            {
                C1surrestxt rec = db.C1surrestxt.Find(i.idSurResTxt);
                db.C1surrestxt.Remove(rec);
            }
            if (i.idSurResNum>0)
            {
                C1surressca rec2 = db.C1surressca.Find(i.idSurResNum);
                db.C1surressca.Remove(rec2);
            }
        }
        db.SaveChanges();
        return RedirectToAction("Clients/" + surveyNum);
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
