using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class MeshBuilder
    {
        private List<Vertex> Vertices;
        private List<Triangle> Triangles;
        private List<IndexBuilder> Indices;
        private List<Edge> Edges;
        private List<Edge> BoundaryEdges;

        public MeshBuilder()
        {
            Vertices = new List<Vertex>();
            Triangles = new List<Triangle>();
            Indices = new List<IndexBuilder>();
            Edges = new List<Edge>();
            BoundaryEdges = new List<Edge>();
        }

        public void AddTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            var tri = new Triangle(v1, v2, v3);
            Triangles.Add(tri);

            foreach (var v in tri.Vertices)
            {
                if (!Vertices.Contains(v))
                    Vertices.Add(v);
            }
        }

        public void AddStud(Vector3 position/*, Vector3 normal*/)
        {
            var dist = 0.245f;

            var angleStep = (2f * (float)Math.PI) / 12f;
            var curAngle = angleStep / 2f;
            float studHeight = 0.178f;
            var studTop = position + Vector3.UnitY * studHeight;

            for (int i = 0; i < 12; i++)
            {
                var a1 = curAngle + (angleStep / 2f);
                var a2 = curAngle - (angleStep / 2f);

                //var faceNormal = new Vector3()
                //{
                //    X = (float)Math.Cos(curAngle),
                //    Z = (float)Math.Sin(curAngle)
                //};

                var dir1 = new Vector3()
                {
                    X = (float)Math.Cos(a1),
                    Z = (float)Math.Sin(a1)
                };
                var dir2 = new Vector3()
                {
                    X = (float)Math.Cos(a2),
                    Z = (float)Math.Sin(a2)
                };

                var v1 = position + (dir1 * dist);
                var v2 = position + (dir2 * dist);
                var v3 = position + (dir1 * dist) + Vector3.UnitY * studHeight;
                var v4 = position + (dir2 * dist) + Vector3.UnitY * studHeight;
                
                AddTriangle(new Vertex(v1, dir1), new Vertex(v2, dir2), new Vertex(v3, dir1));
                AddTriangle(new Vertex(v2, dir2), new Vertex(v4, dir2), new Vertex(v3, dir1));
                AddTriangle(new Vertex(v3, Vector3.UnitY), new Vertex(v4, Vector3.UnitY), new Vertex(studTop, Vector3.UnitY));
                curAngle += angleStep;
            }
        }

        private void SimplifyVertices()
        {
            var simplifiyedVerts = new List<Vertex>();

            foreach (var vert in Vertices)
            {
                if (!simplifiyedVerts.Any(x => x.Equals(vert)))
                    simplifiyedVerts.Add(vert);
            }
            
            foreach (var triangle in Triangles)
            {
                triangle.ReassignVertices(
                    simplifiyedVerts.FirstOrDefault(x => x.Equals(triangle.V1)),
                    simplifiyedVerts.FirstOrDefault(x => x.Equals(triangle.V2)),
                    simplifiyedVerts.FirstOrDefault(x => x.Equals(triangle.V3))
                    );
            }

            Vertices = simplifiyedVerts;
        }

        private void GenerateGeometryInfo()
        {
            Indices.Clear();
            foreach (var tri in Triangles)
            {
                foreach (var v in tri.Vertices)
                    Indices.Add(new IndexBuilder(Vertices.IndexOf(v), v));
            }

            Edges.Clear();
            Edges.AddRange(Triangles.SelectMany(t => t.Edges).Distinct());

            BoundaryEdges.Clear();

            foreach (var edge in Edges)
            {
                var connectedTriangles = Triangles.Where(t => t.Edges.Contains(edge));
                if (connectedTriangles.Count() == 1)
                    BoundaryEdges.Add(edge);
            }
        }

        private IEnumerable<Triangle> GetTangentFaces(Triangle triangle)
        {
            foreach(var tri in Triangles)
            {
                if (tri != triangle && tri.Vertices.Count(x => triangle.ContainsVertex(x.Position)) == 2)
                    yield return tri;
            }
        }

        private IEnumerable<Triangle> GetVertexFaces(Vertex vertex)
        {
            foreach (var tri in Triangles)
            {
                if (tri.ContainsVertex(vertex.Position))
                    yield return tri;
            }
        }

        private void GenerateRoundEdgeData()
        {
            float edgeWidthRatio = 15.5f / 0.8f;

            for (int t = 0; t < Triangles.Count; t++)
            {
                var triangle = Triangles[t];
                //var tangentFaces = GetTangentFaces(triangle).ToList();
                //var tangentNormals = tangentFaces.Select(x => x.Normal).EqualsDistinct().ToList();

                var center = triangle.GetCenter();
                var edgePerps = new Vector3[3];
                var perpDists = new List<float>();

                for (int i = 0; i < 3; i++)
                {
                    edgePerps[i] = triangle.GetEdgePerp(i);
                }
                //Console.WriteLine(string.Join(", ", perpDists.Select(x => (x * edgeWidthRatio) + 100)));

                var xAxis2D = (triangle.Vertices[1].Position - triangle.Vertices[0].Position).Normalized();
                var yAxis2D = edgePerps[0];

                var planarCoords = new Vector2[3];
                for (int i = 0; i < 3; i++)
                {
                    var index = Indices[(t * 3) + i];
                    planarCoords[i] = Vector3.ProjectToPlane2D(index.Vertex.Position, center, triangle.Normal, xAxis2D, yAxis2D);

                    //for (int j = 0; j < 3; j++)
                    //{
                    //    var test = triangle.GetPlanarCoordsRelativeToEdge(j, i);
                    //    Console.WriteLine(string.Join(", ", test.Select(x => x * edgeWidthRatio)));
                    //}
                    //var test1 = triangle.GetPlanarCoordsRelativeToEdge((i + 2) % 3, i);
                    //var test2 = triangle.GetPlanarCoordsRelativeToEdge(i, i);
                    //Console.WriteLine(string.Join(", ", test1.Select(x=>x * edgeWidthRatio)));
                    //Console.WriteLine(string.Join(", ", test2.Select(x => x * edgeWidthRatio)));
                }

                var minCoord = Vector2.Min(planarCoords);
                for (int i = 0; i < 3; i++)
                    planarCoords[i] -= minCoord;
                var maxCoord = Vector2.Max(planarCoords);

                Console.WriteLine($"Triangle {t}:");

                for (int i = 0; i < 3; i++)
                {
                    var index = Indices[(t * 3) + i];
                    var coords = index.RoundEdgeData.Coords;
                    var perp1 = edgePerps[i];
                    
                    for (int j = 0; j < 3; j++)
                    {
                        var otherIndex = Indices[(t * 3) + j];
                        var otherIdxDiff = planarCoords[j] - planarCoords[i];
                        coords[(j * 2)].X = 100;
                        coords[(j * 2) + 1].X = 100;
                        //coords[(j * 2)].X = (100 + Math.Abs(v1)) * (float)Math.Sign(v1 == 0 ? 1 : v1);
                        //coords[(j * 2) + 1].X = (100 + Math.Abs(v2)) * (float)Math.Sign(v2 == 0 ? 1 : v2);
                        coords[(j * 2)].Y = Math.Abs(otherIdxDiff.X) * edgeWidthRatio;
                        coords[(j * 2) + 1].Y = Math.Abs(otherIdxDiff.Y) * edgeWidthRatio;

                        //var facesTangentToVertex = GetVertexFaces(otherIndex.Vertex).ToList();
                        //var faceNormals = facesTangentToVertex.Select(x => x.Normal).EqualsDistinct().ToList();

                        if (BoundaryEdges.Count(x => x.Contains(otherIndex.Vertex)) == 0)
                        {
                            coords[(j * 2)] = new Vector2(1000, 1000);
                            coords[(j * 2) + 1] = new Vector2(1000, 1000);
                        }
                    }

                    Console.WriteLine(index.RoundEdgeData.ToString());
                }
                Console.WriteLine("");

            }
        }

        private void CalculateAverageNormals()
        {
            foreach (var idx in Indices)
            {
                var tangentFaces = GetVertexFaces(idx.Vertex).ToList();
                var faceNormals = tangentFaces.Select(x => x.Normal).DistinctValues().ToList();

                Vector3 avgNormal = Vector3.Zero;
                foreach (var norm in faceNormals)
                    avgNormal += norm;

                avgNormal /= faceNormals.Count;
                avgNormal.Normalize();

                idx.AverageNormal = avgNormal;
            }
        }

        public Mesh BuildMesh()
        {
            SimplifyVertices();
            GenerateGeometryInfo();
            CalculateAverageNormals();
            GenerateRoundEdgeData();
            

            var avgNormalDistinct = Indices.Select(x => x.AverageNormal).DistinctValues().ToList();
            var roundEdgeDataDistinct = Indices.Select(x => x.RoundEdgeData).EqualsDistinct().ToList();
            int totalOffset = 0;

            for (int i = 0; i < roundEdgeDataDistinct.Count; i++)
            {
                roundEdgeDataDistinct[i].FileOffset = totalOffset;
                if (256 - ((totalOffset + 12) % 256) < 12)
                    roundEdgeDataDistinct[i].PackData();
                totalOffset += roundEdgeDataDistinct[i].Coords.Length * 2;
            }

            var mesh = new Mesh()
            {
                Vertices = Vertices.ToArray(),
                Indices = new IndexReference[Indices.Count],
                AverageNormals = avgNormalDistinct.ToArray(),
                EdgeShaderValues = roundEdgeDataDistinct.ToArray(),
                Type = MeshType.Standard
            };

            
            for (int i = 0; i < Indices.Count; i++)
            {
                var idx = Indices[i];
                var roundEdgeData = roundEdgeDataDistinct.First(x => x.Equals(idx.RoundEdgeData));
                var avgNormal = avgNormalDistinct.First(x => x.Equals(idx.AverageNormal));
                mesh.Indices[i] = new IndexReference(idx.VertexIndex)
                {
                    AverageNormalIndex = avgNormalDistinct.IndexOf(avgNormal),
                    RoundEdgeDataOffset = roundEdgeData.FileOffset,
                    ShaderDataIndex = roundEdgeDataDistinct.IndexOf(roundEdgeData)
                };
            }

            mesh.CullingInfos = new CullingInfo[]
            {
                new CullingInfo(MeshCullingType.MainModel,0, mesh.Vertices.Length, 0, mesh.Indices.Length)
            };

            return mesh;
        }

        private class IndexBuilder
        {
            public int VertexIndex { get; set; }
            public Vertex Vertex { get; set; }
            public Vector3 AverageNormal { get; set; }
            public RoundEdgeData RoundEdgeData { get; set; }

            public IndexBuilder()
            {

            }

            public IndexBuilder(int vertexIndex, Vertex vertex)
            {
                VertexIndex = vertexIndex;
                Vertex = vertex;
                RoundEdgeData = new RoundEdgeData();
            }
        }

        
    }
}
