using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddElementParts")]
    public class ElementPart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string ElementID { get; set; }

        [ForeignKey("ElementID")]
        public virtual LddElement Element { get; set; }

        public string PartID { get; set; }

        public int MaterialID { get; set; }

        public virtual ICollection<ElementMaterial> SubMaterials { get; set; }

        public virtual ICollection<ElementDecoration> Decorations { get; set; }

        public ElementPart()
        {
            SubMaterials = new List<ElementMaterial>();
            Decorations = new List<ElementDecoration>();
        }

        public ElementPart Clone()
        {
            var config = new ElementPart()
            {
                PartID = PartID,
                MaterialID = MaterialID
            };
            foreach (var dec in Decorations)
                config.Decorations.Add(new ElementDecoration(dec.SurfaceID, dec.DecorationID));
            foreach (var mat in SubMaterials)
                config.SubMaterials.Add(new ElementMaterial(mat.SurfaceID, mat.MaterialID));
            return config;
        }
    }
}
