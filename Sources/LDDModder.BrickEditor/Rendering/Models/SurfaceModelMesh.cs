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

        public SurfaceComponent Component => MeshReference.Parent as SurfaceComponent;

        public PartSurface Surface => Component?.Surface;

        public ModelMeshReference MeshReference { get; private set; }

        public GLSurfaceModel SurfaceModel { get; private set; }
        public SurfaceModelMesh(GLSurfaceModel baseModel, ModelMeshReference meshRef, int startIndex, int indexCount, int startVertex) : base(meshRef)
        {
            MeshReference = meshRef;
            SurfaceModel = baseModel;
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
