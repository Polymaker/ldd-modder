using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct ROUNDEDGE_SHADER_DATA
    {
        public Vector2[] Coords;

        public ROUNDEDGE_SHADER_DATA(float[] values)
        {
            Coords = new Vector2[values.Length / 2];

            for (int i = 0; i < Coords.Length; i++)
                Coords[i] = new Vector2(values[i * 2], values[(i * 2) + 1]);
        }

        public ROUNDEDGE_SHADER_DATA(Vector2[] coords)
        {
            Coords = coords;
        }
    }
}
