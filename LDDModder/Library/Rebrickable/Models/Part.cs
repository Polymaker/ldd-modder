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

        [JsonProperty("year_from")]
        public int YearFrom { get; set; }

        [JsonProperty("year_to")]
        public int YearTo { get; set; }

        [JsonProperty("part_url")]
        public Uri PartUrl { get; set; }

        [JsonProperty("part_img_url")]
        public Uri PartImgUrl { get; set; }

        [JsonProperty("prints")]
        public string[] Prints { get; set; }

        [JsonProperty("molds")]
        public string[] Molds { get; set; }

        [JsonProperty("alternates")]
        public string[] Alternates { get; set; }

        [JsonProperty("external_ids")]
        public ExternalPartIDs ExternalIds { get; set; }
    }
}
