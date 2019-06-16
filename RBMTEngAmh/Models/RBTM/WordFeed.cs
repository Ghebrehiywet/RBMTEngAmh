using RBMTEngAmh.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Models.RBTM
{
    [Table("Source_Language")]
    public class WordFeed : WordFeedObj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
    }

    [Table("Target_Language")]
    public class TargetWordFeed : WordFeedObj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TargetLangId { get; set; }

        [Required]
        [ForeignKey("WordFeeds")]
        public long sourceId { get; set; }
        virtual public WordFeed WordFeeds { get; set; }
    }
    public class WordFeedObj
    {

        [Required]
        public WordPOSType WordPOSType { get; set; }
        [Required]
        public WordType WordType { get; set; }
        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DisplayName("Root Word")]
        public string RootWord { get; set; }

        //  NOUN
        public string IrregularPluralNoun { get; set; }

        //  VERB
        [DisplayName("Irregular_Past")]
        public string IrregularPastVerb { get; set; }
        [DisplayName("Irregular_PP")]
        public string IrregularPPVerb { get; set; }

        //  Adjective

        [NotMapped]
        public string WordRule { get; set; }
        [NotMapped]
        public string Translated { get; internal set; }

    }
}