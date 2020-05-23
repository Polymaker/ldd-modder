using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("part_count")]
        public int PartCount { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }
}
