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
    public class AdminsController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();



        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Login");
                //return RedirectToAction("Index");
            }

            return View(admin);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TempAdmin tempAdmin)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admins.Where(c => c.admin_mail.Equals(tempAdmin.admin_mail)
                                && c.admin_pass.Equals(tempAdmin.admin_pass)).FirstOrDefault();

                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(tempAdmin.admin_mail, false);

                    Session["id"] = admin.adminID;
                    Session["user_mail"] = admin.admin_mail;
                    Session["user_username"] = admin.admin_username;


                    string adminMail = (string)Session["user_mail"];
                    string adminUsername = (string)Session["user_username"];


                    Session["user_mail"] = adminMail;
                    
                    Session["user_username"] = adminUsername;
                    Session["user_name"] = "";
                    Session["user_rating"] ="" ;
                    Session["user_phone"] = "";
                    Session["user_stat"] = "";
                    Session["user_type"] = "Admin";

                    return RedirectToAction("Index", "Home");
                    //return Content("Login Successful!");
                }
                else
                    return Content("Login Failed");
                //return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            // for cookies logout
            FormsAuthentication.SignOut();

            // for session clear
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }




        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        public ActionResult Profile()
        {
            return View();
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "adminID,admin_pass,admin_username,admin_mail")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "adminID,admin_pass,admin_username,admin_mail")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
