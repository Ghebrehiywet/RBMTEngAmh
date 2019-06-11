using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Models.RBTM
{
    public enum WordPOSType
    {
        Noun = 1,
        Verb,
        Adjective,
        Adverb,
        Conjunction,
        Disjunction,
        Preposition
    }
    public enum WordType
    {
        Regular = 1,
        Irregular,
        UnChanged
    }
    public enum Languages
    {
        English = 1,
        Amharic
    }
}