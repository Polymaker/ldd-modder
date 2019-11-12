using LDDModder.LDD.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Data
{
    public class LddPartInfo
    {
        [JsonProperty]
        public int PartId { get; set; }
        [JsonProperty]
        public List<int> Aliases { get; set; } = new List<int>();
        [JsonProperty]
        public string Platform { get; set; }
        [JsonProperty]
        public string Category { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public string PrimitiveFilename { get; set; }
        [JsonProperty]
        public string[] MeshFilenames { get; set; }
        [JsonProperty]
        public bool Decorated { get; set; }
        [JsonProperty]
        public bool Flexible { get; set; }
        [JsonProperty]
        public DateTime LastUpdate { get; set; }

        public LddPartInfo()
        {
        }

        public LddPartInfo(Primitive primitive, string primitivePath, string[] meshPaths)
        {
            PartId = primitive.ID;
            Aliases = primitive.Aliases.ToList();
            Description = primitive.Name;
            PrimitiveFilename = primitivePath;
            Platform = primitive.Platform.Name;
            Category = primitive.MainGroup.Name;
            MeshFilenames = meshPaths;
            Flexible = primitive.FlexBones.Any();
            Decorated = meshPaths.Length > 1;
        }
    }
}
