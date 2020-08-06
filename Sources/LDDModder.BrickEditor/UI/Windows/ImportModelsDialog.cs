using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assimp;
using LDDModder.Simple3D;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class ImportModelsDialog : Form
    {
        public ProjectManager ProjectManager { get; set; }

        public PartProject Project => ProjectManager.CurrentProject;

        private Assimp.AssimpContext AssimpContext;

        public Assimp.Scene SceneToImport { get; set; }

        public int PreferredSurfaceID { get; set; }

        public List<ImportModelInfo> ModelsToImport { get; private set; }

        public IEnumerable<ImportModelInfo> SelectedModels => ModelsToImport.Where(x => x.Selected);

        private List<SurfaceItem> SurfaceList { get; }

        private bool HasInitialized { get; set; }

        public bool SelectFileOnStart { get; set; }

        public ImportModelsDialog(ProjectManager projectManager)
        {
            ProjectManager = projectManager;
            InitializeComponent();
            InitializeGridView();
            ModelsToImport = new List<ImportModelInfo>();
            SurfaceList = new List<SurfaceItem>();
            WarningMessageLabel.Text = string.Empty;
            progressBar1.Visible = false;
        }

        private void InitializeGridView()
        {
            ModelsGridView.AutoGenerateColumns = false;

            SelectionColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TexturedColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FlexibleColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AssimpContext = new Assimp.AssimpContext();
            RebuildSurfaceList();
            UpdateSurfaceComboBox();
            //FillModelsGridView();
            HasInitialized = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (SelectFileOnStart)
                ShowSelectFileDialog();
        }

        private void FillModelsGridView()
        {
            ModelsGridView.DataSource = null;
            ModelsToImport.Clear();

            foreach (var mesh in SceneToImport.Meshes)
            {
                if (!mesh.Faces.Any(x => x.IndexCount == 3))
                    continue;

                var modelInfo = new ImportModelInfo()
                {
                    Selected = true,
                    Name = mesh.Name,
                    TriangleCount = mesh.FaceCount,
                    IsFlexible = mesh.HasBones,
                    IsTextured = mesh.HasTextureCoords(0),
                    Mesh = mesh,
                    SurfaceID = PreferredSurfaceID >=0 ? PreferredSurfaceID : 0
                };
                ModelsToImport.Add(modelInfo);
            }

            ModelsGridView.DataSource = ModelsToImport;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                ImportModels();
                DialogResult = DialogResult.OK;
            }
        }

        private void ImportModels()
        {
            ProjectManager.StartBatchChanges();
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = 0;
            progressBar1.Maximum = SelectedModels.Count();

            var bonesToImport = GetBoneMappings();

            if (bonesToImport.Count > 0 && SelectedModels.Any(x => x.IsFlexible))
            {
                Project.Flexible = true;
     
                foreach (var boneMap in bonesToImport)
                {
                    var existing = Project.Bones.FirstOrDefault(x => x.BoneID == boneMap.AssimpID);

                    if (existing == null)
                        existing = new PartBone(boneMap.AssimpID);
                    existing.Transform = boneMap.Transform;
                    
                    if (boneMap.ParentName != null)
                    {
                        var parentbone = bonesToImport.FirstOrDefault(x => x.Name == boneMap.ParentName);
                        existing.TargetBoneID = parentbone.AssimpID;


                    }

                    if (existing.Project == null)
                        Project.Bones.Add(existing);
                }


            }

            foreach (var model in SelectedModels)
            {
                var geom = Meshes.MeshConverter.AssimpToLdd(SceneToImport, model.Mesh);
                var surface = Project.Surfaces.FirstOrDefault(x => x.SurfaceID == model.SurfaceID);

                if (surface == null)
                {
                    surface = new PartSurface(model.SurfaceID, Project.Surfaces.Max(x => x.SubMaterialIndex) + 1);
                    Project.Surfaces.Add(surface);
                }

                var partModel = surface.Components.FirstOrDefault(x => x.ComponentType == ModelComponentType.Part);

                if (partModel == null)
                {
                    partModel = new PartModel();
                    surface.Components.Add(partModel);
                }

                //if (model.Mesh.HasBones)
                //{

                    


                //    foreach (var meshBone in model.Mesh.Bones)
                //    {
                        


                //        //var bone = new PartBone(meshBone)
                //    }
                //}

                var modelMesh = Project.AddMeshGeometry(geom, model.Name);
                partModel.Meshes.Add(new ModelMeshReference(modelMesh));

                progressBar1.Value += 1;
            }

            if (bonesToImport.Any())
                Project.RebuildBoneConnections();

            ProjectManager.EndBatchChanges();
        }

        private List<BoneMapping> GetBoneMappings()
        {
            var bones = new List<BoneMapping>();
            var boneNames = SceneToImport.Meshes.SelectMany(x => x.Bones).Select(x => x.Name).Distinct().ToList();

            var boneNodes = Assimp.AssimpHelper.GetNodeHierarchy(SceneToImport.RootNode)
                .Where(x => boneNames.Contains(x.Name))/*.OrderBy(x => x.GetLevel())*/.ToList();

            boneNames = boneNodes.Select(x => x.Name).ToList();//names in order

            foreach (var boneNode in boneNodes)
            {
                //var meshBone = SceneToImport.Meshes.SelectMany(x => x.Bones).FirstOrDefault(x => x.Name == boneNode.Name);
                //var pos1 = ItemTransform.FromMatrix(meshBone.OffsetMatrix.ToLDD());

                string parentName = boneNode.Parent?.Name;
                if (!boneNames.Contains(parentName))
                    parentName = null;

                var boneTransform = boneNode.GetFinalTransform().ToLDD();
                //var yAxis = boneTransform.TransformNormal(Simple3D.Vector3.UnitY);
                //boneTransform = Simple3D.Matrix4.FromAngleAxis((float)Math.PI * -0.5f, yAxis) * boneTransform;

                var mapping = new BoneMapping()
                {
                    Name = boneNode.Name,
                    ParentName = parentName,
                    Transform = ItemTransform.FromMatrix(boneTransform),
                    AssimpID = boneNames.IndexOf(boneNode.Name)
                };
                bones.Add(mapping);
            }

            //Adjust bone rotation for LDD
            foreach (var bone in bones)
            {
                var target = bones.FirstOrDefault(x => x.ParentName == bone.Name);
                if (target != null)
                {
                    var dir = (target.Transform.Position - bone.Transform.Position).Normalized();
                    var angle = Simple3D.Vector3d.AngleBetween(Simple3D.Vector3d.UnitX, dir);
                    var yAxis = Simple3D.Vector3d.Cross(Simple3D.Vector3d.UnitX, dir);
                    var rot = Simple3D.Matrix4d.FromAngleAxis(angle, yAxis);
                    bone.Transform.Rotation = Quaterniond.ToEuler(rot.ExtractRotation()) * (180d / Math.PI);
                }
                else if(bone.ParentName != null)
                {
                    var parent = bones.FirstOrDefault(x => x.Name == bone.ParentName);
                    bone.Transform.Rotation = parent.Transform.Rotation;
                }
            }

            return bones;
        }

        private bool ValidateSelection()
        {
            WarningMessageLabel.Text = string.Empty;
            bool hasError = false;

            if (SelectedModels.Any(x => x.IsFlexible))
            {
                var allFlexible = SelectedModels.All(x => x.IsFlexible);
                if (!allFlexible)
                {
                    WarningMessageLabel.Text = WarningNotAllFlexible.Text;
                    hasError = true;
                }
            }

            return !hasError;
        }

        #region Classes

        public class ImportModelInfo
        {
            public bool Selected { get; set; }
            public string Name { get; set; }
            public int TriangleCount { get; set; }
            public bool IsTextured { get; set; }
            public bool IsFlexible { get; set; }
            public int SurfaceID { get; set; }
            public Assimp.Mesh Mesh { get; set; }
        }

        class SurfaceItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ExistingMeshes { get; set; }

            public SurfaceItem(int iD, string name)
            {
                ID = iD;
                Name = name;
            }
        }

        class BoneMapping
        {
            public string Name { get; set; }
            public string ParentName { get; set; }
            public ItemTransform Transform { get; set; }

            public int AssimpID { get; set; }
            public int LddID { get; set; }
        }

        #endregion

        private void ModelsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!HasInitialized)
                return;

            if (ModelsGridView.Columns[e.ColumnIndex] == SurfaceColumn)
            {
                RebuildSurfaceList();
                UpdateSurfaceComboBox();
            }
            else if (ModelsGridView.Columns[e.ColumnIndex] == SelectionColumn)
            {
                ValidateSelection();
            }
        }

        private void ModelsGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (ModelsGridView.Columns[e.ColumnIndex] == SurfaceColumn)
                {
                    ModelsGridView.BeginEdit(true);
                    (ModelsGridView.EditingControl as ComboBox).DroppedDown = true;
                }
            }
        }

        #region Surface ComboBox Handling

        private void RebuildSurfaceList()
        {
            SurfaceList.Clear();
            int existingSurfaces = Project.Surfaces.Max(x => x.SurfaceID);
            int maxSurface = existingSurfaces;
            if (ModelsToImport.Any())
                maxSurface = Math.Max(maxSurface, ModelsToImport.Max(x => x.SurfaceID));

            for (int i = 0; i <= maxSurface + 1; i++)
            {
                if (i <= existingSurfaces)
                {
                    SurfaceList.Add(new SurfaceItem(i, $"Surface {i}")
                    {
                        ExistingMeshes = Project.Surfaces[i].GetAllMeshReferences().Count()
                    });
                }
                else
                    SurfaceList.Add(new SurfaceItem(i, $"*Surface {i}"));
            }
        }

        private void UpdateSurfaceComboBox()
        {
            SurfaceColumn.DataSource = SurfaceList.ToArray();
            SurfaceColumn.DisplayMember = "Name";
            SurfaceColumn.ValueMember = "ID";
        }

        private void UpdateRowSurfaceComboBox(DataGridViewRow row)
        {
            var cboCell = row.Cells[SurfaceColumn.Index] as DataGridViewComboBoxCell;
            var currentModel = row.DataBoundItem as ImportModelInfo;

            var visibleSurfaces = new List<SurfaceItem>();

            foreach(var surface in SurfaceList)
            {
                if (surface.ID == 0)
                {
                    visibleSurfaces.Add(surface);
                    continue;
                }
                int meshCount = surface.ExistingMeshes + ModelsToImport
                    .Count(x => x.Selected && x != currentModel && x.SurfaceID == surface.ID);
                
            }
        }

        #endregion

        private void BrowseModelBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            ShowSelectFileDialog();
        }

        public void ShowSelectFileDialog()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Mesh files|*.dae;*.obj;*.stl|Wavefront|*.obj|Collada|*.dae|STL|*.stl|All files|*.*";
                if (!string.IsNullOrEmpty(BrowseModelBox.Value))
                    ofd.FileName = BrowseModelBox.Value;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        BrowseModelBox.Value = ofd.FileName;
                        progressBar1.Style = ProgressBarStyle.Marquee;
                        progressBar1.Visible = true;
                        SceneToImport = AssimpContext.ImportFile(ofd.FileName, 
                            Assimp.PostProcessSteps.Triangulate | 
                            Assimp.PostProcessSteps.GenerateNormals | 
                            Assimp.PostProcessSteps.FlipUVs);
                        
                        FillModelsGridView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not import file!");
                    }
                    finally
                    {
                        progressBar1.Visible = false;
                    }
                }
            }
        }

        private void CheckUncheckButton_Click(object sender, EventArgs e)
        {
            if (ModelsToImport.Count == 0)
                return;

            int checkedCount = ModelsToImport.Count(x => x.Selected);
            int uncheckedCount = ModelsToImport.Count(x => !x.Selected);

            foreach (var model in ModelsToImport)
                model.Selected = (checkedCount < uncheckedCount);

            foreach(DataGridViewRow row in ModelsGridView.Rows)
                ModelsGridView.UpdateCellValue(SelectionColumn.Index, row.Index);
        }
    }
}
