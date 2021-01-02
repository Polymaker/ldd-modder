using LDDModder.LDD.Primitives.Collisions;
using LDDModder.Modding;
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

        private Vector3 EditedScale { get; set; }

        public CollisionModel(PartCollision collision) : base(collision)
        {
            PartCollision = collision;

            SetTransformFromElement();

            UpdateScaleTransform();
        }

        private void UpdateScaleTransform()
        {
            var scale = (Vector3)PartCollision.GetSize().ToGL() * 2f;
            ScaleTransform = Matrix4.CreateScale(scale);
            Scale = scale;
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, scale);
        }

        public void TransformSize(Vector3 amount)
        {
            EditedScale = Scale + amount;
            ScaleTransform = Matrix4.CreateScale(EditedScale);
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, EditedScale);
        }

        protected override void OnBeginEditTransform()
        {
            base.OnBeginEditTransform();
            EditedScale = Scale;
        }

        protected override void OnEndEditTransform(bool canceled)
        {
            if (!canceled)
            {
                PartCollision.SetSize(EditedScale.ToLDD() / 2f);
                UpdateScaleTransform();
            }
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

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == nameof(PartBoxCollision.Size) || 
                e.PropertyName == nameof(PartSphereCollision.Radius))
            {
                UpdateScaleTransform();
            }
        }

        public override void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {
            if (CollisionType == CollisionType.Box)
                DrawBoxCollision();
            else
                DrawSphereCollision();
        }

        private void DrawBoxCollision()
        {
            var wireColor = IsSelected ? RenderHelper.WireframeColorAlt : RenderHelper.WireframeColor;

            RenderHelper.RenderWithStencil(IsSelected, 
               () =>
               {
                   RenderHelper.BeginDrawModel(ModelManager.CubeModel, ScaleTransform * Transform, RenderHelper.CollisionMaterial);
                   RenderHelper.ModelShader.IsSelected.Set(IsSelected);

                   ModelManager.CubeModel.DrawElements();

                   RenderHelper.EndDrawModel(ModelManager.CubeModel);

                   RenderHelper.DrawBoundingBox(Transform, BoundingBox, wireColor, 1.5f);
               },
               () =>
               {
                   RenderHelper.DrawBoundingBox(Transform, BoundingBox, RenderHelper.SelectionOutlineColor, 4f);
               });
        }

        private void DrawSphereCollision()
        {
            RenderHelper.RenderWithStencil(
                () =>
                {
                    RenderHelper.BeginDrawModel(ModelManager.SphereModel, ScaleTransform * Transform, RenderHelper.CollisionMaterial);
                    RenderHelper.ModelShader.IsSelected.Set(IsSelected);
                    ModelManager.SphereModel.DrawElements();
                    RenderHelper.EndDrawModel(ModelManager.SphereModel);
                },
                () =>
                {
                    var wireColor = IsSelected ? RenderHelper.SelectionOutlineColor : RenderHelper.WireframeColor;

                    RenderHelper.ApplyStencilMask();

                    RenderHelper.BeginDrawWireframe(ModelManager.SphereModel.VertexBuffer, ScaleTransform * Transform, 
                        IsSelected ? 4f : 2.5f, wireColor);
                    ModelManager.SphereModel.DrawElements();
                    RenderHelper.EndDrawWireframe(ModelManager.SphereModel.VertexBuffer);
                });
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
