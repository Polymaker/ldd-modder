using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class RoundEdgeData : IEquatable<RoundEdgeData>
    {
        public int FileOffset { get; set; }

        //public float[] Values { get; set; }

        public Vector2[] Coords { get; set; }

        public bool IsEndOfRow => Coords.Length > 6;

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

        [Obsolete]
        public RoundEdgeData(params Vector3[] values)
        {
            float[] vals = new float[values.Length * 3];
            for (int i = 0; i < values.Length; i++)
            {
                vals[(i * 3) + 0] = values[i].X;
                vals[(i * 3) + 1] = values[i].Y;
                vals[(i * 3) + 2] = values[i].Z;
            }

            Coords = new Vector2[vals.Length / 2];
            for (int i = 0; i < Coords.Length; i++)
                Coords[i] = new Vector2(vals[(i * 2) + 0], vals[(i * 2) + 1]);
        }

        public RoundEdgeData(params Vector2[] values)
        {
            Coords = values;
        }

        public RoundEdgeData(int offset, float[] values)
        {
            FileOffset = offset;
            Coords = new Vector2[values.Length / 2];
            for (int i = 0; i < Coords.Length; i++)
                Coords[i] = new Vector2(values[(i * 2) + 0], values[(i * 2) + 1]);
        }

        public bool Equals(RoundEdgeData other)
        {
            for (int i = 0; i < Math.Min(Coords.Length, other.Coords.Length); i++)
            {
                if (!Coords[i].Equals(other.Coords[i]))
                    return false;
            }
            return true;
        }

        public void PackData()
        {
            var coords = new List<Vector2>(Coords);
            coords.Add(new Vector2(0, 0));
            coords.Add(new Vector2(0, 0));
            Coords = coords.ToArray();
        }

        public override string ToString()
        {
            return string.Join(", ", Coords);
        }
    }
}
