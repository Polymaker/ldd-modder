using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.Rebrickable.Models
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
        public Color Color { get; set; }

        [JsonProperty("set_num")]
        public string SetNum { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("is_spare")]
        public bool IsSpare { get; set; }

        [JsonProperty("element_id")]
        public int? ElementId { get; set; }

        [JsonProperty("num_sets")]
        public int NumSets { get; set; }
    }
}
