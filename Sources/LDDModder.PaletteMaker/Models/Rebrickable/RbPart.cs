using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbParts")]
    public class RbPart
    {
        [Key]
        public string PartID { get; set; }

        public string Name { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual RbCategory Category { get; set; }

        public string ParentPartID { get; set; }

        public bool IsPrintOrPattern { get; set; }

        public virtual ICollection<RbPartRelation> Relationships { get; set; }

        public RbPart()
        {
            Relationships = new List<RbPartRelation>();
        }
    }
}
