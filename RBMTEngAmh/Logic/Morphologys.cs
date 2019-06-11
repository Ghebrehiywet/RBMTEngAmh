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

        public WordProp EnglishMorphology4Nouns(ApplicationDbContext db, string GivenWord)
        {
            this._db = db;

            #region Noun Morphology Region

            List<WordProp> lstWordProp = new List<WordProp>();

            var nouns = this._db.SourceWordFeeds.Where(item => item.WordPOSType == WordPOSType.Noun).ToList();
            if (nouns != null)
            {
                foreach (var noun in nouns)
                {
                    WordProp wordProp = new WordProp();
                    wordProp.WordPOSType = WordPOSType.Noun;
                    if (noun.WordType == WordType.Irregular)
                    {
                        wordProp.Type = WordType.Irregular;
                        wordProp.value = noun.RootWord;
                        if (!string.IsNullOrEmpty(noun.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(noun.IrregularPluralNoun))
                        {
                            wordProp.PluralForIrregular = noun.IrregularPluralNoun;
                        }
                    }
                    else
                    {
                        wordProp.Type = WordType.Regular;
                        wordProp.value = noun.RootWord;
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
                    machine.Add("Q0", "Q1", word.value);

                    //  Ends with y, y is changed to -i and appends -es
                    if (word.value.Last().Equals('y'))
                    {
                        machine.Add("Q1", "Q3", transValues[2]);
                        word.TransforedValue = word.value.Substring(0, word.value.Length - 1) + transValues[2];
                    }
                    //  Ends with ch, appends -es
                    else if (word.value.Substring(word.value.Length - 2).Equals("ch"))
                    {
                        machine.Add("Q1", "Q3", transValues[1]);
                        word.TransforedValue = word.value + transValues[1];
                    }
                    //  appends -s for all regular nouns
                    else
                    {
                        machine.Add("Q1", "Q3", transValues[0]);
                        word.TransforedValue = word.value + transValues[0];
                    }
                }

                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  Singlular
                    machine.Add("Q0", "Q2", word.value);
                    word.TransforedValue = word.PluralForIrregular;
                    //  Plural
                    machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else if (word.Type == WordType.UnChanged)  //Unchanged
                {
                    //  Singular
                    machine.Add("Q0", "Q2", word.value);
                    word.TransforedValue = word.value;
                    word.PluralForIrregular = word.value;
                    //  Plural
                    machine.Add("Q0", "Q2", word.value);
                }
                else
                {

                }
            }

            foreach (var word in lstWordProp)
            {
                if (word.value == GivenWord)
                {
                    //string[] list = { word.value, "" };
                    //var machineResponse = machine.Accepts(list);
                    word.WordRule = word.value + "[N]" + " + [S]";
                    word.selected = true;
                    break;
                }
                else if (word.TransforedValue == GivenWord)
                {
                    word.WordRule = word.value + "[N]" + " + [PL]";
                    word.selected = true;
                    break;
                }
            }
            var selectedWord = lstWordProp.Where(item => item.selected)?.FirstOrDefault();
            return selectedWord;

            #endregion
        }

        public TargetWordFeed AmharicMorphology4Nouns(ApplicationDbContext db, string GivenWord)
        {
            this._db = db;

            #region Noun Morphology Region

            List<WordProp> lstWordProp = new List<WordProp>();
           var nouns = this._db.TargetWordFeed.Include(item => item.WordFeeds).Where(item => item.WordPOSType == WordPOSType.Noun && item.WordFeeds.RootWord == GivenWord)?.FirstOrDefault();
            return nouns;
            //if (nouns != null)
            //{
            //    foreach (var noun in nouns)
            //    {
            //        WordProp wordProp = new WordProp();
            //        wordProp.WordPOSType = WordPOSType.Noun;
            //        if (noun.WordType == WordType.Irregular)
            //        {
            //            wordProp.Type = WordType.Irregular;
            //            wordProp.value = noun.RootWord;
            //            wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
            //            if (!string.IsNullOrEmpty(noun.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(noun.IrregularPluralNoun))
            //            {
            //                wordProp.PluralForIrregular = noun.IrregularPluralNoun;
            //            }
            //        }
            //        else
            //        {
            //            wordProp.Type = WordType.Regular;
            //            wordProp.value = noun.RootWord;
            //            wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
            //        }
            //        lstWordProp.Add(wordProp);
            //    }
            //}


            string[] transValues = { "ዎች", "ኦች" };
            string[] ends = { "Q2", "Q3" };
            StateMachine<string> machine = new StateMachine<string>("AmharicNounMorphology", "Q0", ends);
            foreach (var word in lstWordProp)
            {
                if (word.Type == WordType.Regular) //Regular+Singlular
                {
                    machine.Add("Q0", "Q1", word.value);

                    //  Ends with Sabi'E
                    if (Constants.SabE.Contains(word.value.Last()))
                    {
                        machine.Add("Q1", "Q3", transValues[1]);
                        var lastWord = word.value.Last() + 1;
                        word.TransforedValue = word.value.Substring(0, word.value.Length - 1) + Convert.ToChar(lastWord) + transValues[1]?.Substring(1);
                    }
                    else
                    {
                        machine.Add("Q1", "Q3", transValues[0]);
                        word.TransforedValue = word.value + transValues[0];
                    }
                }

                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  Singlular
                    machine.Add("Q0", "Q2", word.value);
                    word.TransforedValue = word.PluralForIrregular;
                    //  Plural
                    machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else if (word.Type == WordType.UnChanged)  //Unchanged
                {
                    //  Singular
                    machine.Add("Q0", "Q2", word.value);
                    word.TransforedValue = word.value;
                    word.PluralForIrregular = word.value;
                    //  Plural
                    machine.Add("Q0", "Q2", word.value);
                }
                else
                {

                }
            }

            foreach (var word in lstWordProp)
            {
                if (word.sourceLangValue == GivenWord)
                {
                    //string[] list = { word.value, "" };
                    //var machineResponse = machine.Accepts(list);
                    word.WordRule = word.value + "[N]" + " + [S]";
                    word.selected = true;
                    break;
                }
                else if (word.TransforedValue == GivenWord)
                {
                    word.WordRule = word.value + "[N]" + " + [PL]";
                    word.selected = true;
                    break;
                }
            }
            //var selectedWord = lstWordProp.Where(item => item.selected)?.FirstOrDefault();
            //return selectedWord;

            #endregion
        }
        public WordProp xAmharicMorphology4Nouns(ApplicationDbContext db, string GivenWord)
        {
            this._db = db;

            #region Noun Morphology Region

            List<WordProp> lstWordProp = new List<WordProp>();

            var nouns = this._db.TargetWordFeed.Where(item => item.WordPOSType == WordPOSType.Noun).Include(item => item.WordFeeds).ToList();
            if (nouns != null)
            {
                foreach (var noun in nouns)
                {
                    WordProp wordProp = new WordProp();
                    wordProp.WordPOSType = WordPOSType.Noun;
                    if (noun.WordType == WordType.Irregular)
                    {
                        wordProp.Type = WordType.Irregular;
                        wordProp.value = noun.RootWord;
                        wordProp.sourceLangValue = noun.WordFeeds?.RootWord;
                        if (!string.IsNullOrEmpty(noun.IrregularPluralNoun) || !string.IsNullOrWhiteSpace(noun.IrregularPluralNoun))
                        {
                            wordProp.PluralForIrregular = noun.IrregularPluralNoun;
                        }
                    }
                    else
                    {
                        wordProp.Type = WordType.Regular;
                        wordProp.value = noun.RootWord;
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
                    machine.Add("Q0", "Q1", word.value);

                    //  Ends with Sabi'E
                    if (Constants.SabE.Contains(word.value.Last()))
                    {
                        machine.Add("Q1", "Q3", transValues[1]);
                        var lastWord = word.value.Last() + 1;
                        word.TransforedValue = word.value.Substring(0, word.value.Length - 1) + Convert.ToChar(lastWord) + transValues[1]?.Substring(1);
                    }
                    else
                    {
                        machine.Add("Q1", "Q3", transValues[0]);
                        word.TransforedValue = word.value + transValues[0];
                    }
                }

                else if (word.Type == WordType.Irregular)  //Irregular
                {
                    //  Singlular
                    machine.Add("Q0", "Q2", word.value);
                    word.TransforedValue = word.PluralForIrregular;
                    //  Plural
                    machine.Add("Q0", "Q2", word.PluralForIrregular);
                }
                else if (word.Type == WordType.UnChanged)  //Unchanged
                {
                    //  Singular
                    machine.Add("Q0", "Q2", word.value);
                    word.TransforedValue = word.value;
                    word.PluralForIrregular = word.value;
                    //  Plural
                    machine.Add("Q0", "Q2", word.value);
                }
                else
                {

                }
            }

            foreach (var word in lstWordProp)
            {
                if (word.sourceLangValue == GivenWord)
                {
                    //string[] list = { word.value, "" };
                    //var machineResponse = machine.Accepts(list);
                    word.WordRule = word.value + "[N]" + " + [S]";
                    word.selected = true;
                    break;
                }
                else if (word.TransforedValue == GivenWord)
                {
                    word.WordRule = word.value + "[N]" + " + [PL]";
                    word.selected = true;
                    break;
                }
            }
            var selectedWord = lstWordProp.Where(item => item.selected)?.FirstOrDefault();
            return selectedWord;

            #endregion
        }
    }
}