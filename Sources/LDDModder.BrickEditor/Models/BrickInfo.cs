using LDDModder.LDD.Parts;
using LDDModder.LDD.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Models
{
    public class BrickInfo
    {
        [JsonProperty]
        public int PartId { get; set; }
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

        [JsonIgnore]
        public bool Validated { get; set; }

        public BrickInfo()
        {
        }

        public BrickInfo(Primitive primitive, string primitivePath, string[] meshPaths)
        {
            PartId = primitive.ID;
            Description = primitive.Name;
            PrimitiveFilename = primitivePath;
            Platform = primitive.Platform.Name;
            Category = primitive.MainGroup.Name;
            MeshFilenames = meshPaths;
            Flexible = primitive.FlexBones.Any();
            Decorated = meshPaths.Length > 1;
        }

        public BrickInfo(PartWrapper part) : this(
            part.Primitive, 
            $"{part.PartID}.xml", 
            part.Surfaces.Select(x=>x.GetFileName()).ToArray())
        {
        }


        public void UpdateInfo(Primitive primitive)
        {
            PartId = primitive.ID;
            Description = primitive.Name;
            PrimitiveFilename = $"{PartId}.xml";
            Platform = primitive.Platform.Name;
            Category = primitive.MainGroup.Name;
            Flexible = primitive.FlexBones.Any();
        }

        public void UpdateInfo(PartWrapper part)
        {
            UpdateInfo(part.Primitive);
            MeshFilenames = part.Surfaces.Select(x => x.GetFileName()).ToArray();
            Decorated = MeshFilenames.Length > 1;
        }
    }
}
