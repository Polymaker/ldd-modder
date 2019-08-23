using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Quaternion
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector3 Xyz
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public float LengthSquared => X * X + Y * Y + Z * Z + W * W;

        public float Length => (float)Math.Sqrt(LengthSquared);

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public void Normalize()
        {
            float scale = 1f / Length;
            Xyz *= scale;
            W *= scale;
        }

        public void ToAxisAngle(out Vector3 axis, out float angle)
        {
            Quaternion q = this;
            if (Math.Abs(q.W) > 1f)
            {
                q.Normalize();
            }
            angle = 2f * (float)Math.Acos((double)q.W);
            float den = (float)Math.Sqrt(1.0 - (double)(q.W * q.W));
            if (den > 0.0001f)
            {
                axis = q.Xyz / den;
            }
            else
            {
                axis = Vector3.UnitX;
            }
        }
    }
}
