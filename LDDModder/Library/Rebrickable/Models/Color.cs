using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Models
{
    public class Color
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rgb")]
        public string Rgb { get; set; }

        [JsonProperty("is_trans")]
        public bool IsTrans { get; set; }

        [JsonProperty("external_ids")]
        public ExternalColorIDs ExternalIds { get; set; }
    }
}
