using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Models
{
   public class Set
    {
        [JsonProperty("set_num")]
        public string SetNumber { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("year")]
        public int Year { get; set; }
        [JsonProperty("theme_id")]
        public int ThemeID { get; set; }
        [JsonProperty("num_parts")]
        public int PartCount { get; set; }
        [JsonProperty("set_img_url")]
        public string SetImageUrl { get; set; }
        [JsonProperty("set_url")]
        public string SetUrl { get; set; }
        [JsonProperty("last_modified_dt")]
        public DateTime? last_modified_dt { get; set; }
    }
}
