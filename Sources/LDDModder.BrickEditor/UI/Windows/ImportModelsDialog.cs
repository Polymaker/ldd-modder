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

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class ImportModelsDialog : Form
    {
        public PartProject Project { get; set; }

        public Assimp.Scene SceneToImport { get; set; }

        public int PreferredSurfaceID { get; set; }

        public List<ImportModelInfo> ModelsToImport { get; private set; }

        private List<SurfaceItem> SurfaceList;

        private bool HasInitialized { get; set; }

        public ImportModelsDialog()
        {
            InitializeComponent();
            InitializeGridView();
            ModelsToImport = new List<ImportModelInfo>();
            SurfaceList = new List<SurfaceItem>();
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
            RebuildSurfaceList();
            UpdateSurfaceComboBox();
            FillModelsGridView();
            HasInitialized = true;
        }

        

        private void FillModelsGridView()
        {
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
            ValidateSelection();

        }

        private void ValidateSelection()
        {
            ModelsToImport.RemoveAll(x => !x.Selected);
            DialogResult = DialogResult.OK;
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


        #endregion

        private void ModelsGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (HasInitialized && ModelsGridView.Columns[e.ColumnIndex] == SurfaceColumn)
            {
                UpdateSurfaceComboBox();
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
    }
}
