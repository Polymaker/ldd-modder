using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public struct BSphere
    {
        public Vector3 Center;
        public float Radius;

        public float Diameter
        {
            get { return Radius * 2f; }
            set
            {
                Radius = value * 0.5f;
            }
        }

        public BSphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}
