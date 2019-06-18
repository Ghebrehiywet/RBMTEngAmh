using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Models.RBTM
{
    public enum WordPOSType
    {
        Noun = 1,
        Pronouns,
        Verb,
        Adjective,
        Adverb,
        Preposition,
        Conjunction,
        Disjunction
    }
    public enum WordType
    {
        Regular = 1,
        Irregular,
        UnChanged
    }
    public enum Gender
    {
        ForBoth = 0,
        Male,
        Female
    }
    public enum Languages
    {
        English = 1,
        Amharic
    }
    public enum Number
    {
        Singular = 1,
        Plural
    }
}