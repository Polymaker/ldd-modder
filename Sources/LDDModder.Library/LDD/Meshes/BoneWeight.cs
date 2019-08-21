using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public struct BoneWeight
    {
        public int BoneID { get; set; }
        public float Weight { get; set; }

        public BoneWeight(int boneID, float weight)
        {
            BoneID = boneID;
            Weight = weight;
        }
    }
}
