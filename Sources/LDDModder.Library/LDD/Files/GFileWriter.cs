using LDDModder.IO;
using LDDModder.LDD.Files.MeshStructures;
using LDDModder.LDD.Meshes;
using LDDModder.Simple3D;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files
{
    public class GFileWriter
    {
        public static bool Debuger { get; private set; }

        public static void WriteMesh(Stream stream, MeshFile mesh)
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

        #region File structure writing

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

                if (culling.AdjacentStuds != null && culling.AdjacentStuds.Length > 0)
                    extraDataVal = 2;
                else if (culling.Studs != null && culling.Studs.Length > 0)
                    extraDataVal = 1;

                bw.Write(extraDataVal);

                if (extraDataVal > 0)
                {
                    WriteSizedBlock(bw, () =>
                    {
                        int itemCount = culling.Studs?.Length ?? 0;
                        bw.Write(itemCount);
                        for (int i = 0; i < itemCount; i++)
                        {
                            WriteSizedBlock(bw, () =>
                            {
                                bw.Write(culling.Studs[i].ConnectorIndex);
                                bw.Write(culling.Studs[i].Indices.Length);
                                for (int j = 0; j < culling.Studs[i].Indices.Length; j++)
                                {
                                    bw.Write(culling.Studs[i].Indices[j].ArrayIndex);
                                    bw.Write(culling.Studs[i].Indices[j].Value2);
                                    bw.Write(culling.Studs[i].Indices[j].Value3);
                                    bw.Write(culling.Studs[i].Indices[j].Value4);
                                }
                            });
                        }
                    });
                }

                if (extraDataVal > 1)
                {
                    WriteSizedBlock(bw, () =>
                    {
                        int itemCount = culling.AdjacentStuds?.Length ?? 0;
                        bw.Write(itemCount);
                        for (int i = 0; i < itemCount; i++)
                        {
                            WriteSizedBlock(bw, () =>
                            {
                                bw.Write(culling.AdjacentStuds[i].ConnectorIndex);
                                bw.Write(culling.AdjacentStuds[i].Indices.Length);
                                for (int j = 0; j < culling.AdjacentStuds[i].Indices.Length; j++)
                                {
                                    bw.Write(culling.AdjacentStuds[i].Indices[j].ArrayIndex);
                                    bw.Write(culling.AdjacentStuds[i].Indices[j].Value2);
                                    bw.Write(culling.AdjacentStuds[i].Indices[j].Value3);
                                    bw.Write(culling.AdjacentStuds[i].Indices[j].Value4);
                                }
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

        #endregion

        #region Convertion from Object oriented structure to File format structure

        private class ShaderDataHelper
        {
            public ListIndexer<Vector3> AvgNormals { get; }
            public ListIndexer<RoundEdgeData> RoundEdgeData { get; }

            public ShaderDataHelper(List<Vector3> normals, List<RoundEdgeData> roundEdges)
            {
                AvgNormals = new ListIndexer<Vector3>(normals);
                RoundEdgeData = new ListIndexer<RoundEdgeData>(roundEdges);
            }
        }

        private static MESH_FILE BuildMeshFile(MeshFile mesh)
        {
            if (!mesh.Cullings.Any(x => x.Type == MeshCullingType.MainModel))
            {
                if (Debugger.IsAttached)
                    throw new InvalidOperationException("Mesh does not contain culling information");
                else
                    Debug.WriteLine("Mesh has no culling information!");
            }

            var file = new MESH_FILE
            {
                Header = MESH_HEADER.Create(mesh)
            };

            var avgNormals = mesh.GetAverageNormals().DistinctValues().ToList();
            var outlines = mesh.GetRoundEdgeShaderData().EqualsDistinct().ToList();
            for (int i = 21; i < outlines.Count; i += 21)
                outlines[i].PackData();
            var shaderData = new ShaderDataHelper(avgNormals, outlines);

            file.AverageNormals = avgNormals.ToArray();
            file.RoundEdgeShaderData = outlines.Select(x => new ROUNDEDGE_SHADER_DATA(x.Coords)).ToArray();

            file.Geometry = SerializeMeshGeometry(shaderData, mesh.Geometry);
            file.Culling = new MESH_CULLING[mesh.Cullings.Count];

            for (int i = 0; i < mesh.Cullings.Count; i++)
                file.Culling[i] = SerializeMeshCulling(shaderData, mesh.Cullings[i]);

            return file;
        }

        private static MESH_DATA SerializeMeshGeometry(ShaderDataHelper shaderData, MeshGeometry meshGeometry)
        {
            var meshData = MESH_DATA.Create(meshGeometry);
            var vertIndexer = new ListIndexer<Vertex>(meshGeometry.Vertices);
            for (int i = 0; i < meshGeometry.Vertices.Count; i++)
            {
                var vert = meshGeometry.Vertices[i];
                meshData.Positions[i] = vert.Position;
                meshData.Normals[i] = vert.Normal;

                if (meshGeometry.IsTextured)
                    meshData.UVs[i] = vert.TexCoord;

                if (vert.BoneWeights.Any())
                {
                    var boneWeights = vert.BoneWeights.Select(x => new MESH_BONE_WEIGHT { BoneID = x.BoneID, Weight = x.Weight });
                    meshData.Bones[i] = new MESH_BONE_MAPPING(boneWeights);
                }
            }

            for (int i = 0; i < meshGeometry.Indices.Count; i++)
            {
                var idx = meshGeometry.Indices[i];
                meshData.Indices[i].VertexIndex = vertIndexer.IndexOf(idx.Vertex);

                meshData.Indices[i].AverageNormalIndex = shaderData.AvgNormals.IndexOf(idx.AverageNormal);
                int reIdx = shaderData.RoundEdgeData.IndexOf(idx.RoundEdgeData);
                int reOffset = reIdx * 12 + ((int)Math.Floor(reIdx / 21d) * 4);
                meshData.Indices[i].REShaderOffset = reOffset;
            }

            return meshData;
        }

        private static MESH_CULLING SerializeMeshCulling(ShaderDataHelper shaderData, MeshCulling meshCulling)
        {
            var culling = new MESH_CULLING
            {
                Type = (int)meshCulling.Type,
                FromVertex = meshCulling.FromVertex,
                VertexCount = meshCulling.VertexCount,
                FromIndex = meshCulling.FromIndex,
                IndexCount = meshCulling.IndexCount
            };

            if (meshCulling.ReplacementMesh != null)
                culling.ReplacementGeometry = SerializeMeshGeometry(shaderData, meshCulling.ReplacementMesh);

            if (meshCulling.Studs != null && meshCulling.Studs.Any())
                culling.Studs = meshCulling.Studs.Select(x => x.Serialize()).ToArray();

            if (meshCulling.AdjacentStuds != null && meshCulling.AdjacentStuds.Any())
                culling.AdjacentStuds = meshCulling.AdjacentStuds.Select(x => x.Serialize()).ToArray();

            return culling;
        }

        #endregion

    }
}
