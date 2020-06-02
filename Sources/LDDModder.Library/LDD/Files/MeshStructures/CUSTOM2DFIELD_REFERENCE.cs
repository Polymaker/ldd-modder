using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    public struct CUSTOM2DFIELD_REFERENCE
    {
        public int ConnectorIndex;
        public CUSTOM2DFIELD_INDEX[] Indices;

        public CUSTOM2DFIELD_REFERENCE(int connectorIndex, int indexCount)
        {
            ConnectorIndex = connectorIndex;
            Indices = new CUSTOM2DFIELD_INDEX[indexCount];
        }

        public CUSTOM2DFIELD_REFERENCE(int connectorIndex, CUSTOM2DFIELD_INDEX[] indices)
        {
            ConnectorIndex = connectorIndex;
            Indices = indices;
        }
    }
}
