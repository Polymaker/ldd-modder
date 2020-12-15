using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Rebrickable
{
    [Table("RbSets")]
    public class RbSet
    {
        [Key]
        public string SetID { get; set; }

        public string Name { get; set; }

        public int? Year { get; set; }

        public int? ThemeID { get; set; }

        //Bug with EntityFramework
        public System.DateTime? InventoryDate { get; set; }

        [ForeignKey("ThemeID")]
        public virtual RbTheme Theme { get; set; }

        public virtual ICollection<RbSetPart> Parts { get; set; }

        public RbSet()
        {
            Parts = new List<RbSetPart>();
        }
    }
}
