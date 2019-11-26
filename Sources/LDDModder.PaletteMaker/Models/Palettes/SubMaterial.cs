using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Palettes
{
    [Table("SubMaterials")]
    public class SubMaterial
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ConfigurationID { get; set; }

        [ForeignKey("ConfigurationID")]
        public virtual PartConfiguration Configuration { get; set; }

        public int SurfaceID { get; set; }

        public int MaterialID { get; set; }

    }
}
