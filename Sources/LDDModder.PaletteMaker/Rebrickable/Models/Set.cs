using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class Set
    {
        [JsonProperty("set_num")]
        public string SetNum { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("theme_id")]
        public int ThemeId { get; set; }

        [JsonProperty("num_parts")]
        public int NumParts { get; set; }

        [JsonProperty("set_img_url")]
        public string SetImgUrl { get; set; }

        [JsonProperty("set_url")]
        public string SetUrl { get; set; }

        [JsonProperty("last_modified_dt")]
        public DateTime? LastModifiedDate { get; set; }
    }
}
