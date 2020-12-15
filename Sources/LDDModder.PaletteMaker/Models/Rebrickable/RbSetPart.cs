using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbSetParts")]
    public class RbSetPart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string SetID { get; set; }

        [ForeignKey("SetID")]
        public virtual RbSet Set { get; set; }

        public string PartID { get; set; }

        [ForeignKey("PartID")]
        public virtual RbPart Part { get; set; }

        public string ElementID { get; set; }

        public int? ColorID { get; set; }

        [ForeignKey("ColorID")]
        public virtual RbColor Color { get; set; }

        public int Quantity { get; set; }

        public bool IsSpare { get; set; }

        [NotMapped]
        public string CategoryName => Part?.Category?.Name;
    }
}
