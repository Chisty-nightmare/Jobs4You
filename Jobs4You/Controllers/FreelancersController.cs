using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Jobs4You.Models;

namespace Jobs4You.Controllers
{
    public class FreelancersController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: Freelancers
        public ActionResult Index()
        {
            return View(db.Freelancers.ToList());
        }

        // GET: Freelancers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freelancer freelancer = db.Freelancers.Find(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        // GET: Freelancers/Create
        /* public ActionResult Create()
         {
             return View();
         }*/


        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Freelancer freelancer)
        {
            if (ModelState.IsValid)
            {
                db.Freelancers.Add(freelancer);
                db.SaveChanges();
                return RedirectToAction("Login");
                //return RedirectToAction("Index");
            }

            return View(freelancer);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TempFreelancer tempFreelancer)
        {
            if (ModelState.IsValid)
            {
                var freelancer = db.Freelancers.Where(c => c.freelancer_mail.Equals(tempFreelancer.freelancer_mail)
                                && c.freelancer_pass.Equals(tempFreelancer.freelancer_pass)).FirstOrDefault();

                if (freelancer != null)
                {

                    //Login cookie
                    FormsAuthentication.SetAuthCookie(tempFreelancer.freelancer_mail, false);

                    //Login session
                    Session["id"] = freelancer.freelancerID;
                    Session["user_mail"] = freelancer.freelancer_mail;
                    Session["user_name"] = freelancer.freelancer_name;
                    Session["user_rating"] = freelancer.freelancer_rating;
                    Session["user_phone"] = freelancer.freelancer_phone;
                    Session["user_username"] = freelancer.freelancer_username;
                    Session["user_stat"] = freelancer.freelancer_stat;


                    string freelancerMail = (string)Session["user_mail"];
                    string freelancerName = (string)Session["user_name"];
                    string freelancerRating = (string)Session["user_rating"];
                    string freelancerPhone = (string)Session["user_phone"];
                    string freelancerUsername = (string)Session["user_username"];
                    string freelancerStat = (string)Session["user_stat"];


                    Session["user_mail"] = freelancerMail;
                    Session["user_name"] = freelancerName;
                    Session["user_rating"] = freelancerRating;
                    Session["user_phone"] = freelancerPhone;
                    Session["user_username"] = freelancerUsername;
                    Session["user_stat"] = freelancerStat;
                    Session["user_type"] = "Freelancer";


                    return RedirectToAction("Index", "Home");
                    //return Content("Login Successful!");
                }
                else
                    return Content("Login Failed");
                //return RedirectToAction("Index");
            }
            return View();
        }

        // for logout or Signout
        public ActionResult Logout()
        {
            // for cookies logout
            FormsAuthentication.SignOut();

            // for session clear
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Profile()
        {


            return View();
        }



        // POST: Freelancers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "freelancerID,freelancer_username,freelancer_pass,freelancer_phone,freelancer_stat,freelancer_rating,freelancer_mail,freelancer_name")] Freelancer freelancer)
        {
            if (ModelState.IsValid)
            {
                db.Freelancers.Add(freelancer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(freelancer);
        }

        // GET: Freelancers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freelancer freelancer = db.Freelancers.Find(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        // POST: Freelancers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "freelancerID,freelancer_username,freelancer_pass,freelancer_phone,freelancer_stat,freelancer_rating,freelancer_mail,freelancer_name")] Freelancer freelancer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(freelancer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(freelancer);
        }

        // GET: Freelancers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freelancer freelancer = db.Freelancers.Find(id);
            if (freelancer == null)
            {
                return HttpNotFound();
            }
            return View(freelancer);
        }

        // POST: Freelancers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Freelancer freelancer = db.Freelancers.Find(id);
            db.Freelancers.Remove(freelancer);
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
