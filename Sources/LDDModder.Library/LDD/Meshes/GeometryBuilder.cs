using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class GeometryBuilder
    {
        private Dictionary<int, List<Vertex>> VertexDict;
        private List<Triangle> _Triangles;
        private List<Vertex> _Vertices;

        public IList<Vertex> Vertices => _Vertices.AsReadOnly();
        public IList<Triangle> Triangles => _Triangles.AsReadOnly();

        public GeometryBuilder()
        {
            VertexDict = new Dictionary<int, List<Vertex>>();
            _Triangles = new List<Triangle>();
            _Vertices = new List<Vertex>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="checkDuplicate">Slightly improve performences when turned off.</param>
        /// <returns></returns>
        public Vertex AddVertex(Vertex vertex, bool checkDuplicate = true)
        {
            var vh = vertex.GetHash();

            if (VertexDict.ContainsKey(vh))
            {
                if (checkDuplicate)
                {
                    var existing = VertexDict[vh].FirstOrDefault(x => x.Equals(vertex));
                    if (existing != null)
                        return existing;
                }
            }
            else
                VertexDict.Add(vh, new List<Vertex>());

            VertexDict[vh].Add(vertex);
            _Vertices.Add(vertex);
            return vertex;
        }

        public void AddTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            _Triangles.Add(new Triangle(AddVertex(v1), AddVertex(v2), AddVertex(v3)));
        }

        public void AddTriangle(int v1, int v2, int v3)
        {
            _Triangles.Add(new Triangle(_Vertices[v1], _Vertices[v2], _Vertices[v3]));
        }

        //??? I don't rembemer what I wanted to do with this
        //public void SortVertices()
        //{
        //    var orderDict = new Dictionary<Vertex, int>();
        //    int vI = 0;

        //    foreach (var tri in Triangles)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            if (!orderDict.ContainsKey(tri.Vertices[i]))
        //                orderDict.Add(tri.Vertices[i], vI++);
        //        }
        //    }

        //    _Vertices.Clear();
        //    _Vertices.AddRange(orderDict.OrderBy(kv => kv.Value).Select(kv => kv.Key));
        //}

        public MeshGeometry GetGeometry()
        {
            var geom = new MeshGeometry();
            var verts = VertexDict.Values.SelectMany(x => x);
            geom.SetVertices(verts);
            geom.SetTriangles(_Triangles);
            return geom;
        }
    }
}
