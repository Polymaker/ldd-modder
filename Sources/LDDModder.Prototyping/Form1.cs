using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.Prototyping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //TestPrimitives();
            //TestGFiles();
            SolveShaderData();
            //TestCustomBrick();
            //TestLddFiles();
        }

        private void TestPrimitives()
        {
            var loadedPrimitives = new List<Primitive>();

            foreach (var primitiveFile in Directory.EnumerateFiles(@"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\db\Primitives\", "*.xml"))
            {
                try
                {
                    using (var fs = File.Open(primitiveFile, FileMode.Open, FileAccess.Read))
                    {
                        var primitive = Primitive.FromXmlFile(fs);
                        if (primitive != null)
                            loadedPrimitives.Add(primitive);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }


            var connectorsUsage = loadedPrimitives.SelectMany(p => p.Connectors.Select(c => new ConnectorUse(p, c))).ToList();
            connectorsUsage.AddRange(loadedPrimitives.SelectMany(p => p.FlexBones.SelectMany(y => y.Connectors.Select(c => new ConnectorUse(p, c)))));

            //var studConnections = connectorsUsage.Select(x => x.Connector).OfType<Custom2DFieldConnector>();
            //var maleNodes = studConnections.Where(x=>x.SubType == 23).SelectMany(x => x).OrderBy(x => x.Value1).ThenBy(x => x.Value2).ThenBy(x => x.Value3).Select(x => x.ToString()).Distinct();
            //var femaleNodes = studConnections.Where(x => x.SubType == 22).SelectMany(x => x).OrderBy(x => x.Value1).ThenBy(x => x.Value2).ThenBy(x => x.Value3).Select(x => x.ToString()).Distinct();
            //Console.WriteLine("Custom2DField type 22:");
            //foreach (var studNode in femaleNodes)
            //    Console.WriteLine(studNode);
            //Console.WriteLine("Custom2DField type 23:");
            //foreach (var studNode in maleNodes)
            //    Console.WriteLine(studNode);

            foreach (var connectorGroup in connectorsUsage.GroupBy(x => new { x.Connector.Type, x.Connector.SubType })
                .OrderBy(x => x.Key.Type).ThenBy(x => x.Key.SubType))
            {
                Console.WriteLine($"{connectorGroup.Key.Type} connector subtype {connectorGroup.Key.SubType}");
                var primitives = connectorGroup.Select(x => x.Primitive).Distinct();
                foreach (var usedBy in primitives.OrderBy(x => x.Connectors.Count).Take(10))
                {
                    Console.WriteLine($"   Used by {usedBy.ID} - {usedBy.Name}");
                }
            }
        }

        private class ConnectorUse
        {
            public Primitive Primitive { get; set; }
            public Connector Connector { get; set; }

            public ConnectorUse(Primitive primitive, Connector connector)
            {
                Primitive = primitive;
                Connector = connector;
            }
        }

        private void TestGFiles()
        {
            string meshDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");

            using (var fs = File.OpenRead(Path.Combine(meshDirectory, "4286.g")))
            {
                var brickMesh = GFileReader.ReadMesh(fs);
                int triCtr = 0;
                foreach(var triangle in brickMesh.Geometry.Triangles)
                {
                    //var shaderDataVariations = new List<Vector2>();

                    //for (int j = 0; j < 3; j++)
                    //{
                    //    var index = triangle.Indices[j];
                    //    var shaderData = index.RoundEdgeData;
                    //    for (int n = 0; n < shaderData.Length; n++)
                    //    {
                    //        shaderData[n].X = (float)Math.Round(shaderData[n].X, 4);
                    //        shaderData[n].Y = (float)Math.Round(shaderData[n].Y, 4);
                    //    }
                    //    shaderDataVariations.AddRange(shaderData);
                    //}

                    //shaderDataVariations = shaderDataVariations.DistinctValues().ToList();
                    //var values1 = shaderDataVariations.Select(x => Math.Abs(x.X)).EqualsDistinct().OrderBy(x => x).ToList();
                    //var values2 = shaderDataVariations.Select(x => Math.Abs(x.Y)).EqualsDistinct().OrderBy(x => x).ToList();

                    Console.WriteLine($"Triangle {triCtr++} vertices:");
                    for (int j = 0; j < 3; j++)
                    {
                        var index = triangle.Indices[j];
                        var shaderData = index.RoundEdgeData;

                        Console.WriteLine($"  Vertex {j}: {index.Vertex}");

                        Console.WriteLine("  RE: " + string.Join(", ", shaderData.Take(6)));
                    }
                }

            }

            /*
            int meshRead = 0;

            foreach (var meshFilename in Directory.EnumerateFiles(meshDirectory, "*.g*"))
            {
                Console.WriteLine("Reading file " + Path.GetFileName(meshFilename));

                Mesh2 mesh = null;
                try
                {
                    using (var fs = File.OpenRead(meshFilename))
                        mesh = GFileReader.ReadMesh(fs);

                    //if (mesh.CullingInfos.Count(x=>x.CullingType == MeshCullingType.MainModel) > 1)
                    //    Trace.WriteLine("More than one type 2 variant!");

                    //foreach (var variant in mesh.CullingInfos)
                    //{
                    //    if (variant.CullingType == MeshCullingType.Stud && variant.Studs.Length > 1)
                    //    {
                    //        Trace.WriteLine("Type 1 with more than one stud!");
                    //    }
                    //    else if (variant.CullingType == MeshCullingType.Tube)
                    //    {
                    //        if (variant.Studs.Length > 1)
                    //            Trace.WriteLine("Type 8 with more than one stud!");
                    //        if (variant.UnknownData.Count > 1)
                    //            Trace.WriteLine("More than one unknown data!");
                    //        if (variant.Vertices != null && variant.Vertices.Length > 0)
                    //            Trace.WriteLine("Type 8 with vertex data!");
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                }

                if (++meshRead > 30)
                {
                    meshRead = 0;
                    GC.Collect();
                }
            }
            */
        }

        private void SolveShaderData()
        {
            string meshDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\");
            float edgeWidthRatio = 15.5f / 0.8f;

            using (var fs = File.OpenRead(Path.Combine(meshDirectory, "4286.g")))
            //using (var fs = File.OpenRead("4286.g.orig"))
            {
                var brickMesh = GFileReader.ReadMesh(fs);

                //var mesh = brickMesh.OriginalData.Value;
                //for (int t = 0; t < 3; t++)
                //{
                //    int sidx = mesh.GetShaderDataIndexFromOffset(mesh.Geometry.Indices[(9 * 3) + t].REShaderOffset);
                //    for (int x = 0; x < 3; x++)
                //    {
                //        var tmp = mesh.RoundEdgeShaderData[sidx].Coords[x * 2];
                //        mesh.RoundEdgeShaderData[sidx].Coords[x * 2] = mesh.RoundEdgeShaderData[sidx].Coords[(x * 2) + 1];
                //        mesh.RoundEdgeShaderData[sidx].Coords[(x * 2) + 1] = tmp;
                //    }
                //    mesh.RoundEdgeShaderData[sidx].Coords[1].X = mesh.RoundEdgeShaderData[sidx].Coords[1].X * -1f;

                //    mesh.RoundEdgeShaderData[sidx].Coords[5].X = mesh.RoundEdgeShaderData[sidx].Coords[5].X * -1f;
                //    if (t == 0)
                //    {
                //        mesh.RoundEdgeShaderData[sidx].Coords[0].X = mesh.RoundEdgeShaderData[sidx].Coords[0].X - 15.5F;
                //    }

                //}

                //using (var fs2 = File.Open(Path.Combine(meshDirectory, "4286.g"), FileMode.Create))
                //    GFileWriter.WriteMeshFile(fs2, mesh);

                int triCtr = 0;
                foreach (var tri in brickMesh.Geometry.Triangles)
                {
                    Console.WriteLine($"Triangle {triCtr++}:");
                    foreach (var idx in tri.Indices)
                    {
                        var values = new List<Vector2>();
                        idx.RoundEdgeData.Take(6).ToArray();

                        for (int i = 0; i < 3; i++)
                        {
                            var dist1 = Vector3.GetPlanarDistance(tri.Edges[i].P1.Position, tri.Edges[i].P2.Position, 
                                idx.Vertex.Position, tri.Normal);

                            var dist2 = Vector3.GetPlanarDistance(tri.Edges[i].P2.Position, tri.Edges[i].P1.Position,
                                idx.Vertex.Position, tri.Normal);

                            values.Add(Vector2.Empty);
                            values.Add(dist2);
                        }

                        var adjustedShaderData = idx.RoundEdgeData.Take(6).ToArray();

                        for (int n = 0; n < adjustedShaderData.Length; n++)
                        {
                            adjustedShaderData[n].X = (float)Math.Round(adjustedShaderData[n].X, 4);
                            adjustedShaderData[n].Y = (float)Math.Round(adjustedShaderData[n].Y, 4);

                            if (Math.Abs(adjustedShaderData[n].X) >= 100 && Math.Abs(adjustedShaderData[n].X) < 1000)
                            {
                                var sign = Math.Sign(adjustedShaderData[n].X);
                                adjustedShaderData[n].X -= sign * 100f;
                                adjustedShaderData[n].X = (float)Math.Round(adjustedShaderData[n].X / edgeWidthRatio, 4);
                                adjustedShaderData[n].Y = (float)Math.Round(adjustedShaderData[n].Y / edgeWidthRatio, 4);
                                adjustedShaderData[n].X += sign * 100f;
                            }
                            else
                            {
                                //adjustedShaderData[n].X = -1;
                                //adjustedShaderData[n].Y = -1;
                            }
                        }

                        Console.WriteLine($"  Pos: {idx.Vertex.Position.Rounded()}");
                        Console.WriteLine("  RE: " + string.Join(", ", adjustedShaderData.Take(6)));

                    }

                }
            }
        }

        private void TestRoundEdgeData(Mesh mesh)
        {
            for (int t = 0; t < mesh.Indices.Length / 3; t++)
            {
                var edgePerps = new Vector3[3];
                var vertices = new Vector3[3];

                for (int i = 0; i < 3; i++)
                {
                    vertices[i] = mesh.GetIndexVertex((t * 3) + i).Position;

                    var idx1 = mesh.Indices[(t * 3) + i];
                    var idx2 = mesh.Indices[(t * 3) + (i + 1) % 3];
                    var idx3 = mesh.Indices[(t * 3) + (i + 2) % 3];
                    edgePerps[i] = Vector3.GetPerpendicular(
                        mesh.Vertices[idx1.VertexIndex].Position,
                        mesh.Vertices[idx2.VertexIndex].Position,
                        mesh.Vertices[idx3.VertexIndex].Position);
                }

                for (int i = 0; i < 3; i++)
                {
                    var index = mesh.Indices[(t * 3) + i];
                    var pos1 = vertices[i];
                    Vector3 avgNormal = Vector3.Empty;
                    if (index.AverageNormalIndex < mesh.AverageNormals.Length)
                        avgNormal = mesh.AverageNormals[index.AverageNormalIndex];
                    var shaderData = mesh.EdgeShaderValues[index.ShaderDataIndex].Coords;
                    if (shaderData.Length > 6)
                        shaderData = shaderData.Take(6).ToArray();

                    for (int d = 0; d < 6; d++)
                    {
                        var coord = shaderData[d];
                        if (coord.X == 1000)
                            continue;

                        if (Math.Floor(d / 2f) == i)
                        {
                            if (Math.Abs(coord.X) == 100)
                            {

                            }
                        }
                        //FindValue(coord.X, i, d * 2, vertices, edgePerps);
                        //FindValue(coord.Y, i, (d * 2) + 1, vertices, edgePerps);
                    }
                }
            }
        }

        private void FindValue(float value, int vertIdx, int valueIdx, Vector3[] verts, Vector3[] edgePerps)
        {
            float edgeWidthRatio = 15.5f / 0.8f;

            var pos1 = verts[vertIdx];
            bool matched = false;

            bool MatchValue(float v)
            {
                var adj = v * edgeWidthRatio;
                if (valueIdx % 2 == 0)
                    adj += 100f;

                return Math.Abs(value).EqualOrClose(adj, 0.00001f);
            }

            for (int i = 0; i < 3; i++)
            {
                var pos2 = verts[i];
                var dist = Vector3.Distance(pos1, pos2);

                if (MatchValue(dist))
                {
                    matched = true;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                var perp = edgePerps[i];
                
            }

            if (!matched && Math.Abs(value) != 100)
            {

            }
        }

        private void TestLddFiles()
        {
            /*var test = LocalizationFile.Read(@"C:\Program Files (x86)\LEGO Company\LEGO Digital Designer\Assets\de\localizedStrings.loc");
            test.Save("localizedStrings_en.loc");*/
            //var test = LifFile.Open(@"test.lif");
            var lifFile = LifFile.Open(@"db.lif");
            //Path.GetDirectoryName(Application.ExecutablePath)
            //lifFile.ExtractTo("DB");
            using (var fs = File.Open("test.lif", FileMode.Create))
                lifFile.Save(fs);
            lifFile.Dispose();
            
        }

        public void TestCustomBrick()
        {
            var primitive = new Primitive()
            {
                ID = 33333,
                Name = "Test brick",
                Platform = LDD.Data.Platform.System,
                MainGroup = LDD.Data.MainGroup.Bricks,
                DesignVersion = 1
            };
            primitive.Aliases.Add(primitive.ID);

            //var mesh = new LDD.Meshes.Mesh
            //{
                
            //    Vertices = new Vertex[24],
            //    Indices = new IndexReference[12 * 3],
            //    Type = MeshType.Standard
            //};
            var builder = new MeshBuilder();
            var normals = new Vector3[]
            {
                new Vector3(0,1,0),
                new Vector3(1,0,0),
                new Vector3(0,0,-1),
                new Vector3(-1,0,0),
                new Vector3(0,-1,0),
                new Vector3(0,0,1),
            };

            //int curVert = 0;
            //int curIdx = 0;

            var brickSize = new Vector3(1.6f, 0.32f, 0.8f);
            var posOffset = new Vector3(0.4f, brickSize.Y / 2f, 0f);
            for (int i = 0; i < 6; i++)
            {
                var curDir = normals[i];
                
                float faceDist;
                float hDist;
                float vDist;
                Vector3 left;
                Vector3 right;
                Vector3 up;
                Vector3 down;
                if (Math.Abs(curDir.X) == 1)
                {
                    int sign = Math.Sign(curDir.X);
                    down = Vector3.UnitZ * sign * -1;
                    up = Vector3.UnitZ * sign ;
                    left = Vector3.UnitY * -1f;
                    right = Vector3.UnitY;
                    hDist = brickSize.Y / 2f;
                    vDist = brickSize.Z / 2f;
                    faceDist = brickSize.X / 2f;
                }
                else if (Math.Abs(curDir.Y) == 1)
                {
                    int sign = Math.Sign(curDir.Y);
                    left = Vector3.UnitX * -1;
                    right = Vector3.UnitX;
                    up = Vector3.UnitZ * sign * -1;
                    down = Vector3.UnitZ * sign;
                    hDist = brickSize.X / 2f;
                    vDist = brickSize.Z / 2f;
                    faceDist = brickSize.Y / 2f;
                }
                else//Z
                {
                    int sign = Math.Sign(curDir.Z);
                    left = Vector3.UnitX * sign * -1;
                    right = Vector3.UnitX * sign;
                    down = Vector3.UnitY * -1f;
                    up = Vector3.UnitY;
                    hDist = brickSize.X / 2f;
                    vDist = brickSize.Y / 2f;
                    faceDist = brickSize.Z / 2f;
                }

                var faceCenter = (curDir * faceDist) + posOffset;
                var v1 = faceCenter + (left * hDist) + (down * vDist);
                var v2 = faceCenter + (right * hDist) + (down * vDist);
                var v3 = faceCenter + (left * hDist) + (up * vDist);
                var v4 = faceCenter + (right * hDist) + (up * vDist);

                builder.AddTriangle(new Vertex(v1, curDir), new Vertex(v2, curDir), new Vertex(v3, curDir));
                builder.AddTriangle(new Vertex(v2, curDir), new Vertex(v4, curDir), new Vertex(v3, curDir));

                //mesh.Vertices[curVert] = new Vertex(v1, curDir);
                //mesh.Vertices[curVert + 1] = new Vertex(v2, curDir);
                //mesh.Vertices[curVert + 2] = new Vertex(v3, curDir);
                //mesh.Vertices[curVert + 3] = new Vertex(v4, curDir);

                //mesh.Indices[curIdx++] = new IndexReference(curVert);
                //mesh.Indices[curIdx++] = new IndexReference(curVert + 1);
                //mesh.Indices[curIdx++] = new IndexReference(curVert + 2);

                //mesh.Indices[curIdx++] = new IndexReference(curVert + 1);
                //mesh.Indices[curIdx++] = new IndexReference(curVert + 3);
                //mesh.Indices[curIdx++] = new IndexReference(curVert + 2);

                //curVert += 4;
            }

            /*
            mesh.AverageNormals = new Vector3[]
            {
                new Vector3(83,0,0),
                new Vector3(0,0,1)
            };

            mesh.EdgeShaderValues = new RoundEdgeData[]
                {
                    new RoundEdgeData(
                        new Vector3(-115.5f,0,-115.5f),
                        new Vector3(0,100,0),
                        new Vector3(-100,15.5f,100),
                        new Vector3(0,-100,15.5f)),
                    new RoundEdgeData(
                        new Vector3(-115.5f,15.5f,-100),
                        new Vector3(0,115.5f,0),
                        new Vector3(-115.5f,0,100),
                        new Vector3(15.5f,-100,15.5f)),
                    new RoundEdgeData(
                        new Vector3(-100,0,-115.5f),
                        new Vector3(15.5f,100,15.5f),
                        new Vector3(-100,15.5f,115.5f),
                        new Vector3(0,-115.5f,0)),
                    new RoundEdgeData(
                        new Vector3(-150f,0,-115.5f),
                        new Vector3(0,100,0),
                        new Vector3(-100,15.5f,100),
                        new Vector3(0,-100,15.5f)),
                    new RoundEdgeData(
                        new Vector3(-115.5f,15.5f,-100),
                        new Vector3(0,115.5f,0),
                        new Vector3(-150f,0,100),
                        new Vector3(15.5f,-100,15.5f)),
                    new RoundEdgeData(
                        new Vector3(-100,0,-115.5f),
                        new Vector3(15.5f,100,15.5f),
                        new Vector3(-100,15.5f,150f),
                        new Vector3(0,-115.5f,0)),
                };

            //mesh.GenerateFaceNormals();

            for (int i = 0; i < mesh.Indices.Length; i++)
            {
                mesh.Indices[i].ShaderDataIndex = i % mesh.EdgeShaderValues.Length;
            }

            mesh.CullingInfos = new CullingInfo[]
            {
                new CullingInfo(MeshCullingType.MainModel,0, mesh.Vertices.Length, 0, mesh.Indices.Length)
            };*/

            builder.AddStud(new Vector3(0, brickSize.Y, 0));
            builder.AddStud(new Vector3(0.8f, brickSize.Y, 0));
            var mesh = builder.BuildMesh();

            primitive.Bounding = BoundingBox.FromVertices(mesh.Vertices);
            primitive.GeometryBounding = primitive.Bounding;

            string primitiveDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\");

            var xmlDoc = new XDocument(primitive.SerializeToXml());
            xmlDoc.Save(Path.Combine(primitiveDirectory, $"{primitive.ID}.xml"));

            var meshFilename = Path.Combine(primitiveDirectory, "LOD0", $"{primitive.ID}.g");
            using (var fs = File.Open(meshFilename, FileMode.Create))
                GFileReader.Write(fs, mesh);

            //Mesh result;
            //using (var fs = File.Open(meshFilename, FileMode.Open))
            //    result = GFileReader.Read(fs);
        }
    }
}
