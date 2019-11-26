using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.Palettes
{
    [Table("LegoElements")]
    public class LegoElement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string DesignID { get; set; }

        public string ElementID { get; set; }

        public bool IsAssembly { get; set; }

        public ICollection<PartConfiguration> Configurations { get; set; } = new List<PartConfiguration>();
    }
}
