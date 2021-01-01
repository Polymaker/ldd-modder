using LDDModder.IO;
using LDDModder.Simple3D;
using LDDModder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public bool HasRoundEdgeData { get; set; }

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

                if (distinctVert.TryGetValue(vh, out List<Vertex> verts))
                    verts.Add(v);
                else
                    distinctVert.Add(vh, new List<Vertex>() { v });
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
                Triangles[i].RebuildIndices();
        }

        public void RebuildIndices()
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

        public bool CheckIfRoundEdgaDataDefined()
        {
            return Indices.Any(x => !x.RoundEdgeData.IsEmpty);
        }

        #region Convertion From/To Stream


        public void Save(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fs = File.Open(filename, FileMode.Create))
                Save(fs);
        }

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

        public static MeshGeometry FromFile(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return FromStream(fs);
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
                {
                    var reData = new RoundEdgeData(br.ReadSingles(12));
                    if (!geom.HasRoundEdgeData && !reData.IsEmpty)
                        geom.HasRoundEdgeData = true;
                    reCoords.Add(reData);
                }

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

        #region Xml Saving/Loading

        string FormatVector3(Simple3D.Vector3 v)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", v.X, v.Y, v.Z);
        }
        string FormatVector2(Simple3D.Vector2 v)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", v.X, v.Y);
        }

        public XDocument ConvertToXml()
        {
            var doc = new XDocument();
            var root = new XElement("LddGeometry");
            root.AddNumberAttribute("VertexCount", VertexCount);
            root.AddNumberAttribute("IndexCount", IndexCount);
            root.AddBooleanAttribute("IsTextured", IsTextured);
            root.AddBooleanAttribute("IsFlexible", IsFlexible);
            doc.Add(root);


            //var vertices = root.AddElement("Vertices");
            root.Add(new XComment("Format: X1 Y1 Z1 X2 Y2 Z2 X3 Y3 Z3 ..."));
            var positions = root.AddElement("Positions");
            positions.Value = string.Join(" ", Vertices.Select(v => FormatVector3(v.Position)));

            root.Add(new XComment("Format: X1 Y1 Z1 X2 Y2 Z2 X3 Y3 Z3 ..."));
            var normals = root.AddElement("Normals");
            normals.Value = string.Join(" ", Vertices.Select(v => FormatVector3(v.Normal)));

            if (IsTextured)
            {
                root.Add(new XComment("Format: X1 Y1 X2 Y2 X3 Y3 ..."));
                var uvs = root.AddElement("UVs");
                uvs.Value = string.Join(" ", Vertices.Select(v => FormatVector2(v.TexCoord)));
            }
            RebuildIndices();
            var indices = root.AddElement("Indices");
            indices.Value = string.Join(" ", Indices.Select(x => x.VIndex));

            if (IsFlexible)
            {
                root.Add(new XComment("Format: VertexIndex BoneID Weight ..."));
                var bones = root.AddElement("BoneWeights");
                for (int i = 0; i < VertexCount; i++)
                {
                    var vert = Vertices[i];
                    foreach (var bw in vert.BoneWeights)
                    {
                        bones.Value += string.Format(CultureInfo.InvariantCulture, 
                            "{0} {1} {2} ", i, bw.BoneID, bw.Weight);
                    }
                }
                bones.Value = bones.Value.TrimEnd();
            }
            
            if (CheckHasRoundEdgeData())
            {
                //root.Add(new XComment("Format: Index{n}Coord{1,6}.X Index{n}Coord{1,6}.Y"));
                var outlines = root.AddElement("Outlines");
                var coords = Indices.SelectMany(x => x.RoundEdgeData.Coords.Take(6)).ToList();
                outlines.Value = string.Join(" ", coords.Select(v => FormatVector2(v)));
            }

            return doc;
        }

        public void SaveAsXml(Stream stream)
        {
            var xmlDoc = ConvertToXml();
            xmlDoc.Save(stream);
        }

        public void SaveAsXml(string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fs = File.Open(filename, FileMode.Create))
                SaveAsXml(fs);
        }

        public static MeshGeometry FromXml(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return FromXml(fs);
        }

        public static MeshGeometry FromXml(Stream stream)
        {
            var doc = XDocument.Load(stream);
            return doc != null ? FromXml(doc) : null;
        }

        public static MeshGeometry FromXml(XDocument document)
        {
            var geomElem = document.Element("LddGeometry");
            if (geomElem == null)
                return null;

            if (geomElem.HasElement("Positions", out XElement posElem) &&
                geomElem.HasElement("Normals", out XElement normElem) &&
                geomElem.HasElement("Indices", out XElement indElem))
            {
                var positions = ParseVector3Array(posElem.Value);
                var normals = ParseVector3Array(normElem.Value);
                List<Vector2> uvs = null;
                if (geomElem.HasElement("UVs", out XElement uvElem))
                    uvs = ParseVector2Array(uvElem.Value);

                var indices = indElem.Value.Split(' ').Select(v => int.Parse(v)).ToList();

                var vertices = new List<Vertex>();
                var triangles = new List<Triangle>();
                var reCoords = new List<RoundEdgeData>();

                for (int i = 0; i < positions.Count; i++)
                {
                    vertices.Add(new Vertex(
                        positions[i],
                        normals[i],
                        uvs?[i] ?? Vector2.Empty
                        ));
                }

                if (geomElem.HasElement("BoneWeights", out XElement boneWeights))
                {
                    var bwValues = boneWeights.Value.Split(' ');
                    for (int i = 0; i < bwValues.Length; i += 3)
                    {
                        int vIdx = int.Parse(bwValues[i]);
                        int bID = int.Parse(bwValues[i + 1]);
                        float weight = float.Parse(bwValues[i + 2], CultureInfo.InvariantCulture);
                        vertices[vIdx].BoneWeights.Add(new BoneWeight(bID, weight));
                    }
                }

                if (geomElem.HasElement("Outlines", out XElement outlineElem))
                {
                    var outlineUVs = ParseVector2Array(outlineElem.Value);
                    if (outlineUVs.Count % 6 != 0)
                    {
                        //problem
                    }

                    for (int i = 0; i < outlineUVs.Count; i += 6)
                    {
                        var reData = new RoundEdgeData();
                        for (int j = 0; j < 6; j++)
                            reData.Coords[j] = outlineUVs[i + j];

                        reCoords.Add(reData);
                    }
                }

                for (int i = 0; i < indices.Count; i += 3)
                {
                    var tri = new Triangle(
                        vertices[indices[i]],
                        vertices[indices[i + 1]],
                        vertices[indices[i + 2]]);
                    triangles.Add(tri);

                    for (int j = 0; j < 3; j++)
                    {
                        if (i + j >= reCoords.Count)
                            break;
                        tri.Indices[j].RoundEdgeData = reCoords[i + j];
                    }
                }

                var geom = new MeshGeometry();
                geom.SetVertices(vertices);
                geom.SetTriangles(triangles);

                return geom;
            }

            return null;
        }

        static List<Vector3> ParseVector3Array(string value)
        {
            var values = value.Split(' ');
            var result = new List<Vector3>();
            for (int i = 0; i < values.Length; i += 3)
            {
                
                result.Add(new Vector3
                {
                    X = float.Parse(values[i], CultureInfo.InvariantCulture),
                    Y = float.Parse(values[i + 1], CultureInfo.InvariantCulture),
                    Z = float.Parse(values[i + 2], CultureInfo.InvariantCulture)
                });
            }
            return result;
        }

        static List<Vector2> ParseVector2Array(string value)
        {
            var values = value.Split(' ');
            var result = new List<Vector2>();
            for (int i = 0; i < values.Length; i += 2)
            {

                result.Add(new Vector2
                {
                    X = float.Parse(values[i], CultureInfo.InvariantCulture),
                    Y = float.Parse(values[i + 1], CultureInfo.InvariantCulture)
                });
            }
            return result;
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
            Edge.CompareByPosition = false;
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

        public MeshGeometry GetPartialGeometry(int startIndex, int indexCount, int startVertex, int vertexCount)
        {
            var builder = new GeometryBuilder();

            for (int i = 0; i < vertexCount; i++)
            {
                var v = Vertices[i + startVertex];
                builder.AddVertex(v.Clone(), false);
            }

            var triangleIndices = GetTriangleIndices();

            for (int i = 0; i < indexCount; i += 3)
            {
                int idx1 = triangleIndices[i + startIndex];
                int idx2 = triangleIndices[i + 1 + startIndex];
                int idx3 = triangleIndices[i + 2 + startIndex];
                builder.AddTriangle(idx1 - startVertex, idx2 - startVertex, idx3 - startVertex);
            }

            var geom = builder.GetGeometry();
            for (int i = 0; i < indexCount; i++)
            {
                var originalIndex = Indices[i + startIndex];
                geom.Indices[i].RoundEdgeData = originalIndex.RoundEdgeData.Clone();
                geom.Indices[i].AverageNormal = originalIndex.AverageNormal;
            }

            return geom;
        }
    
        public void TransformVertices(Matrix4 matrix)
        {
            foreach(var vert in Vertices)
            {
                vert.Position = Matrix4.TransformPosition(matrix, vert.Position);
                vert.Normal = Matrix4.TransformNormal(matrix, vert.Normal);
            }
        }

        public void ClearRoundEdgeData()
        {
            foreach(var idx in Indices)
                idx.RoundEdgeData.Reset();
        }

        public bool CheckHasRoundEdgeData()
        {
            HasRoundEdgeData = Indices.Any(x => !x.RoundEdgeData.IsEmpty);
            return HasRoundEdgeData;
        }
    }
}
