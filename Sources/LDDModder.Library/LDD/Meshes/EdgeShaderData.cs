using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class EdgeShaderData
    {
        public int FileOffset { get; set; }

        public float[] Values { get; set; }

        public bool IsEndOfRow => Values.Length > 12;

        public EdgeShaderData()
        {
        }

        public EdgeShaderData(float[] values)
        {
            Values = values;
        }

        public EdgeShaderData(params Vector3[] values)
        {
            Values = new float[values.Length * 3];

            for (int i = 0; i < values.Length; i++)
            {
                Values[(i * 3) + 0] = values[i].X;
                Values[(i * 3) + 1] = values[i].Y;
                Values[(i * 3) + 2] = values[i].Z;
            }
        }

        public EdgeShaderData(int offset, float[] values)
        {
            FileOffset = offset;
            Values = values;
        }
    }
}
