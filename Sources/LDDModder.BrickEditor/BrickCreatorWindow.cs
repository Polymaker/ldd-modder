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

        private IEnumerable<Assimp.Node> GetAllNodes(Assimp.Node node)
        {
            foreach(var n in node.Children)
            {
                yield return n;
                foreach (var nn in GetAllNodes(n))
                    yield return nn;
            }
        }

        public Assimp.Vector3D GetEuler(Assimp.Quaternion quat)
        {
            Assimp.Vector3D pitchYawRoll = new Assimp.Vector3D();
            quat = new Assimp.Quaternion(-quat.Z, -quat.Y, -quat.X, quat.W);
            double sqw = quat.W * quat.W;
            double sqx = quat.X * quat.X;
            double sqy = quat.Y * quat.Y;
            double sqz = quat.Z * quat.Z;

            // If quaternion is normalised the unit is one, otherwise it is the correction factor
            double unit = sqx + sqy + sqz + sqw;
            double test = quat.X * quat.Y + quat.Z * quat.W;
            const float PI = 3.1415926535897931f;
            if (test > 0.499f * unit)
            {
                // Singularity at north pole
                pitchYawRoll.Y = 2f * (float)Math.Atan2(quat.X, quat.W);  // Yaw
                pitchYawRoll.X = PI * 0.5f;                         // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }
            else if (test < -0.499f * unit)
            {
                // Singularity at south pole
                pitchYawRoll.Y = -2f * (float)Math.Atan2(quat.X, quat.W); // Yaw
                pitchYawRoll.X = -PI * 0.5f;                        // Pitch
                pitchYawRoll.Z = 0f;                                // Roll
                return pitchYawRoll;
            }

            pitchYawRoll.Y = (float)Math.Atan2(2 * quat.Y * quat.W - 2 * quat.X * quat.Z, sqx - sqy - sqz + sqw);       // Yaw
            pitchYawRoll.X = (float)Math.Asin(2 * test / unit);                                             // Pitch
            pitchYawRoll.Z = (float)Math.Atan2(2 * quat.X * quat.W - 2 * quat.Y * quat.Z, -sqx + sqy - sqz + sqw);      // Roll

            return pitchYawRoll;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AssimpContext = new Assimp.AssimpContext();


            //TestAssimpBones();
            //ListPlatformsGroups();
            string LddDbDirectory = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\db\");
            var brick = PartMesh.Read(LddDbDirectory, 14301);
            LDDModder.Utilities.LddMeshExporter.ExportLddPart(brick, "14301.dae", "collada");
            InitializeUI();
        }

        private void TestAssimpBones()
        {
            var testScene = AssimpContext.ImportFile(@"14301 blender.dae");
            var boneNodes = GetAllNodes(testScene.RootNode).Where(x => x.Name.Contains("Bone")).ToList();
            int ctr = 0;
            var armatureNode = GetAllNodes(testScene.RootNode).First(x => x.Name.Contains("Armature"));
            var rootTrans = testScene.RootNode.Transform;
            var transSum = armatureNode.Transform;
            var rootInv = rootTrans;
            rootInv.Inverse();

            foreach (var boneNode in boneNodes)
            {
                var bone = testScene.Meshes[0].Bones[ctr++];
                transSum = boneNode.Transform * transSum;
                var invOff = bone.OffsetMatrix;
                invOff.Inverse();
                invOff = rootInv * invOff;
                boneNode.Transform.Decompose(out Assimp.Vector3D scale, out Assimp.Quaternion rot, out Assimp.Vector3D translation);
                invOff.Decompose(out Assimp.Vector3D scale2, out Assimp.Quaternion rot2, out Assimp.Vector3D translation2);
                var fwd = rot.GetMatrix() * new Assimp.Vector3D(0, 0, 1);

                var rotMat = rot.GetMatrix();
                var rotAngles = GetEuler(rot) / (3.1416F * 2f) * 360f;

                var pt1 = transSum * new Assimp.Vector3D();
                pt1.X = (float)Math.Round(pt1.X * 1000F) / 1000f;
                pt1.Y = (float)Math.Round(pt1.Y * 1000F) / 1000f;
                pt1.Z = (float)Math.Round(pt1.Z * 1000F) / 1000f;
                var pt3 = invOff * new Assimp.Vector3D();
                pt3.X = (float)Math.Round(pt3.X * 1000F) / 1000f;
                pt3.Y = (float)Math.Round(pt3.Y * 1000F) / 1000f;
                pt3.Z = (float)Math.Round(pt3.Z * 1000F) / 1000f;
                Console.WriteLine($"{pt1}   {pt3}");
            }
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
                var scene = AssimpContext.ImportFile(filename, 
                    Assimp.PostProcessSteps.Triangulate | 
                    Assimp.PostProcessSteps.GenerateNormals | 
                    Assimp.PostProcessSteps.PreTransformVertices);

                if (scene != null)
                {
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
                    }
                }
            }
            catch { }
        }

        private void CreateBrickButton_Click(object sender, EventArgs e)
        {
            if (!ValidateBrick())
                return;

            int decorationID = 1;
            var decoratedMeshes = BrickMeshes.Where(x => x.DecorationID.HasValue);

            foreach (var grp in decoratedMeshes.GroupBy(x => x.DecorationID.Value).OrderBy(x => x.Key))
            {
                foreach (var mesh in grp)
                    mesh.DecorationID = decorationID;
                decorationID++;
            }

            var partMesh = new PartMesh();

            partMesh.PartInfo = new Primitive()
            {
                ID = int.Parse(IDTextbox.Text),
                Name = NameTextbox.Text,
                Platform = PlatformCombo.SelectedItem as Platform
            };
            
            var validMeshes = BrickMeshes.Where(x => x.IsMainModel || x.DecorationID.HasValue);

            foreach (var meshGroup in validMeshes.GroupBy(x => x.DecorationID).OrderBy(x => x.Key ?? 0))
            {
                var partialMesh = CreatePartialMesh(meshGroup);

                if (!meshGroup.Key.HasValue)
                    partMesh.MainModel = partialMesh;
                else
                    partMesh.DecorationMeshes.Add(partialMesh);
            }

            partMesh.ComputeAverageNormals();

            if (partMesh.DecorationMeshes.Any())
            {
                partMesh.PartInfo.SurfaceMappingTable = Enumerable.Range(0, partMesh.DecorationMeshes.Count + 1).ToArray();
            }

            partMesh.PartInfo.Bounding = partMesh.GetBoundingBox();
            partMesh.PartInfo.GeometryBounding = partMesh.PartInfo.Bounding;
            
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = partMesh.PartInfo.ID.ToString();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    partMesh.Save(Path.GetDirectoryName(sfd.FileName), Path.GetFileNameWithoutExtension(sfd.FileName));
                }
            }
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

    }
}
