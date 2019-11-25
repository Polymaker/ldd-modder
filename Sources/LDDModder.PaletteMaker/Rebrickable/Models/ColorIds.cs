using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class ColorIds
    {
        [JsonProperty("ext_ids")]
        public IList<int> ExtIds { get; set; }

        [JsonProperty("ext_descrs")]
        public IList<IList<string>> ExtDescrs { get; set; }
    }
}
