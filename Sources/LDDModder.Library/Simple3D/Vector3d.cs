using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Vector3d : IEquatable<Vector3d>, IEqualityComparer<Vector3d>
    {

        #region Constants

        public static readonly Vector3d Zero = new Vector3d();

        public static readonly Vector3d UnitX = new Vector3d(1, 0, 0);

        public static readonly Vector3d UnitY = new Vector3d(0, 1, 0);

        public static readonly Vector3d UnitZ = new Vector3d(0, 0, 1);

        public static readonly Vector3d Empty = new Vector3d(double.NaN, double.NaN, double.NaN);

        #endregion

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double this[int i]
        {
            get
            {
                switch (i)
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

        public bool IsEmpty => double.IsNaN(X);

        public double Length
        {
            get
            {
                if (IsEmpty)
                    return 0;
                var a = Math.Sqrt((X * X) + (Y * Y));
                return (double)Math.Sqrt((a * a) + (Z * Z));
            }
        }

        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3d(double value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3d(double[] values)
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
            if (!(obj is Vector3d))
                return false;
            return Equals((Vector3d)obj);
        }

        public bool Equals(Vector3d other)
        {
            return Equals(other, 0.00001d);
        }

        public bool Equals(Vector3d other, double tolerence)
        {
            if (IsEmpty || other.IsEmpty)
                return IsEmpty == other.IsEmpty;
            return Distance(this, other) <= tolerence;
            //return Math.Abs(X - other.X) < tolerence && Math.Abs(Y - other.Y) < tolerence && Math.Abs(Z - other.Z) < tolerence;
        }

        public bool Equals(Vector3d x, Vector3d y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public int GetHashCode(Vector3d obj)
        {
            return obj.GetHashCode();
        }

        public static bool operator ==(Vector3d left, Vector3d right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3d left, Vector3d right)
        {
            return !(left == right);
        }

        #endregion

        #region Operators

        public static Vector3d operator *(Vector3d vec, double number)
        {
            return new Vector3d(vec.X * number, vec.Y * number, vec.Z * number);
        }

        public static Vector3d operator *(double number, Vector3d vec)
        {
            return new Vector3d(vec.X * number, vec.Y * number, vec.Z * number);
        }

        public static Vector3d operator /(Vector3d vec, double number)
        {
            return new Vector3d(vec.X / number, vec.Y / number, vec.Z / number);
        }

        public static Vector3d operator +(Vector3d vec1, Vector3d vec2)
        {
            return new Vector3d(vec1.X + vec2.X, vec1.Y + vec2.Y, vec1.Z + vec2.Z);
        }

        public static Vector3d operator -(Vector3d vec1, Vector3d vec2)
        {
            return new Vector3d(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z);
        }

        public static Vector3d operator *(Matrix3 matrix, Vector3d vector)
        {
            Vector3d result = default;
            result.X = vector.X * matrix.A1 + vector.Y * matrix.A2 + vector.Z * matrix.A3;
            result.Y = vector.X * matrix.B1 + vector.Y * matrix.B2 + vector.Z * matrix.B3;
            result.Z = vector.X * matrix.C1 + vector.Y * matrix.C2 + vector.Z * matrix.C3;
            return result;
        }

        public static Vector3d operator *(Matrix4 matrix, Vector3d vector)
        {
            Vector3d result = default;
            result.X = vector.X * matrix.A1 + vector.Y * matrix.A2 + vector.Z * matrix.A3 + matrix.A4;
            result.Y = vector.X * matrix.B1 + vector.Y * matrix.B2 + vector.Z * matrix.B3 + matrix.B4;
            result.Z = vector.X * matrix.C1 + vector.Y * matrix.C2 + vector.Z * matrix.C3 + matrix.C4;
            return result;
        }

        public static explicit operator Vector3d(Vector3 vector)
        {
            return new Vector3d(vector.X, vector.Y, vector.Z);
        }

        #endregion

        #region Functions

        public void Normalize()
        {
            if (!IsEmpty && Length > double.Epsilon)
            {
                var len = Length;
                X /= len;
                Y /= len;
                Z /= len;
            }
        }

        public Vector3d Normalized()
        {
            if (IsEmpty || Length <= double.Epsilon)
                return this;
            return this / Length;
        }

        public Vector3d Rounded(int decimals = 4)
        {
            if (IsEmpty)
                return this;

            return new Vector3d(
                (double)Math.Round(X, decimals),
                (double)Math.Round(Y, decimals),
                (double)Math.Round(Z, decimals));
        }

        public static double Dot(Vector3d left, Vector3d right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        public static Vector3d Cross(Vector3d left, Vector3d right)
        {
            return new Vector3d
            {
                X = (left.Y * right.Z) - (left.Z * right.Y),
                Y = (left.Z * right.X) - (left.X * right.Z),
                Z = (left.X * right.Y) - (left.Y * right.X)
            };
        }

        public static double Distance(Vector3d left, Vector3d right)
        {
            var dx = left.X - right.X;
            var dy = left.Y - right.Y;
            var dz = left.Z - right.Z;
            return (double)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }

        public static double AngleBetween(Vector3d first, Vector3d second)
        {
            double dot = Dot(first, second);
            double result = Math.Acos(MathHelper.Clamp(dot / (first.Length * second.Length), -1.0d, 1.0d));
            if (double.IsNaN(result) || Math.Abs(result) <= double.Epsilon)
            {
                if (first.Equals(second))
                    return 0d;
            }
            return result;
        }

        public static Vector3d CalculateNormal(Vector3d v1, Vector3d v2, Vector3d v3)
        {
            var u = v2 - v1;
            var v = v3 - v1;
            var n = new Vector3d((u.Y * v.Z) - (u.Z * v.Y), (u.Z * v.X) - (u.X * v.Z), (u.X * v.Y) - (u.Y * v.X));
            return n.Normalized();
        }

        public static Vector3d ProjectToPlane(Vector3d point, Vector3d planeOrigin, Vector3d planeNormal)
        {
            var v = point - planeOrigin;
            var dist = (v.X * planeNormal.X) + (v.Y * planeNormal.Y) + (v.Z * planeNormal.Z);
            return point - (planeNormal * dist);
        }

        //public static Vector2 ProjectToPlane2D(Vector3d point, Vector3d planeOrigin, Vector3d planeNormal)
        //{
        //    var v = point - planeOrigin;
        //    var dist = Dot(v, planeNormal);

        //    var projected = point - (planeNormal * dist);

        //    if (projected.X == planeOrigin.X)
        //        return new Vector2(projected.Y, projected.Z);
        //    else if (projected.Y == planeOrigin.Y)
        //        return new Vector2(projected.X, projected.Z);

        //    return new Vector2(projected.X, projected.Y);
        //}

        //public static Vector2 ProjectToPlane2D(Vector3d point, Vector3d planeOrigin, Vector3d planeNormal, Vector3d xAxis, Vector3d yAxis)
        //{
        //    var v = point - planeOrigin;

        //    var t1 = Dot(xAxis, v);
        //    var t2 = Dot(yAxis, v);

        //    return new Vector2(t1, t2);
        //}

        //public static Vector2 GetPlanarDistance(Vector3d v1, Vector3d v2, Vector3d pt, Vector3d planeNormal)
        //{
        //    var hAxis = (v2 - v1).Normalized();
        //    var vAxis = Cross(planeNormal, hAxis);
        //    var v = pt - v1;
        //    var t1 = Dot(hAxis, v);
        //    var t2 = Dot(vAxis, v);

        //    return new Vector2(t1, t2);
        //}

        //public static Vector2 GetPlanarDistance(Vector3d v1, Vector3d v2, Vector3d pt)
        //{
        //    var planeNormal = CalculateNormal(v1, v2, pt);
        //    var hAxis = (v2 - v1).Normalized();
        //    var vAxis = Cross(planeNormal, hAxis);
        //    var v = pt - v1;
        //    var t1 = Dot(hAxis, v);
        //    var t2 = Dot(vAxis, v);

        //    return new Vector2(t1, t2);
        //}

        public static Vector3d GetPerpendicular(Vector3d v1, Vector3d v2, Vector3d point)
        {
            var dir1 = (v2 - v1).Normalized();
            var dir2 = (point - v1).Normalized();
            var c = Cross(dir1, dir2).Normalized();
            var perp = Cross(c, dir1).Normalized();
            var d1 = Distance(point, v1 + perp);
            var d2 = Distance(point, v1 - perp);
            if (d2 < d1)
                perp *= -1;
            return perp;
        }

        public static Vector3d GetPerpIntersection(Vector3d v1, Vector3d v2, Vector3d point)
        {
            var line1 = (v2 - v1).Normalized();
            var line2 = (point - v1);
            var a = AngleBetween(line1, line2.Normalized());
            var d = Math.Cos(a) * line2.Length;
            //var perp = GetPerpendicular(v1, v2, point);
            return v1 + (line1 * d);
        }

        public static Vector3d Min(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.X < v2.X ? v1.X : v2.X, v1.Y < v2.Y ? v1.Y : v2.Y, v1.Z < v2.Z ? v1.Z : v2.Z);
        }

        public static Vector3d Max(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.X > v2.X ? v1.X : v2.X, v1.Y > v2.Y ? v1.Y : v2.Y, v1.Z > v2.Z ? v1.Z : v2.Z);
        }

        public static Vector3d Avg(Vector3d v1, Vector3d v2)
        {
            return (v1 + v2) / 2d;
        }

        #endregion

        public override string ToString()
        {
            return $"[{X}; {Y}; {Z}]";
        }
    }
}
