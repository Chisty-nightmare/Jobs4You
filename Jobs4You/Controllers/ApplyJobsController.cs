using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jobs4You.Models;

namespace Jobs4You.Controllers
{
    public class ApplyJobsController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: ApplyJobs
        public ActionResult Index()
        {
            // var applyJobs = db.ApplyJobs.Include(a => a.Freelancer).Include(a => a.job);

            
           
            if (User.Identity.IsAuthenticated)
            {
                if(Session["user_type"]=="Client")
                 {
                    int check = (int)Session["id"];
                    //    using (var ctx = new Jobs4youEntities1())
                    //  {
                    //   var applyJobs = ctx.ApplyJobs.SqlQuery("select * from ApplyJobs INNER JOIN jobs ON ApplyJobs.jobID = jobs.jobID where clientID ="+(int)Session["id"]).ToList<ApplyJob>();

                    //     return View(applyJobs.ToList());
                    //  }

                    var applyJobs = (from a in db.ApplyJobs
                                     join s in db.jobs on a.jobID equals s.jobID
                                     where s.clientID == check
                                     select a).ToList();

                    return View(applyJobs);

                  }
                else
                {
                    var applyJobs = db.ApplyJobs.Include(a => a.Freelancer).Include(a => a.job);
                    return View(applyJobs.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: ApplyJobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyJob applyJob = db.ApplyJobs.Find(id);
            if (applyJob == null)
            {
                return HttpNotFound();
            }
            return View(applyJob);
        }

        public ActionResult Confirm(int ?id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyJob job = db.ApplyJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                if (Session["user_type"] == "Client")
                {
                    int check = (int)Session["id"];
                    //    using (var ctx = new Jobs4youEntities1())
                    //  {
                    //   var applyJobs = ctx.ApplyJobs.SqlQuery("select * from ApplyJobs INNER JOIN jobs ON ApplyJobs.jobID = jobs.jobID where clientID ="+(int)Session["id"]).ToList<ApplyJob>();

                    //     return View(applyJobs.ToList());
                    //  }
                    var ap = from a in db.ApplyJobs
                             join s in db.jobs on a.jobID equals s.jobID
                             where s.clientID == check
                             where a.applyToken==id
                             select new
                             { // composit structure
                                 freelancerID = a.freelancerID,
                                 clientID = s.clientID,
                                 jobID = s.jobID,
                                 amount = s.pricing,
                             };
                    using (var ctx = new Jobs4youEntities1())
                    {
                        if (ap != null)
                        {
                            int jobid = 0;
                            int freeid = 0;

                            foreach (var i in ap)
                            {

                                ctx.Database.ExecuteSqlCommand("insert into Hires (clientID,freelancerID) values('" + i.clientID.ToString() + "','" + i.freelancerID.ToString() + "')");
                            }
                            foreach (var i in ap)
                            {

                                ctx.Database.ExecuteSqlCommand("insert into Payment (clientID,jobID,clientPayStat,freelancerID,amount,frlncrReceiveStat) " +
                                    "values('" + i.clientID.ToString() + "','" + i.jobID.ToString() + "',' 0 ','" + i.freelancerID.ToString() + "','" + i.amount.ToString() + "',' 0 ')");
                                
                                jobid = i.jobID;
                                freeid = i.freelancerID;

                            }



                            // ekhane age id ta variable e aina rakhbo linq diya pore insert korbo invoice e 

                            var invoiceInsert = from payment in db.Payments
                                                where payment.clientID == check
                                                where jobid == payment.jobID
                                                where payment.freelancerID == freeid
                                     select new
                                     { // result selector 
                                         paymentID = payment.paymentID,
                                         pricing = payment.amount,
                                         
                                     };

                            foreach (var i in invoiceInsert) 
                            {
                                ctx.Database.ExecuteSqlCommand("insert into Invoice (paymentID, pricing, details) " +
                                   "values('" + i.paymentID.ToString() + "','" + i.pricing.ToString() + "','Not Paid')");
                            }



                            ctx.Database.ExecuteSqlCommand("Delete from ApplyJobs where applyToken=" + id);
                        }

                    }
                    

                   // var applyJobs = (from a in db.ApplyJobs
                      //               join s in db.jobs on a.jobID equals s.jobID
                     //                where s.clientID == check
                     //                select a).ToList();

                    return RedirectToAction("Index", "ApplyJobs");

                }
                else
                {
                    //  var applyJobs = db.ApplyJobs.Include(a => a.Freelancer).Include(a => a.job);
                    //  return View(applyJobs.ToList());
                    return RedirectToAction("Index", "ApplyJobs");
                }
            }
            return RedirectToAction("Index", "ApplyJobs");
        }

        // GET: ApplyJobs/Create
        public ActionResult Create()
        {
           ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username");
           ViewBag.jobID = new SelectList(db.jobs, "jobID", "details");
            return View();
        }

        // POST: ApplyJobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "applyToken,jobID,freelancerID")] ApplyJob applyJob)
        {
            if (ModelState.IsValid)
            {
                db.ApplyJobs.Add(applyJob);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", applyJob.freelancerID);
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details", applyJob.jobID);
            return View(applyJob);
        }

        // GET: ApplyJobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyJob applyJob = db.ApplyJobs.Find(id);
            if (applyJob == null)
            {
                return HttpNotFound();
            }
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", applyJob.freelancerID);
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details", applyJob.jobID);
            return View(applyJob);
        }

        // POST: ApplyJobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "applyToken,jobID,freelancerID")] ApplyJob applyJob)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applyJob).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", applyJob.freelancerID);
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details", applyJob.jobID);
            return View(applyJob);
        }

        // GET: ApplyJobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyJob applyJob = db.ApplyJobs.Find(id);
            if (applyJob == null)
            {
                return HttpNotFound();
            }
            return View(applyJob);
        }

        // POST: ApplyJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplyJob applyJob = db.ApplyJobs.Find(id);
            db.ApplyJobs.Remove(applyJob);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Query([Bind(Include = "jobID,freelancerID")] ApplyJob applyJob)
        {

            if (User.Identity.IsAuthenticated)
            {


                if (ModelState.IsValid)
                {
                    applyJob.freelancerID = (int)Session["id"];

                    using (var ctx = new Jobs4youEntities1())
                    {
                        ctx.Database.ExecuteSqlCommand("insert into ApplyJobs (jobID,freelancerID) values('20000','40003')");

                    }

                    return RedirectToAction("Index");
                }

                // ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


           // return View();
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
