using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct MESH_DATA
    {
        public Vector3[] Positions;
        public Vector3[] Normals;
        public Vector2[] UVs;
        public MESH_INDEX[] Indices;
        public MESH_BONE_MAPPING[] Bones;

        public static MESH_DATA Create(MESH_HEADER header)
        {
            return new MESH_DATA()
            {
                Positions = new Vector3[header.VertexCount],
                Normals = new Vector3[header.VertexCount],
                UVs = (header.MeshType == 0x3B || header.MeshType == 0x3F) ? new Vector2[header.VertexCount] : new Vector2[0],
                Indices = new MESH_INDEX[header.IndexCount],
                Bones = (header.MeshType == 0x3E || header.MeshType == 0x3F) ? new MESH_BONE_MAPPING[header.VertexCount] : new MESH_BONE_MAPPING[0],
            };
        }

        public static MESH_DATA Create(int vertexCount, int indexCount, bool textured, bool flexible)
        {
            return new MESH_DATA()
            {
                Positions = new Vector3[vertexCount],
                Normals = new Vector3[vertexCount],
                UVs = textured ? new Vector2[vertexCount] : new Vector2[0],
                Indices = new MESH_INDEX[indexCount],
                Bones = flexible ? new MESH_BONE_MAPPING[vertexCount] : new MESH_BONE_MAPPING[0],
            };
        }

        public static MESH_DATA Create(Meshes.MeshGeometry meshGeometry)
        {
            return Create(meshGeometry.Vertices.Count, meshGeometry.Indices.Count, meshGeometry.IsTextured, meshGeometry.IsFlexible);
        }
    }
}
