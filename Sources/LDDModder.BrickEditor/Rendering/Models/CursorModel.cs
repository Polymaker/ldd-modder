using ObjectTK;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Models
{
    public class CursorModel : ModelBase
    {
        private float _UISize;

        public float UISize
        {
            get => _UISize;
            set
            {
                if (_UISize != value)
                {
                    _UISize = value;
                    //UpdateBounding();
                }
            }
        }

        private float ProjectedSize;

        public BSphere BoundingSphere { get; private set; }

        public CursorModel()
        {
            _UISize = 24f;
            //UpdateBounding();
        }

        private void UpdateBounding()
        {
            
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, Vector3.One * ProjectedSize);
            BoundingSphere = new BSphere(Vector3.Zero, ProjectedSize);
        }

        public override bool RayIntersectsBoundingBox(Ray ray, out float distance)
        {
            UpdateBounding();
            return base.RayIntersectsBoundingBox(ray, out distance);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            UpdateBounding();
            var localRay = Ray.Transform(ray, Transform.Inverted());
            return Ray.IntersectsSphere(localRay, BoundingSphere, out distance);
        }

        public override void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {
            //var lineColor = new Vector4(0, 0, 0, 1);

            //var cursorPos2d = camera.WorldPointToScreen(Transform.ExtractTranslation());
            float screenScale = camera.GetUIScaleRatio(Origin);

            float innerCircleSize = screenScale * UISize;
            float outerCircleSize = screenScale * (UISize + 2);
            float axesLength = screenScale * (UISize + (UISize * 0.25f));
            float axesOutlineLength = axesLength + (1 * screenScale);

            var innerCircleScale = Matrix4.CreateScale(innerCircleSize);
            var outerCircleScale = Matrix4.CreateScale(outerCircleSize);

            var cameraRot = camera.GetViewMatrix().ExtractRotation();
            var circleRot = Matrix4.CreateRotationX((float)Math.PI / 2f) * Matrix4.CreateFromQuaternion(cameraRot).Inverted();
            var circleTrans = Matrix4.CreateTranslation(Origin); 

            var innerCircleMat = circleRot * innerCircleScale * circleTrans;
            var outterCircleMat = circleRot * outerCircleScale * circleTrans;

            ProjectedSize = axesLength;

            void renderAxes(Vector4 color, float length, float thickness)
            {
                RenderHelper.DrawLine(Transform, color,
                    new Vector3(-length, 0, 0), new Vector3(length, 0, 0), thickness);
                RenderHelper.DrawLine(Transform, color,
                    new Vector3(0, -length, 0), new Vector3(0, length, 0), thickness);
                RenderHelper.DrawLine(Transform, color,
                    new Vector3(0, 0, -length), new Vector3(0, 0, length), thickness);
            }

            RenderHelper.RenderWithStencil(IsSelected, () =>
            {
                renderAxes(new Vector4(0, 0, 0, 1), axesLength, 2f);

                GL.PushAttrib(AttribMask.LineBit);
                
                GL.LineWidth(1f);
                ModelManager.CircleModel.DrawColored(innerCircleMat, new Vector4(1, 1, 1, 1));
                ModelManager.CircleModel.DrawColored(outterCircleMat, new Vector4(0, 0, 0, 1));
                GL.PopAttrib();


            }, () =>
            {
                renderAxes(new Vector4(1, 1, 1, 1), axesOutlineLength, 4f);

            });

            //RenderHelper.ColorShader.Use();
            //var curView = RenderHelper.ColorShader.ViewMatrix.Value;
            //var curProj = RenderHelper.ColorShader.Projection.Value;
            //RenderHelper.ColorShader.ViewMatrix.Set(Matrix4.Identity);
            //RenderHelper.ColorShader.Projection.Set(UIRenderHelper.ProjectionMatrix);

            //var trans = Matrix4.CreateTranslation(new Vector3(cursorPos2d.X, cursorPos2d.Y, 0));
            //var rot = Matrix4.CreateRotationX((float)Math.PI / 2f);
            //var scale = Matrix4.CreateScale(30);

            //ModelManager.CircleModel.DrawColored(rot * scale * trans, new Vector4(1,0,0,1));
            //scale = Matrix4.CreateScale(31);
            //ModelManager.CircleModel.DrawColored(rot * scale * trans, new Vector4(1, 1, 1, 1));
            //RenderHelper.ColorShader.ViewMatrix.Set(curView);
            //RenderHelper.ColorShader.Projection.Set(curProj);
        }


        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();
            if (!Visible && IsSelected)
                IsSelected = false;
        }
    }
}
