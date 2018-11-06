using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Models
{
    public class ColorIDs
    {
        [JsonProperty("ext_descrs")]
        public List<List<string>> Descriptions { get; set; }
        [JsonProperty("ext_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<int?> IDs { get; set; }
    }

    public class ExternalColorIDs
    {
        [JsonProperty("BrickOwl", NullValueHandling = NullValueHandling.Ignore)]
        public ColorIDs BrickOwl { get; set; }
        [JsonProperty("LDraw", NullValueHandling = NullValueHandling.Ignore)]
        public ColorIDs LDraw { get; set; }
        [JsonProperty("LEGO", NullValueHandling = NullValueHandling.Ignore)]
        public ColorIDs LEGO { get; set; }
        [JsonProperty("BrickLink", NullValueHandling = NullValueHandling.Ignore)]
        public ColorIDs BrickLink { get; set; }
        [JsonProperty("Peeron", NullValueHandling = NullValueHandling.Ignore)]
        public ColorIDs Peeron { get; set; }
    }
}
