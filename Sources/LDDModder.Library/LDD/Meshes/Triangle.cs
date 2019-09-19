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
        public VertexIndex[] Indices { get; private set; }

        public Vertex[] Vertices { get; private set; }

        public Vertex V1 => Vertices[0];

        public Vertex V2 => Vertices[1];

        public Vertex V3 => Vertices[2];

        public Edge[] Edges { get; private set; }

        public Vector3 Normal { get; private set; }

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            Indices = new VertexIndex[]
            {
                new VertexIndex(v1),
                new VertexIndex(v2),
                new VertexIndex(v3)
            };
            Edges = new Edge[]
            {
                new Edge(v1,v2),
                new Edge(v2,v3),
                new Edge(v3,v1)
            };
            Vertices = new Vertex[] { Indices[0].Vertex, Indices[1].Vertex, Indices[2].Vertex };
            CalculateNormal();
        }

        public void ReassignVertices(Vertex v1, Vertex v2, Vertex v3)
        {
            Indices = new VertexIndex[]
            {
                new VertexIndex(v1),
                new VertexIndex(v2),
                new VertexIndex(v3)
            };
            Edges = new Edge[]
            {
                new Edge(v1,v2),
                new Edge(v2,v3),
                new Edge(v3,v1)
            };
            Vertices = new Vertex[] { Indices[0].Vertex, Indices[1].Vertex, Indices[2].Vertex };
            CalculateNormal();
        }

        internal void RebuildEdges()
        {
            Edges = new Edge[]
            {
                new Edge(Vertices[0],Vertices[1]),
                new Edge(Vertices[1],Vertices[2]),
                new Edge(Vertices[2],Vertices[0])
            };
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

        public bool ContainsVertex(Vertex vertex)
        {
            return Indices[0].Vertex == vertex || Indices[1].Vertex == vertex || Indices[2].Vertex == vertex;
        }

        public bool ContainsVertex(Vector3 pos)
        {
            return Vertices.Any(x => x.Position.Equals(pos));
        }

        public bool ContainsEdge(Edge edge, bool compareByPos = false)
        {
            if (compareByPos)
                return ContainsVertex(edge.P1.Position) && ContainsVertex(edge.P2.Position);
            return Edges.Any(x => x.Equals(edge, compareByPos));
        }

        public bool ShareEdge(Triangle other, bool onlyByPos)
        {
            for (int i = 0; i < 3; i++)
            {
                if (other.ContainsEdge(Edges[i], onlyByPos))
                    return true;
            }
            return false;
        }

        public Edge[] GetVerticeEdges(int vertexIndex)
        {
            return new Edge[] { Edges[vertexIndex], Edges[(vertexIndex + 2) % 3] };
        }
    }
}
