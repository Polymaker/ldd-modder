using OpenTK;
using Poly3D.Engine.Meshes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Display.Models
{
    public static class LddMeshLoader
    {
        public static Mesh LoadLddMesh(string filename, bool writeData = false)
        {
            using (var fs = File.OpenRead(filename))
                return LoadLddMesh(fs, writeData);
        }

        public static Mesh LoadLddMesh(Stream stream, bool writeData = false)
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
                int meshType = br.ReadInt32();

                bool isTextured = meshType == 0x3B || meshType == 0X3F;

                for (int i = 0; i < vertexCount; i++)
                    positions.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                for (int i = 0; i < vertexCount; i++)
                    normals.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                if (isTextured)
                {
                    for (int i = 0; i < vertexCount; i++)
                        uvs.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));
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

                if (writeData)
                {
                    string fileName = string.Empty;
                    if (stream is FileStream fs)
                    {
                        fileName = Path.GetFileNameWithoutExtension(fs.Name);
                        if (!fs.Name.EndsWith("g"))
                            fileName += "_" + fs.Name.Last();
                    }

                    int shaderSize = br.ReadInt32();
                    using (var fs2 = File.Open(fileName + "_shader.bin", FileMode.Create))
                    {
                        stream.Seek(-4, SeekOrigin.Current);
                        fs2.Write(br.ReadBytes(4), 0, 4);

                        for (int i = 0; i < shaderSize; i++)
                        {
                            var bytes = br.ReadBytes(4);
                            fs2.Write(bytes, 0, 4);
                        }

                        for (int i = 0; i < indiceCount; i++)
                        {
                            var bytes = br.ReadBytes(4);
                            fs2.Write(bytes, 0, 4);
                        }
                    }

                    
                    Trace.WriteLine(stream.Position.ToString("X2"));
                    
                    using (var fs2 = File.Open(fileName + "_unknown1.bin", FileMode.Create))
                    {

                        int dataSize = br.ReadInt32();
                        stream.Seek(-4, SeekOrigin.Current);
                        fs2.Write(br.ReadBytes(4), 0, 4);

                        for (int i = 0; i < dataSize; i++)
                            fs2.Write(br.ReadBytes(12), 0, 12);
                        for (int i = 0; i < indiceCount; i++)
                            fs2.Write(br.ReadBytes(4), 0, 4);
                    }

                    Trace.WriteLine(stream.Position.ToString("X2"));
                    byte[] buffer = new byte[44];

                    if (meshType == 0x3E || meshType == 0x3F)
                    {
                        using (var fs2 = File.Open(fileName + "_flex.bin", FileMode.Create))
                        using (var bw = new BinaryWriter(fs2))
                        {

                            int dataSize = br.ReadInt32();
                            stream.Seek(-4, SeekOrigin.Current);
                            fs2.Write(br.ReadBytes(4), 0, 4);

                            //int dataRemaining = dataSize;
                            long finalPos = stream.Position + dataSize;

                            for (int i = 0; i < vertexCount; i++)
                            {
                                int pairCount = br.ReadInt32();
                                bw.Write(pairCount);
                                var pairs = new List<Tuple<int, float>>();
                                for(int j = 0; j < pairCount; j++)
                                {
                                    int num = br.ReadInt32();
                                    float val = br.ReadSingle();
                                    pairs.Add(new Tuple<int, float>(num, val));
                                    bw.Write(num);
                                    bw.Write(val);
                                }
                                Trace.WriteLine(string.Join(" ", pairs.Select(p => $"{p.Item1}:{p.Item2.ToString().PadRight(12)}")) + $" sum = {pairs.Sum(p => p.Item2)}");
                                if (stream.Position >= finalPos)
                                    break;
                            }

                            //while (dataRemaining > 0)
                            //{
                            //    int bRead = br.Read(buffer, 0, Math.Min(dataRemaining, buffer.Length));
                            //    Trace.WriteLine(string.Join(" ", buffer.Take(bRead).Select(b => b.ToString("X2"))));
                            //    fs2.Write(buffer, 0, bRead);
                            //    dataRemaining -= bRead;
                            //}
                            //for (int i = 0; i < dataSize; i++)
                            //    fs2.Write(br.ReadBytes(1), 0, 1);
                            //for (int i = 0; i < vertexCount; i++)
                            //    fs2.Write(br.ReadBytes(44), 0, 44);
                            for (int i = 0; i < vertexCount; i++)
                                fs2.Write(br.ReadBytes(4), 0, 4);
                        }
                    }

                    Trace.WriteLine(stream.Position.ToString("X2"));
                    buffer = new byte[32];

                    int variantCount = br.ReadInt32();
                    int variantsSize = br.ReadInt32();

                    for (int i = 0; i < variantCount; i++)
                    {
                        long curPos = stream.Position;
                        int blockSize = br.ReadInt32();
                        int variantType = br.ReadInt32();
                        stream.Position = curPos;

                        using (var fs2 = File.Open(fileName + $"_var{i}_type_{variantType}.bin", FileMode.Create))
                        {
                            int dataRemaining = blockSize;
                            while (dataRemaining > 0)
                            {
                                int bRead = br.Read(buffer, 0, Math.Min(dataRemaining, buffer.Length));
                                fs2.Write(buffer, 0, bRead);
                                dataRemaining -= bRead;
                            }
                        }
                    }
                    Trace.WriteLine(stream.Position.ToString("X2"));


                    //using (var fs2 = File.Open(fileName + "_unknown2.bin", FileMode.Create))
                    //{
                    //    while (stream.Position < stream.Length)
                    //    {
                    //        int bRead = br.Read(buffer, 0, buffer.Length);
                    //        fs2.Write(buffer, 0, bRead);
                    //    }
                    //}

                    //Trace.WriteLine("current offset = " + stream.Position.ToString("X2"));

                    //int shaderSize = br.ReadInt32();
                    //Trace.WriteLine("shaderSize = " + shaderSize);

                    //using (var fs2 = File.Open(fileName + "_shader.bin", FileMode.Create))
                    //{
                    //    for (int i = 0; i < shaderSize; i++)
                    //    {
                    //        var bytes = br.ReadBytes(4);
                    //        fs2.Write(bytes, 0, 4);
                    //    }

                    //    for (int i = 0; i < indiceCount; i++)
                    //    {
                    //        var bytes = br.ReadBytes(4);
                    //        fs2.Write(bytes, 0, 4);
                    //    }
                    //}
                    //Trace.WriteLine("current offset = " + stream.Position.ToString("X2"));
                    //int dataSize = br.ReadInt32();
                    //Trace.WriteLine("dataSize 1 = " + dataSize);

                    //using (var fs2 = File.Open(fileName + "_data1.bin", FileMode.Create))
                    //{
                    //    for (int i = 0; i < dataSize; i++)
                    //    {
                    //        var bytes = br.ReadBytes(12);
                    //        fs2.Write(bytes, 0, 12);
                    //    }

                    //    for (int i = 0; i < indiceCount; i++)
                    //    {
                    //        var bytes = br.ReadBytes(4);
                    //        fs2.Write(bytes, 0, 4);
                    //    }
                    //}


                    //dataSize = br.ReadInt32();
                    //Trace.WriteLine("dataSize 2 = " + dataSize);

                    //dataSize = br.ReadInt32();
                    //Trace.WriteLine("dataSize 3 = " + dataSize);

                    //using (var fs2 = File.Open(fileName + "_data4.bin", FileMode.Create))
                    //{
                    //    var bytes = br.ReadBytes(20);
                    //    fs2.Write(bytes, 0, 20);
                    //}
                }
                
            }

            if (triangles.Count > 0)
                return new Mesh(triangles);

            return null;
        }

    }
}
