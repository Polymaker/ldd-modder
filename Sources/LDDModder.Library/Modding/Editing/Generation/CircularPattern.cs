using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public class CircularPattern
    {
        public List<PartElement> Elements { get; set; }

        public Vector3 Origin { get; set; }

        public Vector3 Axis { get; set; }

        public int Repetitions { get; set; }

        //public List<int> 

        public float Angle { get; set; }
    }
}
