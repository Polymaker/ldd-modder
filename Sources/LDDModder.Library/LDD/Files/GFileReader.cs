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
                        Trace.WriteLine($"Unexpected  average normal header: {dataHeader}");

                    meshFile.AverageNormals = new Vector3[averageNormalsCount - 1];
                    for (int i = 0; i < averageNormalsCount - 1; i++)
                        meshFile.AverageNormals[i] = new Vector3(br.ReadSingles(3));

                    for (int i = 0; i < fileHeader.IndexCount; i++)
                        meshFile.Geometry.Indices[i].AverageNormalIndex = br.ReadInt32();
                }

                long boneMappingPosition = 0;

                // Flex data
                if (fileHeader.MeshType == (int)MeshType.Flexible || fileHeader.MeshType == (int)MeshType.FlexibleTextured)
                {
                    int dataSize = br.ReadInt32();
                    boneMappingPosition = stream.Position;

                    stream.Seek(dataSize, SeekOrigin.Current); //skip over the data

                    var dataOffsets = new List<int>();
                    for (int i = 0; i < fileHeader.VertexCount; i++)
                        dataOffsets.Add(br.ReadInt32());
                    long dataEndPosition = stream.Position;

                    for (int i = 0; i < fileHeader.VertexCount; i++)
                        meshFile.Geometry.Bones[i] = ReadBoneMapping(br, boneMappingPosition, dataOffsets[i]);

                    stream.Position = dataEndPosition;
                }

                int cullingInfoCount = br.ReadInt32();
                int cullingInfoSize = br.ReadInt32();

                meshFile.Cullings = new MESH_CULLING[cullingInfoCount];

                for (int i = 0; i < cullingInfoCount; i++)
                    meshFile.Cullings[i] = ReadCullingInfo(br, meshFile, boneMappingPosition);
            }

            return meshFile;
        }

        private static MESH_BONE_MAPPING ReadBoneMapping(BinaryReaderEx br, long dataPosition, int offset)
        {
            br.BaseStream.Position = dataPosition + offset;
            int boneCount = br.ReadInt32();
            var mapping = new MESH_BONE_MAPPING(boneCount);
            for (int j = 0; j < boneCount; j++)
                mapping.BoneWeights[j] = new MESH_BONE_WEIGHT(br.ReadInt32(), br.ReadSingle());

            return mapping;
        }

        private static MESH_CULLING ReadCullingInfo(BinaryReaderEx br, MESH_FILE meshFile, long boneMappingPosition)
        {
            long startPosition = br.BaseStream.Position;
            int cullingDataSize = br.ReadInt32();

            var culling = new MESH_CULLING()
            {
                Type = br.ReadInt32(),
                FromVertex = br.ReadInt32(),
                VertexCount = br.ReadInt32(),
                FromIndex = br.ReadInt32(),
                IndexCount = br.ReadInt32()
            };

            int alternateMeshOffset = br.ReadInt32();
            int connectorReferenceFlag = br.ReadInt32();

            if (connectorReferenceFlag >= 1)
                culling.Studs = ReadCustom2DFieldReferences(br);
            else
                culling.Studs = new CUSTOM2DFIELD_REFERENCE[0];

            if (connectorReferenceFlag >= 2)
                culling.AdjacentStuds = ReadCustom2DFieldReferences(br);
            else
                culling.AdjacentStuds = new CUSTOM2DFIELD_REFERENCE[0];

            if (connectorReferenceFlag > 2)
                Trace.WriteLine($"Unexpected connector reference flag: {connectorReferenceFlag}");

            if (alternateMeshOffset != 0)
            {
                if (startPosition + alternateMeshOffset != br.BaseStream.Position)
                {
                    Trace.WriteLine("Incorrect data size read");
                    br.BaseStream.Position = startPosition + alternateMeshOffset;
                }
                long meshDataSize = cullingDataSize - alternateMeshOffset;
                culling.AlternateMesh = ReadAlternateMesh(br, meshFile, boneMappingPosition, meshDataSize);
            }

            return culling;
        }

        private static CUSTOM2DFIELD_REFERENCE[] ReadCustom2DFieldReferences(BinaryReaderEx br)
        {
            long finalPosition = br.BaseStream.Position;
            finalPosition += br.ReadInt32(); // Block Size
            
            int connectionRefCount = br.ReadInt32();
            var references = new CUSTOM2DFIELD_REFERENCE[connectionRefCount];

            for (int i = 0; i < connectionRefCount; i++)
            {
                br.ReadInt32(); // Block Size

                var connectorRef = new CUSTOM2DFIELD_REFERENCE(
                    br.ReadInt32(), //Custom2DField Index
                    br.ReadInt32() //Number of referenced studs
                );

                for (int j = 0; j < connectorRef.Indices.Length; j++)
                    connectorRef.Indices[j] = new CUSTOM2DFIELD_INDEX(br.ReadInts(4));

                references[i] = connectorRef;
            }

            if (br.BaseStream.Position != finalPosition)
            {
                Trace.WriteLine("Incorrect data size read");
            }
            return references;
        }

        private static MESH_DATA ReadAlternateMesh(BinaryReaderEx br, MESH_FILE mesh, long boneMappingPosition, long meshDataSize)
        {
            long startPosition = br.Position;

            int vertexCount = br.ReadInt32();
            int indexCount = br.ReadInt32();

            var geom = MESH_DATA.Create(vertexCount, indexCount, mesh.IsTextured, mesh.IsFlexible);


            for (int i = 0; i < vertexCount; i++)
                geom.Positions[i] = new Vector3(br.ReadSingles(3));

            for (int i = 0; i < vertexCount; i++)
                geom.Normals[i] = new Vector3(br.ReadSingles(3));

            if (mesh.IsTextured)
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

            if (mesh.IsFlexible)
            {
                Trace.WriteLine("WARNING Flexible alternate mesh encountered!");
                if (br.Position < startPosition + meshDataSize)
                {
                    var dataOffsets = new List<int>();
                    for (int i = 0; i < vertexCount; i++)
                        dataOffsets.Add(br.ReadInt32());

                    long dataEndPosition = br.Position;
                    for (int i = 0; i < vertexCount; i++)
                        geom.Bones[i] = ReadBoneMapping(br, boneMappingPosition, dataOffsets[i]);

                    br.Position = dataEndPosition;
                }
                else
                {
                    Trace.WriteLine("Flexible alternate mesh does not seem to be supported!");
                    geom.Bones = new MESH_BONE_MAPPING[0];
                }
            }

            return geom;
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

                for (int i = 0; i < meshFile.Cullings.Length; i++)
                {
                    var data = meshFile.Cullings[i];
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

                    if (data.AlternateMesh.HasValue)
                    {
                        var geom = MeshGeometry.Create(data.AlternateMesh.Value);
                        SetShaderData(meshFile, data.AlternateMesh.Value, geom);
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

                    if (!mesh.HasRoundEdgeData && !mesh.Indices[i].RoundEdgeData.IsEmpty)
                        mesh.HasRoundEdgeData = true;
                }
            }
        }

        #endregion

    }
}
