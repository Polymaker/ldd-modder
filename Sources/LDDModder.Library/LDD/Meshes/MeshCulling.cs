using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class MeshCulling
    {
        public MeshCullingType Type { get; }
        /// <summary>
        /// Specifies the first vertex concerned.
        /// </summary>
        public int FromVertex { get; set; }

        /// <summary>
        /// Specifies the number of vertices concerned.
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// Specifies the first index concerned.
        /// </summary>
        public int FromIndex { get; set; }

        /// <summary>
        /// Specifies the number of indices concerned.
        /// </summary>
        public int IndexCount { get; set; }

        public List<Custom2DFieldReference> Studs { get; set; }

        /// <summary>
        /// Only present for MeshCullingType Tube.
        /// Never more than one item.
        /// </summary>
        public List<Custom2DFieldReference> AdjacentStuds { get; set; }

        public MeshGeometry ReplacementMesh { get; set; }

        public MeshCulling(MeshCullingType type)
        {
            Type = type;
            Studs = new List<Custom2DFieldReference>();
            AdjacentStuds = new List<Custom2DFieldReference>();
        }

        public override string ToString()
        {
            return $"{Type}";
        }
    }
}
