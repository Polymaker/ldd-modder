﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LDDModder.Simple3D
{
    [Serializable]
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

        public float this[int i]
        {
            get
            {
                switch(i)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        break;
                }
            }
        }

        #region Permutation Properties

        [XmlIgnore]
        public Vector2 Xy
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        [XmlIgnore]
        public Vector2 Xz
        {
            get => new Vector2(X, Z);
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        [XmlIgnore]
        public Vector2 Yx
        {
            get => new Vector2(Y, X);
            set
            {
                Y = value.X;
                X = value.Y;
            }
        }

        #endregion

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

        public Vector3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(float[] values)
        {
            if (values.Length != 3)
                throw new ArgumentException("The value array must have a length of 3");
            X = values[0];
            Y = values[1];
            Z = values[2];
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
            return Equals(other, 0.0001f);
        }

        public bool Equals(Vector3 other, float tolerence)
        {
            if (IsEmpty || other.IsEmpty)
                return IsEmpty == other.IsEmpty;
            return Distance(this, other) <= tolerence;
            //return Math.Abs(X - other.X) < tolerence && Math.Abs(Y - other.Y) < tolerence && Math.Abs(Z - other.Z) < tolerence;
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

        #region Operators

        public static Vector3 operator *(Vector3 vec, float number)
        {
            return new Vector3(vec.X * number, vec.Y * number, vec.Z * number);
        }

        public static Vector3 operator *(float number, Vector3 vec)
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

        public static Vector3 operator *(Matrix3 matrix, Vector3 vector)
        {
            Vector3 result = default;
            result.X = vector.X * matrix.A1 + vector.Y * matrix.A2 + vector.Z * matrix.A3;
            result.Y = vector.X * matrix.B1 + vector.Y * matrix.B2 + vector.Z * matrix.B3;
            result.Z = vector.X * matrix.C1 + vector.Y * matrix.C2 + vector.Z * matrix.C3;
            return result;
        }

        public static Vector3 operator *(Matrix4 matrix, Vector3 vector)
        {
            Vector3 result = default;
            result.X = vector.X * matrix.A1 + vector.Y * matrix.A2 + vector.Z * matrix.A3 + matrix.A4;
            result.Y = vector.X * matrix.B1 + vector.Y * matrix.B2 + vector.Z * matrix.B3 + matrix.B4;
            result.Z = vector.X * matrix.C1 + vector.Y * matrix.C2 + vector.Z * matrix.C3 + matrix.C4;
            return result;
        }

        public static explicit operator Vector3(Vector3d vector)
        {
            return new Vector3((float)vector.X, (float)vector.Y, (float)vector.Z);
        }

        #endregion

        #region Functions

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
            if (Length == 0)
                return this;
            return Empty;
        }

        public Vector3 Rounded(int decimals = 4)
        {
            if (IsEmpty)
                return this;

            return new Vector3(
                (float)Math.Round(X, decimals), 
                (float)Math.Round(Y, decimals), 
                (float)Math.Round(Z, decimals));
        }

        public static Vector3 RoundByStep(Vector3 vector, float step)
        {
            if (vector.IsEmpty)
                return Empty;

            return new Vector3(
                (float)Math.Round(vector.X / step) * step,
                (float)Math.Round(vector.Y / step) * step,
                (float)Math.Round(vector.Z / step) * step);
        }

        public static Vector3 FloorByStep(Vector3 vector, float step)
        {
            if (vector.IsEmpty)
                return Empty;

            return new Vector3(
                (float)Math.Floor(vector.X / step) * step,
                (float)Math.Floor(vector.Y / step) * step,
                (float)Math.Floor(vector.Z / step) * step);
        }

        public static Vector3 CeilingByStep(Vector3 vector, float step)
        {
            if (vector.IsEmpty)
                return Empty;

            return new Vector3(
                (float)Math.Ceiling(vector.X / step) * step,
                (float)Math.Ceiling(vector.Y / step) * step,
                (float)Math.Ceiling(vector.Z / step) * step);
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
            float result = (float)Math.Acos(MathHelper.Clamp(dot / (first.Length * second.Length), -1.0f, 1.0f));
            if (float.IsNaN(result))
            {
                if (first.Equals(second))
                    return 0f;
            }
            return result;
        }

        public static Vector3 CalculateNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var u = v2 - v1;
            var v = v3 - v1;
            var n = new Vector3((u.Y * v.Z) - (u.Z * v.Y), (u.Z * v.X) - (u.X * v.Z), (u.X * v.Y) - (u.Y * v.X));
            return n.Normalized();
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

        public static Vector2 GetPlanarDistance(Vector3 v1, Vector3 v2, Vector3 pt, Vector3 planeNormal)
        {
            var hAxis = (v2 - v1).Normalized();
            var vAxis = Cross(planeNormal, hAxis);
            var v = pt - v1;
            var t1 = Dot(hAxis, v);
            var t2 = Dot(vAxis, v);

            return new Vector2(t1, t2);
        }

        public static Vector2 GetPlanarDistance(Vector3 v1, Vector3 v2, Vector3 pt)
        {
            var planeNormal = CalculateNormal(v1, v2, pt);
            var hAxis = (v2 - v1).Normalized();
            var vAxis = Cross(planeNormal, hAxis);
            var v = pt - v1;
            var t1 = Dot(hAxis, v);
            var t2 = Dot(vAxis, v);

            return new Vector2(t1, t2);
        }

        public static Vector3 GetPerpendicular(Vector3 v1, Vector3 v2, Vector3 point)
        {
            var d = (v2 - v1).Normalized();
            var v = point - v1;
            var t = Dot(v, d);
            var P = v1 + (t * d);
            return (point - P).Normalized();

            //var dir1 = (v2 - v1).Normalized();
            //var dir2 = (point - v1).Normalized();
            //var c = Cross(dir1, dir2).Normalized();
            //var perp = Cross(c, dir1).Normalized();
            //var d1 = Distance(point, v1 + perp);
            //var d2 = Distance(point, v1 - perp);
            //if (d2 < d1)
            //    perp *= -1;
            //return perp;
        }

        public static Vector3 GetPerpendicular(Vector3 v1, Vector3 v2, Vector3 point, out float distance)
        {
            var d = (v2 - v1).Normalized();
            var v = point - v1;
            var t = Dot(v, d);
            var P = v1 + (t * d);
            distance = Distance(P, point);
            return (point - P).Normalized();
        }

        public static float GetPerpendicularDistance(Vector3 v1, Vector3 v2, Vector3 point)
        {
            var d = (v2 - v1).Normalized();
            var v = point - v1;
            var t = Dot(v, d);
            var P = v1 + (t * d);
            return Distance(P, point);
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

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X < v2.X ? v1.X : v2.X, v1.Y < v2.Y ? v1.Y : v2.Y, v1.Z < v2.Z ? v1.Z : v2.Z);
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X > v2.X ? v1.X : v2.X, v1.Y > v2.Y ? v1.Y : v2.Y, v1.Z > v2.Z ? v1.Z : v2.Z);
        }

        public static Vector3 Avg(Vector3 v1, Vector3 v2)
        {
            return (v1 + v2) / 2f;
        }

        #endregion

        public override string ToString()
        {
            return $"[{X}; {Y}; {Z}]";
        }
    }
}
