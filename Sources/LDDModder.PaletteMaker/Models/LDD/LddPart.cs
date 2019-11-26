using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddParts")]
    public class LddPart
    {
        [Key]
        public string DesignID { get; set; }

        public string Name { get; set; }

        public string Aliases { get; set; }

        public bool IsAssembly { get; set; }
    }
}
