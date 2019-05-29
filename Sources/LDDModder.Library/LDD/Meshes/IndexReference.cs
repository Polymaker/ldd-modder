using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class IndexReference
    {
        public int VertexIndex { get; set; }
        public int ShaderDataIndex { get; set; }
        public int RoundEdgeDataOffset { get; set; }
        public int AverageNormalIndex { get; set; }

        public IndexReference()
        {
        }

        public IndexReference(int vertexIndex)
        {
            VertexIndex = vertexIndex;
        }
    }
}
