using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct MESH_STUD
    {
        public int ConnectorIndex;
        public int Value2;
        public int DataArrayIndex;
        public int Value4;
        public int Value5;
        public int Value6;

        public MESH_STUD(int connectorIndex, int value2, int value3, int value4, int value5, int value6)
        {
            ConnectorIndex = connectorIndex;
            Value2 = value2;
            DataArrayIndex = value3;
            Value4 = value4;
            Value5 = value5;
            Value6 = value6;
        }

        public MESH_STUD(int[] values)
        {
            ConnectorIndex = values[0];
            Value2 = values[1];
            DataArrayIndex = values[2];
            Value4 = values[3];
            Value5 = values[4];
            Value6 = values[5];
        }
    }
}
