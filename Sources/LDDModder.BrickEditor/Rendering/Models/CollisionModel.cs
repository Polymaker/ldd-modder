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
        public Vector3 Scale { get; private set; }
        
        public Matrix4 ScaleTransform { get; private set; }

        public PartCollision PartCollision { get; set; }

        public CollisionType CollisionType => PartCollision.CollisionType;

        public Matrix4 ParentTransform { get; set; }

        public CollisionModel(PartCollision collision) : base(collision)
        {
            PartCollision = collision;
            ParentTransform = Matrix4.Identity;

            SetTransformFromElement();

            UpdateScaleTransform();
        }

        private void UpdateScaleTransform()
        {
            Vector3 scale = PartCollision.GetSize().ToGL() * 2f;
            ScaleTransform = Matrix4.CreateScale(scale);
            Scale = scale;
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, scale);
        }

        protected override void ApplyTransformToElement(Matrix4 transform)
        {
            if (PartCollision.Parent is PartBone partBone)
            {
                var parentTrans = partBone.Transform.ToMatrixD().ToGL();
                var localTrans = transform.ToMatrix4d() * parentTrans.Inverted();
                //transform = localTrans.ToMatrix4();
                PartCollision.Transform = ItemTransform.FromMatrix(localTrans.ToLDD());
            }
            else
                base.ApplyTransformToElement(transform);
        }

        protected override Matrix4 GetElementTransform()
        {
            var baseTransform = PartCollision.Transform.ToMatrixD().ToGL();

            if (PartCollision.Parent is PartBone partBone)
            {
                ParentTransform = partBone.Transform.ToMatrix().ToGL();
                baseTransform = baseTransform * partBone.Transform.ToMatrixD().ToGL();
            }

            return baseTransform.ToMatrix4();
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == nameof(PartBoxCollision.Size) || 
                e.PropertyName == nameof(PartSphereCollision.Radius))
            {
                UpdateScaleTransform();
            }
        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);

            if (CollisionType == CollisionType.Box)
                DrawBoxCollision();
            else
                DrawSphereCollision();
        }

        private void DrawBoxCollision()
        {
            RenderHelper.BeginDrawModel(ModelManager.CubeModel, ScaleTransform * Transform, RenderHelper.CollisionMaterial);
            RenderHelper.ModelShader.IsSelected.Set(IsSelected);

            ModelManager.CubeModel.DrawElements();

            RenderHelper.EndDrawModel(ModelManager.CubeModel);

            var wireColor = IsSelected ? RenderHelper.WireframeColorAlt : RenderHelper.WireframeColor;
            RenderHelper.DrawBoundingBox(Transform, BoundingBox, wireColor, 1.5f);
        }

        private void DrawSphereCollision()
        {
            RenderHelper.EnableStencilTest();
            RenderHelper.EnableStencilMask();

            RenderHelper.BeginDrawModel(ModelManager.SphereModel, ScaleTransform * Transform, RenderHelper.CollisionMaterial);
            RenderHelper.ModelShader.IsSelected.Set(IsSelected);
            ModelManager.SphereModel.DrawElements();
            RenderHelper.EndDrawModel(ModelManager.SphereModel);

            var wireColor = IsSelected ? RenderHelper.WireframeColorAlt : RenderHelper.WireframeColor;
            
            RenderHelper.ApplyStencilMask();

            RenderHelper.BeginDrawWireframe(ModelManager.SphereModel.VertexBuffer, ScaleTransform * Transform, 2.5f, wireColor);
            ModelManager.SphereModel.DrawElements();
            RenderHelper.EndDrawWireframe(ModelManager.SphereModel.VertexBuffer);

            RenderHelper.RemoveStencilMask();
            RenderHelper.ClearStencil();
            RenderHelper.DisableStencilTest();
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
