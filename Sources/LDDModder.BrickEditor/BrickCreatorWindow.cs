using LDDModder.LDD.Data;
using LDDModder.LDD.Meshes;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor
{
    public partial class BrickCreatorWindow : Form
    {
        private Assimp.AssimpContext AssimpContext;
        private BindingList<BrickMeshObject> BrickMeshes;

        public BrickCreatorWindow()
        {
            InitializeComponent();
            BrickMeshes = new BindingList<BrickMeshObject>();
        }

       
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AssimpContext = new Assimp.AssimpContext();

            //ListPlatformsGroups();

            InitializeUI();
        }

        private void InitializeUI()
        {
            BrickMeshGridView.AutoGenerateColumns = false;
            PlatformCombo.DataSource = new Platform[] { Platform.System, Platform.Technic, Platform.ActionFigures };
            BrickMeshGridView.DataSource = BrickMeshes;
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

        private void ListPlatformsGroups()
        {
            var PrimitiveDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\Primitives\");

            var platformGroups = new Dictionary<Platform, List<MainGroup>>();
            var platforms = new List<Platform>();
            var groups = new List<MainGroup>();
            var groupUsage = new Dictionary<MainGroup, int>();
            foreach (var xmlFile in Directory.GetFiles(PrimitiveDirectory, "*.xml"))
            {
                try
                {
                    var info = Primitive.FromXmlFile(xmlFile);
                    if (!platformGroups.ContainsKey(info.Platform))
                        platformGroups.Add(info.Platform, new List<MainGroup>());

                    if (!platformGroups[info.Platform].Contains(info.MainGroup))
                        platformGroups[info.Platform].Add(info.MainGroup);

                    if (!platforms.Contains(info.Platform))
                        platforms.Add(info.Platform);
                    if (!groups.Contains(info.MainGroup))
                        groups.Add(info.MainGroup);

                    if (!groupUsage.ContainsKey(info.MainGroup))
                        groupUsage.Add(info.MainGroup, 0);

                    groupUsage[info.MainGroup]++;
                }
                catch { }
            }

            foreach (var grp in groups.GroupBy(x => x.ID).Where(g => g.Count() > 1).ToList())
            {
                var mostUsed = grp.OrderByDescending(x => groupUsage[x]).First();
                groups.RemoveAll(x => x.ID == grp.Key && x.Name != mostUsed.Name);
            }

            foreach (var plat in platforms.OrderBy(x => x.ID))
            {
                var propName = plat.Name.ToLower();
                propName = propName.Replace(',', ' ');
                propName = propName.Replace("  ", " ");
                propName = Regex.Replace(propName, "(^|\\s)(\\w)", (m) =>
                {
                    return m.Groups[2].Value.ToUpper();
                });
                Console.WriteLine($"public static readonly Platform {propName} = new Platform({plat.ID}, \"{plat.Name}\");");
            }

            foreach (var grp in groups.OrderBy(x => x.ID))
            {
                var propName = grp.Name.ToLower();
                propName = propName.Replace(',', ' ');
                propName = propName.Replace("  ", " ");
                propName = propName.Replace("w/", "With");
                propName = Regex.Replace(propName, "(^|\\s)(\\w)", (m) =>
                {
                    return m.Groups[2].Value.ToUpper();
                });
                Console.WriteLine($"public static readonly MainGroup {propName} = new MainGroup({grp.ID}, \"{grp.Name}\");");
            }

            foreach (var kv in platformGroups)
            {
                Console.WriteLine($"Platform {kv.Key}:");
                foreach (var grp in kv.Value.OrderBy(x => x.ID))
                    Console.WriteLine($"  {grp}");
            }
        }

        private void AddMeshButton_Click(object sender, EventArgs e)
        {
            using(var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mesh files (*.dae, *.obj)|*.dae;*.obj|Wavefront (*.obj)|*.obj|Collada (*.dae)|*.dae|All files (*.*)|*.*";
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

                var scene = AssimpContext.ImportFile(filename, 
                    Assimp.PostProcessSteps.Triangulate | 
                    Assimp.PostProcessSteps.GenerateNormals | 
                    Assimp.PostProcessSteps.PreTransformVertices);

                ImportExportProgress.Value = 20;

                if (scene != null)
                {
                    int counter = 1;
                    foreach (var mesh in scene.Meshes)
                    {
                        BrickMeshes.Add(new BrickMeshObject()
                        {
                            Mesh = mesh,
                            MeshScene = scene,
                            MeshFile = filename,
                            MeshName = mesh.Name,
                            IsMainModel = !mesh.HasTextureCoords(0)
                        });

                        float progress = counter++ / (float)scene.MeshCount;
                        ImportExportProgress.Value = 20 + (int)(progress * 80);
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was a problem importing the mesh");
            }

            HideProgressDelayed();
        }

        private void CreateBrickButton_Click(object sender, EventArgs e)
        {
            if (!ValidateBrick())
                return;

            ImportExportProgress.Value = 0;
            ImportExportProgress.Visible = true;

            int decorationID = 1;
            var decoratedMeshes = BrickMeshes.Where(x => x.DecorationID.HasValue);

            foreach (var grp in decoratedMeshes.GroupBy(x => x.DecorationID.Value).OrderBy(x => x.Key))
            {
                foreach (var mesh in grp)
                    mesh.DecorationID = decorationID;
                decorationID++;
            }

            var partMesh = new PartMesh
            {
                Info = new Primitive()
                {
                    ID = int.Parse(IDTextbox.Text),
                    Name = NameTextbox.Text,
                    Platform = PlatformCombo.SelectedItem as Platform
                }
            };

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
            ImportExportProgress.Value = 95;

            if (partMesh.DecorationMeshes.Any())
            {
                partMesh.Info.SurfaceMappingTable = Enumerable.Range(0, partMesh.DecorationMeshes.Count + 1).ToArray();
            }

            partMesh.Info.Bounding = partMesh.GetBoundingBox();
            partMesh.Info.GeometryBounding = partMesh.Info.Bounding;

            ImportExportProgress.Value = 100;

            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = partMesh.Info.ID.ToString();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    partMesh.Save(Path.GetDirectoryName(sfd.FileName), Path.GetFileNameWithoutExtension(sfd.FileName));
                }
            }

            HideProgressDelayed();
        }

        private Mesh CreatePartialMesh(IEnumerable<BrickMeshObject> brickMeshes)
        {
            var builder = new GeometryBuilder();
            foreach (var brickMesh in brickMeshes)
            {
                bool useTexture = brickMesh.IsTextured && !brickMesh.IsMainModel;
                foreach (var face in brickMesh.Mesh.Faces)
                {
                    Vertex[] verts = new Vertex[3];

                    for (int i = 0; i < 3; i++)
                    {
                        var p = brickMesh.Mesh.Vertices[face.Indices[i]];
                        var n = brickMesh.Mesh.Normals[face.Indices[i]];
                        var t = useTexture ?
                            brickMesh.Mesh.TextureCoordinateChannels[0][face.Indices[i]] : new Assimp.Vector3D();

                        verts[i] = new Vertex(
                            new Simple3D.Vector3(p.X, p.Y, p.Z),
                            new Simple3D.Vector3(n.X, n.Y, n.Z),
                            useTexture ? new Simple3D.Vector2(t.X, t.Y) : Simple3D.Vector2.Empty);
                    }

                    builder.AddTriangle(verts[0], verts[1], verts[2]);
                }
            }

            return new Mesh(builder.GetGeometry());
        }

        private bool ValidateBrick()
        {
            var errorMessages = new List<string>();
            //var warningMessages = new List<string>();

            if (string.IsNullOrEmpty(IDTextbox.Text))
                errorMessages.Add("Enter a brick ID");
            else if(!int.TryParse(IDTextbox.Text, out _))
                errorMessages.Add("Brick ID must be numeric");

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

        private void HideProgressDelayed(int delay = 1500)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay);
                Invoke((Action)(() => ImportExportProgress.Visible = false));
            });
        }
    }
}
