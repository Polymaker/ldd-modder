using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public class Plane
    {
        public Vector3 Origin { get; set; }
        public Vector3 Normal { get; set; }

        public Plane(Vector3 origin, Vector3 normal)
        {
            Origin = origin;
            Normal = normal;
        }

        public Vector3 ProjectPoint(Vector3 point)
        {
            var d = Vector3.Dot(Normal, Normal);
            var dist = Vector3.Dot(Origin - point, Normal);
            dist /= d;
            return point + (Normal * dist);
        }

        public Vector3 ProjectVector(Vector3 vector)
        {
            var p2 = ProjectPoint(Origin + vector);
            return (p2 - Origin).Normalized();
        }

        public Vector2 ProjectPoint2D(Vector3 axis, Vector3 point)
        {
            var perpAxis = Vector3.Cross(Normal, axis);
            var v = point - Origin;
            var t1 = Vector3.Dot(axis, v);
            var t2 = Vector3.Dot(perpAxis, v);

            return new Vector2(t1, t2);
        }

        public Vector2 ProjectVector2D(Vector3 axis, Vector3 vector)
        {
            var p2 = ProjectPoint2D(axis, Origin + vector);
            return p2.Normalized();
        }
    }
}
