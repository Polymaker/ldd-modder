using BrightIdeasSoftware;
using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.ProjectHandling.ViewInterfaces;
using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class NavigationPanel : ProjectDocumentPanel, INavigationWindow
    {
        public enum NavigationViewMode
        {
            All,
            Surfaces,
            Collisions,
            Connections,
            Bones,
            Meshes
        }

        private class ViewModeInfo
        {
            public NavigationViewMode ViewMode { get; set; }
            public string DisplayName { get; set; }

            public ViewModeInfo(NavigationViewMode viewMode, string displayName)
            {
                ViewMode = viewMode;
                DisplayName = displayName;
            }
        }

        private NavigationViewMode SelectedView
        {
            get
            {
                if (ViewModeComboBox.SelectedItem != null)
                    return (NavigationViewMode)ViewModeComboBox.SelectedValue;
                return NavigationViewMode.All;
            }
        }

        internal NavigationPanel()
        {
            InitializeComponent();
            InitializeNavigationImageList();
        }

        public NavigationPanel(ProjectManager projectManager) : base (projectManager)
        {
            InitializeComponent();
            projectManager.NavigationWindow = this;

            CloseButtonVisible = false;
            CloseButton = false;
            ContextMenu_Delete.ShortcutKeys = Keys.Delete;

            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float | DockAreas.Document;

            InitializeNavigationImageList();
            InitializeContextMenus();
            InitializeNavigationTreeView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeViewComboBox();

            //ViewModeLabel.Text = $"<{ModelLocalizations.Label_NoActiveProject}> ";
        }

        private void InitializeViewComboBox()
        {
            using (FlagManager.UseFlag("ViewModeComboBox"))
            {
                var viewModes = new List<ViewModeInfo>();

                var currentViewMode = ViewModeComboBox.SelectedItem != null ? 
                    (NavigationViewMode)ViewModeComboBox.SelectedValue : NavigationViewMode.All;

                viewModes.Add(new ViewModeInfo(NavigationViewMode.All, ViewModeAll.Text));
                viewModes.Add(new ViewModeInfo(NavigationViewMode.Surfaces, ViewModeSurfaces.Text));

                if (ProjectManager.IsProjectOpen && CurrentProject.Flexible)
                {
                    viewModes.Add(new ViewModeInfo(NavigationViewMode.Bones, ViewModeBones.Text));
                }
                else
                {
                    viewModes.Add(new ViewModeInfo(NavigationViewMode.Collisions, ViewModeCollisions.Text));
                    viewModes.Add(new ViewModeInfo(NavigationViewMode.Connections, ViewModeConnections.Text));
                }

                ViewModeComboBox.DataSource = viewModes;
                ViewModeComboBox.ValueMember = "ViewMode";
                ViewModeComboBox.DisplayMember = "DisplayName";

                if (viewModes.Any(x => x.ViewMode == currentViewMode))
                    currentViewMode = NavigationViewMode.All;

                ViewModeComboBox.SelectedValue = currentViewMode;
            }
        }

        private void InitializeNavigationImageList()
        {
            NavigationImageList.Images.Add("Surface_Main", Properties.Resources.MainSurfaceIcon);
            NavigationImageList.Images.Add("Surface_Decoration", Properties.Resources.DecorationSurfaceIcon);
            NavigationImageList.Images.Add("Model_MaleStud", Properties.Resources.MaleStudIcon);
            NavigationImageList.Images.Add("Mesh", Properties.Resources.MeshIcon);

            NavigationImageList.Images.Add("Visible", Properties.Resources.VisibleIcon);
            NavigationImageList.Images.Add("NotVisible", Properties.Resources.NotVisibleIcon);
            NavigationImageList.Images.Add("Hidden", Properties.Resources.HiddenIcon);
            NavigationImageList.Images.Add("Hidden2", Properties.Resources.Hidden2Icon);
        }

        private void InitializeNavigationTreeView()
        {
            ProjectTreeView.CanExpandGetter += (model) =>
            {
                if (model is ProjectTreeNode node)
                    return node.HasChildrens();
                return false;
            };

            ProjectTreeView.ChildrenGetter += (model) =>
            {
                if (model is ProjectTreeNode node)
                {

                    return node.Nodes;
                }
                return new ArrayList();
            };

            //ProjectTreeView.CellToolTipGetter += (col, model) =>
            //{
            //    if (model is ProjectElementNode node)
            //    {
               
            //        //return node.Element.Name;
            //    }
            //    return string.Empty;
            //};

            ProjectTreeView.DropSink = new NavigationDropHandler();
            ProjectTreeView.DragSource = new NavigationDragHandler();

            ProjectTreeView.SmallImageList = NavigationImageList;
            olvColumnVisible.HeaderImageKey = "Visible";
            olvColumnVisible.ShowTextInHeader = false;
            olvColumnVisible.Sortable = false;

            olvColumnVisible.ImageGetter = (x) =>
            {
                if (x is ProjectTreeNode projectNode)
                {
                    projectNode.UpdateVisibility();
                    return projectNode.VisibilityImageKey;
                }

                return string.Empty;
            };

            ProjectTreeView.TreeColumnRenderer = new Controls.TreeRendererEx();
        }

        private void ViewModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("ViewModeComboBox") && ProjectManager.IsProjectOpen)
                FilterNavigation();
        }

        #region TreeListView Handling

        private void RebuildNavigation(bool recreate)
        {
            using (FlagManager.UseFlag("BuildingNavTree"))
            {
                if (ProjectManager.IsProjectOpen)
                {
                    var selectedNodeIDs = ProjectTreeView.SelectedObjects
                        .OfType<ProjectTreeNode>().Select(x => x.NodeID).ToList();
                    selectedNodeIDs.RemoveAll(x => string.IsNullOrEmpty(x));

                    var expandedNodeIDs = ProjectTreeView.ExpandedObjects
                        .OfType<ProjectTreeNode>().Select(x => x.NodeID).ToList();
                    expandedNodeIDs.RemoveAll(x => string.IsNullOrEmpty(x));
                    ProjectTreeView.ExpandedObjects = Enumerable.Empty<ProjectTreeNode>();

                    if (recreate)
                    {
                        ProjectTreeView.ClearObjects();

                        ProjectManager.RebuildNavigationTree();

                        ProjectTreeView.AddObjects(ProjectManager.NavigationTreeNodes.ToList());


                        foreach (ProjectCollectionNode node in ProjectTreeView.Roots)
                        {
                            node.Manager = ProjectManager;
                            ProjectTreeView.Expand(node);
                        }
                    }
                    else
                    {
                        foreach (ProjectTreeNode node in ProjectTreeView.Roots)
                        {
                            ProjectTreeView.RemoveObjects(node.GetChildHierarchy().ToList());
                            node.InvalidateChildrens();
                            ProjectTreeView.UpdateObject(node);
                        }
                    }


                    //ProjectTreeView.TreeModel.
                    ExpandNodes(ProjectTreeView.Roots, expandedNodeIDs);

                    if (selectedNodeIDs.Any())
                        SetSelectedNodeIDs(selectedNodeIDs);
                }
                else
                    ProjectTreeView.ClearObjects();

                FilterNavigation();
            }
        }

        private void ExpandNodes(IEnumerable nodes, List<string> nodeIDs)
        {
            foreach (ProjectTreeNode node in nodes)
            {
                if(nodeIDs.Contains(node.NodeID))
                {
                    ProjectTreeView.Expand(node);
                    ExpandNodes(node.Nodes, nodeIDs);
                }
            }
        }

        private void SelectNodes(IEnumerable nodes, List<string> nodeIDs)
        {
            foreach (ProjectTreeNode node in nodes)
            {
                if (nodeIDs.Contains(node.NodeID))
                    ProjectTreeView.SelectedObjects.Add(node);
                
                SelectNodes(node.Nodes, nodeIDs);
            }
        }

        private IEnumerable<ProjectTreeNode> GetTreeNodes(IEnumerable nodes = null)
        {
            if (nodes == null)
                nodes = ProjectTreeView.Roots;

            foreach (var node in nodes.OfType<ProjectTreeNode>())
            {
                yield return node;

                foreach(var subNode in GetTreeNodes(node.Nodes))
                    yield return subNode;
            }
        }

        private void FilterNavigation()
        {
            ProjectTreeView.ModelFilter = new ModelFilter(x =>
            {
                if (x is ProjectTreeNode projectNode)
                    return IsNodeVisible(projectNode);
                return true;
            });
        }

        private bool IsNodeVisible(ProjectTreeNode projectNode)
        {
            if (SelectedView == NavigationViewMode.All)
                return true;

            var rootNode = projectNode.RootNode ?? projectNode;
            var collectionNode = rootNode as ProjectCollectionNode;

            switch (SelectedView)
            {
                case NavigationViewMode.All:
                    break;
                case NavigationViewMode.Surfaces:
                    return collectionNode != null && collectionNode.Collection == CurrentProject.Surfaces;
                case NavigationViewMode.Collisions:
                    return collectionNode != null && collectionNode.Collection == CurrentProject.Collisions;
                case NavigationViewMode.Connections:
                    return collectionNode != null && collectionNode.Collection == CurrentProject.Connections;
                case NavigationViewMode.Bones:
                    return collectionNode != null && collectionNode.Collection == CurrentProject.Bones;
                case NavigationViewMode.Meshes:
                    return collectionNode != null && collectionNode.Collection == CurrentProject.Meshes;
            }

            return false;
        }

        private void ProjectTreeView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (e.ListViewItem.RowObject is ProjectElementNode elementNode)
            {
                if (elementNode.Element is PartSurface)
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }

        private void ProjectTreeView_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            if (e.ListViewItem.RowObject is ProjectElementNode elementNode)
            {

                string newName = e.NewValue as string;
                if (string.IsNullOrEmpty(newName))
                {
                    e.Cancel = true;
                    return;
                }

                if (!e.Cancel)
                {
                    newName = CurrentProject.RenameElement(elementNode.Element, newName);
                    e.NewValue = newName;
                }
            }
        }

        private void ProjectTreeView_SelectionChanged(object sender, EventArgs e)
        {
            if (!(FlagManager.IsSet("BuildingNavTree") ||
                FlagManager.IsSet("ManualSelect") ||
                FlagManager.IsSet("DragDropping") ||
                FlagManager.IsSet("WaitForManualSelect") ||
                FlagManager.IsSet("PreventSelection")))
            {

                using (FlagManager.UseFlag("SelectElements"))
                    ProjectManager.SelectElements(GetSelectedElements());
            }

            FlagManager.Set("WaitForManualSelect", false);
            FlagManager.Set("PreventSelection", false);
        }

        private List<ProjectTreeNode> SelectedItemCache;

        private void ProjectTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hit = ProjectTreeView.OlvHitTest(e.X, e.Y);

                if (hit.Column == olvColumnVisible)
                {
                    FlagManager.Set("PreventSelection");
                    SelectedItemCache = new List<ProjectTreeNode>();
                    SelectedItemCache.AddRange(ProjectTreeView.SelectedObjects.OfType<ProjectTreeNode>());
                }
            }
        }

        private void ProjectTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                FlagManager.Unset("PreventSelection");
        }

        private void ProjectTreeView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("PreventSelection") &&
                !FlagManager.IsSet("ItemSelectionChanged"))
            {
                using (FlagManager.UseFlag("ItemSelectionChanged"))
                    ProjectTreeView.SelectObjects(SelectedItemCache);
            }
        }

        private void ProjectTreeView_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == olvColumnVisible)
            {
                e.Handled = true;

                if (e.Model is ProjectElementNode elementNode)
                {
                    var modelExt = elementNode.Element.GetExtension<ModelElementExtension>();
                    if (modelExt != null)
                    {
                        //Debug.WriteLine($"ProjectTreeView_CellClick {elementNode.Element.Name} IsHidden {modelExt.IsHidden} -> {!modelExt.IsHidden}");
                        //modelExt.IsHidden = !modelExt.IsHidden;

                        //ProjectTreeView.RefreshObject(elementNode);
                        //if (elementNode.Parent is ElementGroupNode)
                        //    ProjectTreeView.RefreshObject(elementNode.Parent);

                        ProjectManager.SetElementHidden(elementNode.Element, !modelExt.IsHidden);
                    }
                }
                else if (e.Model is ElementCollectionNode elementColNode)
                {
                    var femaleStudExt = elementColNode.Element.GetExtension<FemaleStudModelExtension>();

                    if (femaleStudExt != null)
                    {
                        femaleStudExt.ShowAlternateModels = !femaleStudExt.ShowAlternateModels;
                        ProjectTreeView.RefreshObject(elementColNode.Parent);
                    }
                }
                else if (e.Model is ProjectCollectionNode collectionNode)
                {
                    if (collectionNode.Collection == CurrentProject.Surfaces)
                        ProjectManager.ShowPartModels = !ProjectManager.ShowPartModels;
                    else if (collectionNode.Collection == CurrentProject.Collisions)
                        ProjectManager.ShowCollisions = !ProjectManager.ShowCollisions;
                    else if (collectionNode.Collection == CurrentProject.Connections)
                        ProjectManager.ShowConnections = !ProjectManager.ShowConnections;

                    ProjectTreeView.RefreshObject(collectionNode);
                }
                else if (e.Model is ElementGroupNode groupNode && groupNode.SupportsVisibility())
                {

                    //groupNode.ToggleVisibility();
                    //ProjectTreeView.RefreshObject(groupNode);
                }
            }
        }


        public void RefreshNavigationNode(ProjectTreeNode node)
        {
            ExecuteOnThread(() => ProjectTreeView.RefreshObject(node));
        }

        #endregion

        #region ContextMenu Handling

        private void InitializeContextMenus()
        {
            InitializeComponentContextMenu();
            InitializeCollisionContextMenu();
            InitializeConnectionContextMenu();
        }

        private void InitializeComponentContextMenu()
        {
            foreach (ToolStripMenuItem item in ContextMenu_AddElement.DropDownItems)
            {
                string connectionTypeStr = item.Tag as string;
                if (!string.IsNullOrEmpty(connectionTypeStr) &&
                    Enum.TryParse(connectionTypeStr, out ModelComponentType componentType))
                {
                    string menuText = ModelLocalizations.ResourceManager.GetString($"ModelComponentType_{connectionTypeStr}");
                    menuText = menuText.Replace("&", "&&");
                    item.Text = menuText;
                    item.Click += AddComponentMenuItem_Click;
                }
            }
        }

        private void InitializeCollisionContextMenu()
        {
            foreach (ToolStripMenuItem item in ContextMenu_AddCollision.DropDownItems)
            {
                string collisionTypeStr = item.Tag as string;
                if (!string.IsNullOrEmpty(collisionTypeStr) &&
                    Enum.TryParse(collisionTypeStr, out LDD.Primitives.Collisions.CollisionType collisionType))
                {
                    string menuText = ModelLocalizations.ResourceManager.GetString($"CollisionType_{collisionTypeStr}");
                    menuText = menuText.Replace("&", "&&");
                    item.Text = menuText;
                    item.Click += AddCollisionMenuItem_Click;
                }
            }
        }

        private void InitializeConnectionContextMenu()
        {
            foreach (ToolStripMenuItem item in ContextMenu_AddConnection.DropDownItems)
            {
                string connectionTypeStr = item.Tag as string;
                if (!string.IsNullOrEmpty(connectionTypeStr) && 
                    Enum.TryParse(connectionTypeStr, out LDD.Primitives.Connectors.ConnectorType connectorType))
                {
                    string menuText = ModelLocalizations.ResourceManager.GetString($"ConnectorType_{connectionTypeStr}");
                    menuText = menuText.Replace("&", "&&");
                    item.Text = menuText;
                    item.Click += AddConnectionMenuItem_Click;
                }
            }
        }

        private void AddComponentMenuItem_Click(object sender, EventArgs e)
        {
            string connectionTypeStr = (sender as ToolStripMenuItem).Tag as string;

            if (!string.IsNullOrEmpty(connectionTypeStr) &&
                Enum.TryParse(connectionTypeStr,
                out ModelComponentType componentType))
            {
                var focusedSurfaceNode = GetFocusedParentElement<PartSurface>();

                if (focusedSurfaceNode != null)
                {
                    var selectedSurface = focusedSurfaceNode.Element as PartSurface;
                    var newComponent = SurfaceComponent.CreateEmpty(componentType);
                    selectedSurface.Components.Add(newComponent);

                    ProjectManager.SelectElement(newComponent);
                    SelectElementNodeDelayed(newComponent);
                }
            }
        }

        private void AddConnectionMenuItem_Click(object sender, EventArgs e)
        {
            string connectionTypeStr = (sender as ToolStripMenuItem).Tag as string;

            if (!string.IsNullOrEmpty(connectionTypeStr) &&
                Enum.TryParse(connectionTypeStr, 
                out LDD.Primitives.Connectors.ConnectorType connectorType))
            {
                var focusedBoneNode = GetFocusedParentElement<PartBone>();

                var newConnection = ProjectManager.AddConnection(connectorType, focusedBoneNode?.Element as PartBone);

                if (newConnection != null)
                    SelectElementNodeDelayed(newConnection);
            }
        }

        private void AddCollisionMenuItem_Click(object sender, EventArgs e)
        {
            string collisionTypeStr = (sender as ToolStripMenuItem).Tag as string;

            if (!string.IsNullOrEmpty(collisionTypeStr) &&
                Enum.TryParse(collisionTypeStr, out LDD.Primitives.Collisions.CollisionType collisionType))
            {
                var focusedBoneNode = GetFocusedParentElement<PartBone>();

                var newCollision = ProjectManager.AddCollision(collisionType, focusedBoneNode?.Element as PartBone);

                if (newCollision != null)
                    SelectElementNodeDelayed(newCollision);
            }
        }

        private void ContextMenu_AddSurface_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null)
            {
                var newSurface = ProjectManager.AddSurface();
                SelectElementNodeDelayed(newSurface);
            }
        }

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

            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<ProjectTreeNode>();

            bool canRenameNode = (selectedNodes.Count() == 1 &&
                selectedNodes.First() is ProjectElementNode);

            if (canRenameNode && (selectedNodes.First() as ProjectElementNode).Element is PartSurface)
                canRenameNode = false;

            ContextMenu_Rename.Visible = canRenameNode;
            

            var anyPojectElem = GetSelectedElements().Any();
            ContextMenu_Delete.Enabled = anyPojectElem;
            ContextMenu_Duplicate.Enabled = anyPojectElem;
        }

        private void ContextMenu_Duplicate_Click(object sender, EventArgs e)
        {
            if (ProjectTreeView.SelectedObject is ProjectElementNode projectElementNode)
            {
                ProjectManager.DuplicateElement(projectElementNode.Element);

                //switch (projectElementNode.Element)
                //{
                //    case PartCollision collision:
                //        {
                //            var newCol = collision.Clone();
                //            if (newCol.Parent is PartBone bone)
                //                bone.Collisions.Add(newCol);
                //            else
                //                CurrentProject.Collisions.Add(newCol);
                //            ProjectManager.SelectElement(newCol);
                //            break;
                //        }
                //    case PartConnection connection:
                //        {
                //            var newConn = connection.Clone();
                //            if (newConn.Parent is PartBone bone)
                //                bone.Connections.Add(newConn);
                //            else
                //                CurrentProject.Connections.Add(newConn);
                //            ProjectManager.SelectElement(newConn);
                //            break;
                //        }
                //}
            }
        }

        private void ContextMenu_Rename_Click(object sender, EventArgs e)
        {
            if (ProjectTreeView.SelectedObject != null)
            {
                //ProjectTreeView.EditSubItem(ProjectTreeView.SelectedItem, 0);
                ProjectTreeView.StartCellEdit(ProjectTreeView.SelectedItem, 0);
            }
        }

        private void ContextMenu_Delete_Click(object sender, EventArgs e)
        {
            var elements = GetSelectedElements().ToList();


            if (elements.Count > 1)
            {
                //TODO: show confirmation message when deleting more than one
            }

            ProjectManager.ClearSelection();

            ProjectManager.StartBatchChanges();

            var removedElements = elements.Where(x => x.TryRemove()).ToList();

            if (removedElements.OfType<ModelMeshReference>().Any())
                CurrentProject.RemoveUnreferencedMeshes();

            ProjectManager.EndBatchChanges();

        }

        #endregion

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();

            InitializeViewComboBox();
            RebuildNavigation(true);

            if (CurrentProject != null)
            {
                if (CurrentProject.Surfaces.Count == 1)
                {
                    var surfaceNode = FindElementNode(CurrentProject.Surfaces[0]);
                    if (surfaceNode != null)
                        ProjectTreeView.Expand(surfaceNode);
                }
            }

            //string projectTitle = ProjectManager.GetProjectDisplayName();

            //ViewModeLabel.Text = ProjectManager.IsProjectOpen ? projectTitle : $"<{projectTitle}> ";
            
        }

        protected override void OnProjectElementsChanged()
        {
            base.OnProjectElementsChanged();

            ExecuteOnThread(() => RebuildNavigation(false));
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);
            if (e.PropertyName == nameof(PartElement.Name))
            {
                ExecuteOnThread(() => RefreshElementName(e.Element));
            }
            else if (e.PropertyName == nameof(PartProperties.Flexible) && e.Element is PartProperties)
            {
                ExecuteOnThread(() => RebuildNavigation(true));
            }
        }

        private void RefreshElementName(PartElement element)
        {
            var elemNode = FindElementNode(element);
            if (elemNode != null)
            {
                elemNode.Text = element.Name;
                ProjectTreeView.RefreshObject(elemNode);
            }
        }

        public IEnumerable<PartElement> GetSelectedElements()
        {
            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<ProjectTreeNode>();

            IEnumerable<PartElement> GetTopLevelElems(ProjectTreeNode treeNode)
            {
                if (treeNode is ProjectElementNode eNode)
                    yield return eNode.Element;
                else
                {
                    foreach (var node in treeNode.Nodes)
                    {
                        foreach (var subNode in GetTopLevelElems(node))
                            yield return subNode;
                    }
                }
                
            }

            var result = selectedNodes.SelectMany(x => GetTopLevelElems(x));
            return result;
        }

        public IEnumerable<ProjectTreeNode> GetAllTreeNodes()
        {
            var selectedNodes = ProjectTreeView.Objects.OfType<ProjectTreeNode>();
            return selectedNodes.SelectMany(x => x.GetChildHierarchy(true));
        }

        public ProjectElementNode FindElementNode(PartElement element)
        {
            return GetAllTreeNodes().OfType<ProjectElementNode>()
                .FirstOrDefault(x => x.Element == element);
        }

        private void SelectElementNodeDelayed(PartElement element)
        {
            BeginInvoke((Action)(() =>
            {
                var elementNode = FindElementNode(element);
                if (elementNode.Parent != null)
                    ProjectTreeView.Expand(elementNode.Parent);
                using (FlagManager.UseFlag("ManualSelect"))
                    ProjectTreeView.SelectObject(elementNode);
            }));
        }

        public void SetSelectedNodes(IEnumerable<ProjectTreeNode> nodes)
        {
            using (FlagManager.UseFlag("ManualSelect"))
            {
                FlagManager.Set("WaitForManualSelect", true);
                ProjectTreeView.SelectObjects(nodes.ToList());
            }
        }

        public void SetSelectedNodeIDs(IEnumerable<string> selectedNodeIDs)
        {
            var selectedNodes = GetTreeNodes().Where(x => selectedNodeIDs.Contains(x.NodeID));
            SetSelectedNodes(selectedNodes);
        }

        private ProjectElementNode GetFocusedParentElement<T>() where T : PartElement
        {
            var focusedNode = ProjectTreeView.FocusedObject as ProjectTreeNode;
            return focusedNode.GetParents(true)
                .OfType<ProjectElementNode>()
                .FirstOrDefault(x => x.Element.GetType() == typeof(T));
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();

            if (FlagManager.IsSet("SelectElements"))
                return;

            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(SyncProjectSelection));
            else
                SyncProjectSelection();
        }

        private ProjectTreeNode GetFirstVisibleParentNode(ProjectTreeNode projectNode)
        {
            if (!IsNodeVisible(projectNode))
                return projectNode;

            var treeViewItem = ProjectTreeView.ModelToItem(projectNode);
            if (treeViewItem != null)
                return projectNode;

            while (projectNode?.Parent != null)
            {
                treeViewItem = ProjectTreeView.ModelToItem(projectNode.Parent);
                if (treeViewItem != null)
                    return projectNode.Parent;

                projectNode = projectNode.Parent;
            }

            return projectNode;
        }

        private void SyncProjectSelection()
        {
            if (ProjectManager.SelectedElements.Any())
            {
                var elementNodes = GetAllTreeNodes().OfType<ProjectElementNode>();

                var selectedNodes = elementNodes
                    .Where(x => ProjectManager.SelectedElements.Contains(x.Element))
                    .Select(x => GetFirstVisibleParentNode(x));

                SetSelectedNodes(selectedNodes);
            }
            else
            {
                using (FlagManager.UseFlag("ManualSelect"))
                    ProjectTreeView.SelectedObjects = null;
            }
        }

        #region Drag&Drop Handling

        public class NavigationDragHandler : BrightIdeasSoftware.SimpleDragSource
        {
            public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
            {
                var selectedNodes = olv.SelectedObjects.OfType<ProjectTreeNode>();
                if (selectedNodes.Any())
                {
                    int selectionLevel = selectedNodes.First().TreeLevel;
                    var parent = selectedNodes.First().Parent;

                    if (selectedNodes.All(x => x.CanDragDrop()/* && x.Level == selectionLevel*/))
                        return base.StartDrag(olv, button, item);
                }

                return null;
                //return base.StartDrag(olv, button, item);
            }
        }

        public class NavigationDropHandler : BrightIdeasSoftware.SimpleDropSink
        {
            private DragDropEffects CurrentEffect;

            public NavigationDropHandler()
            {
                CanDropOnItem = true;
                CanDropBetween = true;
            }

            protected override void OnCanDrop(OlvDropEventArgs args)
            {
                base.OnCanDrop(args);
                CurrentEffect = args.Effect;
            }

            public override void DrawFeedback(Graphics g, Rectangle bounds)
            {
                if (CurrentEffect != DragDropEffects.None)
                    base.DrawFeedback(g, bounds);
            }

            protected override void DrawFeedbackItemTarget(Graphics g, Rectangle bounds)
            {
                if (DropTargetItem != null)
                {
                    Rectangle rect = CalculateDropTargetRectangle(DropTargetItem, DropTargetSubItemIndex);
                    //rect.Inflate(1, 1);
                    float diameter = rect.Height / 3;
                    var currentSmoothingMode = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (GraphicsPath path = GetRoundedRect(rect, diameter))
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(48, FeedbackColor)))
                        {
                            g.FillPath(brush, path);
                        }
                        using (Pen pen = new Pen(FeedbackColor, 1f))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                    g.SmoothingMode = currentSmoothingMode;
                }
                //base.DrawFeedbackItemTarget(g, bounds);
            }

            protected override void DrawBetweenLine(Graphics g, int x1, int y1, int x2, int y2)
            {
                using (Brush brush = new SolidBrush(FeedbackColor))
                {
                    int num = x1;
                    int num2 = y1;
                    using (GraphicsPath graphicsPath = new GraphicsPath())
                    {
                        graphicsPath.AddLine(num, num2 + 4, num + 6, num2);
                        graphicsPath.AddLine(num + 6, num2, num, num2 - 4);
                        //graphicsPath.AddLine(num, num2 + 6, num, num2 - 6);
                        //graphicsPath.AddBezier(num, num2 - 6, num + 3, num2 - 2, num + 6, num2 - 1, num + 11, num2);
                        //graphicsPath.AddBezier(num + 11, num2, num + 6, num2 + 1, num + 3, num2 + 2, num, num2 + 6);
                        graphicsPath.CloseFigure();
                        g.FillPath(brush, graphicsPath);
                    }
                    num = x2;
                    num2 = y2;
                    using (GraphicsPath graphicsPath2 = new GraphicsPath())
                    {
                        graphicsPath2.AddLine(num, num2 + 4, num - 6, num2);
                        graphicsPath2.AddLine(num - 6, num2, num, num2 - 4);

                        //graphicsPath2.AddLine(num, num2 + 6, num, num2 - 6);
                        //graphicsPath2.AddBezier(num, num2 - 7, num - 3, num2 - 2, num - 6, num2 - 1, num - 11, num2);
                        //graphicsPath2.AddBezier(num - 11, num2, num - 6, num2 + 1, num - 3, num2 + 2, num, num2 + 7);
                        graphicsPath2.CloseFigure();
                        g.FillPath(brush, graphicsPath2);
                    }
                }

                using (Pen pen = new Pen(FeedbackColor, 2f))
                {
                    g.DrawLine(pen, x1 + 4, y1 - 0.5f, x2 - 4, y2 - 0.5f);
                }
            }

            protected override Rectangle CalculateDropTargetRectangle(OLVListItem item, int subItem)
            {
                if (subItem > 0)
                    return item.SubItems[subItem].Bounds;

                Rectangle result = item.Bounds;
                result.X += 3;
                result.Width -= 6;
                return result;
            }
        }

        private void ProjectTreeView_CanDrop(object sender, BrightIdeasSoftware.OlvDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.DropTargetItem != null &&
                e.DataObject is BrightIdeasSoftware.OLVDataObject dataObj)
            {
                var targetNode = e.DropTargetItem.RowObject as ProjectTreeNode;

                var elementNodes = dataObj.ModelObjects.OfType<ProjectElementNode>().ToList();

                if (e.DropTargetLocation == DropTargetLocation.Item)
                {
                    if (elementNodes.All(x => x.CanDropOn(targetNode)))
                        e.Effect = DragDropEffects.Move;
                }
                else if (e.DropTargetLocation == DropTargetLocation.BelowItem)
                {
                    if (elementNodes.All(x => x.CanDropAfter(targetNode)))
                        e.Effect = DragDropEffects.Move;
                }
                else if (e.DropTargetLocation == DropTargetLocation.AboveItem)
                {
                    if (elementNodes.All(x => x.CanDropBefore(targetNode)))
                        e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void ProjectTreeView_Dropped(object sender, BrightIdeasSoftware.OlvDropEventArgs e)
        {
            if (e.DataObject is BrightIdeasSoftware.OLVDataObject dataObj)
            {
                var elementNodes = dataObj.ModelObjects.OfType<ProjectElementNode>().ToList();
                var targetNode = e.DropTargetItem.RowObject as ProjectTreeNode;
                var targetElement = (targetNode as ProjectElementNode)?.Element;
                var movedNodes = new List<ProjectElementNode>();

                ProjectManager.StartBatchChanges();

                foreach (var elemTypeGroup in elementNodes.GroupBy(x => x.ElementType))
                {
                    var draggedElement = elemTypeGroup.Select(x => x.Element).ToList();

                    IElementCollection targetCollection = null;

                    if (targetNode is ElementCollectionNode elemCollectionNode &&
                        elemCollectionNode.CollectionType == elemTypeGroup.Key)
                    {
                        targetCollection = elemCollectionNode.Collection;
                    }
                    else if (elemTypeGroup.Key == typeof(ModelMeshReference) && 
                        targetElement is SurfaceComponent surface)
                    {
                        targetCollection = surface.Meshes;
                    }
                    else if (targetElement is PartBone bone)
                    {
                        if (elemTypeGroup.Key == typeof(PartCollision))
                        {
                            targetCollection = bone.Collisions;
                        }
                        else if (elemTypeGroup.Key == typeof(PartConnection))
                        {
                            targetCollection = bone.Connections;
                        }
                    }
                    else if (targetElement.GetElementType() == elemTypeGroup.Key)
                    {
                        targetCollection = targetElement.GetParentCollection();
                    }

                    if (targetCollection != null)
                    {
                        using (FlagManager.UseFlag("DragDropping"))
                        {
                            if (targetElement != null && 
                                (e.DropTargetLocation == DropTargetLocation.BelowItem || 
                                e.DropTargetLocation == DropTargetLocation.AboveItem) &&
                                targetCollection.Contains(targetElement))
                            {
                                int targetIndex = targetCollection.IndexOf(targetElement);

                                if (e.DropTargetLocation == DropTargetLocation.AboveItem)
                                {
                                    draggedElement.ForEach(x => x.TryRemove());
                                    targetCollection.InsertAllAt(targetIndex, draggedElement);
                                }
                                else
                                {
                                    draggedElement.ForEach(x => x.TryRemove());
                                    targetCollection.InsertAllAt(targetIndex + 1, draggedElement);
                                }
                            }
                            else
                            {
                                draggedElement.ForEach(x => x.TryRemove());
                                targetCollection.AddRange(draggedElement);
                            }

                            movedNodes.AddRange(elemTypeGroup);    
                        }
                    }
                }

                ProjectManager.EndBatchChanges();

                if (movedNodes.Any())
                    SetSelectedNodes(movedNodes);
            }
        }

        #endregion

    }

}
