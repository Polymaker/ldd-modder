using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class MeshIndexList : IEnumerable<VertexIndex>
    {
        private MeshGeometry Geometry { get; }

        private IList<Triangle> Triangles => Geometry.Triangles;

        private class IndexEnumerator : IEnumerator<VertexIndex>
        {
            private int CurTri = -1;
            private int CurIdx = 0;

            private MeshIndexList List { get; }

            public VertexIndex Current => GetCurrent();

            object IEnumerator.Current => GetCurrent();

            public IndexEnumerator(MeshIndexList list)
            {
                List = list;
            }

            private VertexIndex GetCurrent()
            {
                if (CurTri < 0)
                    return null;
                return List.Geometry.Triangles[CurTri].Indices[CurIdx];
            }

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                if ((CurTri * 3) + CurIdx < List.Count - 1)
                {
                    if (CurTri < 0)
                        CurTri = 0;
                    else
                    {
                        CurIdx = (++CurIdx) % 3;
                        if (CurIdx == 0)
                            CurTri++;
                    }
                    
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                CurTri = -1;
                CurIdx = 0;
            }
        }

        public VertexIndex this[int index]
        {
            get
            {
                int a = index % 3;
                int b = (index - a) / 3;
                return Triangles[b].Indices[a];
            }
            set => throw new NotSupportedException();
        }

        public int Count => Triangles.Count * 3;

        public bool IsReadOnly => true;

        public MeshIndexList(MeshGeometry geometry)
        {
            Geometry = geometry;
        }

        public bool Contains(VertexIndex item)
        {
            return Triangles.Any(t => t.Indices.Contains(item));
        }

        public int IndexOf(VertexIndex item)
        {
            for (int i = 0; i < Triangles.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Triangles[i].Indices[j] == item)
                        return (i * 3) + j;
                }
            }

            return -1;
        }

        

        public IEnumerator<VertexIndex> GetEnumerator()
        {
            return new IndexEnumerator(this);
            //foreach (var triangle in Triangles)
            //{
            //    foreach (var idx in triangle.Indices)
            //        yield return idx;
            //}
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
