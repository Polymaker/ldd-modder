using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Models
{
    public class PartialModel
    {
        public IVertexBuffer VertexBuffer { get; set; }
        public int StartIndex { get; set; }
        public int StartVertex { get; set; }
        public int IndexCount { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public BBox BoundingBox { get; set; }

        public PartialModel(IVertexBuffer vertexBuffer, int startIndex, int startVertex, int indexCount, PrimitiveType primitiveType)
        {
            VertexBuffer = vertexBuffer;
            StartIndex = startIndex;
            StartVertex = startVertex;
            IndexCount = indexCount;
            PrimitiveType = primitiveType;
        }

        public void CalculateBoundingBox()
        {

        }

        public void DrawElements()
        {
            VertexBuffer.DrawElementsBaseVertex(PrimitiveType, StartVertex, IndexCount, StartIndex * 4);
        }
    }
}
