using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jobs4You.Models;

namespace Jobs4You.Controllers
{
    public class PaymentsController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: Payments
        public ActionResult Index()
        {
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

                    var payment = (from a in db.Payments
                                   where a.clientID == check
                                   where a.clientPayStat == 0
                                   select a).ToList();

                    return View(payment);

                }
                else if(Session["user_type"] == "Freelancer")
                {
                    int check = (int)Session["id"];
                    var payment = (from a in db.Payments
                                   where a.freelancerID == check
                                   where a.frlncrReceiveStat == 0
                                   
                                   select a).ToList();

                    return View(payment);
                }
                else if(Session["user_type"] == "Admin")
                {
                    var payments = db.Payments.Include(p => p.Client).Include(p => p.Freelancer).Include(p => p.job);
                    return View(payments.ToList());
                }
                
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username");
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username");
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "paymentID,clientID,jobID,clientPayStat,freelancerID,amount,frlncrReceiveStat")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", payment.clientID);
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", payment.freelancerID);
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details", payment.jobID);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", payment.clientID);
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", payment.freelancerID);
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details", payment.jobID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "paymentID,clientID,jobID,clientPayStat,freelancerID,amount,frlncrReceiveStat")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", payment.clientID);
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", payment.freelancerID);
            ViewBag.jobID = new SelectList(db.jobs, "jobID", "details", payment.jobID);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
