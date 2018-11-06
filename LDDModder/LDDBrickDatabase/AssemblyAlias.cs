using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("AssemblyAliases")]
    public class AssemblyAlias
    {
        [Key, Column(Order = 0), ForeignKey("Assembly")]
        public int AssemblyID { get; set; }
        public virtual Assembly Assembly { get; set; }
        [Key, Column(Order = 1)]
        public int Alias { get; set; }
    }
}
