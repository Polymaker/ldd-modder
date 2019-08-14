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
        public Vertex Vertex { get; private set; }

        public RoundEdgeData RoundEdgeData { get; set; }

        public Vector3 AverageNormal { get; set; }

        public VertexIndex(Vertex vertex)
        {
            Vertex = vertex;
            AverageNormal = Vector3.UnitY;
            RoundEdgeData = RoundEdgeData.NoOutline;
        }

        internal void ReassignVertex(Vertex vertex)
        {
            Vertex = vertex;
        }
    }
}
