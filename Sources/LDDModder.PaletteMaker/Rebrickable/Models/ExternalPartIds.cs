using Newtonsoft.Json;
using System.Collections.Generic;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class ExternalPartIds
    {
        [JsonProperty("BrickLink")]
        public List<string> BrickLink { get; set; }

        [JsonProperty("BrickOwl")]
        public List<string> BrickOwl { get; set; }

        [JsonProperty("LEGO")]
        public List<string> LEGO { get; set; }

        [JsonProperty("LDraw")]
        public List<string> LDraw { get; set; }

        [JsonProperty("Peeron")]
        public List<string> Peeron { get; set; }
    }
}
