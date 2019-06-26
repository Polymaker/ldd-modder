using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct MESH_CULLING_DATA
    {
        public byte[] Data;

        public MESH_CULLING_DATA(byte[] data)
        {
            Data = data;
        }
    }
}
