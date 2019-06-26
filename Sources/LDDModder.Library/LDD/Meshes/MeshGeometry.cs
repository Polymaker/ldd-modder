using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class MeshGeometry
    {
        private List<Vertex> _Vertices;
        private List<Triangle2> _Triangles;
        private IndexList _Indices;
        public IList<Vertex> Vertices => _Vertices.AsReadOnly();

        public IList<Triangle2> Triangles => _Triangles.AsReadOnly();

        private class IndexList : IList<VertexIndex>
        {
            private readonly MeshGeometry Geometry;

            private IList<Triangle2> Triangles => Geometry.Triangles;

            public IndexList(MeshGeometry geometry)
            {
                Geometry = geometry;
            }

            private int GetTriangleIndex(int index)
            {
                return (int)Math.Floor(index / 3d);
            }

            public VertexIndex this[int index]
            {
                get => Triangles[GetTriangleIndex(index)].Indices[index % 3];
                set => throw new NotSupportedException();
            }

            public int Count => Triangles.Count * 3;

            public bool IsReadOnly => true;

            public void Add(VertexIndex item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(VertexIndex item)
            {
                return Triangles.Any(t => t.Indices.Contains(item));
            }

            public void CopyTo(VertexIndex[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<VertexIndex> GetEnumerator()
            {
                foreach(var triangle in Triangles)
                {
                    foreach (var idx in triangle.Indices)
                        yield return idx;
                }
            }

            public int IndexOf(VertexIndex item)
            {
                for (int i = 0; i < Triangles.Count; i++)
                {
                    for (int j = 0; j< 3; j++)
                    {
                        if (Triangles[i].Indices[j] == item)
                            return (i * 3) + j;
                    }
                }

                return -1;
            }

            public void Insert(int index, VertexIndex item)
            {
                throw new NotSupportedException();
            }

            public bool Remove(VertexIndex item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public IList<VertexIndex> Indices => _Indices;

        public MeshGeometry()
        {
            _Vertices = new List<Vertex>();
            _Triangles = new List<Triangle2>();
            _Indices = new IndexList(this);
        }

        private Vertex AddVertex(Vertex vertex)
        {
            var existing = _Vertices.FirstOrDefault(x => x.Equals(vertex));

            if (existing == null)
            {
                _Vertices.Add(vertex);
                return vertex;
            }

            return existing;
        }

        public void SetVertices(IEnumerable<Vertex> vertices)
        {
            _Vertices.Clear();
            _Vertices.AddRange(vertices);
        }

        public void AddTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            _Triangles.Add(new Triangle2(AddVertex(v1), AddVertex(v2), AddVertex(v3)));
        }

        public void AddTriangleFromIndices(int i1, int i2, int i3)
        {
            if (i1 >= _Vertices.Count)
                throw new IndexOutOfRangeException("i1");
            if (i2 >= _Vertices.Count)
                throw new IndexOutOfRangeException("i2");
            if (i3 >= _Vertices.Count)
                throw new IndexOutOfRangeException("i3");
            _Triangles.Add(new Triangle2(_Vertices[i1], _Vertices[i2], _Vertices[i3]));
        }
    }
}
