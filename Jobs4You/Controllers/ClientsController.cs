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
    public class ClientsController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }


        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Client client)
        {
            Client clients = db.Clients.Where(u => u.client_mail.Equals(client.client_mail)).FirstOrDefault();
            if (clients != null) {
                Content("Duplicate mail");
                return RedirectToAction("SignUp");
            }
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Login");
                //return RedirectToAction("Index");
            }

            return View(client);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(TempClient tempClient)
        {
            if (ModelState.IsValid)
            {
                var client = db.Clients.Where(c => c.client_mail.Equals(tempClient.client_mail)
                                && c.client_pass.Equals(tempClient.client_pass)).FirstOrDefault();

                if (client != null)
                {

                    //Login cookie
                    FormsAuthentication.SetAuthCookie(tempClient.client_mail,false);

                    //Login session
                    Session["id"] = client.clientID;
                    Session["user_mail"] = client.client_mail;
                    Session["user_name"] = client.client_name;
                    Session["user_rating"] = client.client_rating;
                    Session["user_phone"] = client.client_phone;
                    Session["user_username"] = client.client_username;
                    Session["user_stat"] = client.client_stat;


                    string clientMail = (string)Session["user_mail"];
                    string clientName= (string)Session["user_name"];
                    string clientRating= (string)Session["user_rating"];
                    string clientPhone= (string)Session["user_phone"];
                    string clientUsername= (string)Session["user_username"];
                    string clientStat = (string)Session["user_stat"];


                    Session["user_mail"] = clientMail;
                    Session["user_name"] = clientName;
                    Session["user_rating"] = clientRating;
                    Session["user_phone"] = clientPhone;
                    Session["user_username"] = clientUsername;
                    Session["user_stat"] = clientStat;
                    Session["user_type"] = "Client";



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
        public ActionResult Logout() {
            // for cookies logout
            FormsAuthentication.SignOut();

            // for session clear
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        // for profile section 
        public ActionResult Profile()
        {


            return View();
        }



        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "clientID,client_username,client_pass,client_phone,client_stat,client_rating,client_mail,client_name")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "clientID,client_username,client_pass,client_phone,client_stat,client_rating,client_mail,client_name")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
