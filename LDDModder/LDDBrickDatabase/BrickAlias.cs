using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("BrickAliases")]
    public class BrickAlias
    {
        [Key, Column(Order = 0), ForeignKey("Brick")]
        public int BrickID { get; set; }
        public virtual Brick Brick { get; set; }
        [Key, Column(Order = 1)]
        public int Alias { get; set; }
    }
}
