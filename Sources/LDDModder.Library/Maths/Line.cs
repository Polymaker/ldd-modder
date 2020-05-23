using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Maths
{
    public struct Line
    {
        #region Fields

        private bool _IsVertical;
        private float _A;
        private float _B;
        private float _X;

        #endregion

        #region Properties

        public float A { get { return _A; } }

        public float B { get { return _B; } }

        public float X { get { return _X; } }

        public bool IsVertical { get { return _IsVertical; } }

        public bool IsHorizontal { get { return !IsVertical && A.EqualOrClose(0); } }

        public Vector2 Vector
        {
            get
            {
                if (IsVertical)
                    return new Vector2(0, 1);
                return new Vector2(1, A).Normalized();
            }
        }

        #endregion

        #region Ctors

        public Line(float x)
        {
            _IsVertical = true;
            _X = x;
            _A = 0;
            _B = 0;
        }

        public Line(float a, float b)
        {
            _IsVertical = false;
            _X = 0;
            _A = a;
            _B = b;
        }

        #endregion

        #region Static Ctors

        public static Line FromPoints(Vector2 p1, Vector2 p2)
        {
            var left = p1.X < p2.X ? p1 : p2;
            var right = p1.X < p2.X ? p2 : p1;
            var dx = right.X - left.X;
            var dy = right.Y - left.Y;

            if (dx.EqualOrClose(0, 0.00001f))
                return new Line(p1.X);//vertical line

            var slope = dy / dx;

            if (float.IsInfinity(slope))
                return new Line(p1.X);//vertical line

            var b = left.Y + ((left.X * -1) * slope);

            if (slope.EqualOrClose(0, 0.00001f))
                slope = 0;//horizontal line

            return new Line(slope, b);
        }

        #endregion

        #region Equality operators

        public static bool operator ==(Line left, Line right)
        {
            if (left.IsVertical != right.IsVertical)
                return false;
            if (left.IsVertical)
                return left.X.EqualOrClose(right.X, 0.001f);

            return left.A.EqualOrClose(right.A, 0.0001f) && left.B.EqualOrClose(right.B, 0.0001f);
        }

        public static bool operator !=(Line left, Line right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Line))
                return false;

            return this == (Line)obj;
        }

        public override int GetHashCode()
        {
            var hashCode = 1178949888;
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Functions

        public Vector2 GetPointForX(float x)
        {
            if (IsVertical)
                return Vector2.Empty;
            return new Vector2(x, B + (x * A));
        }

        public Vector2 GetPointForY(float y)
        {
            if (IsVertical)
                return new Vector2(X, y);
            else if (IsHorizontal)
                return Vector2.Empty;
            return new Vector2((y - B) / A, y);
        }

        public static bool AreParallel(Line l1, Line l2)
        {
            if (l1.IsVertical || l2.IsVertical)
                return l1.IsVertical && l2.IsVertical;

            return l1.A.EqualOrClose(l2.A);
        }

        public static bool ArePerpendicular(Line l1, Line l2)
        {
            if ((l1.IsHorizontal && l2.IsVertical) || (l2.IsHorizontal && l1.IsVertical))
                return true;
            return l2.A.EqualOrClose((1f / l1.A) * -1f) || l1.A.EqualOrClose((1f / l2.A) * -1f);
        }

        public static Line GetPerpendicular(Line line, Vector2 pt)
        {
            if (line.IsVertical)
                return new Line(0, pt.Y);
            if (line.IsHorizontal)
                return new Line(pt.X);
            var newSlope = (1 / line.A) * -1;
            var b = pt.Y - (pt.X * newSlope);
            return new Line(newSlope, b);
        }

        public Line GetPerpendicular(Vector2 pt)
        {
            return GetPerpendicular(this, pt);
        }

        public static bool LinesIntersect(Line l1, Line l2, out Vector2 intersection)
        {
            intersection = Vector2.Empty;
            if (AreParallel(l1, l2))
                return false;
            if (l1.IsVertical)
            {
                intersection = l2.GetPointForX(l1.X);
                return true;
            }
            else if (l2.IsVertical)
            {
                intersection = l1.GetPointForX(l2.X);
                return true;
            }
            intersection = new Vector2((l2.B - l1.B) / (l1.A - l2.A), l1.A * (l2.B - l1.B) / (l1.A - l2.A) + l1.B);
            return true;
        }

        public bool Intersect(Line line, out Vector2 intersection)
        {
            return LinesIntersect(this, line, out intersection);
        }

        public bool Intersect(Line line)
        {
            Vector2 dummy;
            return LinesIntersect(this, line, out dummy);
        }

        public Vector2 GetIntersection(Line line)
        {
            Vector2 intersection;
            Intersect(line, out intersection);
            return intersection;
        }

        public Vector2 GetClosestPointOnLine(Vector2 point)
        {
            var perp = GetPerpendicular(this, point);
            Vector2 inter;
            if (Intersect(perp, out inter))
                return inter;
            return Vector2.Empty;
        }

        #endregion
    }
}
