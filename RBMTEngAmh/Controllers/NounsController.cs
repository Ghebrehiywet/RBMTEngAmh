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
    public class NounsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Nouns
        public ActionResult Index()
        {
            return View(db.nouns.ToList());
        }

        // GET: Nouns/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nouns nouns = db.nouns.Find(id);
            if (nouns == null)
            {
                return HttpNotFound();
            }
            return View(nouns);
        }

        // GET: Nouns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nouns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Regular,Irregular,IrregularPlural")] Nouns nouns)
        {
            if (ModelState.IsValid)
            {
                db.nouns.Add(nouns);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nouns);
        }

        // GET: Nouns/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nouns nouns = db.nouns.Find(id);
            if (nouns == null)
            {
                return HttpNotFound();
            }
            return View(nouns);
        }

        // POST: Nouns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Regular,Irregular,IrregularPlural")] Nouns nouns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nouns).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nouns);
        }

        // GET: Nouns/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nouns nouns = db.nouns.Find(id);
            if (nouns == null)
            {
                return HttpNotFound();
            }
            return View(nouns);
        }

        // POST: Nouns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Nouns nouns = db.nouns.Find(id);
            db.nouns.Remove(nouns);
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
