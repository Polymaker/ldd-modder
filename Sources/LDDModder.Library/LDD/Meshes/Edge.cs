using System.Collections.Generic;

namespace LDDModder.LDD.Meshes
{
    public class Edge : IEqualityComparer<Edge>
    {
        public Vertex P1 { get; set; }
        public Vertex P2 { get; set; }

        public Edge(Vertex p1, Vertex p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge edge)
                return (edge.P1.Equals(P1) && edge.P2.Equals(P2)) || 
                    (edge.P1.Equals(P2) && edge.P2.Equals(P1));
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 162377905;
            int p1h = P1.GetHashCode();
            int p2h = P2.GetHashCode();
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

        public bool Contains(Vertex vertex)
        {
            return P1.Equals(vertex) || P2.Equals(vertex);
        }

        public bool Equals(Edge x, Edge y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Edge obj)
        {
            return obj.GetHashCode();
        }
    }
}
