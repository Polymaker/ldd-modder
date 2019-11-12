using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Data
{
    public class LddPartListCache
    {
        [JsonProperty]
        public string PrimitivesPath { get; set; }
        [JsonProperty]
        public string MeshesPath { get; set; }
        [JsonProperty]
        public List<LddPartInfo> Parts { get; set; }
        [JsonProperty]
        public DateTime LastUpdate { get; set; }

        public LddPartListCache()
        {
        }

        public bool ContainsPart(int id, out LddPartInfo foundBrick)
        {
            foundBrick = Parts.FirstOrDefault(x => x.PartId == id);
            return foundBrick != null;
        }
    }
}
