using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct CUSTOM2DFIELD_INDEX
    {
        public int ArrayIndex;
        public int Value2;
        public int Value3;
        public int Value4;

        public CUSTOM2DFIELD_INDEX(int arrayIndex, int value2, int value3, int value4)
        {
            ArrayIndex = arrayIndex;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }

        public CUSTOM2DFIELD_INDEX(int[] values)
        {
            ArrayIndex = values[0];
            Value2 = values[1];
            Value3 = values[2];
            Value4 = values[3];
        }
    }
}
