using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbColorMatches")]
    public class RbColorMatch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int RbColorID { get; set; }

        [ForeignKey("RbColorID")]
        public virtual RbColor Color { get; set; }

        public string Platform { get; set; }

        public string ColorName { get; set; }

        public int ColorID { get; set; }
    }
}
