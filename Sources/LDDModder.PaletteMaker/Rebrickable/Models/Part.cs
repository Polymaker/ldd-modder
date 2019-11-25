using Newtonsoft.Json;
using System.Collections.Generic;

namespace LDDModder.PaletteMaker.Rebrickable.Models
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
        public string PartUrl { get; set; }

        [JsonProperty("part_img_url")]
        public string PartImgUrl { get; set; }

        [JsonProperty("prints")]
        public IList<string> Prints { get; set; }

        [JsonProperty("molds")]
        public IList<string> Molds { get; set; }

        [JsonProperty("alternates")]
        public IList<string> Alternates { get; set; }

        [JsonProperty("external_ids")]
        public ExternalPartIds ExternalIds { get; set; }

        [JsonProperty("print_of")]
        public string PrintOf { get; set; }
    }

}
