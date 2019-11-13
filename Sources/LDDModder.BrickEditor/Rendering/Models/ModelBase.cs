using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class ModelBase
    {
        public bool Visible { get; set; }

        public BBox BoundingBox { get; set; }

        public Matrix4 Transform { get; set; }

        public ModelBase()
        {
            Visible = true;
            Transform = Matrix4.Identity;
        }

        public virtual bool RayIntersectsBoundingBox(Ray ray, out float distance)
        {
            var localRay = Ray.Transform(ray, Transform.Inverted());
            return Ray.IntersectsBox(localRay, BoundingBox, out distance);
        }

        public abstract bool RayIntersects(Ray ray, out float distance);

        public virtual BBox GetWorldBoundingBox()
        {
            var corners = BoundingBox.GetCorners();
            for (int i = 0; i < 8; i++)
                corners[i] = Vector3.TransformPosition(corners[i], Transform);
            return BBox.FromVertices(corners);
        }

        public virtual void RenderModel(Camera camera)
        {

        }
    }
}
