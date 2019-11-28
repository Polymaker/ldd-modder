using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddElementMaterials")]
    public class ElementMaterial
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ElementPartID { get; set; }

        [ForeignKey("ElementPartID")]
        public virtual ElementPart Part { get; set; }

        public int SurfaceID { get; set; }

        public int MaterialID { get; set; }

        public ElementMaterial()
        {
        }

        public ElementMaterial(int surfaceID, int materialID)
        {
            SurfaceID = surfaceID;
            MaterialID = materialID;
        }
    }
}
