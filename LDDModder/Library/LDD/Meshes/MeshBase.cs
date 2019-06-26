using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.Meshes
{
    public abstract class MeshBase
    {
        public bool IsFlexible { get; set; }

        public bool IsTextured => this is TexturedMesh;

        public MeshType Type => IsTextured ? (IsFlexible ? MeshType.FlexibleTextured : MeshType.StandardTextured) : (IsFlexible ? MeshType.Flexible : MeshType.Standard);

        public List<Vertex> Vertices { get; set; }

    }
}
