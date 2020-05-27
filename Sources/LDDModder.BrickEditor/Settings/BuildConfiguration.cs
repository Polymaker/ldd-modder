using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class BuildConfiguration
    {
        //[JsonProperty("name")]
        public string Name { get; set; }
        //[JsonProperty("output")]
        public string OutputPath { get; set; }
        //[JsonProperty("output")]
        public bool CreateZip { get; set; }
        //[JsonProperty("useLOD0")]
        public bool MeshInLOD0 { get; set; }
    }
}
