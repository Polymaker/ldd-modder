using Newtonsoft.Json;
using System.Collections.Generic;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class ExternalColorIds
    {
        [JsonProperty("BrickLink")]
        public ColorIds BrickLink { get; set; }

        [JsonProperty("BrickOwl")]
        public ColorIds BrickOwl { get; set; }

        [JsonProperty("LEGO")]
        public ColorIds LEGO { get; set; }

        [JsonProperty("LDraw")]
        public ColorIds LDraw { get; set; }

        //[JsonProperty("Peeron")]
        //public List<ColorIds> Peeron { get; set; }
    }
}
