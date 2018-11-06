using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDBrickDatabase
{
    [Table("Assemblies")]
    public class Assembly
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public int PlatformID { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public virtual ICollection<AssemblyAlias> Aliases { get; set; }
    }
}
