using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MESH_HEADER
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Header;
        public int VertexCount;
        public int IndexCount;
        public int MeshType;

        public static MESH_HEADER Create(Meshes.MeshType meshType, int vertexCount, int indexCount)
        {
            return new MESH_HEADER
            {
                Header = "10GB",
                VertexCount = vertexCount,
                IndexCount = indexCount,
                MeshType = (int)meshType
            };
        }

        public static MESH_HEADER Create(Meshes.MeshGeometry meshGeometry)
        {
            return Create(meshGeometry.GetMeshType(), meshGeometry.Vertices.Count, meshGeometry.Indices.Count);
        }

        public static MESH_HEADER Create(Files.MeshFile mesh)
        {
            return Create(mesh.Geometry);
        }
    }
}
