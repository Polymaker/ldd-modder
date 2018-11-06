using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("Palettes")]
    public class Palette
    {
        [Key, Column(Order = 0)]
        public string BagName { get; set; }
        [Key, Column(Order = 1)]
        public string Name { get; set; }
        public int Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public virtual ICollection<PaletteBrick> Bricks { get; set; }
        public virtual ICollection<PaletteAssembly> Assemblies { get; set; }

        public Palette()
        {
            Bricks = new List<PaletteBrick>();
            Assemblies = new List<PaletteAssembly>();
        }
    }
}
