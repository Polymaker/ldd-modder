using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class PartColor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rgb")]
        public string RgbHex { get; set; }

        [JsonProperty("is_trans")]
        public bool IsTransparent { get; set; }

        [JsonProperty("external_ids")]
        public ExternalColorIds ExternalColorIds { get; set; }
    }
}
