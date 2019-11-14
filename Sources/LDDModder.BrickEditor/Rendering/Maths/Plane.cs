using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public struct Plane
    {
        public Vector3 Origin { get; set; }
        public Vector3 Normal { get; set; }
        public float Distance { get; set; }

        public Plane(Vector3 origin, Vector3 normal, float distance)
        {
            Origin = origin;
            Normal = normal;
            Distance = distance;
        }

        public Vector2 ProjectPoint2D(Vector3 axis, Vector3 point)
        {
            var perpAxis = Vector3.Cross(Normal, axis);
            var v = point - Origin;
            var t1 = Vector3.Dot(axis, v);
            var t2 = Vector3.Dot(perpAxis, v);

            return new Vector2(t1, t2);
        }
    }
}
