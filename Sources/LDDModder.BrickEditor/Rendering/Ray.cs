using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class Ray
    {
        public Vector3 Origin { get; set; }

        public Vector3 Direction { get; set; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Direction = direction.Normalized();
            Origin = origin;
        }

        public Vector3 GetPoint(float distance)
        {
            return Origin + Direction * distance;
        }

        public static Ray FromPoints(Vector3 pt1, Vector3 pt2)
        {
            return new Ray(pt1, (pt2 - pt1).Normalized());
        }

        public static Ray Transform(Ray ray, Matrix4 transform)
        {
            var origin = Vector3.TransformPosition(ray.Origin, transform);
            var dir = Vector3.TransformVector(ray.Direction, transform).Normalized();
            return new Ray(origin, dir);
        }

        public static bool RayIntersectsBox(Ray ray, BBox box, out float distance, bool forwardOnly = true)
        {
            var rayInv = Vector3.Divide(Vector3.One, ray.Direction);
            var t1 = Vector3.Multiply(box.Min - ray.Origin, rayInv);
            var t2 = Vector3.Multiply(box.Max - ray.Origin, rayInv);

            var vMin = Vector3.ComponentMin(t1, t2);
            var vMax = Vector3.ComponentMax(t1, t2);
            var min = Math.Max(vMin.X, System.Math.Max(vMin.Y, vMin.Z));
            var max = Math.Min(vMax.X, System.Math.Min(vMax.Y, vMax.Z));

            distance = float.NaN;

            if (max >= min)
                distance = forwardOnly & min < 0 ? max : min;

            return !float.IsNaN(distance);
        }
    }
}
