using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("PaletteBricks")]
    public class PaletteBrick
    {
        [Key, Column(Order = 0)]
        public string BagName { get; set; }
        [Key, Column(Order = 1)]
        public string PaletteName { get; set; }
        [ForeignKey("BagName,PaletteName")]
        public virtual Palette Palette { get; set; }
        [Key, Column(Order = 2)]
        public int DesignID { get; set; }
        [Key, Column(Order = 3)]
        public int MaterialID { get; set; }
        [Key, Column(Order = 4)]
        public int? ItemNos { get; set; }
    }
}
