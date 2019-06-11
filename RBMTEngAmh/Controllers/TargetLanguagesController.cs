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
    public class TargetLanguagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TargetLanguages
        public ActionResult Index()
        {
            var wordFeeds = db.TargetWordFeed.Include(t => t.WordFeeds);
            return View(wordFeeds.ToList());
        }

        // GET: TargetLanguages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetWordFeed targetWordFeed = db.TargetWordFeed.Find(id);
            if (targetWordFeed == null)
            {
                return HttpNotFound();
            }
            return View(targetWordFeed);
        }

        // GET: TargetLanguages/Create
        public ActionResult Create()
        {
            ViewBag.sourceId = new SelectList(db.SourceWordFeeds, "id", "RootWord");
            return View();
        }

        // POST: TargetLanguages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TargetWordFeed targetWordFeed)
        {
            if (ModelState.IsValid)
            {
                db.TargetWordFeed.Add(targetWordFeed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sourceId = new SelectList(db.SourceWordFeeds, "id", "RootWord", targetWordFeed.sourceId);
            return View(targetWordFeed);
        }

        // GET: TargetLanguages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetWordFeed targetWordFeed = db.TargetWordFeed.Find(id);
            if (targetWordFeed == null)
            {
                return HttpNotFound();
            }
            ViewBag.sourceId = new SelectList(db.SourceWordFeeds, "id", "RootWord", targetWordFeed.sourceId);
            return View(targetWordFeed);
        }

        // POST: TargetLanguages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TargetWordFeed targetWordFeed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(targetWordFeed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sourceId = new SelectList(db.SourceWordFeeds, "id", "RootWord", targetWordFeed.sourceId);
            return View(targetWordFeed);
        }

        // GET: TargetLanguages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TargetWordFeed targetWordFeed = db.TargetWordFeed.Find(id);
            if (targetWordFeed == null)
            {
                return HttpNotFound();
            }
            return View(targetWordFeed);
        }

        // POST: TargetLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TargetWordFeed targetWordFeed = db.TargetWordFeed.Find(id);
            db.TargetWordFeed.Remove(targetWordFeed);
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
