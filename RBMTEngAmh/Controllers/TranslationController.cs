using RBMTEngAmh.Logic;
using RBMTEngAmh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RBMTEngAmh.Controllers
{
    public class TranslationController : Controller
    {
        Morphology morphology = new Morphology();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Translation
        public ActionResult Index()
        {
            ViewBag.ResultDisplay = false;
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection, string sentences)
        {
            sentences = sentences.Replace('.', ' ');
            sentences = sentences.Trim();
            var listOfWords = sentences.Split(' ');

            string sourceLanguageRule = string.Empty;
            string targetLanguageRule = string.Empty;
            foreach (var word in listOfWords)
            {
                //  English
                var englishMorphology4Nouns = morphology.EnglishMorphology4Nouns(db, word);
                sourceLanguageRule += englishMorphology4Nouns.WordRule + "*";
                //  Amharic
                var amharicMorphology4Nouns = morphology.AmharicMorphology4Nouns(db, englishMorphology4Nouns.value);
                targetLanguageRule += englishMorphology4Nouns.WordRule.Replace(englishMorphology4Nouns.value, amharicMorphology4Nouns.RootWord) + "*";

            }
            ViewBag.SourceLanguageRule = sourceLanguageRule;
            ViewBag.TargetLanguageRule = targetLanguageRule;

            ViewBag.ResultDisplay = true;
            ViewBag.ResultDisplayMessage = "this is result";
            return View();
        }

        // GET: Translation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Translation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Translation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Translation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Translation/Edit/5
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

        // GET: Translation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Translation/Delete/5
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
