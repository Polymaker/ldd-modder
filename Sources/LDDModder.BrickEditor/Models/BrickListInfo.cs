using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Models
{
    public class BrickListInfo
    {
        [JsonProperty]
        public string SourcePath { get; set; }

        [JsonProperty]
        public DateTime LastUpdate { get; set; }

        [JsonIgnore]
        public bool IsFromLif => (SourcePath ?? string.Empty).ToUpper().EndsWith("LIF");

        [JsonProperty]
        public List<BrickInfo> Bricks { get; set; }

        //[JsonIgnore]
        //public Dictionary<int, BrickInfo> Dictionary { get; private set; }

        public BrickListInfo()
        {
            Bricks = new List<BrickInfo>();
            //Dictionary = new Dictionary<int, BrickInfo>();
        }

        //public void RebuildDictionary()
        //{
        //    Dictionary.Clear();
        //    foreach (var brick in Bricks)
        //        Dictionary[brick.PartId] = brick;
        //}

        public BrickInfo GetBrick(int id)
        {
            return Bricks.FirstOrDefault(x => x.PartId == id);
        }

        public bool ContainsBrick(int id, out BrickInfo foundBrick)
        {
            foundBrick = Bricks.FirstOrDefault(x => x.PartId == id);
            return foundBrick != null;
            //if (Dictionary.Count == 0)
            //{
            //    foundBrick = Bricks.FirstOrDefault(x => x.PartId == id);
            //    return foundBrick != null;
            //}
            //else
            //    return Dictionary.TryGetValue(id, out foundBrick);
        }
    }
}
