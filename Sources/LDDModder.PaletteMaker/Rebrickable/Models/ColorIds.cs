using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class ColorIds
    {
        [JsonProperty("ext_ids")]
        public List<int> ExtIds { get; set; }

        [JsonProperty("ext_descrs")]
        public List<string[]> ExtDescrs { get; set; }
    }
}
