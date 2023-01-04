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
    public class HiresController : Controller
    {
        private Jobs4youEntities1 db = new Jobs4youEntities1();

        // GET: Hires
        public ActionResult Index()
        {
            var hires = db.Hires.Include(h => h.Client).Include(h => h.Freelancer);
            return View(hires.ToList());
        }

        // GET: Hires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hire hire = db.Hires.Find(id);
            if (hire == null)
            {
                return HttpNotFound();
            }
            return View(hire);
        }

        // GET: Hires/Create
        public ActionResult Create()
        {
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username");
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username");
            return View();
        }

        // POST: Hires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HireID,clientID,freelancerID")] Hire hire)
        {
            if (ModelState.IsValid)
            {
                db.Hires.Add(hire);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", hire.clientID);
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", hire.freelancerID);
            return View(hire);
        }

        // GET: Hires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hire hire = db.Hires.Find(id);
            if (hire == null)
            {
                return HttpNotFound();
            }
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", hire.clientID);
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", hire.freelancerID);
            return View(hire);
        }

        // POST: Hires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HireID,clientID,freelancerID")] Hire hire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.clientID = new SelectList(db.Clients, "clientID", "client_username", hire.clientID);
            ViewBag.freelancerID = new SelectList(db.Freelancers, "freelancerID", "freelancer_username", hire.freelancerID);
            return View(hire);
        }

        // GET: Hires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hire hire = db.Hires.Find(id);
            if (hire == null)
            {
                return HttpNotFound();
            }
            return View(hire);
        }

        // POST: Hires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hire hire = db.Hires.Find(id);
            db.Hires.Remove(hire);
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
