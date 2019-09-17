using LDDModder.Simple3D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class RoundEdgeData : IEquatable<RoundEdgeData>, IEqualityComparer<RoundEdgeData>
    {
        public const float COORD_SCALE = 15.5f / 0.8f;

        public enum EdgeCombineMode
        {
            Union,
            Intersection
        }

        public struct CoordPair
        {
            public Vector2 Left;
            public Vector2 Right;

            public EdgeCombineMode Operation
            {
                get => Right.X < 0 ? EdgeCombineMode.Union : EdgeCombineMode.Intersection;
                set => Right.X = MathHelper.SetSign(Right.X, value == EdgeCombineMode.Union ? -1 : 1);
            }

            public CoordPair(Vector2 left, Vector2 right)
            {
                Left = left;
                Right = right;
            }
        }

        public class PairWrapper : IEnumerable<CoordPair>
        {
            private readonly RoundEdgeData owner;

            public CoordPair this[int i]
            {
                get => new CoordPair(owner.Coords[i * 2], owner.Coords[(i * 2) + 1]);
                set
                {
                    owner.Coords[i * 2] = value.Left;
                    owner.Coords[(i * 2) + 1] = value.Right;
                }
            }

            protected internal PairWrapper(RoundEdgeData owner)
            {
                this.owner = owner;
            }

            public IEnumerator<CoordPair> GetEnumerator()
            {
                var list = new List<CoordPair>();
                for (int i = 0; i < owner.Coords.Length / 2; i++)
                    list.Add(this[i]);
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }


        public Vector2[] Coords { get; set; }

        public PairWrapper Pairs { get; private set; }

        public static readonly Vector2 EmptyCoord = new Vector2(1000, 1000);

        public static RoundEdgeData NoOutline => new RoundEdgeData(EmptyCoord, EmptyCoord, EmptyCoord, EmptyCoord, EmptyCoord, EmptyCoord);

        public RoundEdgeData()
        {
            Coords = new Vector2[6];
            Pairs = new PairWrapper(this);
        }

        public RoundEdgeData(float[] values)
        {
            Coords = new Vector2[values.Length / 2];
            for (int i = 0; i < Coords.Length; i++)
                Coords[i] = new Vector2(values[(i * 2) + 0], values[(i * 2) + 1]);
            Pairs = new PairWrapper(this);
        }

        public RoundEdgeData(params Vector2[] values)
        {
            Coords = values;
            Pairs = new PairWrapper(this);
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
            for (int i = 0; i < 6; i++)
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

        public void Set(int pairIndex, Vector2 coords, bool adjust = true)
        {
            Set(pairIndex, EmptyCoord, coords, EdgeCombineMode.Union, adjust);
        }

        public void Set(int pairIndex, Vector2 coord1, Vector2 coord2, EdgeCombineMode mode, bool adjust = true)
        {
            if (coord1.IsEmpty)
                coord1 = EmptyCoord;

            if (coord2.IsEmpty)
                coord2 = EmptyCoord;

            if (coord1 != EmptyCoord && adjust)
            {
                coord1 *= COORD_SCALE;
                coord1.X += 100;
            }

            if (coord2 != EmptyCoord && adjust)
            {
                coord2 *= COORD_SCALE;
                coord2.X += 100;
                if (mode == EdgeCombineMode.Union)
                    coord2.X *= -1;
            }

            Coords[(pairIndex * 2)] = coord1;
            Coords[(pairIndex * 2) + 1] = coord2;
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

        public RoundEdgeData Clone()
        {
            return new RoundEdgeData(Coords);
        }
    }
}
