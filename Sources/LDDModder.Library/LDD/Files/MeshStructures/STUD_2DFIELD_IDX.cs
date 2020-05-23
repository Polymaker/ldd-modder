using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct STUD_2DFIELD_IDX
    {
        public int ArrayIndex;
        public int Value2;
        public int Value3;
        public int Value4;

        public STUD_2DFIELD_IDX(int arrayIndex, int value2, int value3, int value4)
        {
            ArrayIndex = arrayIndex;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }

        public STUD_2DFIELD_IDX(int[] values)
        {
            ArrayIndex = values[0];
            Value2 = values[1];
            Value3 = values[2];
            Value4 = values[3];
        }
    }
}
