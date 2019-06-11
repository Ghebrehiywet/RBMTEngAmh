using RBMTEngAmh.Logic;
using RBMTEngAmh.Models;
using RBMTEngAmh.Models.RBTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RBMTEngAmh.Controllers
{
    public class MorphimExtractionController : Controller
    {
        Morphology morphology = new Morphology();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MorphimExtraction
        public ActionResult Index()
        {
            var nouns = db.nouns;
            var verbs = db.verbs;

            return View();
        }

        [HttpPost]
        public ActionResult Index(string GivenWord, int? Language, FormCollection collection)
        {
            GivenWord = GivenWord.Trim();
            if (Language == 1)
            {
                var nounmorphology = morphology.EnglishMorphology4Nouns(db, GivenWord);
                ViewBag.Morphology4Nouns = nounmorphology;
            }
            if (Language == 2)
            {
                var nounmorphology = morphology.xAmharicMorphology4Nouns(db, GivenWord);
                ViewBag.Morphology4Nouns = nounmorphology;
            }

            ViewBag.GivenWord = GivenWord;
            return View();
        }

        // GET: MorphimExtraction/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MorphimExtraction/Create
        public ActionResult Create()
        {
            //var sss = new SelectList(db.WordFeeds, "id", "RootWord");
            //var ss = new SelectList(db.WordFeeds, "id", "RootWord");

            return View();
        }

        // POST: MorphimExtraction/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, WordFeed wordFeed)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (wordFeed.WordPOSType == WordPOSType.Noun)
                    {
                        Nouns nouns = new Nouns()
                        {
                            Regular = wordFeed.RootWord,
                            Irregular = wordFeed.RootWord,
                            IrregularPlural = wordFeed.IrregularPluralNoun,
                        };
                        db.nouns.Add(nouns);
                    }
                    else if (wordFeed.WordPOSType == WordPOSType.Verb)
                    {
                        Verbs verbs = new Verbs()
                        {
                            Irregular_past = wordFeed.IrregularPastVerb,
                            Irregular_PP = wordFeed.IrregularPPVerb,
                            Irregular_stem = wordFeed.RootWord,
                            Regular_stem = wordFeed.RootWord
                        };
                        db.verbs.Add(verbs);
                    }
                    else if (wordFeed.WordPOSType == WordPOSType.Adjective)
                    {
                        //db.verbs.Add(wordFeed);

                    }
                    db.SaveChanges();
                    return View("Index");
                }

                return View("Index", wordFeed);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: MorphimExtraction/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MorphimExtraction/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MorphimExtraction/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MorphimExtraction/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
