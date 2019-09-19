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

        class FaceEdge// : IEquatable<FaceEdge>
        {
            public Triangle Face { get; set; }
            public Edge Edge { get; set; }

            public Vector3 P1 { get; set; }
            public Vector3 P2 { get; set; }
            public Vector3 FaceNormal { get; set; }
            public Vector3 EdgeNormal { get; set; }
            public Vector3 PerpVec { get; set; }
            public bool IsShared { get; set; } 

            public Vector3 Direction => (P2 - P1).Normalized();


            public FaceEdge(Triangle tri, Edge edge)
            {
                Face = tri;
                Edge = edge;
                P1 = edge.P1.Position;
                P2 = edge.P2.Position;
                FaceNormal = tri.Normal;
                EdgeNormal = edge.EdgeNormal;
                IsShared = true;
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
                return base.GetHashCode();
                //var hashCode = 162377905;
                //int p1h = P1.Rounded(4).GetHashCode();
                //int p2h = P2.Rounded(4).GetHashCode();

                //if (p1h < p2h)
                //{
                //    hashCode = hashCode * -1521134295 + p1h;
                //    hashCode = hashCode * -1521134295 + p2h;
                //}
                //else
                //{
                //    hashCode = hashCode * -1521134295 + p2h;
                //    hashCode = hashCode * -1521134295 + p1h;
                //}

                //hashCode = hashCode * -1521134295 + EdgeNormal.Rounded().GetHashCode();
                //hashCode = hashCode * -1521134295 + FaceNormal.Rounded().GetHashCode();

                //return hashCode;
            }

        }

        class EdgeLine
        {
            public FaceEdge Edge { get; set; }
            public Edge TriangleEdge => Edge?.Edge;
            public Vector2 P1 { get; set; }
            public Vector2 P2 { get; set; }
            public Vector2 Direction => (P2 - P1).Normalized();
            public Vector2 Perpendicular { get; set; }
            //public Maths.Line Equation { get; set; }
            public bool IsTriangleEdge { get; set; }
            public bool UsedInUnion { get; set; }

            public EdgeLine(FaceEdge edge, Vector2 p1, Vector2 p2)
            {
                Edge = edge;
                P1 = p1;
                P2 = p2;
                //Equation = Maths.Line.FromPoints(p1, p2);
            }

            public bool IsColinear(EdgeLine other)
            {
                var similarDir = Direction.Equals(other.Direction, 0.001f) || Direction.Equals(other.Direction * -1f, 0.001f);
                return similarDir && (Contains(other.P1) || Contains(other.P2));

                //return Direction.Equals(other.Direction, 0.001f) || Direction.Equals(other.Direction * -1f, 0.001f);
            }

            public bool Contains(Vector3 position)
            {
                return Edge.Contains(position);
            }

            public bool Contains(Vector2 position)
            {
                return P1.Equals(position, 0.001f) || P2.Equals(position, 0.001f);
            }

            public Vector2 Opposite2D(Vector3 pt)
            {
                if (Edge.P1.Equals(pt))
                    return P2;
                return P1;
            }
        }

        public static void ComputeEdgeOutlines(IEnumerable<Triangle> triangles, float breakAngle = 35f)
        {
            //outline thickness = 0.013 (world space)

            var triangleList = triangles is List<Triangle> ? (List<Triangle>)triangles : triangles.ToList();

            var edgeFaces = BuildEdgeFacesDictionary(triangleList);

            var hardEdges = ComputeHardEdges(edgeFaces, breakAngle);

            var edgesPerVert = BuildVertexEdgesDictionary(hardEdges);

            foreach (var triangle in triangleList)
            {
                var nearEdges = new List<FaceEdge>();
                for (int i = 0; i < 3; i++)
                {
                    if (edgesPerVert.TryGetValue(triangle.Vertices[i].Position.Rounded(), out List<FaceEdge> list))
                        nearEdges.AddRange(list);
                }

                nearEdges = nearEdges.Distinct()
                    .Where(x => Vector3.AngleBetween(triangle.Normal, x.FaceNormal) < fPI * 0.30f)
                    .ToList();

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
                        IsTriangleEdge = triangle.ContainsEdge(edge.Edge),
                        Perpendicular = facePlane.ProjectVector2D(xAxis, edge.PerpVec)
                    });
                }

                //if (triangle.ContainsVertex(new Vector3(-0.31998f, -0.799826f, -0.3503f)) &&
                //    triangle.ContainsVertex(new Vector3(-0.31998f, -0.799826f, -0.27031f)) &&
                //    triangle.ContainsVertex(new Vector3(-0.295583f, -0.922212f, -0.27031f)))
                //{

                //}

                for (int coordPairIdx = 0; coordPairIdx < 3; coordPairIdx++)
                {
                    var idxPos = triangle.Vertices[coordPairIdx].Position;

                    var vertEdges = edgeLines.Where(x => x.Contains(idxPos))
                        .OrderByDescending(x => x.IsTriangleEdge)
                        .ThenBy(x=> Vector3.AngleBetween(x.Edge.FaceNormal, triangle.Normal))
                        .ToList();
                    
                    RemoveNonIntersectingEdges(facePlane, triangle, coordPairIdx, vertEdges);
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
                        var pos2D = facePlane.ProjectPoint2D(xAxis, idxPos);
                        var centerNormal = Vector2.Avg(pos2D + vertEdges[0].Perpendicular, pos2D + vertEdges[1].Perpendicular);
                        centerNormal = (centerNormal - pos2D).Normalized();
                        var dir1 = (vertEdges[0].Opposite2D(idxPos) - pos2D).Normalized();
                        var dir2 = (vertEdges[1].Opposite2D(idxPos) - pos2D).Normalized();
                        var totalAngle = Vector2.AngleBetween(centerNormal, dir1) + Vector2.AngleBetween(centerNormal, dir2);

                        bool isObtuse = totalAngle >= fPI;
                        var edgeCoords1 = GetTexCoordsForEdge(triangle, vertEdges[0].Edge);
                        var edgeCoords2 = GetTexCoordsForEdge(triangle, vertEdges[1].Edge);
                        
                        for (int j = 0; j < 3; j++)
                        {
                            var reData = triangle.Indices[j].RoundEdgeData;
                            reData.Set(coordPairIdx, edgeCoords1[j], edgeCoords2[j],
                                isObtuse ? RoundEdgeData.EdgeCombineMode.Intersection : RoundEdgeData.EdgeCombineMode.Union);
                        }

                        //if (!isObtuse)
                        //{
                        //    vertEdges[0].UsedInUnion = true;
                        //    vertEdges[1].UsedInUnion = true;
                        //}
                    }
                    else if (vertEdges.Count > 2)
                    {

                    }
                }
            }
        }

        private static Dictionary<SimpleEdge, List<FaceEdge>> BuildEdgeFacesDictionary(List<Triangle> triangleList)
        {
            var edgeFaces = new Dictionary<SimpleEdge, List<FaceEdge>>();

            foreach (var triangle in triangleList)
            {
                Vector3 center = triangle.GetCenter();
                for (int i = 0; i < 3; i++)
                {
                    var simpleEdge = new SimpleEdge(triangle.Edges[i]);

                    if (!edgeFaces.ContainsKey(simpleEdge))
                        edgeFaces.Add(simpleEdge, new List<FaceEdge>());

                    var faceEdge = new FaceEdge(triangle, triangle.Edges[i])
                    {
                        PerpVec = Vector3.GetPerpendicular(simpleEdge.P1, simpleEdge.P2, center)
                    };

                    edgeFaces[simpleEdge].Add(faceEdge);

                    triangle.Indices[i].RoundEdgeData.Reset();
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
                    kv.Value[0].IsShared = false;    
                    hardEdges.Add(kv.Value[0]);
                    continue;
                }

                if (kv.Value.Count > 2)
                    continue;

                var e1Normal = kv.Value[0].EdgeNormal;
                var e2Normal = kv.Value[1].EdgeNormal;

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

        private static void RemoveNonIntersectingEdges(Plane facePlane, Triangle triangle, int vertIndex, List<EdgeLine> edges)
        {
            var vertPos = triangle.Vertices[vertIndex].Position;
            var oppEdges = triangle.GetVerticeEdges(vertIndex);

            for (int i = edges.Count - 1; i >= 0; i--)
            {
                var curEdge = edges[i];
                var planarPerp = facePlane.ProjectVector(curEdge.Edge.PerpVec);

                var triCoords = ProjectTriangle(triangle, vertPos, planarPerp, true);

                if (triCoords.All(x => x.Y <= 0.001f))
                {
                    edges.RemoveAt(i);
                    continue;
                }

                var inter1 = Vector3.GetPerpIntersection(oppEdges[0].P1.Position, oppEdges[0].P2.Position, vertPos + planarPerp * 0.013f);
                var inter2 = Vector3.GetPerpIntersection(oppEdges[1].P1.Position, oppEdges[1].P2.Position, vertPos + planarPerp * 0.013f);

                if (!(oppEdges[0].IsInside(inter1) || oppEdges[1].IsInside(inter2)))
                {
                    edges.RemoveAt(i);
                    continue;
                }

                if (!curEdge.Edge.IsShared)
                {
                    if (!(triangle.ContainsVertex(curEdge.TriangleEdge.P1) ||
                        triangle.ContainsVertex(curEdge.TriangleEdge.P2)))
                    {
                        edges.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        private static Vector2[] ProjectTriangle(Triangle triangle, Vector3 origin, Vector3 axis, bool inverse)
        {
            Vector2[] coords = new Vector2[3];
            var plane = new Plane(origin, triangle.Normal);

            for (int i = 0; i < 3; i++)
            {
                coords[i] = plane.ProjectPoint2D(axis, triangle.Vertices[i].Position);
                if (inverse)
                    coords[i] = new Vector2(coords[i].Y, coords[i].X);
            }

            return coords;
        }

        private static Vector2[] GetTexCoordsForEdge(Triangle triangle, FaceEdge edge)
        {
            var plane = new Plane(triangle.V1.Position, triangle.Normal);

            Vector3 axisP1 = edge.P1;
            
            if (edge.IsContained(triangle))
                axisP1 = triangle.Edges.First(x => edge.Equals(x)).P1.Position;
            else
                axisP1 = triangle.ContainsVertex(edge.P1) ? edge.P1 : edge.P2;

            axisP1 = plane.ProjectPoint(axisP1);
            plane.Origin = axisP1;

            Vector3 yAxis = plane.ProjectVector(edge.PerpVec); //Flatten perpendicular vector to 2D plane

            Vector2[] coords = new Vector2[3];

            for (int i = 0; i < 3; i++)
            {
                var pos = plane.ProjectPoint2D(yAxis, triangle.Vertices[i].Position);
                coords[i] = new Vector2(pos.Y, pos.X);
            }

            var minX = coords.Min(c => c.X);
            for (int i = 0; i < 3; i++)
                coords[i].X -= minX;

            return coords;
        }

    }
}
