using System;
using System.Collections.Generic;

namespace LDDModder.Simple3D
{
    public struct Vector2 : IEquatable<Vector2>, IEqualityComparer<Vector2>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public bool IsEmpty => float.IsNaN(X);

        public float Length
        {
            get
            {
                if (IsEmpty)
                    return 0;
                return (float)Math.Sqrt((X * X) + (Y * Y));
            }
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float[] values)
        {
            if (values.Length != 2)
                throw new ArgumentException("The value array must have a length of 2");
            X = values[0];
            Y = values[1];
        }

        public static explicit operator Vector2(Assimp.Vector2D vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        #region Equality operators

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;
            return Equals((Vector2)obj);
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !(left == right);
        }

        public bool Equals(Vector2 other)
        {
            return Equals(other, 0.00001f);
        }

        public bool Equals(Vector2 other, float tolerence)
        {
            if (IsEmpty || other.IsEmpty)
                return IsEmpty == other.IsEmpty;
            return Math.Abs(X - other.X) < tolerence && Math.Abs(Y - other.Y) < tolerence;
        }

        public bool Equals(Vector2 x, Vector2 y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Vector2 obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        //public override int GetHashCode()
        //{
        //    var hashCode = 1861411795;
        //    hashCode = hashCode * -1521134295 + X.GetHashCode();
        //    hashCode = hashCode * -1521134295 + Y.GetHashCode();
        //    return hashCode;
        //}

        #endregion

        #region Arithmetic operators

        public static Vector2 operator *(Vector2 vec, float number)
        {
            return new Vector2(vec.X * number, vec.Y * number);
        }

        public static Vector2 operator /(Vector2 vec, float number)
        {
            return new Vector2(vec.X / number, vec.Y / number);
        }

        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        #endregion

        #region Methods & functions

        public void Normalize()
        {
            if (IsEmpty)
            {
                var len = Length;
                X /= len;
                Y /= len;
            }
        }

        public Vector2 Normalized()
        {
            if (!IsEmpty)
                return this / Length;
            return Vector2.Empty;
        }

        public static Vector2 Min(params Vector2[] vectors)
        {
            var result = new Vector2(float.MaxValue,float.MaxValue);

            for (int i = 0; i < vectors.Length; i++)
            {
                result.X = (float)Math.Min(result.X, vectors[i].X);
                result.Y = (float)Math.Min(result.Y, vectors[i].Y);
            }

            return result;
        }

        public static Vector2 Max(params Vector2[] vectors)
        {
            var result = Vector2.Zero;

            for (int i = 0; i < vectors.Length; i++)
            {
                result.X = (float)Math.Max(result.X, vectors[i].X);
                result.Y = (float)Math.Max(result.Y, vectors[i].Y);
            }

            return result;
        }

        public static float Dot(Vector2 left, Vector2 right)
        {
            return (left.X * right.X) + (left.Y * right.Y);
        }

        public Vector2 GetPerpendicular(Vector2 vec)
        {
            return new Vector2(vec.Y, -vec.X);
        }

        public float AngleBetween(Vector2 first, Vector2 second)
        {
            var dot = Dot(first, second);
            return (float)Math.Acos(MathHelper.Clamp(dot / (first.Length * second.Length), -1.0f, 1.0f));
        }

        public Vector2 Rounded(int decimals = 4)
        {
            if (IsEmpty)
                return this;
            return new Vector2(
                (float)Math.Round(X, decimals),
                (float)Math.Round(Y, decimals));
        }

        #endregion

        public override string ToString()
        {
            return $"[{X}; {Y}]";
        }

        #region Constants

        public static readonly Vector2 Zero = new Vector2();

        public static readonly Vector2 Empty = new Vector2(float.NaN, float.NaN);

        #endregion

    }
}
