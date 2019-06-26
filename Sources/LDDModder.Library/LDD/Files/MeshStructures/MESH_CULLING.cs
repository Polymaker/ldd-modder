using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct MESH_CULLING
    {
        public int Type;
        public int FromVertex;
        public int VertexCount;
        public int FromIndex;
        public int IndexCount;
        public MESH_STUD[] Studs;
        public MESH_CULLING_DATA[] UnknownData;
        public MESH_DATA? ReplacementGeometry;
    }
}
