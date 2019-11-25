using Newtonsoft.Json;
using System.Collections.Generic;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class ExternalPartIds
    {
        [JsonProperty("BrickLink")]
        public IList<string> BrickLink { get; set; }

        [JsonProperty("BrickOwl")]
        public IList<string> BrickOwl { get; set; }

        [JsonProperty("LEGO")]
        public IList<string> LEGO { get; set; }

        [JsonProperty("LDraw")]
        public IList<string> LDraw { get; set; }

        [JsonProperty("Peeron")]
        public IList<string> Peeron { get; set; }
    }
}
