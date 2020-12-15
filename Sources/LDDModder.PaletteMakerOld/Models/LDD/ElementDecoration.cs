using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddElementDecorations")]
    public class ElementDecoration
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ElementPartID { get; set; }

        [ForeignKey("ElementPartID")]
        public virtual ElementPart Part { get; set; }

        public int SurfaceID { get; set; }

        public string DecorationID { get; set; }

        public ElementDecoration()
        {
            SurfaceID = 0;
            DecorationID = string.Empty;
        }

        public ElementDecoration(int surfaceID, string decorationID)
        {
            SurfaceID = surfaceID;
            DecorationID = decorationID;
        }
    }
}
