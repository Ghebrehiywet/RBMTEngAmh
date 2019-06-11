using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Models.RBTM
{
    [Table("Translation_Rules")]
    public class TranslationRules
    {
        [Key]
        public int RuleId { get; set; }
        [Required]
        public string EnglishRules { get; set; }
        [Required]
        public string AnharicRules { get; set; }
    }
}