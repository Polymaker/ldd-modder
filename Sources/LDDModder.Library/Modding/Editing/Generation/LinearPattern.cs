using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class LinearPattern
    {
        public List<PartElement> Elements { get; set; }

        public Vector3 AxisX { get; set; }

        public int RepetitionsX { get; set; }

        public float DistanceX { get; set; }

        public Vector3 AxisY { get; set; }

        public struct PatternAxis
        {
            public Vector3 Axis;
            public int Repetitions;
            public float Distance;
        }
    }
}
