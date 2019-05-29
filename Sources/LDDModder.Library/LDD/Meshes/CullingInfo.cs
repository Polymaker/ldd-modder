using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Meshes
{
    public class CullingInfo
    {
        public MeshCullingType CullingType { get; set; }

        /// <summary>
        /// Specifies the first vertex concerned.
        /// </summary>
        public int FromVertex { get; set; }

        /// <summary>
        /// Specifies the number of vertices concerned.
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// Specifies the first index concerned.
        /// </summary>
        public int FromIndex { get; set; }

        /// <summary>
        /// Specifies the number of indices concerned.
        /// </summary>
        public int IndexCount { get; set; }

        public StudInformation[] Studs { get; set; }

        public Vertex[] Vertices { get; set; }

        public IndexReference[] Indices { get; set; }

        public List<byte[]> UnknownData { get; set; }

        public CullingInfo()
        {
            UnknownData = new List<byte[]>();
            Studs = new StudInformation[0];
            Vertices = new Vertex[0];
            Indices = new IndexReference[0];
        }

        public CullingInfo(MeshCullingType cullingType, int fromVertex, int vertexCount, int fromIndex, int indexCount)
        {
            CullingType = cullingType;
            FromVertex = fromVertex;
            VertexCount = vertexCount;
            FromIndex = fromIndex;
            IndexCount = indexCount;
            UnknownData = new List<byte[]>();
            Studs = new StudInformation[0];
            Vertices = new Vertex[0];
            Indices = new IndexReference[0];
        }

        public CullingInfo(MeshCullingType variantType, int[] headerValues)
        {
            CullingType = variantType;
            FromVertex = headerValues[0];
            VertexCount = headerValues[1];
            FromIndex = headerValues[2];
            IndexCount = headerValues[3];
            UnknownData = new List<byte[]>();
            Studs = new StudInformation[0];
            Vertices = new Vertex[0];
            Indices = new IndexReference[0];
        }

        public static CullingInfo Read(BinaryReader br, Mesh mesh)
        {
            long startPosition = br.BaseStream.Position;
            int blockSize = br.ReadInt32();

            try
            {
                var variantType = (MeshCullingType)br.ReadInt32();
                int[] headerValues = br.ReadInts(4);
                int vertexDataOffset = br.ReadInt32();
                int extraDataBlock = br.ReadInt32();

                var cullingInfo = new CullingInfo(variantType, headerValues);

                if (extraDataBlock >= 1)
                {
                    int studBlockSize = br.ReadInt32();
                    int studCount = br.ReadInt32();
                    cullingInfo.Studs = new StudInformation[studCount];

                    for (int i = 0; i < studCount; i++)
                    {
                        int infoSize = br.ReadInt32();
                        if (infoSize != 0x1C)
                        {
                            Trace.WriteLine("Unexpected stud info size!");
                            br.BaseStream.Skip(infoSize - 4);
                            continue;
                        }
                        cullingInfo.Studs[i] = new StudInformation(br.ReadInts(6));
                    }
                }

                if (extraDataBlock >= 2)
                {
                    int block2Size = br.ReadInt32();
                    int dataCount = br.ReadInt32();

                    for (int i = 0; i < dataCount; i++)
                    {
                        int dataSize = br.ReadInt32();
                        var unknownData = br.ReadBytes(dataSize - 4);
                        cullingInfo.UnknownData.Add(unknownData);
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
                    cullingInfo.Vertices = new Vertex[vertexCount];
                    cullingInfo.Indices = new IndexReference[indexCount];
                    var positions = new List<Vector3>();
                    var normals = new List<Vector3>();
                    var uvs = new List<Vector2>();

                    for (int i = 0; i < vertexCount; i++)
                        positions.Add(new Vector3(br.ReadSingles(3)));

                    for (int i = 0; i < vertexCount; i++)
                        normals.Add(new Vector3(br.ReadSingles(3)));

                    if (mesh.Type == MeshType.StandardTextured || mesh.Type == MeshType.FlexibleTextured)
                    {
                        for (int i = 0; i < vertexCount; i++)
                            uvs.Add(new Vector2(br.ReadSingles(2)));
                    }

                    if (mesh.Type == MeshType.StandardTextured || mesh.Type == MeshType.FlexibleTextured)
                    {
                        for (int i = 0; i < vertexCount; i++)
                            cullingInfo.Vertices[i] = new Vertex(positions[i], normals[i], uvs[i]);
                    }
                    else
                    {
                        for (int i = 0; i < vertexCount; i++)
                            cullingInfo.Vertices[i] = new Vertex(positions[i], normals[i]);
                    }

                    for (int i = 0; i < indexCount; i++)
                        cullingInfo.Indices[i] = new IndexReference(br.ReadInt32());

                    for (int i = 0; i < indexCount; i++)
                        cullingInfo.Indices[i].AverageNormalIndex = br.ReadInt32();

                    for (int i = 0; i < indexCount; i++)
                    {
                        int shaderOffset = br.ReadInt32();
                        cullingInfo.Indices[i].AverageNormalIndex = mesh.ShaderOffsetToIndex(shaderOffset);
                    }
                }

                return cullingInfo;
            }
            finally
            {
                if (startPosition + blockSize != br.BaseStream.Position)
                {
                    Trace.WriteLine("Incorrect data size read");
                    br.BaseStream.Position = startPosition + blockSize;
                }
            }
            
        }

        public void Write(BinaryWriter bw)
        {
            WriteBlock(bw, () =>
            {
                bw.Write((int)CullingType);
                bw.Write(FromVertex);
                bw.Write(VertexCount);
                bw.Write(FromIndex);
                bw.Write(IndexCount);

                long vertexOffsetPos = bw.BaseStream.Position;
                bw.Write(0);//dummy offset;

                int extraDataVal = 0;

                if (UnknownData != null && UnknownData.Count > 0)
                    extraDataVal = 2;
                else if (Studs != null && Studs.Length > 0)
                    extraDataVal = 1;

                bw.Write(extraDataVal);

                if (extraDataVal > 0)
                {
                    WriteBlock(bw, () =>
                    {
                        int studCount = Studs?.Length ?? 0;
                        bw.Write(studCount);
                        for (int i = 0; i < studCount; i++)
                        {
                            WriteBlock(bw, () =>
                            {
                                bw.Write(Studs[i].ConnectorIndex);
                                bw.Write(Studs[i].Value2);
                                bw.Write(Studs[i].Value3);
                                bw.Write(Studs[i].Value4);
                                bw.Write(Studs[i].Value5);
                                bw.Write(Studs[i].Value6);
                            });
                        }
                    });
                }

                if (extraDataVal > 1)
                {
                    WriteBlock(bw, () =>
                    {
                        int dataCount = UnknownData?.Count ?? 0;
                        bw.Write(dataCount);

                        for (int i = 0; i < dataCount; i++)
                        {
                            WriteBlock(bw, () =>
                            {
                                bw.Write(UnknownData[i]);
                            });
                        }
                    });
                }

                if ((Vertices?.Length ?? 0) > 0)
                {
                    long currentPos = bw.BaseStream.Position;
                    bw.BaseStream.Position = vertexOffsetPos;
                    bw.Write((int)currentPos);
                    bw.BaseStream.Position = currentPos;

                }
            });
        }

        private void WriteBlock(BinaryWriter bw, Action action)
        {
            long startPos = bw.BaseStream.Position;
            bw.Write(0);//dummy size;
            action();
            long endPos = bw.BaseStream.Position;
            bw.BaseStream.Position = startPos;
            bw.Write((int)(endPos - startPos));
            bw.BaseStream.Position = endPos;
        }
    }
}
