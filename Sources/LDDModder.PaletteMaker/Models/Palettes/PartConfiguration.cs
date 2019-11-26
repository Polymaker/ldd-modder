using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Palettes
{
    [Table("PartConfigs")]
    public class PartConfiguration
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int LegoElementID { get; set; }

        [ForeignKey("LegoElementID")]
        public virtual LegoElement Element { get; set; }

        public string DesignID { get; set; }

        public int MaterialID { get; set; }

        public ICollection<SubMaterial> SubMaterials { get; set; } = new List<SubMaterial>();

        public ICollection<Decoration> Decorations { get; set; } = new List<Decoration>();
    }
}
