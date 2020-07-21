using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.Modding.Editing;
using OpenTK;

namespace LDDModder.BrickEditor.Rendering
{
    public class BoneModel : PartElementModel
    {
        public PartBone Bone => Element as PartBone;

        public BoneModel(PartBone element) : base(element)
        {
            UpdateBoundingBox();
            SetTransformFromElement();
        }

        private void UpdateBoundingBox()
        {
            if (Bone.Bounding != null && !Bone.Bounding.IsEmpty)
            {
                BoundingBox = BBox.FromCenterSize(
                    (Vector3)Bone.Bounding.Center.ToGL(),
                    (Vector3)Bone.Bounding.Size.ToGL());
            }
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);
            if (e.PropertyName == nameof(PartBone.Bounding))
                UpdateBoundingBox();
        }

        protected override void ApplyTransformToElement(Matrix4 transform)
        {
            base.ApplyTransformToElement(transform);

        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            return RayIntersectsBoundingBox(ray, out distance);
        }

        public override void RenderModel(Camera camera)
        {
            base.RenderModel(camera);
            if (IsSelected)
            {
                RenderHelper.DrawBoundingBox(Transform, BoundingBox,
                        new Vector4(0f, 1f, 1f, 1f), 1.5f);
            }
        }
    }
}
