using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class SurfaceModelMesh : PartElementModel
    {
        public int StartIndex { get; set; }
        public int StartVertex { get; set; }
        public int IndexCount { get; set; }
        //public bool Visible { get; set; }
        //public Matrix4 Transform { get; set; }

        public SurfaceComponent Component { get; set; }

        public PartSurface Surface => Component?.Surface;

        public ModelMeshReference Mesh { get; set; }

        

        //public BBox BoundingBox { get; set; }

        public GLSurfaceModel SurfaceModel { get; set; }

        public SurfaceModelMesh(ModelMeshReference mesh, int startIndex, int indexCount, int startVertex) : base(mesh)
        {
            Mesh = mesh;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;
            //Transform = Matrix4.Identity;
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            return SurfaceModel.RayIntersects(ray, this, out distance);
        }
    }
}
