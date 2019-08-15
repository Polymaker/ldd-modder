using Assimp;
using LDDModder.LDD.Files;
using LDDModder.LDD.Files.MeshStructures;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.BrickEditor
{
    public partial class Form1 : Form
    {
        private string LddMeshDirectory;
        private string PrimitiveDirectory;
        public Form1()
        {
            InitializeComponent();
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            LddMeshDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");
            PrimitiveDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\");

            //int brickID = 3020;
            //LDD.Meshes.Mesh brickMesh = null;
            //using (var fs = File.OpenRead(Path.Combine(meshDirectory, $"{brickID}.g")))
            //    brickMesh = GFileReader.ReadMesh(fs);

            //using (var fs = File.Create($"{brickID} cust.g"))
            //    GFileWriter.WriteMesh(fs, brickMesh);
            //TestCustomBrick();
            TestExportMesh();
        }

        private void TestExportMesh()
        {
            AssimpContext importer = new AssimpContext();

            var formats = importer.GetSupportedExportFormats();
            
            int brickID = 3020;
            LDD.Meshes.Mesh brickMesh = null;
            using (var fs = File.OpenRead(Path.Combine(LddMeshDirectory, $"{brickID}.g")))
                brickMesh = GFileReader.ReadMesh(fs);

            var converted = MeshConverter.ConvertFromLDD(brickMesh);

            var meshScene = importer.ImportFile(@"..\..\duck.dae",
                PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.PreTransformVertices);
            meshScene.Meshes.RemoveAt(0);
            meshScene.Meshes.Add(converted);
            meshScene.RootNode.Children.Remove(meshScene.RootNode.Children[1]);
            meshScene.RootNode.Children.Remove(meshScene.RootNode.Children[1]);
            meshScene.Lights.Clear();

            //meshScene.Materials.Clear();
            //var test = new Assimp.Scene();
            //converted.Name = "Mesh0";
            //test.Meshes.Add(converted);
            //test.RootNode = new Node("Scene");
            //var meshNode = new Node("#Mesh0");

            //meshNode.MeshIndices.Add(0);
            //test.RootNode.Children.Add(meshNode);

            var expFormat = formats.FirstOrDefault(x => x.FormatId == "collada");
            importer.ExportFile(meshScene, $"{brickID}.{expFormat.FileExtension}", expFormat.FormatId);
        }

        private void TestCustomBrick()
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            string meshDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");
            string primitiveDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\");

            int brickID = 123123;

            AssimpContext importer = new AssimpContext();
            //importer.SetConfig(new Assimp.Configs.GlobalScaleConfig(0.2f));
            importer.Scale = 1.5f;
            var meshScene = importer.ImportFile(@"..\..\duck.dae", 
                PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.PreTransformVertices);
            
            var converted = MeshConverter.ConvertToLDD(meshScene.Meshes[0], true);
            converted.Geometry.CalculateAverageNormals();

            converted.Cullings.Add(new MeshCulling(MeshCullingType.MainModel)
            {
                VertexCount = converted.Geometry.VertexCount,
                IndexCount = converted.Geometry.IndexCount
            });
            using (var fs = File.Create(Path.Combine(meshDirectory, $"{123123}.g")))
                GFileWriter.WriteMesh(fs, converted);

            
            var primitive = new Primitive()
            {
                ID = brickID,
                Name = "Test brick",
                Platform = LDD.Data.Platform.System,
                MainGroup = LDD.Data.MainGroup.Bricks,
                DesignVersion = 1
            };

            primitive.Aliases.Add(primitive.ID);
            primitive.Bounding = BoundingBox.FromVertices(converted.Geometry.Vertices);
            primitive.GeometryBounding = primitive.Bounding;
            var xmlDoc = new XDocument(primitive.SerializeToXml());
            xmlDoc.Save(Path.Combine(primitiveDirectory, $"{primitive.ID}.xml"));

            
        }

        public void OutputMesh(MESH_FILE meshFile, string filePath)
        {
            using (var sw = new StreamWriter(File.Open(filePath, FileMode.Create)))
            {
                sw.WriteLine("# File Header #");
                sw.WriteLine($"Header: {meshFile.Header.Header}");
                sw.WriteLine($"Vertex count: {meshFile.Header.VertexCount}");
                sw.WriteLine($"Index count: {meshFile.Header.IndexCount}");
                sw.WriteLine($"Mesh type: {meshFile.Header.MeshType}");
                sw.WriteLine(string.Empty);

                PrintGeometry(sw, meshFile, meshFile.Geometry, true);

                if (meshFile.Geometry.Bones != null && meshFile.Geometry.Bones.Length > 0)
                {
                    sw.WriteLine("# Flex bones # //FlexBone ID references FlexBone in XML");

                    for (int i = 0; i < meshFile.Geometry.Bones.Length; i++)
                    {
                        sw.WriteLine($"Vertex {i}:");
                        foreach (var boneWeight in meshFile.Geometry.Bones[i].BoneWeights)
                            sw.WriteLine($"\tFlexBone ID: {boneWeight.BoneID}\tBone Weight: {boneWeight.Weight}");

                        sw.WriteLine(string.Empty);
                    }
                }

                if (meshFile.Culling != null && meshFile.Culling.Length > 0)
                {
                    sw.WriteLine("# Mesh/Stud culling #");
                    for (int i = 0; i < meshFile.Culling.Length; i++)
                    {
                        sw.WriteLine($"Culling info {i}:");
                        var info = meshFile.Culling[i];
                        sw.WriteLine($"\tCulling type: {info.Type}");
                        sw.WriteLine($"\tFirst vertex: {info.FromVertex}");
                        sw.WriteLine($"\tVertex count: {info.VertexCount}");
                        sw.WriteLine($"\tFirst index: {info.FromIndex}");
                        sw.WriteLine($"\tIndex count: {info.IndexCount}");

                        if (info.Studs != null && info.Studs.Length > 0)
                        {
                            sw.WriteLine("\tStuds: (Connector index; unknown; Array index; unknown; unknown; unknown) //References to Custom2DField in XML");
                            for (int j = 0; j < info.Studs.Length; j++)
                                sw.WriteLine($"\t\tStud {j}: " + string.Join("; ", info.Studs[j].ToArray()));
                        }
                        sw.WriteLine(string.Empty);

                        if (info.ReplacementGeometry.HasValue)
                        {
                            sw.WriteLine("\tReplacement geometry: (used when all stud are hidden)");
                            PrintGeometry(sw, meshFile, info.ReplacementGeometry.Value, false);
                        }
                    }
                }
            }

        }

        private void PrintGeometry(StreamWriter sw, MESH_FILE meshFile, MESH_DATA meshGeom, bool mainGeom)
        {
            string prefix = mainGeom ? string.Empty : "\t";
            if (!mainGeom)
            {
                sw.WriteLine($"{prefix}Vertex count: {meshGeom.Positions.Length}");
                sw.WriteLine($"{prefix}Index count: {meshGeom.Indices.Length}");
                sw.WriteLine(string.Empty);
            }

            sw.WriteLine($"{prefix}# 3D Data #");

            sw.WriteLine($"{prefix}Vertices:");
            for (int i = 0; i < meshGeom.Positions.Length; i++)
                sw.WriteLine($"{prefix}Vertex {i}: {meshGeom.Positions[i]}");
            sw.WriteLine(string.Empty);

            sw.WriteLine($"{prefix}Normals:");
            for (int i = 0; i < meshGeom.Positions.Length; i++)
                sw.WriteLine($"{prefix}Normal {i}: {meshGeom.Normals[i]}");
            sw.WriteLine(string.Empty);

            if (meshGeom.UVs != null && meshGeom.UVs.Length > 0)
            {
                sw.WriteLine($"{prefix}UVs:");
                for (int i = 0; i < meshGeom.Positions.Length; i++)
                    sw.WriteLine($"{prefix}UV {i}: {meshGeom.UVs[i]}");
                sw.WriteLine(string.Empty);
            }

            sw.WriteLine($"{prefix}Indices:");

            for (int i = 0; i < meshGeom.Indices.Length; i++)
                sw.WriteLine($"{prefix}Index {i} = Vertex {meshGeom.Indices[i].VertexIndex}");
            sw.WriteLine(string.Empty);

            sw.WriteLine($"{prefix}# Round edge shader data (outlines on bricks) #");
            if (mainGeom)
            {
                sw.WriteLine($"{prefix}Shader data:");
                for (int i = 0; i < meshFile.RoundEdgeShaderData.Length; i++)
                {
                    var coords = meshFile.RoundEdgeShaderData[i].Coords;
                    sw.WriteLine($"{prefix}Data {i}: " + string.Join(", ", coords));
                }
                sw.WriteLine(string.Empty);
            }

            sw.WriteLine($"{prefix}Indices references:");
            for (int i = 0; i < meshGeom.Indices.Length; i++)
                sw.WriteLine($"{prefix}Index {i} = Data {meshGeom.Indices[i].REShaderOffset}");
            sw.WriteLine(string.Empty);

            sw.WriteLine($"{prefix}# Average normals (used by shader) #");
            if (mainGeom)
            {
                sw.WriteLine($"{prefix}Average normals:");
                for (int i = 0; i < meshFile.AverageNormals.Length; i++)
                    sw.WriteLine($"{prefix}Avg Normal {i}: {meshFile.AverageNormals[i]}");

                sw.WriteLine(string.Empty);
            }

            sw.WriteLine($"{prefix}Indices references:");
            for (int i = 0; i < meshGeom.Indices.Length; i++)
                sw.WriteLine($"{prefix}Index {i} = Avg Normal {meshGeom.Indices[i].AverageNormalIndex}");

            sw.WriteLine(string.Empty);
        }
    }
}
