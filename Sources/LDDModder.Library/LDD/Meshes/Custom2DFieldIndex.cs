using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Custom2DFieldIndex
    {
        /// <summary>
        /// Index (1D) of the referenced element in the Custom2DField array
        /// </summary>
        public int Index { get; set; }
        public int Value2 { get; set; }
        /// <summary>
        /// Always Zero
        /// </summary>
        public int Dummy { get; set; }
        public int Value4 { get; set; }

        public Custom2DFieldIndex() { }

        public Custom2DFieldIndex(int[] values)
        {
            Index = values[0];
            Value2 = values[1];
            Dummy = values[2];
            Value4 = values[3];
        }

        public Custom2DFieldIndex(LDD.Files.MeshStructures.STUD_2DFIELD_IDX studRef)
        {
            Index = studRef.ArrayIndex;
            Value2 = studRef.Value2;
            Dummy = studRef.Value3;
            Value4 = studRef.Value4;
        }

        public LDD.Files.MeshStructures.STUD_2DFIELD_IDX Serialize()
        {
            return new Files.MeshStructures.STUD_2DFIELD_IDX(Index, Value2, Dummy, Value4);
        }

        public override string ToString()
        {
            return $"{Index}; {Value2}; {Dummy}; {Value4}";
        }
    }
}
