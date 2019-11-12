using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public struct Plane
    {
        public Vector3 Origin { get; set; }
        public Vector3 Normal { get; set; }
        public float Distance { get; set; }
    }
}
