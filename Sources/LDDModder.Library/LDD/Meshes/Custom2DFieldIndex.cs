using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Custom2DFieldIndex
    {
        public int Index { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public int Value4 { get; set; }

        public Custom2DFieldIndex() { }

        public Custom2DFieldIndex(int[] values)
        {
            Index = values[0];
            Value2 = values[1];
            Value3 = values[2];
            Value4 = values[3];
        }

        public Custom2DFieldIndex(LDD.Files.MeshStructures.STUD_2DFIELD_IDX studRef)
        {
            Index = studRef.ArrayIndex;
            Value2 = studRef.Value2;
            Value3 = studRef.Value3;
            Value4 = studRef.Value4;
        }

        public LDD.Files.MeshStructures.STUD_2DFIELD_IDX Serialize()
        {
            return new Files.MeshStructures.STUD_2DFIELD_IDX(Index, Value2, Value3, Value4);
        }

        public override string ToString()
        {
            return $"{Index}; {Value2}; {Value3}; {Value4}";
        }
    }
}
