using RBMTEngAmh.Models;
using RBMTEngAmh.Models.RBTM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Logic
{
    public class Morphology
    {
        private ApplicationDbContext _db;
        public Morphology(ApplicationDbContext db)
        {
            this._db = db;
        }

        public WordPropNoun EnglishMorphology4Nouns(string GivenWord)
        {
            #region Noun Morphology Region

            List<WordPropNoun> lstWordProp = new List<WordPropNoun>();

            var nouns = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Noun).ToList();
            if (nouns != null)
            {
                foreach (var noun in nouns)
                {
                    WordPropNoun wordProp = new WordPropNoun();
                    wordProp.WordPOSType = WordPOSType.Noun;
                    wordProp.Gender = noun.Gender;
                    if (noun.WordType == WordType.Irregular)
                    {
                        wordProp.Type = WordType.Irregular;
                        wordProp.RootWord = noun.RootWord;
                        if (!string.IsNullOrEmpty(noun.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(noun.IrregularPluralNoun))
                        {
                            wordProp.PluralForIrregular = noun.IrregularPluralNoun;
                        }
                    }
                    else
                    {
                        wordProp.Type = WordType.Regular;
                        wordProp.RootWord = noun.RootWord;
                    }
                    lstWordProp.Add(wordProp);
                }
            }


            string[] transValues = { "s", "es", "ies" };
            string[] ends = { "Q2", "Q3" };
            StateMachine<string> machine = new StateMachine<string>("EnglishNounMorphology", "Q0", ends);
            foreach (var word in lstWordProp)
            {
                if (word.Type == WordType.Regular) //Regular+Singlular
                {
                    machine.Add("Q0", "Q1", word.RootWord);

                    //  Ends with y, y is changed to -i and appends -es
                    if (word.RootWord.Last().Equals('y'))
                    {
                        machine.Add("Q1", "Q3", transValues[2]);
                        word.RootWordPlural = word.RootWord.Substring(0, word.RootWord.Length - 1) + transValues[2];
                    }
                    //  Ends with ch, appends -es
                    else if (word.RootWord.Substring(word.RootWord.Length - 2).Equals("ch"))
                    {
                        machine.Add("Q1", "Q3", transValues[1]);
                        word.RootWordPlural = word.RootWord + transValues[1];
                    }
                    //  appends -s for all regular nouns
                    else
                    {
                        machine.Add("Q1", "Q3", transValues[0]);
                        word.RootWordPlural = word.RootWord + transValues[0];
                    }
                }

                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  Singlular
                    machine.Add("Q0", "Q2", word.RootWord);
                    word.RootWordPlural = word.PluralForIrregular;
                    //  Plural
                    machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else if (word.Type == WordType.UnChanged)  //Unchanged
                {
                    //  Singular
                    machine.Add("Q0", "Q2", word.RootWord);
                    word.RootWordPlural = word.RootWord;
                    word.PluralForIrregular = word.RootWord;
                    //  Plural
                    machine.Add("Q0", "Q2", word.RootWord);
                }
                else
                {

                }
            }

            foreach (var word in lstWordProp)
            {
                if (word.RootWord == GivenWord)
                {
                    //string[] list = { word.value, "" };
                    //var machineResponse = machine.Accepts(list);
                    word.WordRule = word.RootWord + "[N]" + " + [S]";
                    word.number = Number.Singular;
                    word.selected = true;
                    break;
                }
                else if (word.RootWordPlural == GivenWord)
                {
                    word.WordRule = word.RootWord + "[N]" + " + [PL]";
                    word.number = Number.Plural;
                    word.selected = true;
                    break;
                }
            }
            var selectedWord = lstWordProp.Where(item => item.selected)?.FirstOrDefault();
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Nouns(string GivenWord)
        {
            var nouns = this._db.TargetWordFeed.Include(item => item.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Noun && item.WordFeeds.RootWord == GivenWord.Trim())?.FirstOrDefault();
            return nouns;
        }

        public WordPropVerb EnglishMorphology4Verbs(string GivenWord)
        {
            #region Noun Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var verbs = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Verb).ToList();
            if (verbs != null)
            {
                foreach (var verb in verbs)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Verb;
                    wordPropVerb.Gender = verb.Gender;
                    if (verb.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = verb.RootWord;
                        if (!string.IsNullOrEmpty(verb.IrregularPastVerb) || !string.IsNullOrWhiteSpace(verb.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = verb.IrregularPastVerb;
                        }
                        if (!string.IsNullOrEmpty(verb.IrregularPPVerb) || !string.IsNullOrWhiteSpace(verb.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = verb.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = verb.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }


            string[] repPastTensValues = { "ed" };
            string[] presentParticipleValues = { "ing" };
            string[] thridPersonValues = { "s", "es" };
            string[] ends = { "Q2", "Q3" };

            StateMachine<string> machine = new StateMachine<string>("EnglishVerbMorphology", "Q0", ends);
            foreach (var word in lstWordPropVerb)
            {
                if (word.Type == WordType.Regular) //Regular+Singlular
                {
                    machine.Add("Q0", "Q1", word.RootWord);
                    machine.Add("Q0", "Q2", word.RootWord);

                    //  Past tens/ -ed
                    machine.Add("Q1", "Q3", repPastTensValues[0]);
                    word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                    word.RegularPPVerb = word.RootWord + repPastTensValues[0];

                    machine.Add("Q2", "Q3", presentParticipleValues[0]);


                    //if (word.RootWord.Length >= 3)
                    //{//  CVC
                    //    if (!Constants.Vowel.Contains(word.RootWord.ElementAt(word.RootWord.Length - 3)) &&
                    //        Constants.Vowel.Contains(word.RootWord.ElementAt(word.RootWord.Length - 2)) &&
                    //        !Constants.Vowel.Contains(word.RootWord.Last()))
                    //    {
                    //        word.ThirpPersonSinglular = word.RootWord + thridPersonValues[0];
                    //    }
                    //}
                    //else
                    //{
                    //    word.ThirpPersonSinglular = word.RootWord + thridPersonValues[0];
                    //}


                    ////  Ends with y, y is changed to -i and appends -es
                    //if (word.RootWord.Last().Equals('y'))
                    //{
                    //    machine.Add("Q1", "Q3", repPastTensValues[2]);
                    //    word.TransforedValue = word.RootWord.Substring(0, word.RootWord.Length - 1) + repPastTensValues[2];
                    //}
                    ////  Ends with ch, s appends -es
                    //else if (word.RootWord.Substring(word.RootWord.Length - 2).Equals("ch") || word.RootWord.Last().Equals('s'))
                    //{
                    //    machine.Add("Q1", "Q3", repPastTensValues[1]);
                    //    word.TransforedValue = word.RootWord + repPastTensValues[1];
                    //}
                    ////  appends -s for all regular nouns
                    //else
                    //{
                    //    machine.Add("Q1", "Q3", repPastTensValues[0]);
                    //    word.TransforedValue = word.RootWord + repPastTensValues[0];

                    //    machine.Add("Q2", "Q3", presentParticipleValues[0]);
                    //    word.TransforedValue = word.RootWord + presentParticipleValues[0];
                    //}

                }
                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  PastTens + PP
                    machine.Add("Q0", "Q3", word.RootWord);
                    word.IrregularPastVerb = word.IrregularPastVerb;
                    word.IrregularPPVerb = word.IrregularPPVerb;
                    ////  Plural
                    //machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else if (word.Type == WordType.UnChanged)  //Unchanged
                {
                    ////  Singular
                    //machine.Add("Q0", "Q3", word.RootWord);
                    //word.TransforedValue = word.RootWord;
                    //word.PluralForIrregular = word.RootWord;
                    ////  Plural
                    //machine.Add("Q0", "Q2", word.RootWord);
                }
                else
                {

                }

                //  Present participle/ -ing
                if (word.RootWord.Length >= 3)
                {//  CVC
                    if (!Constants.Vowel.Contains(word.RootWord.ElementAt(word.RootWord.Length - 3)) &&
                        Constants.Vowel.Contains(word.RootWord.ElementAt(word.RootWord.Length - 2)) &&
                        !Constants.Vowel.Contains(word.RootWord.Last()))
                    {
                        word.PresentParticiple = word.RootWord + word.RootWord.Last() + presentParticipleValues[0];
                    }
                    else
                    {
                        word.PresentParticiple = word.RootWord + presentParticipleValues[0];
                    }
                }
                else
                {
                    word.PresentParticiple = word.RootWord + presentParticipleValues[0];
                }

                //  Third person singular
                if (Constants.Vowel.Contains(word.RootWord.Last()))
                { //  do, go, 
                    word.ThirpPersonSinglular = word.RootWord + thridPersonValues[1];
                }
                //  Ends with y, y is changed to -i and appends -es
                else if (word.RootWord.Last().Equals('y'))
                {
                    word.ThirpPersonSinglular = word.RootWord.Substring(0, word.RootWord.Length - 1) + "i" + thridPersonValues[1];
                }
                else
                {
                    word.ThirpPersonSinglular = word.RootWord + thridPersonValues[0];
                }

            }
            //  V1/PAST,ING,PL/S
            foreach (var word in lstWordPropVerb)
            {
                word.WordRule = word.RootWord + "[V]";

                if (word.RootWord == GivenWord)
                {
                    word.WordRule += " + [PL]";
                    word.number = Number.Plural;
                    word.selected = true;
                    break;
                }
                else if (word.ThirpPersonSinglular == GivenWord)
                {
                    word.WordRule += " + [3S]";
                    word.number = Number.Singular;
                    word.selected = true;
                    break;
                }
                if (word.PresentParticiple == GivenWord)
                {
                    word.WordRule += " + [ING]";
                    word.selected = true;
                    break;
                }
                else if (word.IrregularPastVerb == GivenWord ||
                    word.RegularPastVerb == GivenWord ||
                    word.RegularPPVerb == GivenWord ||
                    word.IrregularPPVerb == GivenWord)
                {
                    word.WordRule += " + [PAST]";
                    word.selected = true;
                    break;
                }
            }
            var selectedWord = lstWordPropVerb.Where(item => item.selected)?.FirstOrDefault();
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Verbs(string GivenWord)
        {
            var verbs = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Verb && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return verbs;
        }

        public WordPropNoun EnglishMorphology4Prepostion(string GivenWord)
        {
            #region Prepostion Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var prepostions = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Preposition).ToList();
            if (prepostions != null)
            {
                foreach (var item in prepostions)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Preposition;
                    wordPropVerb.Gender = item.Gender;
                    if (item.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = item.RootWord;
                        if (!string.IsNullOrEmpty(item.IrregularPastVerb) || !string.IsNullOrWhiteSpace(item.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = item.IrregularPastVerb;
                        }
                        else if (!string.IsNullOrEmpty(item.IrregularPPVerb) || !string.IsNullOrWhiteSpace(item.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = item.IrregularPPVerb;
                        }
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = item.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            var selectedWord = lstWordPropVerb.Where(item => item.RootWord == GivenWord.Trim())?.FirstOrDefault();
            if (selectedWord != null)
                selectedWord.WordRule = GivenWord + "[PREP]";
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Prepostion(string GivenWord)
        {
            var prepostions = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Preposition && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return prepostions;
        }

        public WordPropNoun EnglishMorphology4Pronouns(string GivenWord)
        {
            #region Prepostion Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var pronouns = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Pronouns).ToList();
            if (pronouns != null)
            {
                foreach (var item in pronouns)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Preposition;
                    wordPropVerb.Gender = item.Gender;
                    if (item.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = item.RootWord;
                        if (!string.IsNullOrEmpty(item.IrregularPastVerb) || !string.IsNullOrWhiteSpace(item.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = item.IrregularPastVerb;
                        }
                        else if (!string.IsNullOrEmpty(item.IrregularPPVerb) || !string.IsNullOrWhiteSpace(item.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = item.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = item.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            var selectedWord = lstWordPropVerb.Where(item => item.RootWord == GivenWord.Trim())?.FirstOrDefault();
            if (selectedWord != null)
                selectedWord.WordRule = GivenWord + "[PRON]";
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Pronouns(string GivenWord)
        {
            var pronouns = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Pronouns && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return pronouns;
        }

        public WordPropNoun EnglishMorphology4Adjectives(string GivenWord)
        {
            #region Prepostion Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var adjectives = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Adjective).ToList();
            if (adjectives != null)
            {
                foreach (var item in adjectives)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Preposition;
                    wordPropVerb.Gender = item.Gender;
                    if (item.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = item.RootWord;
                        if (!string.IsNullOrEmpty(item.IrregularPastVerb) || !string.IsNullOrWhiteSpace(item.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = item.IrregularPastVerb;
                        }
                        else if (!string.IsNullOrEmpty(item.IrregularPPVerb) || !string.IsNullOrWhiteSpace(item.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = item.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = item.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            var selectedWord = lstWordPropVerb.Where(item => item.RootWord == GivenWord.Trim())?.FirstOrDefault();
            if (selectedWord != null)
                selectedWord.WordRule = GivenWord + "[ADJ]";
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Adjectives(string GivenWord)
        {
            var adjectives = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Adjective && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return adjectives;
        }

        public WordPropNoun EnglishMorphology4Adverbs(string GivenWord)
        {
            #region Prepostion Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var adverbs = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Adverb).ToList();
            if (adverbs != null)
            {
                foreach (var item in adverbs)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Preposition;
                    wordPropVerb.Gender = item.Gender;
                    if (item.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = item.RootWord;
                        if (!string.IsNullOrEmpty(item.IrregularPastVerb) || !string.IsNullOrWhiteSpace(item.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = item.IrregularPastVerb;
                        }
                        else if (!string.IsNullOrEmpty(item.IrregularPPVerb) || !string.IsNullOrWhiteSpace(item.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = item.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = item.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            var selectedWord = lstWordPropVerb.Where(item => item.RootWord == GivenWord.Trim())?.FirstOrDefault();
            if (selectedWord != null)
                selectedWord.WordRule = GivenWord + "[ADV]";
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Adverbs(string GivenWord)
        {
            var adverbs = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Adverb && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return adverbs;
        }

        public WordPropNoun EnglishMorphology4Conjunctions(string GivenWord)
        {
            #region Prepostion Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var conjunctions = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Conjunction).ToList();
            if (conjunctions != null)
            {
                foreach (var item in conjunctions)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Preposition;
                    wordPropVerb.Gender = item.Gender;
                    if (item.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = item.RootWord;
                        if (!string.IsNullOrEmpty(item.IrregularPastVerb) || !string.IsNullOrWhiteSpace(item.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = item.IrregularPastVerb;
                        }
                        else if (!string.IsNullOrEmpty(item.IrregularPPVerb) || !string.IsNullOrWhiteSpace(item.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = item.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = item.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            var selectedWord = lstWordPropVerb.Where(item => item.RootWord == GivenWord.Trim())?.FirstOrDefault();
            if (selectedWord != null)
                selectedWord.WordRule = GivenWord + "[CONJ]";
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Conjunctions(string GivenWord)
        {
            var conjunctions = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Conjunction && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return conjunctions;
        }

        public WordPropNoun EnglishMorphology4Disjunctions(string GivenWord)
        {
            #region Prepostion Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var disjunctions = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Disjunction).ToList();
            if (disjunctions != null)
            {
                foreach (var item in disjunctions)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Preposition;
                    wordPropVerb.Gender = item.Gender;
                    if (item.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = item.RootWord;
                        if (!string.IsNullOrEmpty(item.IrregularPastVerb) || !string.IsNullOrWhiteSpace(item.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = item.IrregularPastVerb;
                        }
                        else if (!string.IsNullOrEmpty(item.IrregularPPVerb) || !string.IsNullOrWhiteSpace(item.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = item.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = item.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            var selectedWord = lstWordPropVerb.Where(item => item.RootWord == GivenWord.Trim())?.FirstOrDefault();
            if (selectedWord != null)
                selectedWord.WordRule = GivenWord + "[DISJ]";
            return selectedWord;

            #endregion
        }
        public TargetWordFeed AmharicMorphology4Disjunctions(string GivenWord)
        {
            var disjunctions = this._db.TargetWordFeed.Include(x => x.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Disjunction && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return disjunctions;
        }



        internal TranslationRules ReOrderRules(string rule)
        {
            var reOrderRules = this._db.TranslationRules.Where(item => item.EnglishRules == rule).FirstOrDefault();
            return reOrderRules;
        }







        //public WordPropNoun AmharicConstructMorphology4Nouns(WordPropNoun nounProp, string GivenWord)
        //{
        //    #region Noun Morphology Region

        //    List<WordPropNoun> lstWordProp = new List<WordPropNoun>();

        //    var nouns = this._db.TargetWordFeed.Where(item => item.WordPOSType == WordPOSType.Noun).Include(item => item.WordFeeds).ToList();
        //    if (nouns != null)
        //    {
        //        foreach (var noun in nouns)
        //        {
        //            WordPropNoun wordProp = new WordPropNoun();
        //            wordProp.WordPOSType = WordPOSType.Noun;
        //            wordProp.Gender = noun.Gender;
        //            if (noun.WordType == WordType.Irregular)
        //            {
        //                wordProp.Type = WordType.Irregular;
        //                wordProp.RootWord = noun.RootWord;
        //                wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
        //                if (!string.IsNullOrEmpty(noun.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(noun.IrregularPluralNoun))
        //                {
        //                    wordProp.PluralForIrregular = noun.IrregularPluralNoun;
        //                }
        //            }
        //            else
        //            {
        //                wordProp.Type = WordType.Regular;
        //                wordProp.RootWord = noun.RootWord;
        //                wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
        //            }
        //            lstWordProp.Add(wordProp);
        //        }
        //    }


        //    string[] transValues = { "ዎች", "ኦች" };
        //    string[] ends = { "Q2", "Q3" };
        //    StateMachine<string> machine = new StateMachine<string>("AmharicNounMorphology", "Q0", ends);
        //    foreach (var word in lstWordProp)
        //    {
        //        if (word.Type == WordType.Regular) //Regular+Singlular
        //        {
        //            machine.Add("Q0", "Q1", word.RootWord);

        //            //  Ends with Sabi'E
        //            if (Constants.SabE.Contains(word.RootWord.Last()))
        //            {
        //                machine.Add("Q1", "Q3", transValues[1]);
        //                var lastWord = word.RootWord.Last() + 1;
        //                word.RootWordPlural = word.RootWord.Substring(0, word.RootWord.Length - 1) + Convert.ToChar(lastWord) + transValues[1]?.Substring(1);
        //            }
        //            else
        //            {
        //                machine.Add("Q1", "Q3", transValues[0]);
        //                word.RootWordPlural = word.RootWord + transValues[0];
        //            }
        //        }

        //        else if (word.Type == WordType.Irregular)  //Irregular
        //        {
        //            //  Singlular
        //            machine.Add("Q0", "Q2", word.RootWord);
        //            word.RootWordPlural = word.PluralForIrregular;
        //            //  Plural
        //            machine.Add("Q0", "Q2", word.PluralForIrregular);
        //        }
        //        else if (word.Type == WordType.UnChanged)  //Unchanged
        //        {
        //            //  Singular
        //            machine.Add("Q0", "Q2", word.RootWord);
        //            word.RootWordPlural = word.RootWord;
        //            word.PluralForIrregular = word.RootWord;
        //            //  Plural
        //            machine.Add("Q0", "Q2", word.RootWord);
        //        }
        //        else
        //        {

        //        }
        //    }

        //    var wordPropNoun = EnglishMorphology4Nouns(GivenWord);
        //    foreach (var word in lstWordProp)
        //    {
        //        if (wordPropNoun.RootWord == word?.sourceLangValue)
        //        {
        //            if (wordPropNoun.number == Number.Singular)
        //            {
        //                //string[] list = { word.value, "" };
        //                //var machineResponse = machine.Accepts(list);
        //                word.WordRule = word.RootWord + "[N]" + " + [S]";
        //                word.number = Number.Singular;
        //                word.selected = true;
        //                break;
        //            }
        //            else if (wordPropNoun.number == Number.Plural)
        //            {
        //                word.WordRule = word.RootWordPlural + "[N]" + " + [PL]";
        //                word.number = Number.Plural;
        //                word.selected = true;
        //                break;
        //            }
        //        }
        //    }
        //    var selectedWord = lstWordProp.Where(item => item.selected)?.FirstOrDefault();
        //    return selectedWord;

        //    #endregion
        //}

        public WordPropNoun AmharicConstructMorphology4Nouns(WordPropNoun nounProp, string GivenWord)
        {
            #region Noun Morphology Region

            List<WordPropNoun> lstWordProp = new List<WordPropNoun>();

            var nouns = this._db.TargetWordFeed.Where(item => item.WordPOSType == WordPOSType.Noun).Include(item => item.WordFeeds).ToList();
            if (nouns != null)
            {
                foreach (var noun in nouns)
                {
                    WordPropNoun wordProp = new WordPropNoun();
                    wordProp.WordPOSType = WordPOSType.Noun;
                    wordProp.Gender = noun.Gender;
                    if (noun.WordType == WordType.Irregular)
                    {
                        wordProp.Type = WordType.Irregular;
                        wordProp.RootWord = noun.RootWord;
                        wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
                        if (!string.IsNullOrEmpty(noun.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(noun.IrregularPluralNoun))
                        {
                            wordProp.PluralForIrregular = noun.IrregularPluralNoun;
                        }
                    }
                    else
                    {
                        wordProp.Type = WordType.Regular;
                        wordProp.RootWord = noun.RootWord;
                        wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
                    }
                    lstWordProp.Add(wordProp);
                }
            }


            string[] transValues = { "ዎች", "ኦች" };
            string[] ends = { "Q2", "Q3" };
            StateMachine<string> machine = new StateMachine<string>("AmharicNounMorphology", "Q0", ends);
            foreach (var word in lstWordProp)
            {
                if (word.Type == WordType.Regular) //Regular+Singlular
                {
                    machine.Add("Q0", "Q1", word.RootWord);

                    //  Ends with Sabi'E
                    if (Constants.SabE.Contains(word.RootWord.Last()))
                    {
                        machine.Add("Q1", "Q3", transValues[1]);
                        var lastWord = word.RootWord.Last() + 1;
                        word.RootWordPlural = word.RootWord.Substring(0, word.RootWord.Length - 1) + Convert.ToChar(lastWord) + transValues[1]?.Substring(1);
                    }
                    else
                    {
                        machine.Add("Q1", "Q3", transValues[0]);
                        word.RootWordPlural = word.RootWord + transValues[0];
                    }
                }

                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  Singlular
                    machine.Add("Q0", "Q2", word.RootWord);
                    word.RootWordPlural = word.PluralForIrregular;
                    //  Plural
                    machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else if (word.Type == WordType.UnChanged)  //Unchanged
                {
                    //  Singular
                    machine.Add("Q0", "Q2", word.RootWord);
                    word.RootWordPlural = word.RootWord;
                    word.PluralForIrregular = word.RootWord;
                    //  Plural
                    machine.Add("Q0", "Q2", word.RootWord);
                }
                else
                {

                }
            }

            var wordPropNoun = EnglishMorphology4Nouns(GivenWord);
            foreach (var word in lstWordProp)
            {
                if (wordPropNoun.RootWord == word?.sourceLangValue)
                {
                    if (wordPropNoun.number == Number.Singular)
                    {
                        //string[] list = { word.value, "" };
                        //var machineResponse = machine.Accepts(list);
                        word.WordRule = word.RootWord + "[N]" + " + [S]";
                        word.number = Number.Singular;
                        word.selected = true;
                        break;
                    }
                    else if (wordPropNoun.number == Number.Plural)
                    {
                        word.WordRule = word.RootWordPlural + "[N]" + " + [PL]";
                        word.number = Number.Plural;
                        word.selected = true;
                        break;
                    }
                }
            }
            var selectedWord = lstWordProp.Where(item => item.selected)?.FirstOrDefault();
            return selectedWord;

            #endregion
        }
        public WordPropVerb FilterVerbs(string givenWord)
        {
            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var verbs = this._db.TargetWordFeed.Where(item => item.WordPOSType == WordPOSType.Verb).Include(item => item.WordFeeds).ToList();
            if (verbs != null)
            {
                foreach (var verb in verbs)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Verb;
                    wordPropVerb.Gender = verb.Gender;
                    wordPropVerb.sourceLangValue = verb.WordFeeds?.RootWord;
                    if (verb.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = verb.RootWord;
                        if (!string.IsNullOrEmpty(verb.IrregularPastVerb) || !string.IsNullOrWhiteSpace(verb.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = verb.IrregularPastVerb;
                        }
                        if (!string.IsNullOrEmpty(verb.IrregularPPVerb) || !string.IsNullOrWhiteSpace(verb.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = verb.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = verb.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            string[] repPastTensValues = { "ኧዋል", "አለች" };
            string[] presentParticipleValues = { "እየ" };
            string[] thridPersonValues = { "" };
            string[] ends = { "Q2", "Q3" };

            StateMachine<string> machine = new StateMachine<string>("EnglishVerbMorphology", "Q0", ends);
            foreach (var word in lstWordPropVerb)
            {
                if (word.Type == WordType.Regular) //Regular+Singlular
                {
                    machine.Add("Q0", "Q1", word.RootWord);
                    machine.Add("Q0", "Q2", word.RootWord);

                    //  Past tens/ -ed
                    if (word.Gender == Gender.Male)
                    {
                        machine.Add("Q1", "Q3", repPastTensValues[0]);
                        word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                        word.RegularPPVerb = word.RootWord + repPastTensValues[0];
                    }
                    else if (word.Gender == Gender.Female)
                    {
                        machine.Add("Q1", "Q3", repPastTensValues[1]);
                        word.RegularPastVerb = word.RootWord + repPastTensValues[1];
                        word.RegularPPVerb = word.RootWord + repPastTensValues[1];
                    }
                    else
                    {
                        machine.Add("Q1", "Q3", repPastTensValues[0]);
                        word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                        word.RegularPPVerb = word.RootWord + repPastTensValues[0];
                    }
                    //word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                    //word.RegularPPVerb = word.RootWord + repPastTensValues[0];

                    //  TODO:   Pending
                    //machine.Add("Q2", "Q3", presentParticipleValues[0]);
                }
                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  PastTens + PP
                    machine.Add("Q0", "Q3", word.RootWord);
                    word.IrregularPastVerb = word.IrregularPastVerb;
                    word.IrregularPPVerb = word.IrregularPPVerb;
                    ////  Plural
                    //machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else
                {

                }


            }
            var result = lstWordPropVerb.Where(item => item.RootWord == givenWord)?.FirstOrDefault();
            return result;
        }
        public WordPropNoun AmharicConstructMorphology4Verbs(WordPropNoun wordPropNoun, string GivenWord)
        {
            #region Noun Morphology Region

            List<WordPropVerb> lstWordPropVerb = new List<WordPropVerb>();

            var verbs = this._db.TargetWordFeed.Where(item => item.WordPOSType == WordPOSType.Verb).Include(item => item.WordFeeds).ToList();
            if (verbs != null)
            {
                foreach (var verb in verbs)
                {
                    WordPropVerb wordPropVerb = new WordPropVerb();
                    wordPropVerb.WordPOSType = WordPOSType.Verb;
                    wordPropVerb.Gender = wordPropNoun.Gender;
                    wordPropVerb.number = wordPropNoun.number;
                    wordPropVerb.sourceLangValue = verb.WordFeeds?.RootWord;
                    if (verb.WordType == WordType.Irregular)
                    {
                        wordPropVerb.Type = WordType.Irregular;
                        wordPropVerb.RootWord = verb.RootWord;
                        if (!string.IsNullOrEmpty(verb.IrregularPastVerb) || !string.IsNullOrWhiteSpace(verb.IrregularPastVerb))
                        {
                            wordPropVerb.IrregularPastVerb = verb.IrregularPastVerb;
                        }
                        if (!string.IsNullOrEmpty(verb.IrregularPPVerb) || !string.IsNullOrWhiteSpace(verb.IrregularPPVerb))
                        {
                            wordPropVerb.IrregularPPVerb = verb.IrregularPPVerb;
                        }
                        //else if (!string.IsNullOrEmpty(verb.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(verb.IrregularPluralNoun))
                        //{
                        //    wordPropVerb.PluralForIrregular = verb.IrregularPluralNoun;
                        //}
                    }
                    else
                    {
                        wordPropVerb.Type = WordType.Regular;
                        wordPropVerb.RootWord = verb.RootWord;
                    }
                    lstWordPropVerb.Add(wordPropVerb);
                }
            }

            string[] repPastTensValues = { "ኧዋል", "አለች" };
            //string[] presentParticipleValues = { "እየ" };
            string[] presentParticipleValues = { "አ", "አች", "ኡ" };
            string[] thridPersonValues = { "ኧዋል", "አለች" };
            string[] ends = { "Q2", "Q3" };

            StateMachine<string> machine = new StateMachine<string>("EnglishVerbMorphology", "Q0", ends);
            foreach (var word in lstWordPropVerb)
            {
                if (word.Type == WordType.Regular) //Regular+Singlular
                {
                    machine.Add("Q0", "Q1", word.RootWord);
                    machine.Add("Q0", "Q2", word.RootWord);

                    //  Past tens/ -ed
                    if (word.Gender == Gender.Male)
                    {
                        machine.Add("Q1", "Q3", repPastTensValues[0]);
                        word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                        word.RegularPPVerb = word.RootWord + repPastTensValues[0];
                    }
                    else if (word.Gender == Gender.Female)
                    {
                        machine.Add("Q1", "Q3", repPastTensValues[1]);
                        word.RegularPastVerb = word.RootWord + repPastTensValues[1];
                        word.RegularPPVerb = word.RootWord + repPastTensValues[1];
                    }
                    else
                    {
                        machine.Add("Q1", "Q3", repPastTensValues[0]);
                        word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                        word.RegularPPVerb = word.RootWord + repPastTensValues[0];
                    }


                    //  Plural                    
                    if (Number.Plural == word.number)
                    {
                        machine.Add("Q0", "Q2", repPastTensValues[0]);
                        machine.Add("Q1", "Q3", repPastTensValues[0]);
                        word.RootWordPlural = word.RootWord + repPastTensValues[0];
                    }
                    //word.RegularPastVerb = word.RootWord + repPastTensValues[0];
                    //word.RegularPPVerb = word.RootWord + repPastTensValues[0];

                    //  TODO:   Pending
                    //machine.Add("Q2", "Q3", presentParticipleValues[0]);
                }
                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  PastTens + PP
                    machine.Add("Q0", "Q3", word.RootWord);
                    word.IrregularPastVerb = word.IrregularPastVerb;
                    word.IrregularPPVerb = word.IrregularPPVerb;

                    //  Plural
                    if (Number.Plural == word.number)
                    {
                        machine.Add("Q0", "Q2", repPastTensValues[0]);
                        machine.Add("Q1", "Q3", repPastTensValues[0]);
                        word.PluralForIrregular = word.RootWord + repPastTensValues[0];
                    }
                }
                else
                {

                }


                if (word.Gender == Gender.Male)
                {
                    word.ThirpPersonSinglular = word.RootWord + repPastTensValues[0];
                    word.PresentParticiple = word.RootWord + presentParticipleValues[0];
                }
                else if (word.Gender == Gender.Female)
                {
                    word.ThirpPersonSinglular = word.RootWord + repPastTensValues[1];
                    word.PresentParticiple = word.RootWord + presentParticipleValues[1];
                }
                else
                {
                    word.ThirpPersonSinglular = word.RootWord + repPastTensValues[0];
                    word.PresentParticiple = word.RootWord + presentParticipleValues[2];
                }
            }

            var englishResult = EnglishMorphology4Verbs(GivenWord);
            foreach (var word in lstWordPropVerb)
            {
                var result = FilterVerbs(word.RootWord);
                if (englishResult.RootWord == word?.sourceLangValue)
                {
                    //if (result.RootWord == word?.sourceLangValue)
                    {
                        if (word.number == 0 || word.number == Number.Singular)
                        {
                            word.WordRule = word.RootWord + "[V]" + " + [S]";
                            word.number = Number.Singular;
                            word.selected = true;
                            if (wordPropNoun.Gender == Gender.Male)
                            {
                                if (wordPropNoun.WordRule.Contains("3S"))
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                                else if (wordPropNoun.WordRule.Contains("ING"))
                                {
                                    word.Translated = word.PresentParticiple;
                                }
                                else if (wordPropNoun.WordRule.Contains("PAST"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RegularPastVerb : word.IrregularPastVerb;
                                }
                            }
                            else if (word.Gender == Gender.Female)
                            {
                                if (wordPropNoun.WordRule.Contains("3S"))
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                                else if (wordPropNoun.WordRule.Contains("ING"))
                                {
                                    word.Translated = word.PresentParticiple;
                                }
                                else if (wordPropNoun.WordRule.Contains("PAST"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RegularPastVerb : word.IrregularPastVerb;
                                }
                                else
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                            }
                            else
                            {
                                if (wordPropNoun.WordRule.Contains("3S"))
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                                else if (wordPropNoun.WordRule.Contains("ING"))
                                {
                                    word.Translated = word.PresentParticiple;
                                }
                                else if (wordPropNoun.WordRule.Contains("PAST"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RegularPastVerb : word.IrregularPastVerb;
                                }
                                else
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }

                            }
                            break;
                        }
                        else if (word.number == Number.Plural)
                        {
                            word.WordRule = word.RootWordPlural + "[V]" + " + [PL]";
                            word.number = Number.Plural;
                            word.selected = true;
                            if (wordPropNoun.Gender == Gender.Male)
                            {
                                if (wordPropNoun.WordRule.Contains("3S"))
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                                else if (wordPropNoun.WordRule.Contains("ING"))
                                {
                                    word.Translated = word.PresentParticiple;
                                }
                                else if (wordPropNoun.WordRule.Contains("PAST"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RegularPastVerb : word.IrregularPastVerb;
                                }
                                else if (wordPropNoun.WordRule.Contains("PL"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RootWordPlural : word.PluralForIrregular;
                                }
                            }
                            else if (word.Gender == Gender.Female)
                            {
                                if (wordPropNoun.WordRule.Contains("3S"))
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                                else if (wordPropNoun.WordRule.Contains("ING"))
                                {
                                    word.Translated = word.PresentParticiple;
                                }
                                else if (wordPropNoun.WordRule.Contains("PAST"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RegularPastVerb : word.IrregularPastVerb;
                                }
                                else if (wordPropNoun.WordRule.Contains("PL"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RootWordPlural : word.PluralForIrregular;
                                }
                                else
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                            }
                            else
                            {
                                if (wordPropNoun.WordRule.Contains("3S"))
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }
                                else if (wordPropNoun.WordRule.Contains("ING"))
                                {
                                    word.Translated = word.PresentParticiple;
                                }
                                else if (wordPropNoun.WordRule.Contains("PAST"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RegularPastVerb : word.IrregularPastVerb;
                                }
                                else if (wordPropNoun.WordRule.Contains("PL"))
                                {
                                    word.Translated = word.Type == WordType.Regular ? word.RootWordPlural : word.PluralForIrregular;
                                }
                                else
                                {
                                    word.Translated = word.ThirpPersonSinglular;
                                }

                            }
                            break;
                        }
                    }
                }
            }
            var selectedWord = lstWordPropVerb.Where(item => item.selected)?.FirstOrDefault();
            return selectedWord;

            #endregion
        }
    }
}