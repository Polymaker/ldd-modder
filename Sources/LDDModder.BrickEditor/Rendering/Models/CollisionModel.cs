using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class CollisionModel : PartElementModel
    {
        public GLModel BaseModel { get; private set; }

        public Vector3 Scale { get; private set; }
        
        public Matrix4 ScaleTransform { get; private set; }

        public PartCollision PartCollision { get; set; }

        public CollisionType CollisionType => PartCollision.CollisionType;

        private bool ChangingTransform;

        public CollisionModel(PartCollision collision, GLModel baseModel) : base(collision)
        {
            PartCollision = collision;
            BaseModel = baseModel;

            var baseTransform = collision.Transform.ToMatrix().ToGL();
            SetTransform(baseTransform, false);

            UpdateScaleTransform();
        }

        private void UpdateScaleTransform()
        {
            Vector3 scale = PartCollision.GetSize().ToGL() * 2f;
            ScaleTransform = Matrix4.CreateScale(scale);
            Scale = scale;
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, scale);
        }
        
        protected override void OnTransformChanged()
        {
            base.OnTransformChanged();
            Matrix4 transCopy = Transform;
            transCopy.ClearScale();

            ChangingTransform = true;
            PartCollision.Transform = ItemTransform.FromMatrix(transCopy.ToLDD());
            ChangingTransform = false;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == "Transform" && !ChangingTransform)
            {
                var baseTransform = PartCollision.Transform.ToMatrix().ToGL();
                SetTransform(baseTransform, true);
            }
            else if (e.PropertyName == "Size" || e.PropertyName == "Radius")
            {
                UpdateScaleTransform();
            }
        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);

            RenderHelper.BeginDrawModel(BaseModel.VertexBuffer, ScaleTransform * Transform, BaseModel.Material);
            RenderHelper.ModelShader.IsSelected.Set(IsSelected);

            BaseModel.Draw(CollisionType == CollisionType.Box ?
                OpenTK.Graphics.OpenGL.PrimitiveType.Quads :
                OpenTK.Graphics.OpenGL.PrimitiveType.Triangles);

            RenderHelper.EndDrawModel(BaseModel.VertexBuffer);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            if (CollisionType == CollisionType.Box)
                return RayIntersectsBoundingBox(ray, out distance);

            var pos = Vector3.TransformPosition(Vector3.Zero, Transform);
            var sphere = new BSphere(pos, Scale.X / 2f);

            return Ray.IntersectsSphere(ray, sphere, out distance);
        }
    }
}
