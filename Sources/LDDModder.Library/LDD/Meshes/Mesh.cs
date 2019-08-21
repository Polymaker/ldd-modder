using LDDModder.LDD.Files.MeshStructures;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Mesh
    {
        public MESH_FILE? OriginalData { get; }

        public MeshType Type { get; private set; }

        public MeshGeometry Geometry { get; private set; }

        public List<MeshCulling> Cullings { get; }

        public int IndexCount => Geometry.IndexCount;

        public IList<Vertex> Vertices => Geometry.Vertices;

        public int VertexCount => Geometry.VertexCount;

        public IList<Triangle> Triangles => Geometry.Triangles;

        public MeshIndexList Indices => Geometry.Indices;

        public int TriangleCount => Geometry.TriangleCount;

        public bool IsTextured => Geometry.IsTextured;

        public bool IsFlexible => Geometry.IsFlexible;

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

        public static Mesh Read(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return Files.GFileReader.ReadMesh(fs);
        }

        public MeshGeometry GetCullingGeometry(MeshCulling culling, bool linked = true)
        {
            //var geom = new MeshGeometry();
            var vertMatch = new Dictionary<int, int>();
            var builder = new GeometryBuilder();

            for (int i = 0; i < culling.VertexCount; i++)
            {
                var v = Vertices[i + culling.FromVertex];
                if (!linked)
                    v = v.Clone();
                //geom.AddVertex(v, false);
                builder.AddVertex(v, false);
                vertMatch.Add(i + culling.FromVertex, i);
            }

            var triIdx = Geometry.GetTriangleIndices();
            for (int i = 0; i < culling.IndexCount; i += 3)
            {
                int idx1 = triIdx[i + culling.FromIndex];
                int idx2 = triIdx[i + 1 + culling.FromIndex];
                int idx3 = triIdx[i + 2 + culling.FromIndex];
                idx1 = vertMatch[idx1];
                idx2 = vertMatch[idx2];
                idx3 = vertMatch[idx3];
                builder.AddTriangle(vertMatch[idx1], vertMatch[idx2], vertMatch[idx3]);
                //geom.AddTriangle2(geom.Vertices[idx1], geom.Vertices[idx2], geom.Vertices[idx3]);
            }
            return builder.GetGeometry();
            //return geom;
        }
    }
}
