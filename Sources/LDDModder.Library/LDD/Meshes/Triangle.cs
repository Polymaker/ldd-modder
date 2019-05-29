using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Triangle
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }
        public Vertex V3 { get; set; }

        public Vertex[] Vertices => new Vertex[] { V1, V2, V3 };

        public Edge[] Edges { get; private set; }

        public Vector3 Normal { get; private set; }

        public Triangle()
        {

        }

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            Edges = new Edge[]
            {
                new Edge(v1,v2),
                new Edge(v2,v3),
                new Edge(v3,v1)
            };

            CalculateNormal();
        }

        public void ReassignVertices(Vertex v1, Vertex v2, Vertex v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            Edges = new Edge[]
            {
                new Edge(v1,v2),
                new Edge(v2,v3),
                new Edge(v3,v1)
            };
            CalculateNormal();
        }

        public void CalculateNormal()
        {
            var u = V2.Position - V1.Position;
            var v = V3.Position - V1.Position;
            var n = new Vector3((u.Y * v.Z) - (u.Z * v.Y), (u.Z * v.X) - (u.X * v.Z), (u.X * v.Y) - (u.Y * v.X));
            n.Normalize();
            Normal = n;
        }

        public Vector3 GetCenter()
        {
            return (V1.Position + V2.Position + V3.Position) / 3;
        }

        public bool ContainsVertex(Vector3 pos)
        {
            return Vertices.Any(x => x.Position.Equals(pos));
        }

        public Vector3 GetEdgePerp(int edgeIndex)
        {
            var first = Vertices[edgeIndex];
            var prev = Vertices[(3 + edgeIndex - 1) % 3];
            var next = Vertices[(edgeIndex + 1) % 3];

            var dir1 = (next.Position - first.Position).Normalized();
            var dir2 = (prev.Position - first.Position).Normalized();
            var c = Vector3.Cross(dir1, dir2).Normalized();
            var perp = Vector3.Cross(c, dir1).Normalized();
            var d1 = Vector3.Distance(dir2, perp);
            var d2 = Vector3.Distance(dir2, perp * -1);
            if (d2 < d1)
                perp *= -1;
            return perp;
        }

        public Vector2[] GetPlanarCoordsRelativeToEdge(int edgeIndex)
        {
            var coords = new Vector2[3];
            var xAxis2D = (Edges[edgeIndex].P2.Position - Edges[edgeIndex].P1.Position).Normalized();
            var yAxis2D = GetEdgePerp(edgeIndex);
            var center = GetCenter();

            for (int i = 0; i < 3; i++)
            {
                coords[i] = Vector3.ProjectToPlane2D(Vertices[i].Position, center, Normal, xAxis2D, yAxis2D);
            }
            int oppIdx = (edgeIndex + 2) % 3;
            var minCoord = coords[oppIdx];// Vector2.Min(coords);
            for (int i = 0; i < 3; i++)
                coords[i] -= minCoord;
            return coords;
        }

        public Vector2[] GetPlanarCoordsRelativeToEdge(int edgeIndex, int centerCoordIdx)
        {
            var coords = new Vector2[3];
            var xAxis2D = (Edges[edgeIndex].P2.Position - Edges[edgeIndex].P1.Position).Normalized();
            var yAxis2D = GetEdgePerp(edgeIndex);
            var center = GetCenter();

            for (int i = 0; i < 3; i++)
            {
                coords[i] = Vector3.ProjectToPlane2D(Vertices[i].Position, center, Normal, xAxis2D, yAxis2D);
            }

            var centerCoord = coords[centerCoordIdx];
            for (int i = 0; i < 3; i++)
                coords[i] -= centerCoord;
            return coords;
        }

        //public bool ContainsVertex(Vector3[] pos)
        //{
        //    return Vertices.Any(x => pos.Any(y => y.Equals(x.Position)));
        //}

        //public bool ContainsVertex(Vertex[] verts)
        //{
        //    return Vertices.Any(x => verts.Any(y => y.Position.Equals(x.Position)));
        //}
    }
}
