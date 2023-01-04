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
    public class jobsController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: jobs
        public ActionResult Index()
        {
            var jobs = db.jobs.Include(j => j.Client);
            return View(jobs.ToList());
        }

        // GET: jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: jobs/Create
        public ActionResult Create()
        {
            //if (User.Identity.IsAuthenticated)
            //{
              //  if ((string)Session["user_type"] == "Clients")
              //  {
                    //var client = db.jobs.ToList().Where(d => d.clientID == (int)Session["id"]); 
                ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_mail");

               // }
         //   }

            return View();
        }

        // POST: jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jobID,clientID,pricing,details,duration")] job job)
        {
            if (User.Identity.IsAuthenticated)
            {


                if (ModelState.IsValid)
                {
                    job.clientID = (int)Session["id"];
                    db.jobs.Add(job);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

               // ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
                return View();
            }
            else
            {
               return RedirectToAction("Index", "Home");
            }

        }

        // GET: jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
            return View(job);
        }

        // POST: jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jobID,clientID,pricing,details,duration")] job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
            return View(job);
        }




        public ActionResult Apply(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            // ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
            if (User.Identity.IsAuthenticated)
            {


                if (ModelState.IsValid)
                {
                    // applyJob.freelancerID = (int)Session["id"];
                    int freelancerId = (int)Session["id"];
                    int jobId = job.jobID;

                    using (var ctx = new Jobs4youEntities1())
                    {
                        var jobs = ctx.ApplyJobs
                                        .SqlQuery("Select * from ApplyJobs where jobID="+jobId+" and freelancerID="+freelancerId)
                                        .FirstOrDefault();

                        if(jobs==null)
                        ctx.Database.ExecuteSqlCommand("insert into ApplyJobs (jobID,freelancerID) values('" + jobId.ToString() + "','" + freelancerId.ToString() + "')");
                    }
                    

                 //   using (var ctx = new Jobs4youEntities1())
                 //   {
                        

                  //  }

                    return RedirectToAction("Index", "ApplyJobs");
                }

                ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
                return RedirectToAction("Index", "ApplyJobs");
            }
            else
            {
                return RedirectToAction("Index", "ApplyJobs");
            }
        }

        // POST: jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.





        // GET: jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            job job = db.jobs.Find(id);
            db.jobs.Remove(job);
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
