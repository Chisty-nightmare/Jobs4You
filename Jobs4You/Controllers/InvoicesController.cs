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
    public class InvoicesController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: Invoices
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int check = (int)Session["id"];
                if (Session["user_type"] == "Client")
                {
                    
                    //    using (var ctx = new Jobs4youEntities1())
                    //  {
                    //   var applyJobs = ctx.ApplyJobs.SqlQuery("select * from ApplyJobs INNER JOIN jobs ON ApplyJobs.jobID = jobs.jobID where clientID ="+(int)Session["id"]).ToList<ApplyJob>();

                    //     return View(applyJobs.ToList());
                    //  }

                    var inv = (from a in db.Invoices
                                     join s in db.Payments on a.paymentID equals s.paymentID
                                     where s.clientID == check
                                     select a).ToList();

                    return View(inv);

                }
               else if (Session["user_type"] == "Freelancer")
                {

                    //    using (var ctx = new Jobs4youEntities1())
                    //  {
                    //   var applyJobs = ctx.ApplyJobs.SqlQuery("select * from ApplyJobs INNER JOIN jobs ON ApplyJobs.jobID = jobs.jobID where clientID ="+(int)Session["id"]).ToList<ApplyJob>();

                    //     return View(applyJobs.ToList());
                    //  }

                    var inv = (from a in db.Invoices
                                     join s in db.Payments on a.paymentID equals s.paymentID
                                     where s.freelancerID == check
                                     where a.details!="Not Paid"
                                     where s.frlncrReceiveStat!=1
                                     select a).ToList();



                    return View(inv);

                }
                else if (Session["user_type"] == "Admin")
                {
                     var invoices = db.Invoices.Include(i => i.Payment);
                     return View(invoices.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }
       
    

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }
        public ActionResult received(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice inv = db.Invoices.Find(id);
            if (inv == null)
            {
                return HttpNotFound();
            }
            // ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", job.clientID);
            if (User.Identity.IsAuthenticated)
            {


                if (ModelState.IsValid)
                {
                    if (Session["user_type"] == "Freelancer")
                    {
                        // applyJob.freelancerID = (int)Session["id"];
                        int freelancerId = (int)Session["id"];
                        var invID = inv.invoiceID;

                        using (var ctx = new Jobs4youEntities1())
                        {         
                                
                                ctx.Database.ExecuteSqlCommand("update Payment set frlncrReceiveStat='1' from Payment p inner join Invoice i  on p.paymentID=i.paymentID where i.invoiceID="+invID);
                        }


                        //   using (var ctx = new Jobs4youEntities1())
                        //   {


                        //  }

                        return RedirectToAction("Index", "Invoices");
                    }
                }
                return RedirectToAction("Index", "Invoices");
            }
            else
            {
                return RedirectToAction("Index", "Invoices");
            }
        }


        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.paymentID = new SelectList(db.Payments, "paymentID", "paymentID");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "invoiceID,paymentID,pricing,details")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.paymentID = new SelectList(db.Payments, "paymentID", "paymentID", invoice.paymentID);
            return View(invoice);
        }




        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.paymentID = new SelectList(db.Payments, "paymentID", "paymentID", invoice.paymentID);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "invoiceID,paymentID,pricing,details")] Invoice invoice)
        {
            
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
            {
                 //   var c = invoice.invoiceID;
//    var x = from i in db.Invoices
             //               where i.invoiceID == c
              //              select i;
               //     foreach (var w in x)
               //     {
               //         invoice.paymentID = w.paymentID;
               //     }
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                  //  var s = invoice.paymentID;


                    using (var ctx = new Jobs4youEntities1())
                       {
                        ctx.Database.ExecuteSqlCommand("update Payment set clientPayStat='1' from Payment p inner join Invoice i  on p.paymentID=i.paymentID where i.details!='Not Paid'");

                        }

                        return View(invoice);
                    
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

                
            }
           // ViewBag.paymentID = new SelectList(db.Payments, "paymentID", "paymentID", invoice.paymentID);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
