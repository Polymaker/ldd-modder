using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class Mesh
    {
        public MeshType Type { get; set; }

        public Vertex[] Vertices { get; set; }

        public IndexReference[] Indices { get; set; }

        public RoundEdgeData[] EdgeShaderValues { get; set; }

        public Vector3[] AverageNormals { get; set; }

        public CullingInfo[] CullingInfos { get; set; }

        public Vertex GetIndexVertex(int index)
        {
            return Vertices[Indices[index].VertexIndex];
        }

        public int ShaderOffsetToIndex(int offset)
        {
            for (int i = 0; i < EdgeShaderValues.Length; i++)
            {
                if (EdgeShaderValues[i].FileOffset == offset)
                    return i;
            }
            return -1;
        }

        public void UpdateEdgeShaderOffsets()
        {
            int currentOffset = 0;

            for (int i = 0; i < EdgeShaderValues.Length; i++)
            {
                EdgeShaderValues[i].FileOffset = currentOffset;
                currentOffset += EdgeShaderValues[i].Coords.Length * 2;
            }
        }

        public IndexReference[] GetTriangleIndices(int triangleIndex)
        {
            return new IndexReference[] 
            {
                Indices[(triangleIndex * 3) + 0],
                Indices[(triangleIndex * 3) + 1],
                Indices[(triangleIndex * 3) + 2]
            };
        }

        public Vertex[] GetTriangleVertices(int triangleIndex)
        {
            var indices = GetTriangleIndices(triangleIndex);
            return new Vertex[] 
            { 
                Vertices[indices[0].VertexIndex],
                Vertices[indices[1].VertexIndex],
                Vertices[indices[2].VertexIndex]
            };
        }

        public Vector3 GetTriangleNormal(int triangleIndex)
        {
            var verts = GetTriangleVertices(triangleIndex);
            var u = verts[1].Position - verts[0].Position;
            var v = verts[2].Position - verts[0].Position;
            var n = new Vector3((u.Y * v.Z) - (u.Z * v.Y), (u.Z * v.X) - (u.X * v.Z), (u.X * v.Y) - (u.Y * v.X));
            n.Normalize();
            return n;
        }

        public void GenerateAverageNormals()
        {
            var indicesNormals = new Dictionary<int, Vector3>();

            IEnumerable<int> GetRelatedFaces(Vector3 v)
            {
                for (int i = 0; i < Indices.Length / 3; i++)
                {
                    var verts = GetTriangleVertices(i);
                    if (verts.Any(x => x.Position.Equals(v)))
                        yield return i;
                }
            }

            for (int i = 0; i < Indices.Length; i++)
            {
                var vert = Vertices[Indices[i].VertexIndex];
                var faces = GetRelatedFaces(vert.Position).ToList();
                var faceNorms = faces.Select(x => GetTriangleNormal(x)).Distinct().ToList();

                Vector3 norm = Vector3.Zero;
                foreach (var n in faceNorms)
                    norm += n;

                norm /= faces.Count;
                norm.Normalize();
                indicesNormals.Add(i, norm);
            }

            var distincNormals = indicesNormals.Values.Distinct().ToList();
            
            for (int i = 0; i < Indices.Length; i++)
            {
                var idxNorm = indicesNormals[i];
                for (int j = 0; j < distincNormals.Count; j++)
                {
                    if (distincNormals[j] == idxNorm)
                    {
                        Indices[i].AverageNormalIndex = j;
                        break;
                    }
                }
            }

            //distincNormals.Insert(0, new Vector3(83, 0, 0));
            AverageNormals = distincNormals.ToArray();
        }

        public List<Triangle> CalculateTriangles()
        {
            var triangles = new List<Triangle>();

            for (int i = 0; i < Indices.Length; i += 3)
            {
                triangles.Add(new Triangle(GetIndexVertex(i), GetIndexVertex(i + 1), GetIndexVertex(i + 2)));
            }

            return triangles;
        }

        public List<Edge> CalculateBoundaryEdges()
        {
            var triangles = new List<Triangle>();

            var edges = new List<Edge>();
            var boundaryEdges = new List<Edge>();
            for (int i = 0; i < Indices.Length; i += 3)
            {
                triangles.Add(new Triangle(GetIndexVertex(i), GetIndexVertex(i + 1), GetIndexVertex(i + 2)));

                //edges.Add(new Edge(GetIndexVertex(i), GetIndexVertex(i + 1)));
                //edges.Add(new Edge(GetIndexVertex(i + 1), GetIndexVertex(i + 2)));
                //edges.Add(new Edge(GetIndexVertex(i + 2), GetIndexVertex(i)));
            }
            edges.AddRange(triangles.SelectMany(t => t.Edges).Distinct());
            foreach (var edge in edges)
            {
                var connectedTriangles = triangles.Where(t => t.Edges.Contains(edge));
                if (connectedTriangles.Count() == 1)
                    boundaryEdges.Add(edge);
            }
            //edges = edges.Distinct().ToList();
            return boundaryEdges;
        }


    }
}
