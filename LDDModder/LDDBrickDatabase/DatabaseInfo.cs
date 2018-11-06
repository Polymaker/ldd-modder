using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("Info")]
    public class DatabaseInfo
    {
        public int DatabaseVersion { get; set; }
    }
}
