using LDDModder.LDD.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Models
{
    public class Model
    {
        public const string EXTENSION = "LXFML";

        public string ModelName { get; set; }
        public VersionInfo FileVersion { get; set; }

        public string ApplicationName { get; set; }
        public VersionInfo ApplicationVersion { get; set; }

        public Brand Brand { get; set; }

        public int BrickSetVersion { get; set; }


    }
}
