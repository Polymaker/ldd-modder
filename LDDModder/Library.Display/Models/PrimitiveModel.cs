using OpenTK;
using Poly3D.Engine.Meshes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Models
{
    public class PrimitiveModel
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<Vector3> Normals { get; set; } = new List<Vector3>();
        public List<Vector2> UVs { get; set; } = new List<Vector2>();
        public List<Tuple<int, int, int>> Indices { get; set; } = new List<Tuple<int, int, int>>();
        public bool IsTextured => UVs.Any();
        public Mesh Mesh { get; set; }

        public static PrimitiveModel LoadPrimitiveMesh(string filename, bool writeData = false)
        {
            using (var fs = File.OpenRead(filename))
                return LoadPrimitiveMesh(fs, writeData);
        }

        public static PrimitiveModel LoadPrimitiveMesh(Stream stream, bool writeData = false)
        {
            var model = new PrimitiveModel();
            var vertices = new List<Vertex>();
            var triangles = new List<FaceTriangle>();

            using (var br = new BinaryReader(stream))
            {
                stream.Seek(4, SeekOrigin.Begin);
                int vertexCount = br.ReadInt32();
                int indiceCount = br.ReadInt32();
                int triangleCount = indiceCount / 3;
                int meshType = br.ReadInt32();

                bool isTextured = meshType == 0x3B || meshType == 0X3F;

                for (int i = 0; i < vertexCount; i++)
                    model.Vertices.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                for (int i = 0; i < vertexCount; i++)
                    model.Normals.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                if (isTextured)
                {
                    for (int i = 0; i < vertexCount; i++)
                        model.UVs.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));
                }

                for (int i = 0; i < vertexCount; i++)
                    vertices.Add(new Vertex(model.Vertices[i], model.Normals[i], isTextured ? model.UVs[i] : (Vector2?)null));

                for (int i = 0; i < triangleCount; i++)
                {
                    int idx0 = br.ReadInt32();
                    int idx1 = br.ReadInt32();
                    int idx2 = br.ReadInt32();
                    model.Indices.Add(new Tuple<int, int, int>(idx0, idx1, idx2));
                    triangles.Add(new FaceTriangle(vertices[idx0], vertices[idx1], vertices[idx2]));
                }
            }

            if (triangles.Count > 0)
                model.Mesh = new Mesh(triangles);

            return model;
        }
    }
}
