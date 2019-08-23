using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Simple3D
{
    public struct Vector4
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

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector4))
                return false;
            return Equals((Vector4)obj);
        }

        public bool Equals(Vector4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        public override string ToString()
        {
            return $"[{X};{Y};{Z};{W}]";
        }

        public static readonly Vector4 Zero = new Vector4();

        public static readonly Vector4 Empty = new Vector4(float.NaN, float.NaN, float.NaN, float.NaN);
    }
}
