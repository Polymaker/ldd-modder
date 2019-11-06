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
            public bool IsHard { get; set; }
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
                return edge.Contains(P1, 0.001f) && edge.Contains(P2, 0.001f);
            }

            public bool IsContained(Triangle triangle)
            {
                return triangle.ContainsVertex(P1) && triangle.ContainsVertex(P2);
            }

            public bool Contains(Vector3 position)
            {
                return P1.Equals(position, 0.001f) || P2.Equals(position, 0.001f);
            }

            public bool IsInside(Vector3 pos)
            {
                var maxV = (P2 - P1);
                var diff = (pos - P1);
                if (diff.Length <= 0.001f)
                    return true;
                return diff.Normalized() == maxV.Normalized() && diff.Length <= maxV.Length;
            }

            public bool IsColinear(FaceEdge other)
            {
                var similarDir = Direction.Equals(other.Direction, 0.001f) || Direction.Equals(other.Direction * -1f, 0.001f);
                return similarDir && (Contains(other.P1) || Contains(other.P2));
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
            public bool IsHard => Edge.IsHard;
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

        class TriangleWrapper
        {
            public Triangle Triangle { get; set; }
            public Plane FacePlane { get; set; }
            public Edge[] TriangleEdges => Triangle.Edges;
            public FaceEdge[] FaceEdges { get; set; }
            public EdgeLine[] PlanarEdges { get; set; }

            public Vertex[] Vertices => Triangle.Vertices;
            public VertexIndex[] Indices => Triangle.Indices;
            public Vector3 Center { get; set; }
            public Vector3 Normal { get; set; }
            public Vector3 PlaneAxisX { get; private set; }

            public TriangleWrapper(Triangle triangle)
            {
                Triangle = triangle;
                Normal = triangle.Normal;
                Center = triangle.GetCenter();
                FacePlane = new Plane(Center, Normal);
                FaceEdges = new FaceEdge[3];
                PlanarEdges = new EdgeLine[3];

                PlaneAxisX = (Vertices[0].Position - Center).Normalized();
            }

            public void Build2DEdges()
            {
                for (int i = 0; i < 3; i++)
                {
                    PlanarEdges[i] = ProjectEdge2D(FaceEdges[i]);
                    PlanarEdges[i].IsTriangleEdge = true;
                }
            }

            public EdgeLine ProjectEdge2D(FaceEdge edge)
            {
                var line = new EdgeLine(edge,
                        FacePlane.ProjectPoint2D(PlaneAxisX, edge.P1),
                        FacePlane.ProjectPoint2D(PlaneAxisX, edge.P2));
                line.Perpendicular = FacePlane.ProjectVector2D(PlaneAxisX, edge.PerpVec);
                return line;
            }

            public bool ContainsEdge(Edge edge)
            {
                return Triangle.ContainsEdge(edge);
            }

            public Vector3 GetRoundedVert(int index)
            {
                return Triangle.Vertices[index].Position.Rounded();
            }

            public FaceEdge[] GetOppositeEdges(int vertexIndex)
            {
                return new FaceEdge[] { FaceEdges[vertexIndex], FaceEdges[(vertexIndex + 2) % 3] };
            }

            public EdgeLine[] GetOppositeEdges2D(int vertexIndex)
            {
                return new EdgeLine[] { PlanarEdges[vertexIndex], PlanarEdges[(vertexIndex + 2) % 3] };
            }
        }

        public static void ComputeEdgeOutlines(IEnumerable<Triangle> triangles, float breakAngle = 35f)
        {
            //outline thickness = 0.013 (world space)

            var triangleList = new List<TriangleWrapper>();
            foreach (var tri in triangles)
                triangleList.Add(new TriangleWrapper(tri));

            var edgeFaces = BuildEdgeFacesDictionary(triangleList);

            var hardEdges = ComputeHardEdges(edgeFaces, breakAngle);

            var edgesPerVert = BuildVertexEdgesDictionary(hardEdges);

            float breakAngleRad = breakAngle / 180f * fPI;

            foreach (var triangle in triangleList)
            {
                triangle.Build2DEdges();

                var connectedEdges = new List<FaceEdge>();

                for (int i = 0; i < 3; i++)
                {
                    if (edgesPerVert.TryGetValue(triangle.GetRoundedVert(i), out List<FaceEdge> list))
                        connectedEdges.AddRange(list);
                }

                //if (triangle.Triangle.ContainsVertex(new Vector3(-0.54105f, 0.358637f, -0.990224f), 0.001f) &&
                //    triangle.Triangle.ContainsVertex(new Vector3(-0.471999f, 0.318679f, -0.990257f), 0.001f) &&
                //    triangle.Triangle.ContainsVertex(new Vector3(-0.54105f, 0.358637f, -1.07021f), 0.001f))
                //{

                //}

                connectedEdges = connectedEdges.Distinct()
                    .Where(x => Vector3.AngleBetween(triangle.Normal, x.FaceNormal) < breakAngleRad)
                    .ToList();

                if (!connectedEdges.Any())
                    continue;

                var projectedEdges = new List<EdgeLine>();

                foreach (var edge in connectedEdges.Where(x => x.Face != triangle.Triangle))
                    projectedEdges.Add(triangle.ProjectEdge2D(edge));

                projectedEdges.AddRange(triangle.PlanarEdges.Where(x => x.IsHard));

                for (int coordPairIdx = 0; coordPairIdx < 3; coordPairIdx++)
                {
                    var idxPos = triangle.Vertices[coordPairIdx].Position;

                    var vertEdges = projectedEdges.Where(x => x.Contains(idxPos))
                        .OrderByDescending(x => x.IsTriangleEdge)
                        .ThenBy(x=> Vector3.AngleBetween(x.Edge.FaceNormal, triangle.Normal))
                        .ToList();
                    
                    RemoveNonIntersectingEdges(triangle, coordPairIdx, vertEdges);
                    RemoveDuplicateEdgeLines(vertEdges);

                    if (vertEdges.Count > 2)
                    {
                        vertEdges = vertEdges.Take(2).ToList();
                    }

                    if (vertEdges.Count == 1)
                    {
                        //if (vertEdges[0].UsedInUnion)
                        //    continue;

                        //var edgeCoords = GetTexCoordsForEdge(triangle, vertEdges[0].Edge);
                        var edgeCoords = ProjectTriangle(triangle, idxPos, vertEdges[0].Edge);
                        vertEdges[0].UsedInUnion = true;

                        for (int j = 0; j < 3; j++)
                        {
                            var reData = triangle.Indices[j].RoundEdgeData;
                            reData.Set(coordPairIdx, edgeCoords[j]);
                        }
                    }
                    else if (vertEdges.Count == 2 )
                    {

                        var pos2D = triangle.FacePlane.ProjectPoint2D(triangle.PlaneAxisX, idxPos);

                        var centerNormal = Vector2.Avg(pos2D + vertEdges[0].Perpendicular, pos2D + vertEdges[1].Perpendicular);
                        centerNormal = (centerNormal - pos2D).Normalized();

                        //if ((pos2D + centerNormal * -1).Length < (pos2D + centerNormal).Length)
                        //{
                        //    centerNormal *= -1f;
                        //}

                        var dir1 = (vertEdges[0].Opposite2D(idxPos) - pos2D).Normalized();
                        var dir2 = (vertEdges[1].Opposite2D(idxPos) - pos2D).Normalized();

                        var totalAngle = Vector2.AngleBetween(centerNormal, dir1) + Vector2.AngleBetween(centerNormal, dir2);

                        bool isObtuse = totalAngle >= fPI;
                        //var edgeCoords1 = GetTexCoordsForEdge(triangle, vertEdges[0].Edge);
                        //var edgeCoords2 = GetTexCoordsForEdge(triangle, vertEdges[1].Edge);
                        var edgeCoords1 = ProjectTriangle(triangle, idxPos, vertEdges[0].Edge);
                        var edgeCoords2 = ProjectTriangle(triangle, idxPos, vertEdges[1].Edge);

                        for (int j = 0; j < 3; j++)
                        {
                            var outlineData = triangle.Indices[j].RoundEdgeData;
                            outlineData.SetCoordsPair(coordPairIdx, edgeCoords1[j], edgeCoords2[j],
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

        private static Dictionary<SimpleEdge, List<FaceEdge>> BuildEdgeFacesDictionary(List<TriangleWrapper> triangleList)
        {
            var edgeFaces = new Dictionary<SimpleEdge, List<FaceEdge>>();

            foreach (var triangle in triangleList)
            {
                Vector3 center = triangle.Triangle.GetCenter();

                for (int i = 0; i < 3; i++)
                {
                    var simpleEdge = new SimpleEdge(triangle.TriangleEdges[i]);

                    if (!edgeFaces.ContainsKey(simpleEdge))
                        edgeFaces.Add(simpleEdge, new List<FaceEdge>());

                    var faceEdge = new FaceEdge(triangle.Triangle, triangle.TriangleEdges[i])
                    {
                        PerpVec = Vector3.GetPerpendicular(simpleEdge.P1, simpleEdge.P2, center)
                    };

                    triangle.FaceEdges[i] = faceEdge;
                    edgeFaces[simpleEdge].Add(faceEdge);

                    triangle.Triangle.Indices[i].RoundEdgeData.Reset();
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
                    kv.Value[0].IsHard = true;
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
                    kv.Value[0].IsHard = true;
                    kv.Value[1].IsHard = true;
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

        private static void RemoveNonIntersectingEdges(TriangleWrapper triangleHelper, int vertIndex, List<EdgeLine> edges)
        {
            var triangle = triangleHelper.Triangle;
            var facePlane = triangleHelper.FacePlane;

            var vertPos = triangleHelper.Vertices[vertIndex].Position;
            var oppEdges = triangleHelper.GetOppositeEdges(vertIndex);
            var oppEdges2D = triangleHelper.GetOppositeEdges2D(vertIndex);

            for (int i = edges.Count - 1; i >= 0; i--)
            {
                var curEdge = edges[i];
                var planarPerp = facePlane.ProjectVector(curEdge.Edge.PerpVec);

                var triCoords = ProjectTriangle(triangleHelper, vertPos, curEdge.Edge);

                if (triCoords.All(x => x.Y <= 0.001f))
                {
                    edges.RemoveAt(i);
                    continue;
                }

                //if ((oppEdges2D[0].IsColinear(curEdge) && !oppEdges2D[0].IsHard) ||
                //    (oppEdges2D[1].IsColinear(curEdge) && !oppEdges2D[1].IsHard))
                //{
                //    edges.RemoveAt(i);
                //    continue;
                //}

                var inter1 = Vector3.GetPerpIntersection(oppEdges[0].P1, oppEdges[0].P2, vertPos + planarPerp * 0.013f);
                var inter2 = Vector3.GetPerpIntersection(oppEdges[1].P1, oppEdges[1].P2, vertPos + planarPerp * 0.013f);

                if (!(oppEdges[0].IsInside(inter1) || oppEdges[1].IsInside(inter2)))
                {
                    edges.RemoveAt(i);
                    continue;
                }

                //if (!curEdge.Edge.IsShared)
                //{
                //    if (!(triangle.ContainsVertex(curEdge.TriangleEdge.P1) ||
                //        triangle.ContainsVertex(curEdge.TriangleEdge.P2)))
                //    {
                //        edges.RemoveAt(i);
                //        continue;
                //    }
                //}
            }
        }

        private static Vector2[] ProjectTriangle(TriangleWrapper triangle, Vector3 planeOrigin, FaceEdge edge)
        {
            var planarPerp = triangle.FacePlane.ProjectVector(edge.PerpVec);
            return ProjectTriangle(triangle, planeOrigin, planarPerp, true);
        }

        private static Vector2[] ProjectTriangle(TriangleWrapper triangle, Vector3 planeOrigin, Vector3 axis, bool inverse)
        {
            Vector2[] coords = new Vector2[3];
            var vertPlane = new Plane(planeOrigin, triangle.Normal);

            for (int i = 0; i < 3; i++)
            {
                coords[i] = vertPlane.ProjectPoint2D(axis, triangle.Vertices[i].Position);
                if (inverse)
                    coords[i] = new Vector2(coords[i].Y, coords[i].X);
            }

            var minX = coords.Min(c => c.X);
            for (int i = 0; i < 3; i++)
                coords[i].X -= minX;

            return coords;
        }

        private static Vector2[] GetTexCoordsForEdge(TriangleWrapper triangle, FaceEdge edge)
        {
            Vector3 axisP1 = edge.P1;
            
            if (edge.IsContained(triangle.Triangle))
                axisP1 = triangle.Triangle.Edges.First(x => edge.Edge == x).P1.Position;
            else
                axisP1 = triangle.Triangle.ContainsVertex(edge.P1) ? edge.P1 : edge.P2;

            axisP1 = triangle.FacePlane.ProjectPoint(axisP1);

            var localPlane = new Plane(axisP1, triangle.Normal);


            Vector3 yAxis = localPlane.ProjectVector(edge.PerpVec); //Flatten perpendicular vector to 2D plane

            Vector2[] coords = new Vector2[3];

            for (int i = 0; i < 3; i++)
            {
                var pos = localPlane.ProjectPoint2D(yAxis, triangle.Vertices[i].Position);
                coords[i] = new Vector2(pos.Y, pos.X);
            }

            var minX = coords.Min(c => c.X);
            for (int i = 0; i < 3; i++)
                coords[i].X -= minX;

            return coords;
        }

    }
}
