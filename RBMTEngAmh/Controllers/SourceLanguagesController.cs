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
    public class SourceLanguagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SourceLanguages
        public ActionResult Index()
        {
            return View(db.SourceWordFeeds.ToList());
        }

        // GET: SourceLanguages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordFeed wordFeed = db.SourceWordFeeds.Find(id);
            if (wordFeed == null)
            {
                return HttpNotFound();
            }
            return View(wordFeed);
        }

        // GET: SourceLanguages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SourceLanguages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WordFeed wordFeed)
        {
            if (ModelState.IsValid)
            {
                db.SourceWordFeeds.Add(wordFeed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wordFeed);
        }

        // GET: SourceLanguages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordFeed wordFeed = db.SourceWordFeeds.Find(id);
            if (wordFeed == null)
            {
                return HttpNotFound();
            }
            return View(wordFeed);
        }

        // POST: SourceLanguages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WordFeed wordFeed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wordFeed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wordFeed);
        }

        // GET: SourceLanguages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordFeed wordFeed = db.SourceWordFeeds.Find(id);
            if (wordFeed == null)
            {
                return HttpNotFound();
            }
            return View(wordFeed);
        }

        // POST: SourceLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            WordFeed wordFeed = db.SourceWordFeeds.Find(id);
            db.SourceWordFeeds.Remove(wordFeed);
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
