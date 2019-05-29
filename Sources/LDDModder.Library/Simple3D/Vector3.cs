using System;
using System.Collections.Generic;

namespace LDDModder.Simple3D
{
    public struct Vector3 : IEquatable<Vector3>, IEqualityComparer<Vector3>
    {

        #region Constants

        public static readonly Vector3 Zero = new Vector3();

        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);

        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);

        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        public static readonly Vector3 Empty = new Vector3(float.NaN, float.NaN, float.NaN);

        #endregion

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public bool IsEmpty => float.IsNaN(X);

        public float Length
        {
            get
            {
                if (IsEmpty)
                    return 0;
                var a = Math.Sqrt((X * X) + (Y * Y));
                return (float)Math.Sqrt((a * a) + (Z * Z));
            }
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float[] values)
        {
            if (values.Length != 3)
                throw new ArgumentException("The value array must have a length of 3");
            X = values[0];
            Y = values[1];
            Z = values[2];
        }

        public void Normalize()
        {
            if (!IsEmpty)
            {
                var len = Length;
                X /= len;
                Y /= len;
                Z /= len;
            }
        }

        public Vector3 Normalized()
        {
            if (!IsEmpty)
                return this / Length;
            return Vector3.Empty;
        }


        #region Equality comparison

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
                return false;
            return Equals((Vector3)obj);
        }

        public bool Equals(Vector3 other)
        {
            return Equals(other, 0.00001f);
        }

        public bool Equals(Vector3 other, float tolerence)
        {
            if (IsEmpty || other.IsEmpty)
                return IsEmpty == other.IsEmpty;
            return Math.Abs(X - other.X) < tolerence && Math.Abs(Y - other.Y) < tolerence && Math.Abs(Z - other.Z) < tolerence;
        }

        public bool Equals(Vector3 x, Vector3 y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public int GetHashCode(Vector3 obj)
        {
            return obj.GetHashCode();
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !(left == right);
        }

        #endregion


        public static Vector3 operator *(Vector3 vec, float number)
        {
            return new Vector3(vec.X * number, vec.Y * number, vec.Z * number);
        }

        public static Vector3 operator /(Vector3 vec, float number)
        {
            return new Vector3(vec.X / number, vec.Y / number, vec.Z / number);
        }

        public static Vector3 operator +(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(vec1.X + vec2.X, vec1.Y + vec2.Y, vec1.Z + vec2.Z);
        }

        public static Vector3 operator -(Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z);
        }

        public override string ToString()
        {
            return $"[{X};{Y};{Z}]";
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            return new Vector3
            {
                X = (left.Y * right.Z) - (left.Z * right.Y),
                Y = (left.Z * right.X) - (left.X * right.Z),
                Z = (left.X * right.Y) - (left.Y * right.X)
            };
        }

        public static float Distance(Vector3 left, Vector3 right)
        {
            var dx = left.X - right.X;
            var dy = left.Y - right.Y;
            var dz = left.Z - right.Z;
            return (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }

        public static float AngleBetween(Vector3 first, Vector3 second)
        {
            var dot = Dot(first, second);
            return (float)Math.Acos(MathHelper.Clamp(dot / (first.Length * second.Length), -1.0f, 1.0f));
        }

        public static Vector3 ProjectToPlane(Vector3 point, Vector3 planeOrigin, Vector3 planeNormal)
        {
            var v = point - planeOrigin;
            var dist = (v.X * planeNormal.X) + (v.Y * planeNormal.Y) + (v.Z * planeNormal.Z);
            return point - (planeNormal * dist);
        }

        public static Vector2 ProjectToPlane2D(Vector3 point, Vector3 planeOrigin, Vector3 planeNormal)
        {
            var v = point - planeOrigin;
            var dist = Dot(v, planeNormal);

            var projected = point - (planeNormal * dist);

            if (projected.X == planeOrigin.X)
                return new Vector2(projected.Y, projected.Z);
            else if (projected.Y == planeOrigin.Y)
                return new Vector2(projected.X, projected.Z);

            return new Vector2(projected.X, projected.Y);
        }

        public static Vector2 ProjectToPlane2D(Vector3 point, Vector3 planeOrigin, Vector3 planeNormal, Vector3 xAxis, Vector3 yAxis)
        {
            var v = point - planeOrigin;

            var t1 = Dot(xAxis, v);
            var t2 = Dot(yAxis, v);

            return new Vector2(t1, t2);
        }

        public static Vector3 GetPerpendicular(Vector3 v1, Vector3 v2, Vector3 point)
        {
            var dir1 = (v2 - v1).Normalized();
            var dir2 = (point - v1).Normalized();
            var c = Vector3.Cross(dir1, dir2).Normalized();
            var perp = Vector3.Cross(c, dir1).Normalized();
            var d1 = Vector3.Distance(dir2, perp);
            var d2 = Vector3.Distance(dir2, perp * -1);
            if (d2 < d1)
                perp *= -1;
            return perp;
        }

        public static Vector3 GetPerpIntersection(Vector3 v1, Vector3 v2, Vector3 point)
        {
            var line1 = (v2 - v1).Normalized();
            var line2 = (point - v1);
            var a = AngleBetween(line1, line2.Normalized());
            var d = (float)Math.Cos(a) * line2.Length;
            //var perp = GetPerpendicular(v1, v2, point);
            return v1 + (line1 * d);
        }
    }
}
