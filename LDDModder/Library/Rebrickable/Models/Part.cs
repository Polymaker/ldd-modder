using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Models
{
    public class Part
    {
        [JsonProperty("part_num")]
        public string PartNum { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("part_cat_id")]
        public int PartCatId { get; set; }

        [JsonProperty("part_url")]
        public Uri PartUrl { get; set; }

        [JsonProperty("part_img_url")]
        public Uri PartImgUrl { get; set; }

        [JsonProperty("external_ids")]
        public ExternalPartIDs ExternalIds { get; set; }
    }
}
