using LDDModder.BrickEditor.EditModels;
using LDDModder.BrickEditor.Resources;
using LDDModder.LDD.Meshes;
using LDDModder.Modding.Editing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class NavigationPanel : ProjectDocumentPanel
    {
        public enum NavigationViewMode
        {
            All,
            Surfaces,
            Collisions,
            Connections,
            Bones
        }

        private bool InternalSelection;
        private bool UpdatingNavigation;

        //private class ViewModeModel
        //{
        //    public NavigationViewMode Value { get; set; }
        //    public string Name { get; set; }
        //}

        private NavigationViewMode CurrentViewMode => (NavigationViewMode)ViewModeComboBox.SelectedIndex;

        internal NavigationPanel()
        {
            InitializeComponent();
        }

        public NavigationPanel(ProjectManager projectManager) : base (projectManager)
        {
            InitializeComponent();
            CloseButtonVisible = false;
            CloseButton = false;
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float | DockAreas.Document;

            ProjectTreeView.CanExpandGetter += (model) =>
            {
                if (model is BaseProjectNode node)
                    return node.HasChildrens();
                return false;
            };

            ProjectTreeView.ChildrenGetter += (model) =>
            {
                if (model is BaseProjectNode node)
                    return node.Childrens;
                return new ArrayList();
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeViewComboBox();

            label1.Text = "<No active project>";
        }

        private void InitializeViewComboBox()
        {
            ViewModeComboBox.Items.Add(ViewModeAll.Text);
            ViewModeComboBox.Items.Add(ViewModeSurfaces.Text);
            ViewModeComboBox.Items.Add(ViewModeCollisions.Text);
            ViewModeComboBox.Items.Add(ViewModeConnections.Text);
            ViewModeComboBox.Items.Add(ViewModeBones.Text);
            ViewModeComboBox.SelectedIndex = 1;
        }

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            RebuildNavigation(true);

            label1.Text = ProjectManager.IsProjectOpen ? 
                ProjectManager.GetProjectDisplayName() : "<No active project>";
        }

        private void ViewModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectManager.IsProjectOpen)
                FilterNavigation();
        }

        #region TreeListView Handling

        private void RebuildNavigation(bool recreate)
        {
            UpdatingNavigation = true;

            if (recreate)
                ProjectTreeView.ClearObjects();

            if (ProjectManager.IsProjectOpen)
            {
                if (recreate)
                {
                    ProjectTreeView.AddObject(new ProjectCollectionNode(
                        CurrentProject.Surfaces,
                        ModelLocalizations.Label_Surfaces));

                    ProjectTreeView.AddObject(new ProjectCollectionNode(
                        CurrentProject.Collisions,
                        ModelLocalizations.Label_Collisions));

                    ProjectTreeView.AddObject(new ProjectCollectionNode(
                        CurrentProject.Connections,
                        ModelLocalizations.Label_Connections));

                    foreach (ProjectCollectionNode node in ProjectTreeView.Roots)
                        ProjectTreeView.Expand(node);
                }
                else
                {
                    var expandedNodes = ProjectTreeView.ExpandedObjects
                        .OfType<BaseProjectNode>().Select(x => x.NodeID).ToList();

                    foreach (BaseProjectNode node in ProjectTreeView.Roots)
                    {
                        node.IsDirty = true;
                        ProjectTreeView.UpdateObject(node);
                    }

                    ExpandNodes(ProjectTreeView.Roots, expandedNodes);
                }

                //treeListView1.AddObject(new ProjectGroupNode(
                //    CurrentProject.Bones,
                //    ModelLocalizations.Label_Bones));
            }

            FilterNavigation();

            UpdatingNavigation = false;
        }

        private void ExpandNodes(IEnumerable nodes, List<string> nodeIDs)
        {
            foreach (BaseProjectNode node in nodes)
            {
                if(nodeIDs.Contains(node.NodeID))
                {
                    ProjectTreeView.Expand(node);
                    ExpandNodes(node.Childrens, nodeIDs);
                }
            }
        }


        private void FilterNavigation()
        {

        }


        #endregion

        #region ContextMenu Handling

        private void ElementsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (!ProjectManager.IsProjectOpen)
            {
                e.Cancel = true;
                return;
            }

            ContextMenu_AddElement.Enabled = false;
            ContextMenu_AddCollision.Enabled = !CurrentProject.Flexible;
            ContextMenu_AddConnection.Enabled = !CurrentProject.Flexible;

            AddElementMenu_MaleStud.Enabled = !CurrentProject.Flexible;
            AddElementMenu_FemaleStud.Enabled = !CurrentProject.Flexible;
            AddElementMenu_BrickTube.Enabled = !CurrentProject.Flexible;
            
            if (ProjectTreeView.FocusedItem != null && ProjectTreeView.FocusedItem.Selected)
            {
                var focusedSurfaceNode = GetFocusedParentElement<PartSurface>();

                ContextMenu_AddElement.Enabled = focusedSurfaceNode != null;

                if (CurrentProject.Flexible)
                {
                    var focusedBoneNode = GetFocusedParentElement<PartBone>();
                    ContextMenu_AddCollision.Enabled = focusedBoneNode != null;
                    ContextMenu_AddConnection.Enabled = focusedBoneNode != null;
                }
            }

            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<BaseProjectNode>();
            ElementsMenu_Delete.Enabled = selectedNodes.Any(x => x is ProjectElementNode);
        }

        private void AddCollisionMenu_Box_Click(object sender, EventArgs e)
        {
            var boxCollision = new PartBoxCollision(new Simple3D.Vector3(0.4f));

            var focusedBoneNode = GetFocusedParentElement<PartBone>();

            if (focusedBoneNode != null)
                (focusedBoneNode.Element as PartBone).Collisions.Add(boxCollision);
            else
                CurrentProject.Collisions.Add(boxCollision);

            ProjectManager.SelectElement(boxCollision);

        }

        private void AddCollisionMenu_Sphere_Click(object sender, EventArgs e)
        {
            var sphereCollision = new PartSphereCollision(0.4f);

            var focusedBoneNode = GetFocusedParentElement<PartBone>();

            if (focusedBoneNode != null)
                (focusedBoneNode.Element as PartBone).Collisions.Add(sphereCollision);
            else
                CurrentProject.Collisions.Add(sphereCollision);

            ProjectManager.SelectElement(sphereCollision);
        }

        #endregion

        protected override void OnProjectElementsChanged()
        {
            base.OnProjectElementsChanged();

            if (InvokeRequired)
                BeginInvoke((Action)(() => RebuildNavigation(false)));
            else
                RebuildNavigation(false);
        }

        private void ProjectTreeView_SelectionChanged(object sender, EventArgs e)
        {
            if (!(InternalSelection || UpdatingNavigation))
                ProjectManager.SelectElements(GetSelectedElements());
        }

        public IEnumerable<PartElement> GetSelectedElements()
        {
            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<BaseProjectNode>();
            var elementNodes = selectedNodes.SelectMany(x => x.GetChildHierarchy(true)).OfType<ProjectElementNode>();
            return elementNodes.Select(x => x.Element);
        }

        private ProjectElementNode GetFocusedParentElement<T>() where T : PartElement
        {
            var focusedNode = ProjectTreeView.FocusedObject as BaseProjectNode;
            return focusedNode.GetParents(true)
                .OfType<ProjectElementNode>()
                .FirstOrDefault(x => x.Element.GetType() == typeof(T));
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
            InternalSelection = true;


            InternalSelection = false;
        }

        

        private void ElementsMenu_Delete_Click(object sender, EventArgs e)
        {
            var elements = GetSelectedElements().ToList();


            if (elements.Count > 1)
            {
                //TODO: show confirmation message whene deleting more than one
            }

            ProjectManager.ClearSelection();
            ProjectManager.StartBatchChanges();

            var removedElements = elements.Where(x => x.TryRemove()).ToList();

            if (removedElements.OfType<ModelMeshReference>().Any())
                CurrentProject.RemoveUnreferencedMeshes();

            ProjectManager.EndBatchChanges();

        }

        
    }
}
