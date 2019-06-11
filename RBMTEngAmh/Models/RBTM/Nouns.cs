using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Models.RBTM
{
    [Table("NOUNS")]
    public class Nouns
    {
        [Key]
        public short id { get; set; }
        public string Regular { get; set; }
        public string Irregular { get; set; }
        public string IrregularPlural { get; set; }
    }
}