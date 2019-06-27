using LDDModder.IO;
using LDDModder.LDD.Files.MeshStructures;
using LDDModder.LDD.Meshes;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files
{
    public class GFileWriter
    {
        public static void WriteMesh(Stream stream, Mesh2 mesh)
        {
            try
            {
                var meshFile = BuildMeshFile(mesh);
                WriteMeshFile(stream, meshFile);
            }
            catch
            {
                throw;
            }
        }

        public static void WriteMeshFile(Stream stream, MESH_FILE meshFile)
        {
            using (var bw = new BinaryWriterEx(stream, Encoding.UTF8, true))
            {
                bw.WriteStruct(meshFile.Header);
                void WriteVector3(Vector3 v)
                {
                    bw.WriteSingle(v.X);
                    bw.WriteSingle(v.Y);
                    bw.WriteSingle(v.Z);
                }

                void WriteVector2(Vector2 v)
                {
                    bw.WriteSingle(v.X);
                    bw.WriteSingle(v.Y);
                }

                foreach (var pos in meshFile.Geometry.Positions)
                    WriteVector3(pos);

                foreach (var norm in meshFile.Geometry.Normals)
                    WriteVector3(norm);

                if (meshFile.Geometry.UVs != null && meshFile.Geometry.UVs.Length > 0)
                {
                    foreach (var uv in meshFile.Geometry.UVs)
                        WriteVector2(uv);
                }

                for (int i = 0; i < meshFile.Header.IndexCount; i++)
                    bw.WriteInt32(meshFile.Geometry.Indices[i].VertexIndex);


                //Round Edge Shader Data
                {
                    var allShaderData = meshFile.RoundEdgeShaderData.SelectMany(x => x.Coords).ToList();

                    bw.WriteInt32(allShaderData.Count * 2);
                    foreach (var shaderCoord in allShaderData)
                        WriteVector2(shaderCoord);

                    for (int i = 0; i < meshFile.Header.IndexCount; i++)
                        bw.WriteInt32(meshFile.Geometry.Indices[i].REShaderOffset);
                }

                //Average Normals
                {
                    bw.WriteInt32(meshFile.AverageNormals.Length + 1);
                    WriteVector3(new Vector3(83, 0, 0));

                    foreach (var avgNorm in meshFile.AverageNormals)
                        WriteVector3(avgNorm);

                    for (int i = 0; i < meshFile.Header.IndexCount; i++)
                        bw.WriteInt32(meshFile.Geometry.Indices[i].AverageNormalIndex);
                }

                if (meshFile.Geometry.Bones != null && meshFile.Geometry.Bones.Length > 0)
                {
                    var allBones = meshFile.Geometry.Bones.SelectMany(x => x.BoneWeights).ToList();
                    bw.WriteInt32(allBones.Count * 8);
                    var dataOffsets = new List<int>();
                    for (int i = 0; i < meshFile.Header.VertexCount; i++)
                    {
                        var vertexBones = meshFile.Geometry.Bones[i];
                        bw.WriteInt32(vertexBones.BoneWeights.Length);
                        for (int j = 0; j < vertexBones.BoneWeights.Length; j++)
                        {
                            bw.WriteInt32(vertexBones.BoneWeights[j].BoneID);
                            bw.WriteSingle(vertexBones.BoneWeights[j].Weight);
                        }
                        //bone count (4 bytes) + bones data (id + weight = 8 bytes) 
                        dataOffsets.Add(4 + (vertexBones.BoneWeights.Length * 8));
                    }

                    for (int i = 0; i < meshFile.Header.VertexCount; i++)
                        bw.WriteInt32(dataOffsets[i]);
                }

                //Mesh Culling Info
                {
                    bw.WriteInt32(meshFile.Culling.Length);
                    WriteSizedBlock(bw, () =>
                    {
                        for (int i = 0; i < meshFile.Culling.Length; i++)
                            WriteCullingInfo(bw, meshFile.Culling[i]);
                    }, false);
                }
            }
        }

        private static void WriteCullingInfo(BinaryWriterEx bw, MESH_CULLING culling)
        {
            void WriteVector3(Vector3 v)
            {
                bw.WriteSingle(v.X);
                bw.WriteSingle(v.Y);
                bw.WriteSingle(v.Z);
            }

            void WriteVector2(Vector2 v)
            {
                bw.WriteSingle(v.X);
                bw.WriteSingle(v.Y);
            }
            long startPos = bw.BaseStream.Position;

            WriteSizedBlock(bw, () =>
            {
                bw.WriteInt32(culling.Type);
                bw.WriteInt32(culling.FromVertex);
                bw.WriteInt32(culling.VertexCount);
                bw.WriteInt32(culling.FromIndex);
                bw.WriteInt32(culling.IndexCount);

                long vertexOffsetPos = bw.BaseStream.Position;
                bw.Write(0);//dummy offset;

                int extraDataVal = 0;

                if (culling.UnknownData != null && culling.UnknownData.Length > 0)
                    extraDataVal = 2;
                else if (culling.Studs != null && culling.Studs.Length > 0)
                    extraDataVal = 1;

                bw.Write(extraDataVal);

                if (extraDataVal > 0)
                {
                    WriteSizedBlock(bw, () =>
                    {
                        int studCount = culling.Studs?.Length ?? 0;
                        bw.Write(studCount);
                        for (int i = 0; i < studCount; i++)
                        {
                            WriteSizedBlock(bw, () =>
                            {
                                bw.Write(culling.Studs[i].ConnectorIndex);
                                bw.Write(culling.Studs[i].Value2);
                                bw.Write(culling.Studs[i].DataArrayIndex);
                                bw.Write(culling.Studs[i].Value4);
                                bw.Write(culling.Studs[i].Value5);
                                bw.Write(culling.Studs[i].Value6);
                            });
                        }
                    });
                }

                if (extraDataVal > 1)
                {
                    WriteSizedBlock(bw, () =>
                    {
                        int dataCount = culling.UnknownData?.Length ?? 0;
                        bw.Write(dataCount);

                        for (int i = 0; i < dataCount; i++)
                        {
                            WriteSizedBlock(bw, () =>
                            {
                                bw.Write(culling.UnknownData[i].Data);
                            });
                        }
                    });
                }

                if (culling.ReplacementGeometry != null)
                {
                    var cullingGeom = culling.ReplacementGeometry.Value;
                    long currentPos = bw.BaseStream.Position;
                    bw.BaseStream.Position = vertexOffsetPos;
                    bw.Write((int)(currentPos - startPos));
                    bw.BaseStream.Position = currentPos;

                    bw.WriteInt32(cullingGeom.Positions.Length);
                    bw.WriteInt32(cullingGeom.Indices.Length);

                    foreach (var pos in cullingGeom.Positions)
                        WriteVector3(pos);

                    foreach (var norm in cullingGeom.Normals)
                        WriteVector3(norm);

                    if (cullingGeom.UVs != null && cullingGeom.UVs.Length > 0)
                    {
                        foreach (var uv in cullingGeom.UVs)
                            WriteVector2(uv);
                    }

                    foreach (var index in cullingGeom.Indices)
                        bw.WriteInt32(index.VertexIndex);

                    foreach (var index in cullingGeom.Indices)
                        bw.WriteInt32(index.AverageNormalIndex);

                    foreach (var index in cullingGeom.Indices)
                        bw.WriteInt32(index.REShaderOffset);
                }
            });
        }

        private static void WriteSizedBlock(BinaryWriter bw, Action action, bool inclusive = true)
        {
            long startPos = bw.BaseStream.Position;
            bw.Write(0);//dummy size;
            action();
            long endPos = bw.BaseStream.Position;
            bw.BaseStream.Position = startPos;
            int blockSize = (int)(endPos - startPos);
            if (!inclusive)
                blockSize -= 4;
            bw.Write(blockSize);
            bw.BaseStream.Position = endPos;
        }

        private static MESH_FILE BuildMeshFile(Mesh2 mesh)
        {
            return default(MESH_FILE);
        }

        //private static MESH_CULLING CreateCulling()
    }
}
