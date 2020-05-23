using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct STUD_2DFIELD_REF
    {
        public int ConnectorIndex;
        public STUD_2DFIELD_IDX[] Indices;

        public STUD_2DFIELD_REF(int connectorIndex, int indexCount)
        {
            ConnectorIndex = connectorIndex;
            Indices = new STUD_2DFIELD_IDX[indexCount];
        }

        public STUD_2DFIELD_REF(int connectorIndex, STUD_2DFIELD_IDX[] indices)
        {
            ConnectorIndex = connectorIndex;
            Indices = indices;
        }
    }
}
