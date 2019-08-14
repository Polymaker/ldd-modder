using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class RoundEdgeData : IEquatable<RoundEdgeData>, IEqualityComparer<RoundEdgeData>
    {
        //public int FileOffset { get; set; }

        //public float[] Values { get; set; }

        public Vector2[] Coords { get; set; }

        public static readonly Vector2 EmptyCoord = new Vector2(1000, 1000);

        public static readonly RoundEdgeData NoOutline = new RoundEdgeData(EmptyCoord, EmptyCoord, EmptyCoord, EmptyCoord, EmptyCoord, EmptyCoord);

        //public bool IsEndOfRow => Coords.Length > 6;

        public RoundEdgeData()
        {
            Coords = new Vector2[6];
        }

        public RoundEdgeData(float[] values)
        {
            Coords = new Vector2[values.Length / 2];
            for (int i = 0; i < Coords.Length; i++)
                Coords[i] = new Vector2(values[(i * 2) + 0], values[(i * 2) + 1]);
        }

        public RoundEdgeData(params Vector2[] values)
        {
            Coords = values;
        }


        //public RoundEdgeData(int offset, float[] values)
        //{
        //    FileOffset = offset;
        //    Coords = new Vector2[values.Length / 2];
        //    for (int i = 0; i < Coords.Length; i++)
        //        Coords[i] = new Vector2(values[(i * 2) + 0], values[(i * 2) + 1]);
        //}

        public bool Equals(RoundEdgeData other)
        {
            for (int i = 0; i < Math.Min(Coords.Length, other.Coords.Length); i++)
            {
                if (!Coords[i].Equals(other.Coords[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoundEdgeData))
                return false;
            return Equals((RoundEdgeData)obj);
        }

        public void PackData()
        {
            if (Coords.Length == 6)
            {
                var coords = new List<Vector2>(Coords);
                coords.Add(new Vector2(0, 0));
                coords.Add(new Vector2(0, 0));
                Coords = coords.ToArray();
            }
        }

        public override string ToString()
        {
            return string.Join(", ", Coords);
        }

        public override int GetHashCode()
        {
            var hashCode = 550714527;
            hashCode = hashCode * -1521134295 + Coords[0].GetHashCode();
            hashCode = hashCode * -1521134295 + Coords[1].GetHashCode();
            hashCode = hashCode * -1521134295 + Coords[2].GetHashCode();
            return hashCode;
        }

        public bool Equals(RoundEdgeData x, RoundEdgeData y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(RoundEdgeData obj)
        {
            return obj.GetHashCode();
        }
    }
}
