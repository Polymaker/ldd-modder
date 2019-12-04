using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Vector4d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vector3d Xyz
        {
            get => new Vector3d(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4d(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4d(Vector3d vector, double w)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            W = w;
        }

        #region Operators

        public static Vector4d operator *(Vector4d vec, double number)
        {
            return new Vector4d(vec.X * number, vec.Y * number, vec.Z * number, vec.W * number);
        }

        public static Vector4d operator /(Vector4d vec, double number)
        {
            return new Vector4d(vec.X / number, vec.Y / number, vec.Z / number, vec.W / number);
        }

        public static Vector4d operator +(Vector4d left, Vector4d right)
        {
            return new Vector4d(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        public static Vector4d operator -(Vector4d vec1, Vector4d vec2)
        {
            return new Vector4d(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z, vec1.W - vec2.W);
        }

        public static explicit operator Vector4d(Vector4 vector)
        {
            return new Vector4d(vector.X, vector.Y, vector.Z, vector.W);
        }

        #endregion

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector4d))
                return false;
            return Equals((Vector4d)obj);
        }

        public bool Equals(Vector4d other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        public override string ToString()
        {
            return $"[{X};{Y};{Z};{W}]";
        }

        public static readonly Vector4d Zero = new Vector4d();

        public static readonly Vector4d Empty = new Vector4d(double.NaN, double.NaN, double.NaN, double.NaN);
    }
}
