using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class Theme
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_id")]
        public int? ParentID { get; set; }
    }
}
