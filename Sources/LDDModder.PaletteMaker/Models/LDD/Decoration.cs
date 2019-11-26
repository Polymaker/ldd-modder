using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("Decorations")]
    public class Decoration
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ConfigurationID { get; set; }

        [ForeignKey("ConfigurationID")]
        public virtual PartConfiguration Configuration { get; set; }

        public int SurfaceID { get; set; }

        public string DecorationID { get; set; }

        public Decoration()
        {
            SurfaceID = 0;
            DecorationID = string.Empty;
        }

        public Decoration(int surfaceID, string decorationID)
        {
            SurfaceID = surfaceID;
            DecorationID = decorationID;
        }
    }
}
