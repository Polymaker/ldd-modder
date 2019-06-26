using LDDModder.LDD.Files.MeshStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Mesh2
    {
        public MESH_FILE? OriginalData { get; }

        public MeshType Type { get; }

        public MeshGeometry Geometry { get; private set; }

        internal Mesh2(MESH_FILE originalData, MeshType type)
        {
            OriginalData = originalData;
            Type = type;
        }

        public Mesh2(MeshType type)
        {
            Type = type;
        }

        public void SetGeometry(MeshGeometry geometry)
        {
            Geometry = geometry;
        }
    }
}
