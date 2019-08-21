using LDDModder.Simple3D;
using LDDModder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class MeshGeometry
    {
        private List<Vertex> _Vertices;
        private List<Triangle> _Triangles;

        public MeshIndexList Indices { get; }

        public int IndexCount => Indices.Count;

        public IList<Vertex> Vertices => _Vertices.AsReadOnly();

        public int VertexCount => Vertices.Count;

        public IList<Triangle> Triangles => _Triangles.AsReadOnly();

        public int TriangleCount => Triangles.Count;

        public bool IsTextured => Vertices.Any(x => !x.TexCoord.IsEmpty);

        public bool IsFlexible => Vertices.Any(x => x.BoneWeights.Any());

        public MeshGeometry()
        {
            _Vertices = new List<Vertex>();
            _Triangles = new List<Triangle>();
            Indices = new MeshIndexList(this);
        }

        //public Vertex AddVertex(Vertex vertex, bool checkDuplicate = true)
        //{
        //    if (checkDuplicate)
        //    {
        //        var existing = _Vertices.FirstOrDefault(x => x.Equals(vertex));

        //        if (existing == null)
        //        {
        //            _Vertices.Add(vertex);
        //            return vertex;
        //        }

        //        return existing;
        //    }

        //    _Vertices.Add(vertex);
        //    return vertex;
        //}

        public void SetVertices(IEnumerable<Vertex> vertices)
        {
            _Vertices.Clear();
            _Vertices.AddRange(vertices);
        }

        public void SetTriangles(IEnumerable<Triangle> triangles, bool withVertices = false)
        {
            _Triangles.Clear();
            _Triangles.AddRange(triangles);

            if (withVertices)
            {
                _Vertices.Clear();
                _Vertices.AddRange(_Triangles.SelectMany(x => x.Vertices).Distinct());
            }
        }

        //public void AddTriangle(Vertex v1, Vertex v2, Vertex v3)
        //{
        //    _Triangles.Add(new Triangle(AddVertex(v1), AddVertex(v2), AddVertex(v3)));
        //}

        //public void AddTriangle2(Vertex v1, Vertex v2, Vertex v3)
        //{
        //    _Triangles.Add(new Triangle(v1, v2, v3));
        //}

        public void AddTriangleFromIndices(int i1, int i2, int i3)
        {
            if (i1 >= _Vertices.Count)
                throw new IndexOutOfRangeException("i1");
            if (i2 >= _Vertices.Count)
                throw new IndexOutOfRangeException("i2");
            if (i3 >= _Vertices.Count)
                throw new IndexOutOfRangeException("i3");
            _Triangles.Add(new Triangle(_Vertices[i1], _Vertices[i2], _Vertices[i3]));
        }

        public void SimplifyVertices()
        {
            //var timer = Stopwatch.StartNew();

            //hashcodes are not completely unique but I still use them as a pre-filter
            var distinctVert = new Dictionary<int, List<Vertex>>();
            var vertIndices = new Dictionary<Vertex, List<VertexIndex>>();

            foreach (var idx in Indices)
            {
                if (!vertIndices.ContainsKey(idx.Vertex))
                    vertIndices.Add(idx.Vertex, new List<VertexIndex>());
                vertIndices[idx.Vertex].Add(idx);
            }

            for (int i = 0; i < _Vertices.Count; i++)
            {
                var v = _Vertices[i];
                var vh = v.GetHash();
                if (!distinctVert.ContainsKey(vh))
                    distinctVert.Add(vh, new List<Vertex>());
                distinctVert[vh].Add(v);
            }

            bool hasUnusedVerts = false;
            var simplifiedVerts = new List<Vertex>();
            foreach (var kv in distinctVert)
            {
                var vList = kv.Value;
                while (vList.Any())
                {
                    var v = vList[0];
                    simplifiedVerts.Add(v);
                    vList.RemoveAt(0);
                    foreach (var vv in vList.Where(x => x.Equals(v)).ToArray())
                    {
                        if (vertIndices.ContainsKey(vv))
                            vertIndices[vv].ForEach(idx => idx.ReassignVertex(v));
                        else
                            hasUnusedVerts = true;
                        vList.Remove(vv);
                    }
                }
            }

            if (hasUnusedVerts)
                Debug.WriteLine("Model has unused vertices");

            //timer.Stop();
            //Debug.WriteLine($"removed duplicates in: {timer.Elapsed}");

            _Vertices.Clear();
            _Vertices.AddRange(simplifiedVerts);

            for (int i = 0; i < Triangles.Count; i++)
                Triangles[i].RebuildEdges();
        }

        public void CalculateAverageNormals()
        {
            var vfD = new Dictionary<Vector3, List<Triangle>>();
            foreach (var tri in Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!vfD.ContainsKey(tri.Vertices[i].Position))
                        vfD.Add(tri.Vertices[i].Position, new List<Triangle>());
                    if (!vfD[tri.Vertices[i].Position].Contains(tri))
                        vfD[tri.Vertices[i].Position].Add(tri);
                }
            }

            foreach (var idx in Indices)
            {
                var tangentFaces = vfD[idx.Vertex.Position];// GetVertexFaces(idx.Vertex).ToList();
                var faceNormals = tangentFaces.Select(x => x.Normal).DistinctValues().ToList();

                Vector3 avgNormal = Vector3.Zero;
                foreach (var norm in faceNormals)
                    avgNormal += norm;

                avgNormal /= faceNormals.Count;
                avgNormal.Normalize();

                idx.AverageNormal = avgNormal;
            }
        }

        public static MeshGeometry Create(Files.MeshStructures.MESH_DATA mesh)
        {
            var vertices = new List<Vertex>();
            bool isTextured = mesh.UVs != null && mesh.UVs.Length > 0;
            bool isFlexible = mesh.Bones != null && mesh.Bones.Length > 0;

            for (int i = 0; i < mesh.Positions.Length; i++)
            {
                vertices.Add(new Vertex(
                    mesh.Positions[i],
                    mesh.Normals[i],
                    isTextured ? mesh.UVs[i] : Vector2.Empty
                    ));

                if (isFlexible)
                {
                    var bones = mesh.Bones[i];
                    for (int j = 0; j < bones.BoneWeights.Length; j++)
                        vertices[i].BoneWeights.Add(new BoneWeight(bones.BoneWeights[j].BoneID, bones.BoneWeights[j].Weight));
                }
            }
            var geom = new MeshGeometry();
            geom.SetVertices(vertices);

            for (int i = 0; i < mesh.Indices.Length; i += 3)
            {
                geom.AddTriangleFromIndices(
                    mesh.Indices[i].VertexIndex,
                    mesh.Indices[i + 1].VertexIndex,
                    mesh.Indices[i + 2].VertexIndex);
            }

            return geom;
        }

        public MeshType GetMeshType()
        {
            if (IsFlexible)
                return IsTextured ? MeshType.FlexibleTextured : MeshType.Flexible;
            return IsTextured ? MeshType.StandardTextured : MeshType.Standard;
        }

        public int[] GetTriangleIndices()
        {
            var vertIndexer = new ListIndexer<Vertex>(_Vertices);
            var idxList = new List<int>();

            foreach(var t in Triangles)
            {
                idxList.Add(vertIndexer.IndexOf(t.V1));
                idxList.Add(vertIndexer.IndexOf(t.V2));
                idxList.Add(vertIndexer.IndexOf(t.V3));
            }
            return idxList.ToArray();
        }

        public Vector3[] GetVertexPositions()
        {
            return Vertices.Select(v => v.Position).ToArray();
        }

        public MeshGeometry Clone()
        {
            var geom = new MeshGeometry();

            geom._Vertices.AddRange(Vertices.Select(x => x.Clone()));

            var triIdx = GetTriangleIndices();

            for (int i = 0; i < triIdx.Length; i += 3)
            {
                geom._Triangles.Add(new Triangle(
                    geom.Vertices[triIdx[i]], 
                    geom.Vertices[triIdx[i + 1]], 
                    geom.Vertices[triIdx[i + 2]]));
            }

            for (int i = 0; i < Indices.Count; i++)
            {
                geom.Indices[i].AverageNormal = Indices[i].AverageNormal;
                geom.Indices[i].RoundEdgeData = new RoundEdgeData(Indices[i].RoundEdgeData.Coords);
            }

            return geom;
        }
    }
}
