using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Models.RBTM
{
    [Table("VERBS")]
    public class Verbs
    {
        [Key]
        public int id { get; set; }
        public string Regular_stem { get; set; }

        public string Irregular_stem { get; set; }
        public string Irregular_past { get; set; }
        public string Irregular_PP { get; set; }

    }
}