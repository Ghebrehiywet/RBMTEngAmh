using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RBMTEngAmh.Models;
using RBMTEngAmh.Models.RBTM;

namespace RBMTEngAmh.Controllers
{
    public class VerbsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Verbs
        public ActionResult Index()
        {
            return View(db.verbs.ToList());
        }

        // GET: Verbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Verbs verbs = db.verbs.Find(id);
            if (verbs == null)
            {
                return HttpNotFound();
            }
            return View(verbs);
        }

        // GET: Verbs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Verbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,reg_stem,irreg_stem,irreg_past,irret_PP")] Verbs verbs)
        {
            if (ModelState.IsValid)
            {
                db.verbs.Add(verbs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(verbs);
        }

        // GET: Verbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Verbs verbs = db.verbs.Find(id);
            if (verbs == null)
            {
                return HttpNotFound();
            }
            return View(verbs);
        }

        // POST: Verbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,reg_stem,irreg_stem,irreg_past,irret_PP")] Verbs verbs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(verbs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(verbs);
        }

        // GET: Verbs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Verbs verbs = db.verbs.Find(id);
            if (verbs == null)
            {
                return HttpNotFound();
            }
            return View(verbs);
        }

        // POST: Verbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Verbs verbs = db.verbs.Find(id);
            db.verbs.Remove(verbs);
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
