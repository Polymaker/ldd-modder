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


    }
}
