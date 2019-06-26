using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct MESH_BONE_MAPPING
    {
        public MESH_BONE_WEIGHT[] BoneWeights;

        public MESH_BONE_MAPPING(int count)
        {
            BoneWeights = new MESH_BONE_WEIGHT[count];
        }
    }
}
