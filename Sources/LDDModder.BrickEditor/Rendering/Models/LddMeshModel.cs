using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class LddMeshModel
    {
        public int StartIndex { get; set; }
        public int StartVertex { get; set; }
        public int IndexCount { get; set; }
        public bool Visible { get; set; }
        public Matrix4 Transform { get; set; }
        public ModelMesh Mesh { get; set; }

        public LddMeshModel(ModelMesh mesh, int startIndex, int indexCount)
        {
            Mesh = mesh;
            StartIndex = startIndex;
            IndexCount = indexCount;
            Transform = Matrix4.Identity;
        }

        public LddMeshModel(ModelMesh mesh, int startIndex, int indexCount, int startVertex)
        {
            Mesh = mesh;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;
            Transform = Matrix4.Identity;
        }
    }
}
