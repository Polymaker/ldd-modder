using Newtonsoft.Json;
using System.Collections.Generic;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class ExternalColorIds
    {
        [JsonProperty("BrickLink")]
        public IList<ColorIds> BrickLink { get; set; }

        [JsonProperty("BrickOwl")]
        public IList<ColorIds> BrickOwl { get; set; }

        [JsonProperty("LEGO")]
        public IList<ColorIds> LEGO { get; set; }

        [JsonProperty("LDraw")]
        public IList<ColorIds> LDraw { get; set; }

        //[JsonProperty("Peeron")]
        //public IList<ColorIds> Peeron { get; set; }
    }
}
