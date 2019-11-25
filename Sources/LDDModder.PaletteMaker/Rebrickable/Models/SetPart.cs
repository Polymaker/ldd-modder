using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.PaletteMaker.Rebrickable.Models
{
    public class SetPart
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("inv_part_id")]
        public int InvPartId { get; set; }

        [JsonProperty("part")]
        public Part Part { get; set; }

        [JsonProperty("color")]
        public PartColor Color { get; set; }

        [JsonProperty("set_num")]
        public string SetNum { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("is_spare")]
        public bool IsSpare { get; set; }

        [JsonProperty("element_id")]
        public string ElementId { get; set; }

        [JsonProperty("num_sets")]
        public int NumSets { get; set; }
    }
}
