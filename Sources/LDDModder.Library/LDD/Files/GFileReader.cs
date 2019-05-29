using LDDModder.LDD.Meshes;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files
{
    public class GFileReader
    {
        public static Mesh Read(Stream stream)
        {
            stream.Position = 0;

            using (var br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var header = new string(br.ReadChars(4));
                if (header != "10GB")
                    throw new IOException("The file is not a LDD mesh file (*.g)");
                
                int vertexCount = br.ReadInt32();
                int indexCount = br.ReadInt32();
                var meshType = (MeshType)br.ReadInt32();

                var mesh = new Mesh()
                {
                    Type = meshType,
                    Vertices = new Vertex[vertexCount],
                    Indices = new IndexReference[indexCount]
                };

                // Vertices & triangles
                {
                    var positions = new List<Vector3>();
                    var normals = new List<Vector3>();
                    var uvs = new List<Vector2>();

                    for (int i = 0; i < vertexCount; i++)
                        positions.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                    for (int i = 0; i < vertexCount; i++)
                        normals.Add(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));

                    if (meshType == MeshType.StandardTextured || meshType == MeshType.FlexibleTextured)
                        for (int i = 0; i < vertexCount; i++)
                            uvs.Add(new Vector2(br.ReadSingle(), br.ReadSingle()));

                    for (int i = 0; i < vertexCount; i++)
                    {
                        if (meshType == MeshType.StandardTextured || meshType == MeshType.FlexibleTextured)
                            mesh.Vertices[i] = new Vertex(positions[i], normals[i], uvs[i]);
                        else
                            mesh.Vertices[i] = new Vertex(positions[i], normals[i]);
                    }

                    for (int i = 0; i < indexCount; i++)
                        mesh.Indices[i] = new IndexReference(br.ReadInt32());
                }
                
                // Edge shader data (brick outlines)
                {
                    int shaderDataLength = br.ReadInt32();
                    var shaderData = new Dictionary<int, RoundEdgeData>();

                    int itemCtr = 0;
                    while(itemCtr < shaderDataLength)
                    {
                        bool endOfRow = 256 - ((itemCtr + 12) % 256) < 12;
                        int valueCount = endOfRow ? 16 : 12;
                        int remainingData = shaderDataLength - itemCtr;
                        if (valueCount > remainingData)
                        {
                            if (endOfRow && 12 <= remainingData)
                                valueCount = 12;
                            else
                            {
                                Trace.WriteLine("Shader data length error!!!");
                                stream.Skip(remainingData * 4);
                                break;
                            }
                        }
                        shaderData.Add(itemCtr, new RoundEdgeData(itemCtr, br.ReadSingles(valueCount)));
                        itemCtr += valueCount;
                    }

                    mesh.EdgeShaderValues = shaderData.Values.ToArray();
                    var offsetList = shaderData.Keys.ToList();
                    for (int i = 0; i < indexCount; i++)
                    {
                        mesh.Indices[i].RoundEdgeDataOffset = br.ReadInt32();
                        mesh.Indices[i].ShaderDataIndex = offsetList.IndexOf(mesh.Indices[i].RoundEdgeDataOffset);
                    }
                }

                // Unknown Data
                {
                    
                    int unknownDataLength = br.ReadInt32();
                    mesh.AverageNormals = new Vector3[unknownDataLength - 1];

                    //we skip the first item because it looks like an header and the maximum referenced value seems always one less the specified length
                    var dataHeader = new Vector3(br.ReadSingles(3));
                    if (dataHeader.X != 83 || dataHeader.Y != 0 || dataHeader.Z != 0)
                        Trace.WriteLine($"Unexpected header: {dataHeader}");

                    for (int i = 0; i < unknownDataLength - 1; i++)
                        mesh.AverageNormals[i] = new Vector3(br.ReadSingles(3));

                    int hasInvalidIndex = 0;
                    long firstOccurence = 0;
                    long unknownDataPos = br.BaseStream.Position;
                    for (int i = 0; i < indexCount; i++)
                    {
                        int dataIndex = br.ReadInt32();
                        if (dataIndex < 0 || dataIndex + 1 >= unknownDataLength)
                        {
                            if (firstOccurence == 0)
                            {
                                firstOccurence = stream.Position - 4;
                                //Trace.WriteLine($"Max value {unknownDataLength - 1} actual value: {dataIndex}");
                            }
                            //Trace.WriteLine($"Invalid unknown data index: {dataIndex}");
                            hasInvalidIndex++;
                        }
                        mesh.Indices[i].AverageNormalIndex = dataIndex;
                    }

                    if (hasInvalidIndex > 0)
                        Trace.WriteLine($"Mesh has {hasInvalidIndex} invalid data indices. Data starts at {unknownDataPos:X4}, invalid data at: {firstOccurence:X4}");
                }

                // Flex data
                if (meshType == MeshType.Flexible || meshType == MeshType.FlexibleTextured)
                {
                    int dataSize = br.ReadInt32();
                    long startPos = stream.Position;
                    stream.Seek(dataSize, SeekOrigin.Current);
                    var dataOffsets = new List<int>();
                    for (int i = 0; i < vertexCount; i++)
                        dataOffsets.Add(br.ReadInt32());
                    long dataEndPosition = stream.Position;

                    for (int i = 0; i < vertexCount; i++)
                    {
                        stream.Position = startPos + dataOffsets[i];
                        int boneCount = br.ReadInt32();
                        var bones = new List<BoneWeight>();
                        for (int j = 0; j < boneCount; j++)
                            bones.Add(new BoneWeight(br.ReadInt32(), br.ReadSingle()));
                    }

                    stream.Position = dataEndPosition;
                }

                int cullingInfoCount = br.ReadInt32();
                int cullingInfoSize = br.ReadInt32();

                mesh.CullingInfos = new CullingInfo[cullingInfoCount];

                for (int i = 0; i < cullingInfoCount; i++)
                    mesh.CullingInfos[i] = CullingInfo.Read(br, mesh);

                return mesh;
            }
        }


        public static void Write(Stream stream, Mesh mesh)
        {
            using (var bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write("10GB".ToCharArray());
                bw.Write(mesh.Vertices.Length);
                bw.Write(mesh.Indices.Length);
                bw.Write((int)mesh.Type);

                foreach (var vert in mesh.Vertices)
                {
                    bw.Write(vert.Position.X);
                    bw.Write(vert.Position.Y);
                    bw.Write(vert.Position.Z);
                }
                foreach (var vert in mesh.Vertices)
                {
                    bw.Write(vert.Normal.X);
                    bw.Write(vert.Normal.Y);
                    bw.Write(vert.Normal.Z);
                }

                if (mesh.Type == MeshType.StandardTextured || mesh.Type == MeshType.FlexibleTextured)
                {
                    foreach (var vert in mesh.Vertices)
                    {
                        if (vert.TexCoord != Vector2.Empty)
                        {
                            bw.Write(vert.TexCoord.X);
                            bw.Write(vert.TexCoord.Y);
                        }
                        else
                        {
                            bw.Write(0f);
                            bw.Write(0f);
                        }
                    }
                }

                foreach (var idx in mesh.Indices)
                    bw.Write(idx.VertexIndex);

                //round edge shader data
                {
                    if (mesh.EdgeShaderValues == null || mesh.EdgeShaderValues.Length == 0)
                    {
                        mesh.EdgeShaderValues = new RoundEdgeData[]
                        {
                            new RoundEdgeData(new Vector3(1000,1000,1000),
                            new Vector3(1000,1000,1000),
                            new Vector3(1000,1000,1000),
                            new Vector3(1000,1000,1000))
                        };
                    }

                    int totalValues = mesh.EdgeShaderValues.Sum(x => x.Coords.Length * 2);
                    bw.Write(totalValues);

                    mesh.UpdateEdgeShaderOffsets();

                    foreach (var shaderData in mesh.EdgeShaderValues)
                    {
                        for (int i = 0; i < shaderData.Coords.Length; i++)
                        {
                            bw.Write(shaderData.Coords[i].X);
                            bw.Write(shaderData.Coords[i].Y);
                        }
                    }
                   
                    foreach (var idx in mesh.Indices)
                    {
                        //bw.Write(mesh.EdgeShaderValues[idx.ShaderDataIndex].FileOffset);
                        bw.Write(idx.RoundEdgeDataOffset);
                    }

                }

                //unknown data
                {
                    if (mesh.AverageNormals == null || mesh.AverageNormals.Length == 0)
                    {
                        mesh.AverageNormals = new Vector3[]
                        {
                            new Vector3(0,1,0)
                        };
                    }

                    bw.Write(mesh.AverageNormals.Length + 1);
                    bw.Write(83f); bw.Write(0f); bw.Write(0f);

                    foreach (var data in mesh.AverageNormals)
                    {
                        bw.Write(data.X);
                        bw.Write(data.Y);
                        bw.Write(data.Z);
                    }

                    foreach (var idx in mesh.Indices)
                        bw.Write(idx.AverageNormalIndex);
                }

                //mesh culling
                {
                    bw.Write(mesh.CullingInfos.Length);
                    long totalSizePos = stream.Position;
                    bw.Seek(4, SeekOrigin.Current);

                    foreach (var info in mesh.CullingInfos)
                        info.Write(bw);

                    long totalSize = stream.Position - totalSizePos;
                    stream.Position = totalSizePos;
                    bw.Write((int)totalSize - 4);
                }
            }
        }
    }
}
