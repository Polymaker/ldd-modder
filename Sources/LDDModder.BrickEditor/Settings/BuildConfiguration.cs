using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class BuildConfiguration
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("outputPath", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputPath { get; set; }
        
        [JsonProperty("useLOD0Subdirectory")]
        public bool LOD0Subdirectory { get; set; }

        [JsonProperty("confirmOverwrite")]
        public bool ConfirmOverwrite { get; set; }

        //[JsonProperty("compress", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(false)]
        //public bool CreateZip { get; set; }

        [JsonIgnore]
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public int InternalFlag { get; set; }

        public bool ShouldSerializeName()
        {
            return InternalFlag == 0;
        }

        public bool ShouldSerializeOutputPath()
        {
            return InternalFlag == 0;
        }

        public bool ShouldSerializeLOD0Subdirectory()
        {
            return InternalFlag != 1;
        }
    }
}
