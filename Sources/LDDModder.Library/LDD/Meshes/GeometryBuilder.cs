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

        public int VertexCount => Vertices.Count;

        public int IndexCount => Triangles.Count * 3;

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

        public Triangle AddTriangle(int v1, int v2, int v3)
        {
            var newTriangle = new Triangle(_Vertices[v1], _Vertices[v2], _Vertices[v3]);
            _Triangles.Add(newTriangle);
            return newTriangle;
        }

        public void AddTriangle(Triangle triangle, bool checkDuplicate = false)
        {
            var newTriangle = new Triangle(
                AddVertex(triangle.V1, checkDuplicate),
                AddVertex(triangle.V2, checkDuplicate),
                AddVertex(triangle.V3, checkDuplicate));

            for (int i = 0; i < 3; i++)
            {
                newTriangle.Indices[i].AverageNormal = triangle.Indices[i].AverageNormal;
                newTriangle.Indices[i].RoundEdgeData = triangle.Indices[i].RoundEdgeData.Clone();
            }

            _Triangles.Add(newTriangle);
        }

        public void CombineGeometry(MeshGeometry geometry)
        {
            var triangleIndices = geometry.GetTriangleIndices();
            var addedVertices = geometry.Vertices.Select(x => x.Clone()).ToList();
            
            int vertexOffset = VertexCount;
            addedVertices.ForEach(x => AddVertex(x, false));

            for (int i = 0; i < geometry.IndexCount; i += 3)
            {
                var triangle = AddTriangle(
                    triangleIndices[i] + vertexOffset,
                    triangleIndices[i + 1] + vertexOffset,
                    triangleIndices[i + 2] + vertexOffset);

                triangle.CopyIndexData(geometry.Triangles[i / 3]);
            }
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
            //var verts = VertexDict.Values.SelectMany(x => x);
            geom.SetVertices(_Vertices);
            geom.SetTriangles(_Triangles);
            return geom;
        }
    }
}
