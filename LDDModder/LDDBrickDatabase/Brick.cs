using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("Bricks")]
    public class Brick
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public int PlatformID { get; set; }
        public int Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public virtual ICollection<BrickAlias> Aliases { get; set; }

        public Brick()
        {
            Aliases = new List<BrickAlias>();
        }
    }
}
