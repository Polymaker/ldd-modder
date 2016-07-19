using OpenTK;
using Poly3D.Engine.Meshes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Models
{
    public static class LddMeshLoader
    {
        public static Mesh LoadLddMesh(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return LoadLddMesh(fs);
        }

        public static Mesh LoadLddMesh(Stream stream)
        {
            var positions = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var vertices = new List<Vertex>();
            var triangles = new List<FaceTriangle>();

            using (var br = new BinaryReader(stream))
            {
                stream.Seek(4, SeekOrigin.Begin);
                int vertexCount = br.ReadInt32();
                int indiceCount = br.ReadInt32();
                int triangleCount = indiceCount / 3;
                bool isTextured = br.ReadInt32() == 59;

                if (isTextured)
                {
                    for (int i = 0; i < vertexCount; i++)
                        positions.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                    for (int i = 0; i < vertexCount; i++)
                        normals.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                    for (int i = 0; i < vertexCount; i++)
                        uvs.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));
                }
                else
                {
                    for (int i = 0; i < vertexCount; i++)
                        positions.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                    for (int i = 0; i < vertexCount; i++)
                        normals.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
                }

                for (int i = 0; i < vertexCount; i++)
                    vertices.Add(new Vertex(positions[i], normals[i], isTextured ? uvs[i] : (Vector2?)null));

                for (int i = 0; i < triangleCount; i++)
                {
                    int idx0 = br.ReadInt32();
                    int idx1 = br.ReadInt32();
                    int idx2 = br.ReadInt32();
                    triangles.Add(new FaceTriangle(vertices[idx0], vertices[idx1], vertices[idx2]));
                }
            }

            if (triangles.Count > 0)
                return new Mesh(triangles);

            return null;
        }

    }
}
