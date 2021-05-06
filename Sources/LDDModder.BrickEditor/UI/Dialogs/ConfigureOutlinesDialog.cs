using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Modding;
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
    public partial class ConfigureOutlinesDialog : Form
    {
        public ProjectManager ProjectManager { get; set; }

        public PartProject Project => ProjectManager.CurrentProject;

        private BindingList<OutlinesGroupConfig> OutlinesGroups { get; set; }

        private OutlinesGroupConfig CurrentConfig { get; set; }

        private List<MeshRefNode> AvailableMeshes { get; set; }
        private List<MeshRefNode> AllGroupedMeshes { get; set; }
        private List<MeshRefNode> GroupMeshes { get; set; }

        private Dictionary<string, string> SurfaceGroups { get; set; }

        public ConfigureOutlinesDialog()
        {
            InitializeComponent();
            
        }

        public ConfigureOutlinesDialog(ProjectManager projectManager)
        {
            ProjectManager = projectManager;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureListViews();
            InitializeStuff();
        }

        private void ConfigureListViews()
        {

            BrightIdeasSoftware.OLVColumn CreateMeshColumn()
            {
                var column = new BrightIdeasSoftware.OLVColumn
                {
                    AspectName = "MeshName",
                    Text = "Mesh",
                    FillsFreeSpace = true
                };

                column.GroupKeyGetter += (m) =>
                {
                    if (m is MeshRefNode node)
                        return $"{node.SurfaceId}_{node.ComponentName}";
                    return string.Empty;
                };

                column.GroupKeyToTitleConverter += (gk) =>
                {
                    return GetGroupTitle(gk as string);
                };
                //column.GroupFormatter += (BrightIdeasSoftware.OLVGroup g, BrightIdeasSoftware.GroupingParameters p) =>
                //{
                    
                //};
                return column;
            }

            AvailableMeshList.Columns.Add(CreateMeshColumn());
            GroupMeshList.Columns.Add(CreateMeshColumn());
        }

        private string GetGroupTitle(string groupKey)
        {
            if (SurfaceGroups.ContainsKey(groupKey))
                return SurfaceGroups[groupKey];
            return groupKey;
        }

        private void InitializeStuff()
        {



            OutlinesGroups = new BindingList<OutlinesGroupConfig>(Project.OutlinesConfigs.ToList());

            SurfaceGroups = new Dictionary<string, string>();
            foreach (var comp in Project.GetAllElements<SurfaceComponent>())
            {
                var gk = $"{comp.Surface.SurfaceID}_{comp.Name}";
                SurfaceGroups.Add(gk, $"{ProjectManager.GetSurfaceName(comp.Surface)} - {comp.Name}");
            }

            AvailableMeshes = Project.GetAllMeshReferences().Select(m => new MeshRefNode(m)).ToList();
            GroupMeshes = new List<MeshRefNode>();
            AllGroupedMeshes = new List<MeshRefNode>();

            foreach (var mesh in AvailableMeshes)
            {
                if (OutlinesGroups.Any(g => g.Elements.OfType<ModelMeshReference>().Any(mr => mr.ID == mesh.MeshId)))
                    AllGroupedMeshes.Add(mesh);
            }

            AvailableMeshList.AddObjects(AvailableMeshes);
        }

        private void GroupMeshList_ModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            var draggedMeshes = e.SourceModels.OfType<MeshRefNode>().ToList();
            if (draggedMeshes.Any(m => !GroupMeshes.Contains(m)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void GroupMeshList_ModelDropped(object sender, BrightIdeasSoftware.ModelDropEventArgs e)
        {
            var draggedMeshes = e.SourceModels.OfType<MeshRefNode>().ToList();
            foreach(var m in draggedMeshes)
            {
                if (!AllGroupedMeshes.Contains(m))
                {
                    GroupMeshes.Add(m);
                    AllGroupedMeshes.Add(m);
                    GroupMeshList.AddObject(m);
                }
                AvailableMeshList.RefreshObjects(draggedMeshes);
            }
        }

        private void GroupMeshList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var selectedMeshes = GroupMeshList.SelectedObjects.OfType<MeshRefNode>().ToList();
                if (selectedMeshes.Any())
                {
                    GroupMeshes.Remove(selectedMeshes);
                    AllGroupedMeshes.Remove(selectedMeshes);
                    GroupMeshList.RemoveObjects(selectedMeshes);
                    AvailableMeshList.RefreshObjects(selectedMeshes);
                }
            }
        }

        private class MeshRefNode
        {
            public int SurfaceId { get; set; }
            public string SurfaceName { get; set; }

            public string ComponentId { get; set; }
            public string ComponentName { get; set; }
            public string MeshId { get; set; }
            public string MeshName { get; set; }

            public MeshRefNode() { }

            public MeshRefNode(ModelMeshReference meshReference)
            {
                var comp = (SurfaceComponent)meshReference.Parent;
                SurfaceId = comp.Surface.SurfaceID;
                SurfaceName = ProjectManager.GetSurfaceName(comp.Surface);
                ComponentId = comp.ID;
                ComponentName = comp.Name;
                MeshId = meshReference.ID;
                MeshName = meshReference.Name;
            }
        }

        private void AvailableMeshList_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if (!(e.Model is MeshRefNode meshNode))
                return;

            e.Item.CellPadding = new Rectangle(10, 2, 2, 2);

            if (AllGroupedMeshes.Contains(meshNode))
            {
                e.Item.BackColor = Color.Gainsboro;
                e.Item.ForeColor = Color.DimGray;
            }
        }

        private void GroupMeshList_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if (!(e.Model is MeshRefNode meshNode))
                return;

            e.Item.CellPadding = new Rectangle(10, 2, 2, 2);
        }
    }
}
