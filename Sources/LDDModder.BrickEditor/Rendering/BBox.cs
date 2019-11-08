using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public struct BBox
    {
        public Vector3 Min;

        public Vector3 Max;

        public Vector3 Center => ((Max - Min) / 2f) + Min;

        public float SizeX => Max.X - Min.X;
        public float SizeY => Max.Y - Min.Y;
        public float SizeZ => Max.Z - Min.Z;

        public Vector3 Extent => Max - Min;

        public BBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public static BBox Calculate(IEnumerable<Vector3> vertices)
        {
            Vector3 minPos = new Vector3(99999f);
            Vector3 maxPos = new Vector3(-99999f);

            foreach (var v in vertices)
            {
                minPos = Vector3.ComponentMin(minPos, v);
                maxPos = Vector3.ComponentMax(maxPos, v);
            }

            return new BBox(minPos, maxPos);
        }
    }
}
