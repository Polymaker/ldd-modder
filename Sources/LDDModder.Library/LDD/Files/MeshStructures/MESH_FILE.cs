using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct MESH_FILE
    {
        public MESH_HEADER Header;
        public MESH_DATA Geometry;
        public Vector3[] AverageNormals;
        public ROUNDEDGE_SHADER_DATA[] RoundEdgeShaderData;
        public MESH_CULLING[] Cullings;

        public bool IsTextured => Header.MeshType == 0x3B || Header.MeshType == 0x3F;

        public bool IsFlexible => Header.MeshType == 0x3E || Header.MeshType == 0x3F;

        public ROUNDEDGE_SHADER_DATA GetShaderDataFromOffset(int offset)
        {
            int total = 0;

            for(int i = 0; i < RoundEdgeShaderData.Length; i++)
            {
                if (total == offset)
                    return RoundEdgeShaderData[i];

                total += RoundEdgeShaderData[i].Coords.Length * 2;
            }

            return default(ROUNDEDGE_SHADER_DATA);
        }

        public int GetShaderDataIndexFromOffset(int offset)
        {
            int total = 0;

            for (int i = 0; i < RoundEdgeShaderData.Length; i++)
            {
                if (total == offset)
                    return i;

                total += RoundEdgeShaderData[i].Coords.Length * 2;
            }

            return -1;
        }

        public bool GetShaderDataFromOffset(int offset, out ROUNDEDGE_SHADER_DATA data)
        {
            int total = 0;
            data = default(ROUNDEDGE_SHADER_DATA);

            for (int i = 0; i < RoundEdgeShaderData.Length; i++)
            {
                if (total == offset)
                {
                    data = RoundEdgeShaderData[i];
                    return true;
                }

                total += RoundEdgeShaderData[i].Coords.Length * 2;
            }

            return false;
        }
    }
}
