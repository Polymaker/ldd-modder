using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class PartColor
    {
        [JsonProperty("color_id")]
        public int ColorId { get; set; }

        [JsonProperty("color_name")]
        public string ColorName { get; set; }

        [JsonProperty("num_sets")]
        public int NumSets { get; set; }

        [JsonProperty("num_set_parts")]
        public int NumSetParts { get; set; }

        [JsonProperty("part_img_url")]
        public string PartImgUrl { get; set; }

        [JsonProperty("elements")]
        public List<string> Elements { get; set; }
    }
}
