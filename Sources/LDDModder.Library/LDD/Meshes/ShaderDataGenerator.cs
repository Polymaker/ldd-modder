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

        struct FaceEdge : IEquatable<FaceEdge>
        {
            public Vector3 P1;
            public Vector3 P2;
            public Vector3 Normal;

            public Vector3 Direction => (P2 - P1).Normalized();

            public FaceEdge(Vector3 p1, Vector3 p2, Vector3 normal)
            {
                P1 = p1;
                P2 = p2;
                Normal = normal;
            }

            public bool Equals(FaceEdge other)
            {
                return (
                        (other.P1.Equals(P1) && other.P2.Equals(P2)) || 
                        (other.P1.Equals(P2) && other.P2.Equals(P1))
                    ) && other.Normal.Equals(Normal);
            }

            public override bool Equals(object obj)
            {
                if (obj is FaceEdge fe)
                    return ((fe.P1.Equals(P1) && fe.P2.Equals(P2)) || (fe.P1.Equals(P2) && fe.P2.Equals(P1))) && fe.Normal.Equals(Normal);
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

                hashCode = hashCode * -1521134295 + Normal.GetHashCode();

                return hashCode;
            }

        }

        public const float fPI = (float)Math.PI;

        public static void ComputeEdgeOutlines(IEnumerable<Triangle> triangles, float breakAngle = 35f)
        {
            float edgeWidthRatio = 15.5f / 0.8f;

            var triangleList = triangles is List<Triangle> ? (List<Triangle>)triangles : triangles.ToList();
            Edge.CompareByPosition = true;
            var edgeFaces = new Dictionary<Edge, List<Triangle>>();

            foreach (var tri in triangleList)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!edgeFaces.ContainsKey(tri.Edges[i]))
                        edgeFaces.Add(tri.Edges[i], new List<Triangle>());
                    edgeFaces[tri.Edges[i]].Add(tri);
                }
            }

            //var hardEdges = new List<Edge>();
            var hardEdges2 = new List<FaceEdge>();
            foreach (var kv in edgeFaces)
            {
                if (kv.Value.Count == 1)
                {
                    //hardEdges.Add(kv.Key);
                    hardEdges2.Add(new FaceEdge(kv.Key.P1.Position, kv.Key.P2.Position, kv.Value[0].Normal));
                }

                if (kv.Value.Count > 2)
                    continue;
                var n1 = kv.Value[0].Normal;
                var n2 = kv.Value[1].Normal;

                if (n1.Equals(n2))
                    continue;

                var angleDiff = Vector3.AngleBetween(kv.Value[0].Normal, kv.Value[1].Normal);

                if (float.IsNaN(angleDiff) || float.IsInfinity(angleDiff))
                    continue;
                if (angleDiff < 0)
                    angleDiff = (fPI * 2f + angleDiff) % (fPI * 2f);

                angleDiff = angleDiff / (fPI * 2f) * 360f;

                if (angleDiff >= breakAngle)
                {
                    //hardEdges.Add(kv.Key);
                    hardEdges2.Add(new FaceEdge(kv.Key.P1.Position, kv.Key.P2.Position, kv.Value[0].Normal));
                    hardEdges2.Add(new FaceEdge(kv.Key.P1.Position, kv.Key.P2.Position, kv.Value[1].Normal));
                }
            }

            foreach (var tri in triangleList)
            {
                var nearEdges = hardEdges2.Where(x => 
                    (tri.ContainsVertex(x.P1) || tri.ContainsVertex(x.P2)) && 
                    Vector3.AngleBetween(tri.Normal, x.Normal) < fPI * 0.4f);

                if (!nearEdges.Any())
                    continue;

                int pairCount = 0;
                var directEdges = nearEdges.Where(x => x.IsContained(tri));

                foreach (var edge in directEdges)
                {
                    var edgeCoords = GetEdgeCoords(tri, edge);
                    for (int i = 0; i < 3; i++)
                    {
                        var reData = tri.Indices[i].RoundEdgeData;
                        reData.Coords[1 + (pairCount * 2)] = edgeCoords[i] * edgeWidthRatio;
                        reData.Coords[1 + (pairCount * 2)].X += 100;
                        reData.Coords[1 + (pairCount * 2)].X *= -1;
                    }
                    pairCount++;
                }
            }
        }

        private static Vector2[] GetEdgeCoords(Triangle triangle, FaceEdge edge)
        {
            var plane = new Plane(triangle.V1.Position, triangle.Normal);

            Vector3 axisP1 = edge.P1;
            
            //Vector3 
            if (edge.IsContained(triangle))
            {
                axisP1 = triangle.Edges.First(x => edge.Equals(x)).P1.Position;
            }
            else
            {
                axisP1 = triangle.ContainsVertex(edge.P1) ? edge.P1 : edge.P2;
            }

            Vector3 axisP2 = axisP1.Equals(edge.P1) ? edge.P2 : edge.P1;
            axisP1 = plane.ProjectPoint(axisP1);
            axisP2 = plane.ProjectPoint(axisP2);
            plane.Origin = axisP1;

            Vector3 xAxis = (axisP2 - axisP1).Normalized();
            //if (!edge.P1.Equals(axisP1))
            //    xAxis *= -1;

            Vector2[] coords = new Vector2[3];

            for (int i = 0; i < 3; i++)
                coords[i] = plane.ProjectPoint2D(xAxis, triangle.Vertices[i].Position);

            var minX = coords.Min(c => c.X);
            for (int i = 0; i < 3; i++)
                coords[i].X -= minX;

            if (coords.Count(x => x.Y < 0) > 1)
            {
                //for (int i = 0; i < 3; i++)
                //    coords[i].Y *= -1;
            }

            return coords;
        }
    }
}
