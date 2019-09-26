using LDDModder.BrickEditor.Editing;
using LDDModder.LDD.Data;
using LDDModder.LDD.Files;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LDDModder.BrickEditor
{
    public partial class BrickCreatorWindow : Form
    {
        private Assimp.AssimpContext AssimpContext;
        private BindingList<BrickMeshObject> BrickMeshes;
        private List<Platform> Platforms;
        private List<MainGroup> Groups;
        private string RepoFolder;

        public BrickCreatorWindow()
        {
            InitializeComponent();
            BrickMeshes = new BindingList<BrickMeshObject>();
            BrickMeshes.ListChanged += BrickMeshes_ListChanged;
            Platforms = new List<Platform>();
            Groups = new List<MainGroup>();
        }

        private void BrickMeshes_ListChanged(object sender, ListChangedEventArgs e)
        {
            RemoveAllMeshButton.Enabled = BrickMeshes.Any();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Modding.PartPackage.CreateLDDPackages();
            //string meshDir = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");
            //var project = PartProject.CreateFromLdd(meshDir, 10130);
            //project.SaveUncompressed("10130");

            var curDir = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            int loopCount = 0;

            while (curDir.Name != "Sources" || ++loopCount > 10)
                curDir = curDir.Parent;
            if (curDir.Name == "Sources")
                RepoFolder = curDir.Parent.FullName;

            var asdf = MeshFile.Read(@"D:\CAD\Blender\Orig\32200.g");
            asdf.Save(@"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\db\Primitives\LOD0\32200.g");
            //var part = LDD.Data.LDDPartFiles.Read(LDD.LDDEnvironment.Current.ApplicationDataPath + "\\db", 32200);
            //Utilities.LddMeshExporter.ExportLddPart(part,
            //            Path.Combine(RepoFolder, "LDD Bricks", $"{part.PartID} flex.dae"), "collada");

            InitializeData();
            InitializeUI();
        }
        struct BoneInfo
        {
            public int Index { get; set; }
            public Simple3D.Vector3 Pos { get; set; }

            public BoneInfo(int index, Vector3 pos)
            {
                Index = index;
                Pos = pos;
            }
        }

        private void MakeFlexible(LDDPartFiles part)
        {
            var part2 = LDD.Data.LDDPartFiles.Read(LDD.LDDEnvironment.Current.ApplicationDataPath + "\\db", 32200);
            //Utilities.LddMeshExporter.ExportLddPart(part,
            //            Path.Combine(RepoFolder, "LDD Bricks", $"{part.PartID}.dae"), "collada");
            //var bounding = part.Info.Bounding;
            //var vPosXs = part.MainModel.Vertices.Select(v => v.Position.Rounded(3).X)
            //    .Where(x => x > 1.6f && x < bounding.MaxX - 1.62f)
            //    .Distinct().ToList();
            //vPosXs.Sort();


            //var flexLen = part.Info.Bounding.SizeX - 3.2f;
            //int nbBones = (int)Math.Round(flexLen / 0.4f);
            //float boneSpacing = flexLen / (float)nbBones;
            var bonePositions = new List<BoneInfo>();

            foreach(var flexBone in part2.Info.FlexBones)
                bonePositions.Add(new BoneInfo(flexBone.ID, flexBone.Transform.GetPosition()));

            //bonePositions.Add(new BoneInfo(bonePositions.Count, new Simple3D.Vector3(bounding.MinX, 0, 0)));
            //bonePositions.Add(new BoneInfo(bonePositions.Count, new Simple3D.Vector3(bounding.MinX + 0.8f, 0, 0)));
            //bonePositions.Add(new BoneInfo(bonePositions.Count, new Simple3D.Vector3(bounding.MinX + 1.6f, 0, 0)));

            //for (int i = 0; i < vPosXs.Count; i++)
            //    bonePositions.Add(new BoneInfo(bonePositions.Count, 
            //        new Simple3D.Vector3(vPosXs[i], 0, 0)));

            //bonePositions.Add(new BoneInfo(bonePositions.Count, new Simple3D.Vector3(bounding.MaxX - 1.6f, 0, 0)));
            //bonePositions.Add(new BoneInfo(bonePositions.Count, new Simple3D.Vector3(bounding.MaxX - 0.8f, 0, 0)));
            //bonePositions.Add(new BoneInfo(bonePositions.Count, new Simple3D.Vector3(bounding.MaxX, 0, 0)));
            part.Info.FlexBones.AddRange(part2.Info.FlexBones);



            //for (int i = 0; i < bonePositions.Count; i++)
            //{
            //    var curBone = bonePositions[i];
            //    var flexBone = new FlexBone()
            //    {
            //        ID = curBone.Index,
            //    };
            //    flexBone.Transform.Translation = curBone.Pos;

            //    if (curBone.Index > 0)
            //        flexBone.ConnectionCheck = new Tuple<int, int, int>(0, curBone.Index - 1, 2);

            //    if (curBone.Index % 2 == 0)
            //    {
            //        if (curBone.Index > 0)
            //        {
            //            flexBone.Connectors.Add(new LDD.Primitives.Connectors.BallConnector()
            //            {
            //                SubType = 999003,
            //                FlexAttributes = "-0.06,0.06,20,10,10"
            //            });
            //        }

            //        if (i + 1 < bonePositions.Count)
            //        {
            //            var conn = new LDD.Primitives.Connectors.BallConnector()
            //            {
            //                SubType = 999000
            //            };
            //            conn.Transform.Translation = bonePositions[i + 1].Pos - curBone.Pos;
            //            flexBone.Connectors.Add(conn);
            //        }
            //    }
            //    else
            //    {
            //        flexBone.Connectors.Add(new LDD.Primitives.Connectors.BallConnector()
            //        {
            //            SubType = 999001,
            //            FlexAttributes = "-0.06,0.06,20,10,10"
            //        });


            //        if (i + 1 < bonePositions.Count)
            //        {
            //            var conn = new LDD.Primitives.Connectors.BallConnector()
            //            {
            //                SubType = 999002
            //            };
            //            conn.Transform.Translation = bonePositions[i + 1].Pos - curBone.Pos;
            //            flexBone.Connectors.Add(conn);
            //        }
            //    }

            //    part.Info.FlexBones.Add(flexBone);
            //}

            foreach (var vert in part.MainModel.Vertices)
            {
                var prevBones = bonePositions.Where(b => b.Pos.X <= vert.Position.X)
                    .OrderByDescending(b => b.Pos.X).Take(2);
                var nextBones = bonePositions.Where(b => b.Pos.X > vert.Position.X)
                    .OrderBy(b => b.Pos.X).Take(2);

                var vertBones = prevBones.Concat(nextBones).OrderBy(b => b.Pos.X).ToList();
                var maxDist = vertBones.Max(b => Vector3.Distance(b.Pos, vert.Position));
                var totalDist = vertBones.Sum(b => Vector3.Distance(b.Pos, vert.Position));
                float weightSum = 0;
                var weights = new List<float>();
                if (vertBones.Count == 0)
                {

                }
                foreach (var b in vertBones)
                {
                    var vWeight = Vector3.Distance(b.Pos, vert.Position) / maxDist;
                    vWeight = (1f / (float)vertBones.Count) + (1f - vWeight);
                    weightSum += vWeight;
                    weights.Add(vWeight);
                }

                for (int i = 0; i < vertBones.Count; i++)
                {
                    var vWeight = weights[i] / weightSum;
                    vert.BoneWeights.Add(new BoneWeight(vertBones[i].Index, vWeight));
                }
            }
        }

        private void InitializeUI()
        {
            BrickMeshGridView.AutoGenerateColumns = false;
            
            BrickMeshGridView.DataSource = BrickMeshes;

            PlatformCombo.DataSource = Platforms;
            FilterGroupComboBox();
        }

        private void InitializeData()
        {
            AssimpContext = new Assimp.AssimpContext();
            var dataXml = XDocument.Load("Data\\LddData.xml");

            foreach (var elem in dataXml.Root.Element("Platforms").Elements())
            {
                Platforms.Add(new Platform(int.Parse(elem.Attribute("id").Value), elem.Attribute("name").Value));
            }

            foreach (var elem in dataXml.Root.Element("Groups").Elements())
            {
                Groups.Add(new MainGroup(int.Parse(elem.Attribute("id").Value), elem.Attribute("name").Value));
            }
        }

        private void FilterGroupComboBox()
        {
            var selectedPlatform = PlatformCombo.SelectedItem as Platform;
            if (selectedPlatform != null)
            {
                var platformGroups = Groups.Where(x => selectedPlatform.ID <= x.ID && x.ID <= selectedPlatform.ID + 100);
                GroupCombo.DataSource = platformGroups.ToList();
            }
        }

        #region GridView Management

        private void UpdateGridRowState(DataGridViewRow row)
        {
            if (!(row.DataBoundItem is BrickMeshObject brickMesh))
                return;

            var decorationCell = (DataGridViewTextBoxCell)row.Cells[DecorationNumberColumn.Name];
            //var mainModelCell = (DataGridViewCheckBoxCell)row.Cells[MainModelChkColumn.Index];

            decorationCell.ReadOnly = !brickMesh.IsTextured || brickMesh.IsMainModel;

            if (brickMesh.IsMainModel && brickMesh.DecorationID.HasValue)
            {
                brickMesh.DecorationID = null;
                BrickMeshGridView.InvalidateCell(decorationCell);
            }
        }

        private void BrickMeshGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateGridRowState(BrickMeshGridView.Rows[e.RowIndex]);
        }

        private void BrickMeshGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (BrickMeshGridView.Columns[e.ColumnIndex] == DecorationNumberColumn)
            {
                bool isEmpty = e.FormattedValue == null || (e.FormattedValue is string str && string.IsNullOrEmpty(str));

                if (!isEmpty && !int.TryParse(e.FormattedValue as string, out _))
                {
                    e.Cancel = true;
                    BrickMeshGridView.CancelEdit();
                }
            }
        }

        private void BrickMeshGridView_SelectionChanged(object sender, EventArgs e)
        {
            RemoveMeshButton.Enabled = BrickMeshGridView.CurrentRow != null;
        }

        #endregion

        private class BrickMeshObject
        {
            public string MeshFile { get; set; }
            public Assimp.Scene MeshScene { get; set; }
            public Assimp.Mesh Mesh { get; set; }
            public string MeshName { get; set; }
            public bool IsTextured => Mesh?.HasTextureCoords(0) ?? false;
            public bool IsFlexible => Mesh?.BoneCount > 0;
            public bool IsMainModel { get; set; }
            public int? DecorationID { get; set; }

            public string Info
            {
                get
                {
                    if (Mesh != null)
                        return $"{ Mesh.FaceCount} Triangles";
                    return string.Empty;
                }
            }
        }

        private void AddMeshButton_Click(object sender, EventArgs e)
        {
            using(var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mesh files (*.dae, *.obj, *.stl)|*.dae;*.obj;*.stl|Wavefront (*.obj)|*.obj|Collada (*.dae)|*.dae|STL (*.stl)|*.stl|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                    ImportModel(ofd.FileName);
            }
        }

        private void RemoveMeshButton_Click(object sender, EventArgs e)
        {
            if (BrickMeshGridView.CurrentRow != null)
            {
                var mesh = BrickMeshGridView.CurrentRow.DataBoundItem as BrickMeshObject;
                BrickMeshes.Remove(mesh);
            }
        }

        private void ImportModel(string filename)
        {
            try
            {
                ImportExportProgress.Value = 0;
                ImportExportProgress.Visible = true;

                if (filename.EndsWith("g"))
                {
                    var lddMeshFile = MeshFile.Read(filename);
                    ImportExportProgress.Value = 20;
                    var assimpMesh = LDD.Meshes.MeshConverter.ConvertFromLDD(lddMeshFile);
                    var brickMesh = new BrickMeshObject()
                    {
                        Mesh = assimpMesh,
                        MeshFile = filename,
                        MeshName = Path.GetFileNameWithoutExtension(filename),
                        IsMainModel = !assimpMesh.HasTextureCoords(0)
                    };

                    BrickMeshes.Add(brickMesh);

                    ImportExportProgress.Value = 100;
                }
                else
                {
                    ImportAssimpModel(filename);
                }

                if (string.IsNullOrEmpty(IDTextbox.Text) &&
                    int.TryParse(Path.GetFileNameWithoutExtension(filename), out int partID))
                {
                    IDTextbox.Text = partID.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There was a problem importing the mesh.\r\nCheck 'error.log' file for details.");
                WriteErrorLog($"Error loading mesh file '{filename}':\r\n{ex.ToString()}");
            }

            HideProgressDelayed();
        }

        private void ImportAssimpModel(string filename)
        {
            var scene = AssimpContext.ImportFile(filename,
                    Assimp.PostProcessSteps.GenerateNormals /*| Assimp.PostProcessSteps.PreTransformVertices*/);

            ImportExportProgress.Value = 20;

            if (scene != null)
            {
                int counter = 1;
                int currentDecID = 1;
                if (BrickMeshes.Any(x => x.DecorationID.HasValue))
                    currentDecID = BrickMeshes.Where(x => x.DecorationID.HasValue).Max(x => x.DecorationID.Value) + 1;

                foreach (var mesh in scene.Meshes)
                {
                    var brickMesh = new BrickMeshObject()
                    {
                        Mesh = mesh,
                        MeshScene = scene,
                        MeshFile = filename,
                        MeshName = mesh.Name,
                        IsMainModel = !mesh.HasTextureCoords(0)
                    };
                    if (brickMesh.IsTextured)
                        brickMesh.DecorationID = currentDecID++;

                    BrickMeshes.Add(brickMesh);

                    float progress = counter++ / (float)scene.MeshCount;
                    ImportExportProgress.Value = 20 + (int)(progress * 80);
                }
            }
        }

        private void CreateBrickButton_Click(object sender, EventArgs e)
        {
            if (!ValidateBrick())
                return;

            ImportExportProgress.Value = 0;
            ImportExportProgress.Visible = true;

            try
            {
                CreateBrickFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem creating the brick files.\r\nCheck 'error.log' file for details.");
                WriteErrorLog($"Error creating the brick files:\r\n{ex.ToString()}");
            }

            HideProgressDelayed();
        }

        private void CreateBrickFiles()
        {
            int decorationID = 1;
            var decoratedMeshes = BrickMeshes.Where(x => x.DecorationID.HasValue);

            foreach (var grp in decoratedMeshes.GroupBy(x => x.DecorationID.Value).OrderBy(x => x.Key))
            {
                foreach (var mesh in grp)
                    mesh.DecorationID = decorationID;
                decorationID++;
            }

            var partMesh = new LDDPartFiles
            {
                PartID = int.Parse(IDTextbox.Text),
                Info = new Primitive()
                {
                    ID = int.Parse(IDTextbox.Text),
                    Name = NameTextbox.Text,
                    Platform = PlatformCombo.SelectedItem as Platform,
                    MainGroup = GroupCombo.SelectedItem as MainGroup
                }
            };

            var primitive = partMesh.Info;

            var validMeshes = BrickMeshes.Where(x => x.IsMainModel || x.DecorationID.HasValue).ToList();

            foreach (var meshGroup in validMeshes.GroupBy(x => x.DecorationID).OrderBy(x => x.Key ?? 0))
            {
                var partialMesh = CreatePartialMesh(meshGroup);

                if (!meshGroup.Key.HasValue)
                    partMesh.MainModel = partialMesh;
                else
                    partMesh.DecorationMeshes.Add(partialMesh);

                var progress = partMesh.AllMeshes.Count() / (float)validMeshes.Count;
                ImportExportProgress.Value = (int)(progress * 80);
            }

            partMesh.ComputeAverageNormals();
            partMesh.ComputeRoundEdgeShader();

            ImportExportProgress.Value = 95;

            if (partMesh.DecorationMeshes.Any())
            {
                partMesh.Info.SubMaterials = Enumerable.Range(0, partMesh.DecorationMeshes.Count + 1).ToArray();
            }

            partMesh.Info.Bounding = partMesh.GetBoundingBox();
            partMesh.Info.GeometryBounding = partMesh.Info.Bounding;

            if (BrickMeshes[0].IsFlexible)
            {
                int boneID = 0;
                var scene = BrickMeshes[0].MeshScene;
                var meshNode = Assimp.AssimpHelper.GetMeshNode(BrickMeshes[0].MeshScene, BrickMeshes[0].Mesh);
                var meshTrans = Assimp.AssimpHelper.GetFinalTransform(meshNode).ToLDD();
                int boneCount = BrickMeshes[0].Mesh.BoneCount;
                foreach (var bone in BrickMeshes[0].Mesh.Bones)
                {
                    var boneNode = scene.RootNode.FindNode(bone.Name);
                    var boneTrans = Assimp.AssimpHelper.GetFinalTransform(boneNode).ToLDD();
                    //boneTrans = boneTrans * meshTrans;
                    var flexBone = new FlexBone()
                    {
                        ID = boneID++,
                        Transform = Transform.FromMatrix(boneTrans)
                    };
                    if (flexBone.ID > 0)
                    {
                        flexBone.ConnectionCheck = new Tuple<int, int, int>(0, flexBone.ID - 1, 1);
                    }

                    //    if (flexBone.ID % 2 == 0)
                    //    {
                    //        if (flexBone.ID > 0)
                    //        {
                    //            flexBone.Connectors.Add(new LDD.Primitives.Connectors.BallConnector()
                    //            {
                    //                SubType = 999003,
                    //                FlexAttributes = "-0.06,0.06,20,10,10"
                    //            });
                    //        }

                    //        if (flexBone.ID + 1 < boneCount)
                    //        {
                    //            var conn = new LDD.Primitives.Connectors.BallConnector()
                    //            {
                    //                SubType = 999000
                    //            };
                    //            conn.Transform.Translation = bonePositions[i + 1].Pos - curBone.Pos;
                    //            flexBone.Connectors.Add(conn);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        flexBone.Connectors.Add(new LDD.Primitives.Connectors.BallConnector()
                    //        {
                    //            SubType = 999001,
                    //            FlexAttributes = "-0.06,0.06,20,10,10"
                    //        });


                    //        if (flexBone.ID + 1 < boneCount
                    //        {
                    //            var conn = new LDD.Primitives.Connectors.BallConnector()
                    //            {
                    //                SubType = 999002
                    //            };
                    //            conn.Transform.Translation = bonePositions[i + 1].Pos - curBone.Pos;
                    //            flexBone.Connectors.Add(conn);
                    //        }
                    //    }

                    //    part.Info.FlexBones.Add(flexBone);
                    //}
                    primitive.FlexBones.Add(flexBone);
                }
            }

            if (!string.IsNullOrEmpty(RepoFolder) && Debugger.IsAttached)
            {
                Utilities.LddMeshExporter.ExportRoundEdge(partMesh.MainModel.Geometry,
                    Path.Combine(RepoFolder, "LDD Bricks", $"{partMesh.PartID} RE.dae"), "collada");
            }
            //MakeFlexible(partMesh);

            ImportExportProgress.Value = 100;

            if (SaveInLddCheckBox.Checked)
            {
                string partName = partMesh.PartID.ToString();
                var primitiveDir = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db", "Primitives");
                var meshDir = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db", "Primitives", "LOD0");

                if (partMesh.CheckFilesExists(primitiveDir, partName) || partMesh.CheckFilesExists(meshDir, partName))
                {
                    var res = MessageBox.Show("Some files already exists. Do you want to override them?", "Confirm save",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No)
                        return;
                }

                if (CreateMeshesCheckBox.Checked)
                    partMesh.SaveMeshes(meshDir, partName);

                if (CreatePrimitiveCheckBox.Checked)
                    partMesh.SavePrimitive(primitiveDir, partName);

                MessageBox.Show("Brick files created succesfully.");
            }
            else
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.FileName = partMesh.Info.ID.ToString();

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string baseName = Path.GetFileNameWithoutExtension(sfd.FileName);
                        string targetDir = Path.GetDirectoryName(sfd.FileName);
                        if (partMesh.CheckFilesExists(targetDir, baseName))
                        {
                            var res = MessageBox.Show("Some files already exists. Do you want to override them?", "Confirm save",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (res == DialogResult.No)
                                return;
                        }

                        if (CreateMeshesCheckBox.Checked && CreatePrimitiveCheckBox.Checked)
                            partMesh.SaveAll(targetDir, baseName);
                        else if (CreateMeshesCheckBox.Checked)
                            partMesh.SaveMeshes(targetDir, baseName);
                        else if (CreatePrimitiveCheckBox.Checked)
                            partMesh.SavePrimitive(targetDir, baseName);

                        MessageBox.Show("Brick files created succesfully.");
                    }
                }
            }
        }

        private MeshFile CreatePartialMesh(IEnumerable<BrickMeshObject> brickMeshes)
        {
            var builder = new GeometryBuilder();

            foreach (var brickMesh in brickMeshes)
            {
                bool useTexture = brickMesh.IsTextured && !brickMesh.IsMainModel;
                var meshNode = Assimp.AssimpHelper.GetMeshNode(brickMesh.MeshScene, brickMesh.Mesh);
                var meshTransform = Assimp.AssimpHelper.GetFinalTransform(meshNode).ToLDD();

                //if (brickMesh.Mesh.HasBones)
                //{
                //    var rootBone = brickMesh.Mesh.Bones[0];
                //    var boneNode = brickMesh.MeshScene.RootNode.FindNode(rootBone.Name);
                //    var boneTrans = rootBone.OffsetMatrix.ToLDD().Inverted();
                //    var nodeTrans = boneNode.Transform.ToLDD();

                //    var testTrans = meshTransform * nodeTrans * boneTrans;
                //    var testPt = testTrans.TransformPosition(new Vector3(14.41003f, -0.1385641f, -0.07999998f));
                //    //meshTransform = testTrans;
                //    meshTransform = boneTrans * meshTransform * nodeTrans;
                //}

                var boneWeights = new Dictionary<int, List<BoneWeight>>();
                if (brickMesh.Mesh.HasBones)
                {
                    for (int i = 0; i < brickMesh.Mesh.BoneCount; i++)
                    {
                        var bone = brickMesh.Mesh.Bones[i];
                        foreach (var vw in bone.VertexWeights)
                        {
                            if (!boneWeights.ContainsKey(vw.VertexID))
                                boneWeights.Add(vw.VertexID, new List<BoneWeight>());
                            boneWeights[vw.VertexID].Add(new BoneWeight(i, vw.Weight));
                        }
                    }
                }

                foreach (var face in brickMesh.Mesh.Faces)
                {
                    if (face.IndexCount != 3)
                        continue;

                    Vertex[] verts = new Vertex[3];

                    for (int i = 0; i < 3; i++)
                    {
                        int vIndex = face.Indices[i];
                        var vPos = brickMesh.Mesh.Vertices[vIndex].ToLDD();
                        vPos = meshTransform.TransformPosition(vPos);

                        var vNorm = brickMesh.Mesh.Normals[vIndex].ToLDD();
                        vNorm = meshTransform.TransformVector(vNorm);

                        var vTex = Simple3D.Vector2.Empty;
                        if (useTexture)
                            vTex = brickMesh.Mesh.TextureCoordinateChannels[0][vIndex].ToLDD().Xy;

                        verts[i] = new Vertex(vPos, vNorm, vTex);

                        if (brickMesh.Mesh.HasBones)
                            verts[i].BoneWeights.AddRange(boneWeights[i]);
                    }

                    builder.AddTriangle(verts[0], verts[1], verts[2]);
                }
            }
            var file = new MeshFile(builder.GetGeometry());
            file.CreateDefaultCulling();
            return file;
        }

        private bool ValidateBrick()
        {
            var errorMessages = new List<string>();
            //var warningMessages = new List<string>();

            if (string.IsNullOrEmpty(IDTextbox.Text))
                errorMessages.Add("Enter a brick ID");
            else if(!int.TryParse(IDTextbox.Text, out _))
                errorMessages.Add("Brick ID must be numeric");

            if (string.IsNullOrWhiteSpace(NameTextbox.Text) && CreatePrimitiveCheckBox.Checked)
                errorMessages.Add("Enter a brick name");


            if (!BrickMeshes.Any())
                errorMessages.Add("At least one mesh is required");
            else if (!BrickMeshes.Any(x => x.IsMainModel))
                errorMessages.Add("Main mesh is not defined");

            if (BrickMeshes.Any(x => !x.IsMainModel && !x.DecorationID.HasValue))
            {
                if (MessageBox.Show("There are some meshes that are neither main meshes or decoration meshes."
                    + Environment.NewLine + "Those meshes will be ignored. Do you want to proceed?", 
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }

            if (BrickMeshes.Any(x => x.IsFlexible) &&
                (!BrickMeshes.All(x => x.IsFlexible) ||
                BrickMeshes.Select(x => x.MeshScene).Distinct().Count() > 1))
            {
                if (MessageBox.Show("To make a part flexible all meshes needs to have bone weights and have the same bones."
                    + Environment.NewLine + "Those meshes will be ignored. Do you want to proceed?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }

            if (errorMessages.Any())
            {
                string errorMsg = string.Join("\r\n", errorMessages.Select(x => " - " + x));
                MessageBox.Show("Please fix the following issues:\r\n" + errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return !errorMessages.Any();
        }

        private void ClearAllButton_Click(object sender, EventArgs e)
        {
            IDTextbox.Text = string.Empty;
            NameTextbox.Text = string.Empty;
            PlatformCombo.SelectedIndex = 0;

            BrickMeshes.Clear();
        }

        private void RemoveAllMeshButton_Click(object sender, EventArgs e)
        {
            BrickMeshes.Clear();
        }

        private void HideProgressDelayed(int delay = 1500)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay);
                Invoke((Action)(() => ImportExportProgress.Visible = false));
            });
        }

        private void PlatformCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGroupComboBox();
        }

        private void WriteErrorLog(string message)
        {
            using (var fs = File.Open("error.log", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(message);
            }
        }

        private void CreatePrimitiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CreateBrickButton.Enabled = CreateMeshesCheckBox.Checked || CreatePrimitiveCheckBox.Checked;
        }

        private void CreateMeshesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CreateBrickButton.Enabled = CreateMeshesCheckBox.Checked || CreatePrimitiveCheckBox.Checked;
        }
    }
}
