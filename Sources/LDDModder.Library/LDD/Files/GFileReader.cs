using LDDModder.IO;
using LDDModder.LDD.Files.MeshStructures;
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
        private static MESH_FILE ReadMeshFile(Stream stream)
        {
            stream.Position = 0;

            var meshFile = new MESH_FILE();

            using (var br = new BinaryReaderEx(stream, Encoding.UTF8, true))
            {
                var fileHeader = br.ReadStruct<MESH_HEADER>();

                if (fileHeader.Header != "10GB")
                    throw new IOException("The file is not a LDD mesh file (*.g)");

                meshFile.Header = fileHeader;
                meshFile.Geometry = MESH_DATA.Create(fileHeader);

                // Vertices & triangles
                {
                    for (int i = 0; i < fileHeader.VertexCount; i++)
                        meshFile.Geometry.Positions[i] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());


                    for (int i = 0; i < fileHeader.VertexCount; i++)
                        meshFile.Geometry.Normals[i] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

                    if (fileHeader.MeshType == (int)MeshType.StandardTextured || fileHeader.MeshType == (int)MeshType.FlexibleTextured)
                    {
                        for (int i = 0; i < fileHeader.VertexCount; i++)
                            meshFile.Geometry.UVs[i] = new Vector2(br.ReadSingle(), br.ReadSingle());
                    }

                    for (int i = 0; i < fileHeader.IndexCount; i++)
                        meshFile.Geometry.Indices[i] = new MESH_INDEX { VertexIndex = br.ReadInt32() };
                }

                // Edge shader data (brick outlines)
                {
                    int shaderDataLength = br.ReadInt32();

                    int valueCounter = 0;
                    var shaderData = new List<ROUNDEDGE_SHADER_DATA>();

                    while (valueCounter < shaderDataLength)
                    {
                        bool endOfRow = 256 - ((valueCounter + 12) % 256) < 12;
                        int valueCount = endOfRow ? 16 : 12;
                        int remainingData = shaderDataLength - valueCounter;

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

                        shaderData.Add(new ROUNDEDGE_SHADER_DATA(br.ReadSingles(valueCount)));
                        valueCounter += valueCount;
                    }

                    meshFile.RoundEdgeShaderData = shaderData.ToArray();

                    for (int i = 0; i < fileHeader.IndexCount; i++)
                        meshFile.Geometry.Indices[i].REShaderOffset = br.ReadInt32();
                }

                // Average Normals
                {
                    int averageNormalsCount = br.ReadInt32();

                    //we skip the first item because it looks like an header and the maximum referenced value seems always one less the specified length
                    var dataHeader = new Vector3(br.ReadSingles(3));
                    if (dataHeader.X != 83 || dataHeader.Y != 0 || dataHeader.Z != 0)
                        Trace.WriteLine($"Unexpected header: {dataHeader}");

                    meshFile.AverageNormals = new Vector3[averageNormalsCount - 1];
                    for (int i = 0; i < averageNormalsCount - 1; i++)
                        meshFile.AverageNormals[i] = new Vector3(br.ReadSingles(3));

                    for (int i = 0; i < fileHeader.IndexCount; i++)
                        meshFile.Geometry.Indices[i].AverageNormalIndex = br.ReadInt32();
                }

                // Flex data
                if (fileHeader.MeshType == (int)MeshType.Flexible || fileHeader.MeshType == (int)MeshType.FlexibleTextured)
                {
                    int dataSize = br.ReadInt32();
                    long startPos = stream.Position;
                    stream.Seek(dataSize, SeekOrigin.Current);
                    var dataOffsets = new List<int>();
                    for (int i = 0; i < fileHeader.VertexCount; i++)
                        dataOffsets.Add(br.ReadInt32());
                    long dataEndPosition = stream.Position;

                    for (int i = 0; i < fileHeader.VertexCount; i++)
                    {
                        stream.Position = startPos + dataOffsets[i];
                        int boneCount = br.ReadInt32();
                        meshFile.Geometry.Bones[i] = new MESH_BONE_MAPPING(boneCount);
                        for (int j = 0; j < boneCount; j++)
                            meshFile.Geometry.Bones[i].BoneWeights[j] = new MESH_BONE_WEIGHT(br.ReadInt32(), br.ReadSingle());
                    }

                    stream.Position = dataEndPosition;
                }

                int cullingInfoCount = br.ReadInt32();
                int cullingInfoSize = br.ReadInt32();

                meshFile.Culling = new MESH_CULLING[cullingInfoCount];

                for (int i = 0; i < cullingInfoCount; i++)
                    meshFile.Culling[i] = ReadCullingInfo(br, meshFile);
            }

            return meshFile;
        }

        private static MESH_CULLING ReadCullingInfo(BinaryReaderEx br, MESH_FILE meshFile)
        {
            long startPosition = br.BaseStream.Position;
            int blockSize = br.ReadInt32();
            bool isTextured = meshFile.Header.MeshType == (int)MeshType.StandardTextured || meshFile.Header.MeshType == (int)MeshType.FlexibleTextured;

            var culling = new MESH_CULLING()
            {
                Type = br.ReadInt32(),
                FromVertex = br.ReadInt32(),
                VertexCount = br.ReadInt32(),
                FromIndex = br.ReadInt32(),
                IndexCount = br.ReadInt32()
            };

            int vertexDataOffset = br.ReadInt32();
            int extraDataBlock = br.ReadInt32();

            if (extraDataBlock >= 1)
            {
                int studBlockSize = br.ReadInt32();
                int studCount = br.ReadInt32();
                culling.Studs = new MESH_STUD[studCount];

                for (int i = 0; i < studCount; i++)
                {
                    int infoSize = br.ReadInt32();
                    if (infoSize != 0x1C)
                    {
                        Trace.WriteLine("Unexpected stud info size!");
                        br.BaseStream.Skip(infoSize - 4);
                        continue;
                    }
                    culling.Studs[i] = new MESH_STUD(br.ReadInts(6));
                }
            }
            else
                culling.Studs = new MESH_STUD[0];

            if (extraDataBlock >= 2)
            {
                int block2Size = br.ReadInt32();
                int dataCount = br.ReadInt32();
                culling.UnknownData = new MESH_CULLING_DATA[dataCount];

                for (int i = 0; i < dataCount; i++)
                {
                    int dataSize = br.ReadInt32();
                    var unknownData = br.ReadBytes(dataSize - 4);
                    culling.UnknownData[i] = new MESH_CULLING_DATA(unknownData);
                }
            }

            if (vertexDataOffset != 0)
            {
                if (startPosition + vertexDataOffset != br.BaseStream.Position)
                {
                    Trace.WriteLine("Incorrect data size read");
                    br.BaseStream.Position = startPosition + vertexDataOffset;
                }

                int vertexCount = br.ReadInt32();
                int indexCount = br.ReadInt32();
                var geom = MESH_DATA.Create(vertexCount, indexCount, isTextured, false);
                

                for (int i = 0; i < vertexCount; i++)
                    geom.Positions[i] = new Vector3(br.ReadSingles(3));

                for (int i = 0; i < vertexCount; i++)
                    geom.Normals[i] = new Vector3(br.ReadSingles(3));

                if (isTextured)
                {
                    for (int i = 0; i < vertexCount; i++)
                        geom.UVs[i] = new Vector2(br.ReadSingles(2));
                }

                for (int i = 0; i < indexCount; i++)
                    geom.Indices[i] = new MESH_INDEX() { VertexIndex = br.ReadInt32() };

                for (int i = 0; i < indexCount; i++)
                    geom.Indices[i].AverageNormalIndex = br.ReadInt32();

                for (int i = 0; i < indexCount; i++)
                    geom.Indices[i].REShaderOffset = br.ReadInt32();

                culling.ReplacementGeometry = geom;
            }

            return culling;
        }

        public static Mesh2 ReadMesh(Stream stream)
        {
            try
            {
                var meshFile = ReadMeshFile(stream);
                var meshType = (MeshType)meshFile.Header.MeshType;
                bool isTextured = meshType == MeshType.StandardTextured || meshType == MeshType.FlexibleTextured;
                bool isFlexible = meshType == MeshType.Flexible || meshType == MeshType.FlexibleTextured;

                var baseGeometry = new MeshGeometry();
                var vertices = new List<Vertex>();

                for (int i = 0; i < meshFile.Header.VertexCount; i++)
                {
                    vertices.Add(new Vertex(
                        meshFile.Geometry.Positions[i],
                        meshFile.Geometry.Normals[i],
                        isTextured ? meshFile.Geometry.UVs[i] : Vector2.Empty
                        ));

                    if (isFlexible)
                    {
                        var bones = meshFile.Geometry.Bones[i];
                        for (int j = 0; j < bones.BoneWeights.Length; j++)
                            vertices[i].BoneWeights.Add(new BoneWeight(bones.BoneWeights[j].BoneID, bones.BoneWeights[j].Weight));
                    }
                }

                baseGeometry.SetVertices(vertices);

                for (int i = 0; i < meshFile.Header.IndexCount; i += 3)
                {
                    baseGeometry.AddTriangleFromIndices(
                        meshFile.Geometry.Indices[i].VertexIndex, 
                        meshFile.Geometry.Indices[i + 1].VertexIndex, 
                        meshFile.Geometry.Indices[i + 2].VertexIndex);
                }

                for (int i = 0; i < meshFile.Header.IndexCount; i++)
                {
                    var index = meshFile.Geometry.Indices[i];

                    if (index.AverageNormalIndex >= 0 && index.AverageNormalIndex < meshFile.AverageNormals.Length)
                        baseGeometry.Indices[i].AverageNormal = meshFile.AverageNormals[index.AverageNormalIndex];

                    if (meshFile.GetShaderDataFromOffset(index.REShaderOffset, out ROUNDEDGE_SHADER_DATA shaderData))
                    {
                        baseGeometry.Indices[i].RoundEdgeData = shaderData.Coords/*.Take(6).ToArray()*/;
                    }
                }

                var mesh = new Mesh2(meshFile, (MeshType)meshFile.Header.MeshType);
                mesh.SetGeometry(baseGeometry);
                return mesh;
            }
            catch
            {
                throw;
            }
            return null;
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
