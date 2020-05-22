using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Meshes
{
    public class MeshExportOptions
    {
        public bool IndividualComponents { get; set; }
        public bool IncludeCollisions { get; set; }
        public bool IncludeConnections { get; set; }
        public bool IncludeBones { get; set; }
        public bool IncludeAltMeshes { get; set; }
        public bool IncludeRoundEdgeData { get; set; }
    }
}
