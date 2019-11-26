using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models
{
    [Table("PartMappings")]
    public class PartMapping
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string RebrickableID { get; set; }

        public string LddID { get; set; }

        public bool IsActive { get; set; }
    }
}
