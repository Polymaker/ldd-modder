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

        public bool IsReplacementModel { get; set; }

        public SurfaceModelMesh(GLSurfaceModel baseModel, ModelMeshReference meshRef, int startIndex, int indexCount, int startVertex) : base(meshRef)
        {
            MeshReference = meshRef;
            SurfaceModel = baseModel;
            StartIndex = startIndex;
            IndexCount = indexCount;
            StartVertex = startVertex;

            var baseTransform = meshRef.Transform.ToMatrix().ToGL();
            SetTransform(baseTransform, false);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            return SurfaceModel.RayIntersects(ray, this, out distance);
        }

        private bool ChangingTransform;

        protected override void OnTransformChanged()
        {
            base.OnTransformChanged();
            Matrix4 transCopy = Transform;
            transCopy.ClearScale();

            ChangingTransform = true;
            MeshReference.Transform = ItemTransform.FromMatrix(transCopy.ToLDD());
            ChangingTransform = false;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == nameof(MeshReference.Transform) && !ChangingTransform)
            {
                var baseTransform = MeshReference.Transform.ToMatrix().ToGL();
                SetTransform(baseTransform, true);
            }
        }

        public override void RenderModel(Camera camera)
        {
            SurfaceModel.RenderPartialModel(this);
        }
    }
}
