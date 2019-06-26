using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class VertexIndex
    {
        public Vertex Vertex { get; }

        public Vector2[] RoundEdgeData { get; set; }

        public Vector3 AverageNormal { get; set; }

        public VertexIndex(Vertex vertex)
        {
            Vertex = vertex;
            RoundEdgeData = new Vector2[6];
        }
    }
}
