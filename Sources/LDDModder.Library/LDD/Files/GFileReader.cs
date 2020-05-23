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

        #region Binary Reading

        public static MESH_FILE ReadMeshFile(Stream stream)
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
                culling.Studs = new STUD_2DFIELD_REF[studCount];

                for (int i = 0; i < studCount; i++)
                {
                    int infoSize = br.ReadInt32();
                    if (infoSize != 0x1C)
                    {
                        Trace.WriteLine("Unexpected stud info size!");
                        br.BaseStream.Skip(infoSize - 4);
                        continue;
                    }
                    var connectorRef = new STUD_2DFIELD_REF(br.ReadInt32(), br.ReadInt32());
                    for (int j = 0; j < connectorRef.Indices.Length; j++)
                        connectorRef.Indices[j] = new STUD_2DFIELD_IDX(br.ReadInts(4));
                    culling.Studs[i] = connectorRef;
                }
            }
            else
                culling.Studs = new STUD_2DFIELD_REF[0];

            if (extraDataBlock >= 2)
            {
                int block2Size = br.ReadInt32();
                int dataCount = br.ReadInt32();

                culling.AdjacentStuds = new STUD_2DFIELD_REF[dataCount];

                for (int i = 0; i < dataCount; i++)
                {
                    int dataSize = br.ReadInt32();
                    if (dataSize != 0x4C)
                    {
                        Trace.WriteLine("Unexpected adjacent stud info size!");
                        br.BaseStream.Skip(dataSize - 4);
                        continue;
                    }

                    var connectorRef = new STUD_2DFIELD_REF(br.ReadInt32(), br.ReadInt32());
                    for (int j = 0; j < connectorRef.Indices.Length; j++)
                        connectorRef.Indices[j] = new STUD_2DFIELD_IDX(br.ReadInts(4));

                    culling.AdjacentStuds[i] = connectorRef;
                }
            }
            else
                culling.AdjacentStuds = new STUD_2DFIELD_REF[0];

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

            long readSize = br.BaseStream.Position - startPosition;
            if (readSize != blockSize)
            {

            }
            return culling;
        }

        #endregion

        #region Convertion from File format structure to Object oriented structure

        public static MeshFile ReadMesh(Stream stream)
        {
            try
            {
                var meshFile = ReadMeshFile(stream);
                var meshType = (MeshType)meshFile.Header.MeshType;
                bool isTextured = meshType == MeshType.StandardTextured || meshType == MeshType.FlexibleTextured;
                bool isFlexible = meshType == MeshType.Flexible || meshType == MeshType.FlexibleTextured;

                var mainMesh = MeshGeometry.Create(meshFile.Geometry);

                SetShaderData(meshFile, meshFile.Geometry, mainMesh);

                var mesh = new MeshFile(mainMesh);
                if (stream is FileStream fs)
                    mesh.Filename = fs.Name;

                mesh.SetGeometry(mainMesh);

                for (int i = 0; i < meshFile.Culling.Length; i++)
                {
                    var data = meshFile.Culling[i];
                    var culling = new MeshCulling((MeshCullingType)data.Type)
                    {
                        FromIndex = data.FromIndex,
                        IndexCount = data.IndexCount,
                        FromVertex = data.FromVertex,
                        VertexCount = data.VertexCount,
                    };
                    
                    if (data.Studs != null && data.Studs.Length > 0)
                    {
                        for (int j = 0; j < data.Studs.Length; j++)
                            culling.Studs.Add(new Custom2DFieldReference(data.Studs[j]));
                    }

                    if (data.AdjacentStuds != null && data.AdjacentStuds.Length > 0)
                    {
                        for (int j = 0; j < data.AdjacentStuds.Length; j++)
                            culling.AdjacentStuds.Add(new Custom2DFieldReference(data.AdjacentStuds[j]));
                    }

                    if (data.ReplacementGeometry.HasValue)
                    {
                        var geom = MeshGeometry.Create(data.ReplacementGeometry.Value);
                        SetShaderData(meshFile, data.ReplacementGeometry.Value, geom);
                        culling.ReplacementMesh = geom;
                    }

                    mesh.Cullings.Add(culling);
                }
                return mesh;
            }
            catch
            {
                throw;
            }
        }

        private static void SetShaderData(MESH_FILE file, MESH_DATA data, MeshGeometry mesh)
        {
            for (int i = 0; i < data.Indices.Length; i++)
            {
                var index = data.Indices[i];

                if (index.AverageNormalIndex >= 0 && index.AverageNormalIndex < file.AverageNormals.Length)
                    mesh.Indices[i].AverageNormal = file.AverageNormals[index.AverageNormalIndex];

                if (file.GetShaderDataFromOffset(index.REShaderOffset, out ROUNDEDGE_SHADER_DATA shaderData))
                {
                    mesh.Indices[i].RoundEdgeData = new RoundEdgeData(shaderData.Coords.Take(6).ToArray());
                }
            }
        }

        #endregion

    }
}
