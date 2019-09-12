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

        public static void ComputeEdgeOutlines(IEnumerable<Triangle> triangles, float breakAngle = 35f)
        {
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

            var hardEdges = new List<Edge>();

            foreach(var kv in edgeFaces)
            {
                if (kv.Value.Count != 2)
                    continue;

                var n1 = kv.Value[0].Normal;
                var n2 = kv.Value[1].Normal;

                if (n1.Equals(n2))
                    continue;

                var angleDiff = Vector3.AngleBetween(kv.Value[0].Normal, kv.Value[1].Normal);

                if (float.IsNaN(angleDiff) || float.IsInfinity(angleDiff))
                    continue;
                if (angleDiff < 0)
                    angleDiff = (float)((Math.PI * 2f + angleDiff) % Math.PI * 2f);

                angleDiff = angleDiff / ((float)Math.PI * 2f) * 360f;

                if (angleDiff >= breakAngle)
                    hardEdges.Add(kv.Key);
            }

            foreach (var tri in triangleList)
            {
                var triEdges = hardEdges.Where(x => tri.ContainsEdge(x, true));
                if (!triEdges.Any())
                    continue;


            }
        }
    }
}
