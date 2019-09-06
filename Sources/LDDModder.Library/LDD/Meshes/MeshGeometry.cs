using LDDModder.IO;
using LDDModder.Simple3D;
using LDDModder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        internal void RebuildIndices()
        {
            var triIdx = GetTriangleIndices();
            for (int i = 0; i < IndexCount; i++)
            {
                Indices[i].IIndex = i;
                Indices[i].VIndex = triIdx[i];
            }
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

        #region Convertion From/To Stream

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriterEx(stream, true))
            {
                void WriteVector2(Vector2 v)
                {
                    bw.WriteSingle(v.X);
                    bw.WriteSingle(v.Y);
                }
                void WriteVector3(Vector3 v)
                {
                    bw.WriteSingle(v.X);
                    bw.WriteSingle(v.Y);
                    bw.WriteSingle(v.Z);
                }

                bw.Write(VertexCount);
                bw.Write(IndexCount);
                bw.Write(IsTextured);
                bw.Write(IsFlexible);

                for (int i = 0; i < VertexCount; i++)
                    WriteVector3(Vertices[i].Position);

                for (int i = 0; i < VertexCount; i++)
                    WriteVector3(Vertices[i].Normal);

                if (IsTextured)
                {
                    for (int i = 0; i < VertexCount; i++)
                        WriteVector2(Vertices[i].TexCoord);
                }

                int[] triIndices = GetTriangleIndices();
                bw.WriteArray(triIndices);

                for (int i = 0; i < triIndices.Length; i++)
                    WriteVector3(Indices[i].AverageNormal);

                for (int i = 0; i < triIndices.Length; i++)
                {
                    for (int j = 0; j < 6; j++)
                        WriteVector2(Indices[i].RoundEdgeData.Coords[j]);
                }

                if (IsFlexible)
                {
                    for (int i = 0; i < VertexCount; i++)
                    {
                        bw.Write(Vertices[i].BoneWeights.Count);
                        for (int j = 0; j < Vertices[i].BoneWeights.Count; j++)
                        {
                            bw.Write(Vertices[i].BoneWeights[j].BoneID);
                            bw.Write(Vertices[i].BoneWeights[j].Weight);
                        }
                    }
                }
            }
        }

        public static MeshGeometry FromStream(Stream stream)
        {
            var geom = new MeshGeometry();
            using (var br = new BinaryReaderEx(stream))
            {
                int vertCount = br.ReadInt32();
                int idxCount = br.ReadInt32();
                bool isTexured = br.ReadBoolean();
                bool isFlexible = br.ReadBoolean();

                var positions = new List<Vector3>();
                var normals = new List<Vector3>();
                var texCoords = new List<Vector2>();

                for (int i = 0; i < vertCount; i++)
                    positions.Add(new Vector3(br.ReadSingles(3)));

                for (int i = 0; i < vertCount; i++)
                    normals.Add(new Vector3(br.ReadSingles(3)));

                if (isTexured)
                {
                    for (int i = 0; i < vertCount; i++)
                        texCoords.Add(new Vector2(br.ReadSingles(2)));
                }

                var verts = new List<Vertex>();
                for (int i = 0; i < vertCount; i++)
                {
                    verts.Add(new Vertex(
                        positions[i],
                        normals[i],
                        isTexured ? texCoords[i] : Vector2.Empty
                        ));
                }

                var triIndices = new List<int>();
                var avgNormals = new List<Vector3>();
                var reCoords = new List<RoundEdgeData>();

                for (int i = 0; i < idxCount; i++)
                    triIndices.Add(br.ReadInt32());

                for (int i = 0; i < idxCount; i++)
                    avgNormals.Add(new Vector3(br.ReadSingles(3)));

                for (int i = 0; i < idxCount; i++)
                    reCoords.Add(new RoundEdgeData(br.ReadSingles(12)));

                var triangles = new List<Triangle>();

                for (int i = 0; i < idxCount; i += 3)
                {
                    var tri = new Triangle(
                        verts[triIndices[i]],
                        verts[triIndices[i + 1]],
                        verts[triIndices[i + 2]]);
                    triangles.Add(tri);

                    for (int j = 0; j < 3; j++)
                    {
                        tri.Indices[j].AverageNormal = avgNormals[i + j];
                        tri.Indices[j].RoundEdgeData = reCoords[i + j];
                    }
                }

                if (isFlexible)
                {
                    for (int i = 0; i < vertCount; i++)
                    {
                        int boneCount = br.ReadInt32();
                        for (int j = 0; j < boneCount; j++)
                            verts[i].BoneWeights.Add(new BoneWeight(br.ReadInt32(), br.ReadSingle()));
                    }
                }

                geom.SetVertices(verts);
                geom.SetTriangles(triangles);
            }
            return geom;
        }

        #endregion

        #region Convertion from binary format

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

        #endregion

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
                geom.Indices[i].RoundEdgeData = Indices[i].RoundEdgeData.Clone();
            }

            return geom;
        }

        public void SeparateDistinctSurfaces()
        {
            var uniqueEdges = Triangles.SelectMany(t => t.Edges).EqualsDistinct().ToList();
            var edgeFaces = new Dictionary<Edge, List<Triangle>>();
            foreach (var tri in Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (edgeFaces.TryGetValue(tri.Edges[i], out List<Triangle> faces))
                    {
                        if (!faces.Contains(tri))
                            faces.Add(tri);
                    }
                    else
                        edgeFaces.Add(tri.Edges[i], new List<Triangle>() { tri });
                }
            }

            var triList = Triangles.ToList();

            void AddConnectedFaces(Triangle tria, List<Triangle> remainingFaces, List<Triangle> currentFaces)
            {
                if (!currentFaces.Any())
                {
                    remainingFaces.Remove(tria);
                    currentFaces.Add(tria);
                }

                var connectedFaces = tria.Edges.SelectMany(x => edgeFaces[x]).Where(y => y != tria);
                var facesToAdd = connectedFaces.Intersect(remainingFaces).ToList();

                foreach (var face in facesToAdd)
                {
                    remainingFaces.Remove(face);
                    currentFaces.Add(face);
                }

                foreach (var face in facesToAdd)
                    AddConnectedFaces(face, remainingFaces, currentFaces);
            }

            var geomList = new List<MeshGeometry>();

            while (triList.Any())
            {
                var curTri = triList[0];
                var curTriList = new List<Triangle>();
                AddConnectedFaces(curTri, triList, curTriList);
                var curGeom = new MeshGeometry();
                curGeom.SetTriangles(curTriList, true);
                curGeom.BreakReferences();
                geomList.Add(curGeom);
            }
        }

        private void BreakReferences()
        {
            RebuildIndices();
            var oldIndices = Indices.ToList();
            _Triangles.Clear();
            _Vertices = _Vertices.Select(v => v.Clone()).ToList();

            for (int i = 0; i < oldIndices.Count / 3; i++)
            {
                Vertex[] verts = new Vertex[3];
                for (int j = 0; j < 3; j++)
                    verts[j] = _Vertices[oldIndices[(i * 3) + j].VIndex];

                var newTri = new Triangle(verts[0], verts[1], verts[2]);
                _Triangles.Add(newTri);

                for (int j = 0; j < 3; j++)
                {
                    newTri.Indices[j].AverageNormal = oldIndices[(i * 3) + j].AverageNormal;
                    newTri.Indices[j].RoundEdgeData = oldIndices[(i * 3) + j].RoundEdgeData.Clone();
                }
            }
        }

        public static MeshGeometry Combine(params MeshGeometry[] geometries)
        {
            if (geometries.Length == 1)
                return geometries[0];

            var verts = new List<Vertex>();
            var triangles = new List<Triangle>();

            foreach (var geom in geometries)
            {
                verts.AddRange(geom.Vertices);
                triangles.AddRange(geom.Triangles);
            }

            var newGeom = new MeshGeometry();
            newGeom.SetVertices(verts);
            newGeom.SetTriangles(triangles);
            newGeom.BreakReferences();
            return newGeom;
        }


    }
}
