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
    public class TranslationController : Controller
    {
        Morphology morphology;
        private ApplicationDbContext db = new ApplicationDbContext();

        public TranslationController()
        {
            morphology = new Morphology(db);
        }
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

            string sourceLanguageRule = "";
            string targetLanguageRule = "";
            List<string> rulesOrder = new List<string>();
            Dictionary<string, TargetWordFeed> targetWordFeed = new Dictionary<string, TargetWordFeed>();
            Gender gender = 0;
            foreach (var word in listOfWords)
            {
                TargetWordFeed amharicMorphology = new TargetWordFeed();

                //  English
                var nouns = morphology.EnglishMorphology4Nouns(word);
                if (nouns?.WordRule is null)
                {

                    var verbs = morphology.EnglishMorphology4Verbs(word);
                    if (verbs?.WordRule is null)
                    {
                        var prepostion = morphology.EnglishMorphology4Prepostion(word);
                        if (prepostion?.WordRule is null)
                        {
                            var pronouns = morphology.EnglishMorphology4Pronouns(word);
                            if (pronouns?.WordRule is null)
                            {
                                var adverbs = morphology.EnglishMorphology4Adverbs(word);
                                if (adverbs?.WordRule is null)
                                {
                                    var adjectives = morphology.EnglishMorphology4Adjectives(word);
                                    if (adjectives?.WordRule is null)
                                    {
                                        var conjuctions = morphology.EnglishMorphology4Conjunctions(word);
                                        if (conjuctions?.WordRule is null)
                                        {
                                            var disjunctions = morphology.EnglishMorphology4Disjunctions(word);
                                            if (disjunctions?.WordRule is null)
                                            {
                                                //  Unknown word
                                                disjunctions = new WordPropNoun();
                                                disjunctions.WordRule = "[" + word + "]";

                                                sourceLanguageRule += "[" + word + "]" + "*";
                                                targetLanguageRule += "[" + word + "]" + "*";

                                                TargetWordFeed unknownTargetWordFeed = new TargetWordFeed();
                                                unknownTargetWordFeed.WordRule = disjunctions.WordRule;
                                                unknownTargetWordFeed.Translated = disjunctions.WordRule;
                                                unknownTargetWordFeed.RootWord = word;
                                                //sourceLanguageRule += disjunctions?.WordRule + "*";
                                                //  Amharic
                                                //var amharicMorphology4Disjunctions = morphology.AmharicMorphology4Disjunctions(disjunctions?.RootWord);
                                                //amharicMorphology4Disjunctions.WordRule = ((amharicMorphology4Disjunctions != null) ? disjunctions.WordRule.Replace(disjunctions.RootWord, amharicMorphology4Disjunctions?.RootWord) : disjunctions.WordRule);
                                                //targetLanguageRule += amharicMorphology4Disjunctions.WordRule + "*";

                                                var indxofOpening = disjunctions.WordRule.IndexOf('[');
                                                var indxofClosing = disjunctions.WordRule.IndexOf(']');
                                                if (indxofClosing != -1 && indxofOpening != -1)
                                                {
                                                    var pos = disjunctions.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                                    int index = 0;
                                                    string posString = pos;
                                                    while (rulesOrder.Contains(pos))
                                                    {
                                                        ++index;
                                                        pos = posString.Insert(posString.Length - 1, index.ToString());
                                                    };
                                                    rulesOrder.Add(pos);
                                                    targetWordFeed.Add(pos, unknownTargetWordFeed);
                                                }
                                            }
                                            else
                                            {
                                                sourceLanguageRule += disjunctions?.WordRule + "*";
                                                //  Amharic
                                                var amharicMorphology4Disjunctions = morphology.AmharicMorphology4Disjunctions(disjunctions?.RootWord);
                                                amharicMorphology4Disjunctions.WordRule = ((amharicMorphology4Disjunctions != null) ? disjunctions.WordRule.Replace(disjunctions.RootWord, amharicMorphology4Disjunctions?.RootWord) : disjunctions.WordRule);
                                                targetLanguageRule += amharicMorphology4Disjunctions.WordRule + "*";

                                                var indxofOpening = disjunctions.WordRule.IndexOf('[');
                                                var indxofClosing = disjunctions.WordRule.IndexOf(']');
                                                if (indxofClosing != -1 && indxofOpening != -1)
                                                {
                                                    var pos = disjunctions.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                                    int index = 0;
                                                    string posString = pos;
                                                    while (rulesOrder.Contains(pos))
                                                    {
                                                        ++index;
                                                        pos = posString.Insert(posString.Length - 1, index.ToString());
                                                    };
                                                    rulesOrder.Add(pos);
                                                    targetWordFeed.Add(pos, amharicMorphology4Disjunctions);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            sourceLanguageRule += conjuctions?.WordRule + "*";
                                            //  Amharic
                                            var amharicMorphology4Conjunctions = morphology.AmharicMorphology4Conjunctions(conjuctions?.RootWord);
                                            amharicMorphology4Conjunctions.WordRule = ((amharicMorphology4Conjunctions != null) ? conjuctions.WordRule.Replace(conjuctions.RootWord, amharicMorphology4Conjunctions?.RootWord) : conjuctions.WordRule);
                                            targetLanguageRule += amharicMorphology4Conjunctions.WordRule + "*";

                                            var indxofOpening = conjuctions.WordRule.IndexOf('[');
                                            var indxofClosing = conjuctions.WordRule.IndexOf(']');
                                            if (indxofClosing != -1 && indxofOpening != -1)
                                            {
                                                var pos = conjuctions.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                                int index = 0;
                                                string posString = pos;
                                                while (rulesOrder.Contains(pos))
                                                {
                                                    ++index;
                                                    pos = posString.Insert(posString.Length - 1, index.ToString());
                                                };
                                                rulesOrder.Add(pos);
                                                targetWordFeed.Add(pos, amharicMorphology4Conjunctions);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sourceLanguageRule += adjectives?.WordRule + "*";
                                        //  Amharic
                                        var amharicMorphology4Adjectives = morphology.AmharicMorphology4Adjectives(adjectives?.RootWord);
                                        amharicMorphology4Adjectives.WordRule = ((amharicMorphology4Adjectives != null) ? adjectives.WordRule.Replace(adjectives.RootWord, amharicMorphology4Adjectives?.RootWord) : adjectives.WordRule);
                                        targetLanguageRule += amharicMorphology4Adjectives.WordRule + "*";

                                        var indxofOpening = adjectives.WordRule.IndexOf('[');
                                        var indxofClosing = adjectives.WordRule.IndexOf(']');
                                        if (indxofClosing != -1 && indxofOpening != -1)
                                        {
                                            var pos = adjectives.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                            int index = 0;
                                            string posString = pos;
                                            while (rulesOrder.Contains(pos))
                                            {
                                                ++index;
                                                pos = posString.Insert(posString.Length - 1, index.ToString());
                                            };
                                            rulesOrder.Add(pos);
                                            targetWordFeed.Add(pos, amharicMorphology4Adjectives);
                                        }
                                    }
                                }
                                else
                                {
                                    sourceLanguageRule += adverbs?.WordRule + "*";
                                    //  Amharic
                                    var amharicMorphology4Adverbs = morphology.AmharicMorphology4Adverbs(adverbs?.RootWord);
                                    amharicMorphology4Adverbs.WordRule = ((amharicMorphology4Adverbs != null) ? adverbs.WordRule.Replace(adverbs.RootWord, amharicMorphology4Adverbs?.RootWord) : adverbs.WordRule);
                                    targetLanguageRule += amharicMorphology4Adverbs.WordRule + "*";

                                    var indxofOpening = adverbs.WordRule.IndexOf('[');
                                    var indxofClosing = adverbs.WordRule.IndexOf(']');
                                    if (indxofClosing != -1 && indxofOpening != -1)
                                    {
                                        var pos = adverbs.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                        int index = 0;
                                        string posString = pos;
                                        while (rulesOrder.Contains(pos))
                                        {
                                            ++index;
                                            pos = posString.Insert(posString.Length - 1, index.ToString());
                                        };
                                        rulesOrder.Add(pos);
                                        targetWordFeed.Add(pos, amharicMorphology4Adverbs);
                                    }
                                }
                            }
                            else
                            {
                                sourceLanguageRule += pronouns?.WordRule + "*";
                                //  Amharic
                                var amharicMorphology4Pronouns = morphology.AmharicMorphology4Pronouns(pronouns?.RootWord);
                                amharicMorphology4Pronouns.WordRule = ((amharicMorphology4Pronouns != null) ? pronouns.WordRule.Replace(pronouns.RootWord, amharicMorphology4Pronouns?.RootWord) : pronouns.WordRule);
                                targetLanguageRule += amharicMorphology4Pronouns.WordRule + "*";

                                var indxofOpening = pronouns.WordRule.IndexOf('[');
                                var indxofClosing = pronouns.WordRule.IndexOf(']');
                                if (indxofClosing != -1 && indxofOpening != -1)
                                {
                                    var pos = pronouns.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                    int index = 0;
                                    string posString = pos;
                                    while (rulesOrder.Contains(pos))
                                    {
                                        ++index;
                                        pos = posString.Insert(posString.Length - 1, index.ToString());
                                    };
                                    rulesOrder.Add(pos);
                                    targetWordFeed.Add(pos, amharicMorphology4Pronouns);
                                }
                            }
                        }
                        else
                        {
                            sourceLanguageRule += prepostion?.WordRule + "*";
                            //  Amharic
                            var amharicMorphology4Prepostion = morphology.AmharicMorphology4Prepostion(prepostion?.RootWord);
                            amharicMorphology4Prepostion.WordRule = ((amharicMorphology4Prepostion != null) ? prepostion.WordRule.Replace(prepostion.RootWord, amharicMorphology4Prepostion?.RootWord) : prepostion.WordRule);
                            targetLanguageRule += amharicMorphology4Prepostion.WordRule + "*";

                            var indxofOpening = prepostion.WordRule.IndexOf('[');
                            var indxofClosing = prepostion.WordRule.IndexOf(']');
                            if (indxofClosing != -1 && indxofOpening != -1)
                            {
                                var pos = prepostion.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                                int index = 0;
                                string posString = pos;
                                while (rulesOrder.Contains(pos))
                                {
                                    ++index;
                                    pos = posString.Insert(posString.Length - 1, index.ToString());
                                };
                                rulesOrder.Add(pos);
                                targetWordFeed.Add(pos, amharicMorphology4Prepostion);
                            }
                        }
                    }
                    else
                    {
                        sourceLanguageRule += verbs?.WordRule + "*";
                        //  Amharic
                        var amharicMorphology4Verbs = morphology.AmharicMorphology4Verbs(verbs?.RootWord);
                        verbs.Gender = (gender != 0) ? gender : verbs.Gender; ;
                        if (verbs.number == Number.Plural)
                        {
                            if (verbs.Type == WordType.Regular)
                            {
                                var wordPropReg = morphology.AmharicConstructMorphology4Verbs(verbs, word);
                                amharicMorphology.Translated = verbs.RootWordPlural = wordPropReg?.RootWordPlural;
                                amharicMorphology.RootWord = wordPropReg?.RootWordPlural;
                            }
                            else
                            {
                                var wordPropIrReg = morphology.AmharicConstructMorphology4Verbs(verbs, word);
                                amharicMorphology.Translated = wordPropIrReg?.Translated;
                                amharicMorphology.RootWord = wordPropIrReg?.RootWord;
                            }
                        }
                        else
                        {
                            var wordPropIrReg = morphology.AmharicConstructMorphology4Verbs(verbs, word);
                            amharicMorphology.Translated = wordPropIrReg?.Translated;
                            amharicMorphology.RootWord = wordPropIrReg?.RootWord;

                            //amharicMorphology.Translated = amharicMorphology4Verbs.RootWord;
                        }

                        amharicMorphology4Verbs.WordRule = ((amharicMorphology4Verbs != null) ? verbs.WordRule.Replace(verbs.RootWord, amharicMorphology4Verbs?.RootWord ?? "") : verbs.WordRule);
                        targetLanguageRule += amharicMorphology4Verbs.WordRule + "*";
                        amharicMorphology.WordRule = amharicMorphology4Verbs.WordRule;

                        var indxofOpening = verbs.WordRule.IndexOf('[');
                        var indxofClosing = verbs.WordRule.IndexOf(']');
                        if (indxofClosing != -1 && indxofOpening != -1)
                        {
                            var pos = verbs.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                            int index = 0;
                            string posString = pos;
                            while (rulesOrder.Contains(pos))
                            {
                                ++index;
                                pos = posString.Insert(posString.Length - 1, index.ToString());
                            };
                            rulesOrder.Add(pos);
                            targetWordFeed.Add(pos, amharicMorphology);
                        }
                    }

                }
                else
                {
                    gender = nouns.Gender;
                    sourceLanguageRule += nouns.WordRule + "*";
                    //  Amharic
                    amharicMorphology = morphology.AmharicMorphology4Nouns(nouns.RootWord);

                    if (nouns.number == Number.Plural)
                    {
                        if (nouns.Type == WordType.Regular)
                        {
                            var wordPropReg = morphology.AmharicConstructMorphology4Nouns(nouns, nouns.RootWordPlural);
                            amharicMorphology.Translated = nouns.RootWordPlural = wordPropReg?.RootWordPlural;
                            amharicMorphology.RootWord = wordPropReg.RootWordPlural;
                        }
                        else
                        {
                            var wordPropIrReg = morphology.AmharicConstructMorphology4Nouns(nouns, nouns.PluralForIrregular);
                            amharicMorphology.Translated = nouns.PluralForIrregular = wordPropIrReg.RootWordPlural;
                            amharicMorphology.RootWord = wordPropIrReg.RootWordPlural;
                        }
                    }
                    else
                    {
                        amharicMorphology.Translated = amharicMorphology.RootWord;
                    }

                    amharicMorphology.WordRule = (nouns.WordRule.Replace(nouns.RootWord, amharicMorphology?.RootWord));
                    targetLanguageRule += amharicMorphology.WordRule + "*";

                    var indxofOpening = nouns.WordRule.IndexOf('[');
                    var indxofClosing = nouns.WordRule.IndexOf(']');
                    if (indxofClosing != -1 && indxofOpening != -1)
                    {
                        var pos = nouns.WordRule.Substring(indxofOpening, (indxofClosing - indxofOpening) + 1);
                        int index = 0;
                        string posString = pos;
                        while (rulesOrder.Contains(pos))
                        {
                            ++index;
                            pos = posString.Insert(posString.Length - 1, index.ToString());
                        };
                        rulesOrder.Add(pos);
                        targetWordFeed.Add(pos, amharicMorphology);
                    }
                }
            }

            //  Fetch Re-Order rule
            var reorderRule = morphology.ReOrderRules(string.Join("+", rulesOrder));

            //  Construct Sentences
            String reOrderedSentences = "";
            String translatedSentences = "";
            var rulesSplit = ((string.IsNullOrEmpty(reorderRule?.AmharicRules) || string.IsNullOrWhiteSpace(reorderRule?.AmharicRules)) ?
                string.Join("+", rulesOrder) : reorderRule?.AmharicRules)
                .Split('+');
            if (rulesSplit != null)
            {
                foreach (var rule in rulesSplit)
                {
                    TargetWordFeed wordFeed = targetWordFeed.Where(item => item.Key == rule.Trim()).FirstOrDefault().Value;
                    reOrderedSentences += wordFeed?.WordRule + "*";
                    translatedSentences += (wordFeed?.Translated ?? wordFeed?.WordRule) + '*';
                }
            }

            ViewBag.SourceLanguageRule = sourceLanguageRule;
            ViewBag.SourceRulesOrder = string.Join("+", rulesOrder ?? new List<string>());
            ViewBag.TargetLanguageRule = targetLanguageRule;
            ViewBag.TargetRulesOrder = string.Join("+", rulesOrder);
            ViewBag.ReOrderedSentences = reOrderedSentences;
            ViewBag.OutPutMessage = translatedSentences;

            ViewBag.ResultDisplay = true;
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
