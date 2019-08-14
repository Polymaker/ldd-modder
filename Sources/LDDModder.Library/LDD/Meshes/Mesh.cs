using LDDModder.LDD.Files.MeshStructures;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Mesh
    {
        public MESH_FILE? OriginalData { get; }

        public MeshType Type { get; }

        public MeshGeometry Geometry { get; private set; }

        public List<MeshCulling> Cullings { get; }

        internal Mesh(MESH_FILE originalData, MeshType type)
        {
            OriginalData = originalData;
            Type = type;
            Cullings = new List<MeshCulling>();
        }

        public Mesh(MeshType type)
        {
            Type = type;
            Cullings = new List<MeshCulling>();
        }

        public Mesh(MeshGeometry geometry)
        {
            bool isTextured = geometry.Vertices.Any(x => !x.TexCoord.IsEmpty);
            bool isFlexible = geometry.Vertices.Any(x => x.BoneWeights.Any());

            if (isFlexible)
                Type = isTextured ? MeshType.FlexibleTextured : MeshType.Flexible;
            else
                Type = isTextured ? MeshType.StandardTextured : MeshType.Standard;

            Geometry = geometry;
            Cullings = new List<MeshCulling>();
        }

        public void SetGeometry(MeshGeometry geometry)
        {
            Geometry = geometry;
        }

        public IEnumerable<Vector3> GetAverageNormals()
        {
            var extra = Cullings.Where(c => c.ReplacementMesh != null).SelectMany(c => c.ReplacementMesh.Indices.Select(i => i.AverageNormal));
            return Geometry.Indices.Select(x => x.AverageNormal).Concat(extra);
        }

        public IEnumerable<RoundEdgeData> GetRoundEdgeShaderData()
        {
            var extra = Cullings.Where(c => c.ReplacementMesh != null).SelectMany(c => c.ReplacementMesh.Indices.Select(i => i.RoundEdgeData));
            var test = Geometry.Indices.Select(x => x.RoundEdgeData).ToList();
            return Geometry.Indices.Select(x => x.RoundEdgeData).Concat(extra);
        }
    }
}
