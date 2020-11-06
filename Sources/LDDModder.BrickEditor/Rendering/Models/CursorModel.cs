using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Models
{
    public class CursorModel : ModelBase
    {
        private float _Size;

        public float Size
        {
            get => _Size;
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    UpdateBounding();
                }
            }
        }

        public BSphere BoundingSphere { get; private set; }

        public CursorModel()
        {
            _Size = 0.5f;
            UpdateBounding();
        }

        private void UpdateBounding()
        {
            BoundingBox = BBox.FromCenterSize(Vector3.Zero, Vector3.One * Size);
            BoundingSphere = new BSphere(Vector3.Zero, Size);
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            var localRay = Ray.Transform(ray, Transform.Inverted());
            return Ray.IntersectsSphere(localRay, BoundingSphere, out distance);
        }

        public override void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {
            //var lineColor = new Vector4(0, 0, 0, 1);

            void renderAxes(Vector4 color, float thickness)
            {
                RenderHelper.DrawLine(Transform, color,
                    new Vector3(-Size, 0, 0), new Vector3(Size, 0, 0), thickness);
                RenderHelper.DrawLine(Transform, color,
                    new Vector3(0, -Size, 0), new Vector3(0, Size, 0), thickness);
                RenderHelper.DrawLine(Transform, color,
                    new Vector3(0, 0, -Size), new Vector3(0, 0, Size), thickness);
            }

            RenderHelper.RenderWithStencil(IsSelected, () =>
            {
                renderAxes(new Vector4(0, 0, 0, 1), 1.5f);
            }, () =>
            {
                renderAxes(new Vector4(1, 1, 1, 1), 3f);
            });
            
        }


        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();
            if (!Visible && IsSelected)
                IsSelected = false;
        }
    }
}
