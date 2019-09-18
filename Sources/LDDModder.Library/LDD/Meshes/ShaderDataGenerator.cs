using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public static class ShaderDataGenerator
    {
        public const float fPI = (float)Math.PI;

        public static void ComputeAverageNormals(IEnumerable<Triangle> triangles)
        {
            var triangleList = triangles is List<Triangle> ? (List<Triangle>)triangles : triangles.ToList();

            var facesPerVertPos = new Dictionary<Vector3, List<Triangle>>();

            foreach (var tri in triangleList)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!facesPerVertPos.ContainsKey(tri.Vertices[i].Position))
                        facesPerVertPos.Add(tri.Vertices[i].Position, new List<Triangle>());

                    facesPerVertPos[tri.Vertices[i].Position].Add(tri);
                }
            }

            var vertPosNormals = new Dictionary<Vector3, Vector3>();

            foreach (var kv in facesPerVertPos)
            {
                var faceNormals = kv.Value.Select(x => x.Normal).DistinctValues().ToList();
                Vector3 avgNormal = Vector3.Zero;
                foreach (var norm in faceNormals)
                    avgNormal += norm;

                avgNormal /= faceNormals.Count;
                avgNormal.Normalize();

                vertPosNormals.Add(kv.Key, avgNormal);
            }

            foreach (var tri in triangleList)
            {
                for (int i = 0; i < 3; i++)
                {
                    var idx = tri.Indices[i];
                    idx.AverageNormal = vertPosNormals[idx.Vertex.Position];
                }
            }
        }

        

        struct SimpleEdge : IEquatable<SimpleEdge>
        {
            public Vector3 P1;
            public Vector3 P2;

            public SimpleEdge(Vector3 p1, Vector3 p2)
            {
                P1 = p1;
                P2 = p2;
            }

            public SimpleEdge(Edge edge)
            {
                P1 = edge.P1.Position.Rounded();
                P2 = edge.P2.Position.Rounded();
            }

            public bool Equals(SimpleEdge other)
            {
                return (other.P1.Equals(P1) && other.P2.Equals(P2)) || (other.P1.Equals(P2) && other.P2.Equals(P1));
            }

            public override bool Equals(object obj)
            {
                if (obj is SimpleEdge se)
                    return Equals(se);
                return base.Equals(obj);
            }


            public override int GetHashCode()
            {
                var hashCode = 162377905;
                int p1h = P1/*.Rounded(4)*/.GetHashCode();
                int p2h = P2/*.Rounded(4)*/.GetHashCode();

                if (p1h < p2h)
                {
                    hashCode = hashCode * -1521134295 + p1h;
                    hashCode = hashCode * -1521134295 + p2h;
                }
                else
                {
                    hashCode = hashCode * -1521134295 + p2h;
                    hashCode = hashCode * -1521134295 + p1h;
                }
                return hashCode;
            }
        }

        class FaceEdge : IEquatable<FaceEdge>
        {
            public Vector3 P1;
            public Vector3 P2;
            public Vector3 FaceNormal;
            public Vector3 EdgeNormal;
            public bool IsSingle;

            public Vector3 Direction => (P2 - P1).Normalized();


            public FaceEdge(Triangle tri, Edge edge)
            {
                P1 = edge.P1.Position;
                P2 = edge.P2.Position;
                FaceNormal = tri.Normal;
                EdgeNormal = edge.EdgeNormal;
            }

            public bool Equals(FaceEdge other)
            {
                return (
                        (other.P1.Equals(P1) && other.P2.Equals(P2)) ||
                        (other.P1.Equals(P2) && other.P2.Equals(P1))
                    ) && 
                    other.EdgeNormal.Equals(EdgeNormal) && 
                    other.FaceNormal.Equals(FaceNormal);
            }

            public override bool Equals(object obj)
            {
                if (obj is FaceEdge fe)
                    return Equals(fe);
                return base.Equals(obj);
            }

            public bool Equals(Edge edge)
            {
                return edge.Contains(P1) && edge.Contains(P2);
            }

            public bool IsContained(Triangle triangle)
            {
                return triangle.ContainsVertex(P1) && triangle.ContainsVertex(P2);
            }

            public bool Contains(Vector3 position)
            {
                return P1.Equals(position, 0.001f) || P2.Equals(position, 0.001f);
            }

            public override int GetHashCode()
            {
                var hashCode = 162377905;
                int p1h = P1.Rounded(4).GetHashCode();
                int p2h = P2.Rounded(4).GetHashCode();

                if (p1h < p2h)
                {
                    hashCode = hashCode * -1521134295 + p1h;
                    hashCode = hashCode * -1521134295 + p2h;
                }
                else
                {
                    hashCode = hashCode * -1521134295 + p2h;
                    hashCode = hashCode * -1521134295 + p1h;
                }

                hashCode = hashCode * -1521134295 + EdgeNormal.Rounded().GetHashCode();
                hashCode = hashCode * -1521134295 + FaceNormal.Rounded().GetHashCode();

                return hashCode;
            }

        }

        class EdgeLine
        {
            public FaceEdge Edge { get; set; }
            public Vector2 P1 { get; set; }
            public Vector2 P2 { get; set; }
            public Vector2 Direction => (P2 - P1).Normalized();
            public Maths.Line Equation { get; set; }
            public bool IsTriangleEdge { get; set; }
            public bool UsedInUnion { get; set; }

            public EdgeLine(FaceEdge edge, Vector2 p1, Vector2 p2)
            {
                Edge = edge;
                P1 = p1;
                P2 = p2;
                Equation = Maths.Line.FromPoints(p1, p2);
            }

            public bool IsColinear(EdgeLine other)
            {
                return Equation == other.Equation;
                //return Direction.Equals(other.Direction, 0.001f) || Direction.Equals(other.Direction * -1f, 0.001f);
            }

            public bool Contains(Vector3 position)
            {
                return Edge.Contains(position);
            }

            public Vector2 Opposite(Vector3 pt)
            {
                if (Edge.P1.Equals(pt))
                    return P2;
                return P1;
            }
        }

        private static void RemoveDuplicateEdgeLines(List<EdgeLine> edges)
        {
            for (int i = 0; i < edges.Count - 1; i++)
            {
                for (int j = edges.Count - 1; j > i; j--)
                {
                    if (edges[i].IsColinear(edges[j]))
                        edges.RemoveAt(j);
                }
            }
        }

        public static void ComputeEdgeOutlines(IEnumerable<Triangle> triangles, float breakAngle = 45f)
        {
            //outline thickness = 0.013 (world space)

            var triangleList = triangles is List<Triangle> ? (List<Triangle>)triangles : triangles.ToList();

            var edgeFaces = BuildEdgeFacesDictionary(triangleList);

            var hardEdges = ComputeHardEdges(edgeFaces, breakAngle);

            var edgesPerVert = BuildVertexEdgesDictionary(hardEdges);

            foreach (var triangle in triangleList)
            {
                //var nearEdges = hardEdges.Where(x =>
                //    (triangle.ContainsVertex(x.P1) || triangle.ContainsVertex(x.P2)) &&
                //    Vector3.AngleBetween(triangle.Normal, x.FaceNormal) < fPI * 0.4f);

                var nearEdges = new List<FaceEdge>();
                for (int i = 0; i < 3; i++)
                {
                    if (edgesPerVert.TryGetValue(triangle.Vertices[i].Position.Rounded(), out List<FaceEdge> list))
                        nearEdges.AddRange(list);
                }

                nearEdges = nearEdges.Distinct()
                    .Where(x => Vector3.AngleBetween(triangle.Normal, x.FaceNormal) < fPI * 0.35f)
                    .ToList();

                //nearEdges.RemoveAll(x => x.IsSingle && !x.IsContained(triangle));

                if (!nearEdges.Any())
                    continue;

                var facePlane = new Plane(triangle.GetCenter(), triangle.Normal);
                var xAxis = (triangle.V1.Position - facePlane.Origin).Normalized();

                var edgeLines = new List<EdgeLine>();

                foreach(var edge in nearEdges)
                {
                    edgeLines.Add(new EdgeLine(
                        edge,
                        facePlane.ProjectPoint2D(xAxis, edge.P1),
                        facePlane.ProjectPoint2D(xAxis, edge.P2))
                    {
                        IsTriangleEdge = edge.IsContained(triangle)
                    });
                }

                for (int coordPairIdx = 0; coordPairIdx < 3; coordPairIdx++)
                {
                    var idxPos = triangle.Vertices[coordPairIdx].Position;

                    var vertEdges = edgeLines.Where(x => x.Contains(idxPos))
                        .OrderByDescending(x => x.IsTriangleEdge)
                        .ToList();

                    RemoveDuplicateEdgeLines(vertEdges);

                    if (vertEdges.Count == 1)
                    {
                        //if (vertEdges[0].UsedInUnion)
                        //    continue;

                        var edgeCoords = GetTexCoordsForEdge(triangle, vertEdges[0].Edge);
                        vertEdges[0].UsedInUnion = true;

                        for (int j = 0; j < 3; j++)
                        {
                            var reData = triangle.Indices[j].RoundEdgeData;
                            reData.Set(coordPairIdx, edgeCoords[j]);
                        }
                    }
                    else if (vertEdges.Count == 2 )
                    {
                        var pc = facePlane.ProjectPoint2D(xAxis, idxPos);
                        var p1 = vertEdges[0].Opposite(idxPos);
                        var p2 = vertEdges[1].Opposite(idxPos);

                        var perp1 = vertEdges[0].Equation.GetPerpendicular(pc);
                        var perp2 = vertEdges[1].Equation.GetPerpendicular(pc);


                        var vc = (pc * -1).Normalized();
                        var v1 = (p1 - pc).Normalized();
                        var v2 = (p2 - pc).Normalized();

                        var crossAngle = Vector2.AngleBetween(v1, vc) + Vector2.AngleBetween(v2, vc);

                        //TODO: redo union/intersection detection
                        bool isObtuse = crossAngle >= fPI;
                        var edgeCoords1 = GetTexCoordsForEdge(triangle, vertEdges[0].Edge);
                        var edgeCoords2 = GetTexCoordsForEdge(triangle, vertEdges[1].Edge);

                        for (int j = 0; j < 3; j++)
                        {
                            var reData = triangle.Indices[j].RoundEdgeData;
                            reData.Set(coordPairIdx, edgeCoords1[j], edgeCoords2[j],
                                isObtuse ? RoundEdgeData.EdgeCombineMode.Intersection : RoundEdgeData.EdgeCombineMode.Union);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        private static Dictionary<SimpleEdge, List<FaceEdge>> BuildEdgeFacesDictionary(List<Triangle> triangleList)
        {
            var edgeFaces = new Dictionary<SimpleEdge, List<FaceEdge>>();

            foreach (var tri in triangleList)
            {
                for (int i = 0; i < 3; i++)
                {
                    var simpleEdge = new SimpleEdge(tri.Edges[i]);

                    if (!edgeFaces.ContainsKey(simpleEdge))
                        edgeFaces.Add(simpleEdge, new List<FaceEdge>());

                    edgeFaces[simpleEdge].Add(new FaceEdge(tri, tri.Edges[i]));

                    tri.Indices[i].RoundEdgeData.Reset();
                }
            }

            return edgeFaces;
        }

        private static List<FaceEdge> ComputeHardEdges(Dictionary<SimpleEdge, List<FaceEdge>> edgeFaces, float breakAngle)
        {
            var hardEdges = new List<FaceEdge>();

            foreach (var kv in edgeFaces)
            {
                if (kv.Value.Count == 1)
                {
                    kv.Value[0].IsSingle = true;    
                    hardEdges.Add(kv.Value[0]);
                    continue;
                }

                if (kv.Value.Count > 2)
                    continue;

                var e1Normal = kv.Value[0].FaceNormal;
                var e2Normal = kv.Value[1].FaceNormal;

                if (e1Normal.Equals(e2Normal))
                    continue;

                var angleDiff = Vector3.AngleBetween(e1Normal, e2Normal);

                if (float.IsNaN(angleDiff) || float.IsInfinity(angleDiff))
                    continue;

                if (angleDiff < 0)
                    angleDiff = (fPI * 2f + angleDiff) % (fPI * 2f);

                angleDiff = angleDiff / (fPI * 2f) * 360f;

                if (angleDiff >= breakAngle)
                {
                    hardEdges.Add(kv.Value[0]);
                    hardEdges.Add(kv.Value[1]);
                }
            }
            return hardEdges;
        }

        private static Dictionary<Vector3, List<FaceEdge>> BuildVertexEdgesDictionary(List<FaceEdge> hardEdges)
        {
            var edgesPerVert = new Dictionary<Vector3, List<FaceEdge>>();

            foreach (var hEdge in hardEdges)
            {
                var p1 = hEdge.P1.Rounded();
                if (!edgesPerVert.ContainsKey(p1))
                    edgesPerVert.Add(p1, new List<FaceEdge>());
                edgesPerVert[p1].Add(hEdge);

                var p2 = hEdge.P2.Rounded();
                if (!edgesPerVert.ContainsKey(p2))
                    edgesPerVert.Add(p2, new List<FaceEdge>());
                edgesPerVert[p2].Add(hEdge);
            }

            return edgesPerVert;
        }

        private static Vector2[] GetTexCoordsForEdge(Triangle triangle, FaceEdge edge)
        {
            var plane = new Plane(triangle.V1.Position, triangle.Normal);

            Vector3 axisP1 = edge.P1;
            
            if (edge.IsContained(triangle))
                axisP1 = triangle.Edges.First(x => edge.Equals(x)).P1.Position;
            else
                axisP1 = triangle.ContainsVertex(edge.P1) ? edge.P1 : edge.P2;

            Vector3 axisP2 = axisP1.Equals(edge.P1) ? edge.P2 : edge.P1;
            axisP1 = plane.ProjectPoint(axisP1);
            axisP2 = plane.ProjectPoint(axisP2);
            plane.Origin = axisP1;

            Vector3 xAxis = (axisP2 - axisP1).Normalized();
            Vector3 yAxis = Vector3.Cross(plane.Normal, xAxis).Normalized();

            var center = triangle.GetCenter();
            if (Vector3.Distance(center, axisP1 + yAxis) > Vector3.Distance(center, axisP1 + yAxis * -1))
            {
                xAxis *= -1;
            }

            Vector2[] coords = new Vector2[3];

            for (int i = 0; i < 3; i++)
                coords[i] = plane.ProjectPoint2D(xAxis, triangle.Vertices[i].Position);

            var minX = coords.Min(c => c.X);
            for (int i = 0; i < 3; i++)
                coords[i].X -= minX;

            return coords;
        }

    }
}
