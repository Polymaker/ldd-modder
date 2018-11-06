using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("DecorationMappings")]
    public class DecorationMapping
    {
        [Key, Column(Order = 0)]
        public int DecorationID { get; set; }
        [Key, Column(Order = 1)]
        public int DesignID { get; set; }
        [Key, Column(Order = 2)]
        public int SurfaceID { get; set; }
    }
}
