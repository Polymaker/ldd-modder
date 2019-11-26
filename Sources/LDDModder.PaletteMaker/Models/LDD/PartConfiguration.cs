using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("PartConfigs")]
    public class PartConfiguration
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int LegoElementID { get; set; }

        [ForeignKey("LegoElementID")]
        public virtual LddElement Element { get; set; }

        public string DesignID { get; set; }

        public int MaterialID { get; set; }

        public virtual ICollection<SubMaterial> SubMaterials { get; set; }

        public virtual ICollection<Decoration> Decorations { get; set; }

        public PartConfiguration()
        {
            SubMaterials = new List<SubMaterial>();
            Decorations = new List<Decoration>();
        }

        public PartConfiguration Clone()
        {
            var config = new PartConfiguration()
            {
                DesignID = DesignID,
                MaterialID = MaterialID
            };
            foreach (var dec in Decorations)
                config.Decorations.Add(new Decoration(dec.SurfaceID, dec.DecorationID));
            foreach (var mat in SubMaterials)
                config.SubMaterials.Add(new SubMaterial(mat.SurfaceID, mat.MaterialID));
            return config;
        }
    }
}
