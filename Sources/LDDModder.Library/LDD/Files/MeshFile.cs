using LDDModder.LDD.Files.MeshStructures;
using LDDModder.LDD.Meshes;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files
{
    public class MeshFile
    {
        public MESH_FILE? OriginalData { get; }

        public string Filename { get; internal set; }

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

        internal MeshFile(MESH_FILE originalData, MeshType type)
        {
            OriginalData = originalData;
            Type = type;
            Cullings = new List<MeshCulling>();
        }

        public MeshFile(MeshType type)
        {
            Type = type;
            Cullings = new List<MeshCulling>();
        }

        public MeshFile(MeshGeometry geometry)
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

        public static MeshFile Read(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return GFileReader.ReadMesh(fs);
        }

        public void Save(string filename)
        {
            using (var fs = File.Open(filename, FileMode.Create))
                GFileWriter.WriteMesh(fs, this);
        }

        public void Save(Stream stream)
        {
            GFileWriter.WriteMesh(stream, this);
        }

        public void CreateDefaultCulling()
        {
            if (!Cullings.Any(x => x.Type == MeshCullingType.MainModel))
            {
                Cullings.Add(new MeshCulling(MeshCullingType.MainModel)
                {
                    VertexCount = VertexCount,
                    IndexCount = IndexCount
                });
            }
        }

        public MeshGeometry GetCullingGeometry(MeshCulling culling, bool linked = true)
        {
            var vertMatch = new Dictionary<int, int>();
            var builder = new GeometryBuilder();

            for (int i = 0; i < culling.VertexCount; i++)
            {
                var v = Vertices[i + culling.FromVertex];
                if (!linked)
                    v = v.Clone();
                builder.AddVertex(v, false);
                vertMatch.Add(culling.FromVertex + i, i);
            }

            var triIdx = Geometry.GetTriangleIndices();
            for (int i = 0; i < culling.IndexCount; i += 3)
            {
                int idx1 = triIdx[i + culling.FromIndex];
                int idx2 = triIdx[i + 1 + culling.FromIndex];
                int idx3 = triIdx[i + 2 + culling.FromIndex];
                builder.AddTriangle(idx1 - culling.FromVertex, idx2 - culling.FromVertex, idx3 - culling.FromVertex);
            }
            return builder.GetGeometry();
            //return geom;
        }

    }
}
