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
    public class TranslationRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TranslationRules
        public ActionResult Index()
        {
            return View(db.TranslationRules.ToList());
        }

        // GET: TranslationRules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TranslationRules translationRules = db.TranslationRules.Find(id);
            if (translationRules == null)
            {
                return HttpNotFound();
            }
            return View(translationRules);
        }

        // GET: TranslationRules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TranslationRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( TranslationRules translationRules)
        {
            if (ModelState.IsValid)
            {
                db.TranslationRules.Add(translationRules);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(translationRules);
        }

        // GET: TranslationRules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TranslationRules translationRules = db.TranslationRules.Find(id);
            if (translationRules == null)
            {
                return HttpNotFound();
            }
            return View(translationRules);
        }

        // POST: TranslationRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( TranslationRules translationRules)
        {
            if (ModelState.IsValid)
            {
                db.Entry(translationRules).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(translationRules);
        }

        // GET: TranslationRules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TranslationRules translationRules = db.TranslationRules.Find(id);
            if (translationRules == null)
            {
                return HttpNotFound();
            }
            return View(translationRules);
        }

        // POST: TranslationRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TranslationRules translationRules = db.TranslationRules.Find(id);
            db.TranslationRules.Remove(translationRules);
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
