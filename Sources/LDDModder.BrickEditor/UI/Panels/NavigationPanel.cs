using BrightIdeasSoftware;
using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.Resources;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        private FlagManager FlagManager;

        internal NavigationPanel()
        {
            InitializeComponent();
            InitializeNavigationImageList();
            FlagManager = new FlagManager();
        }

        public NavigationPanel(ProjectManager projectManager) : base (projectManager)
        {
            InitializeComponent();
            
            FlagManager = new FlagManager();
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

            label1.Text = "<No active project>";
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
            NavigationImageList.Images.Add("Hidden", Properties.Resources.VisibleIcon);
            
        }

        private void InitializeNavigationTreeView()
        {
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

            ProjectTreeView.DropSink = new NavigationDropHandler();
            ProjectTreeView.DragSource = new NavigationDragHandler();

            ProjectTreeView.SmallImageList = NavigationImageList;
            olvColumnVisible.HeaderImageKey = "Visible";
            olvColumnVisible.ShowTextInHeader = false;
            olvColumnVisible.Sortable = false;
            olvColumnVisible.ImageGetter = (x) => "Visible";

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
                        .OfType<BaseProjectNode>().Select(x => x.NodeID).ToList();
                    selectedNodeIDs.RemoveAll(x => string.IsNullOrEmpty(x));

                    var expandedNodeIDs = ProjectTreeView.ExpandedObjects
                        .OfType<BaseProjectNode>().Select(x => x.NodeID).ToList();
                    expandedNodeIDs.RemoveAll(x => string.IsNullOrEmpty(x));

                    if (recreate)
                    {
                        ProjectTreeView.ClearObjects();

                        ProjectTreeView.AddObject(new ProjectCollectionNode(
                            CurrentProject.Surfaces,
                            ModelLocalizations.Label_Surfaces));

                        if (CurrentProject.Properties.Flexible)
                        {
                            ProjectTreeView.AddObject(new ProjectCollectionNode(
                                CurrentProject.Bones,
                                "Bones"));
                        }
                        else
                        {
                            ProjectTreeView.AddObject(new ProjectCollectionNode(
                            CurrentProject.Collisions,
                            ModelLocalizations.Label_Collisions));

                            ProjectTreeView.AddObject(new ProjectCollectionNode(
                                CurrentProject.Connections,
                                ModelLocalizations.Label_Connections));
                        }

                        foreach (ProjectCollectionNode node in ProjectTreeView.Roots)
                            ProjectTreeView.Expand(node);
                    }
                    else
                    {
                        foreach (BaseProjectNode node in ProjectTreeView.Roots)
                        {
                            node.IsDirty = true;
                            ProjectTreeView.UpdateObject(node);
                        }
                    }


                    ExpandNodes(ProjectTreeView.Roots, expandedNodeIDs);

                    if (selectedNodeIDs.Any())
                    {
                        var selectedNodes = GetTreeNodes().Where(x => selectedNodeIDs.Contains(x.NodeID));
                        SetSelectedNodes(selectedNodes);
                    }
                }
                else
                    ProjectTreeView.ClearObjects();

                FilterNavigation();
            }
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

        private void SelectNodes(IEnumerable nodes, List<string> nodeIDs)
        {
            foreach (BaseProjectNode node in nodes)
            {
                if (nodeIDs.Contains(node.NodeID))
                    ProjectTreeView.SelectedObjects.Add(node); //ProjectTreeView.SelectObject(node);
                
                SelectNodes(node.Childrens, nodeIDs);
            }
        }

        private IEnumerable<BaseProjectNode> GetTreeNodes(IEnumerable nodes = null)
        {
            if (nodes == null)
                nodes = ProjectTreeView.Roots;

            foreach (var node in nodes.OfType<BaseProjectNode>())
            {
                yield return node;

                foreach(var subNode in GetTreeNodes(node.Childrens))
                    yield return subNode;
            }
        }

        private void FilterNavigation()
        {
            ProjectTreeView.ModelFilter = new ModelFilter(x =>
            {
                if (x is BaseProjectNode projectNode)
                    return IsNodeVisible(projectNode);
                return true;
            });
        }

        private bool IsNodeVisible(BaseProjectNode projectNode)
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
                    break;
            }
            return false;
        }

        private void ProjectTreeView_CellEditStarting(object sender, CellEditEventArgs e)
        {
            if (!(e.ListViewItem.RowObject is ProjectElementNode))
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

                newName = CurrentProject.RenameElement(elementNode.Element, newName);
                e.NewValue = newName;
            }
        }

        #endregion

        #region ContextMenu Handling

        private void InitializeContextMenus()
        {
            InitializeCollisionContextMenu();
            InitializeConnectionContextMenu();

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

        private void AddConnectionMenuItem_Click(object sender, EventArgs e)
        {
            string connectionTypeStr = (sender as ToolStripMenuItem).Tag as string;

            if (!string.IsNullOrEmpty(connectionTypeStr) &&
                Enum.TryParse(connectionTypeStr, out LDD.Primitives.Connectors.ConnectorType connectorType))
            {
                var newConnection = PartConnection.Create(connectorType);

                var focusedBoneNode = GetFocusedParentElement<PartBone>();

                if (focusedBoneNode != null)
                    (focusedBoneNode.Element as PartBone).Connections.Add(newConnection);
                else
                    CurrentProject.Connections.Add(newConnection);

                ProjectManager.SelectElement(newConnection);
            }
        }

        private void AddCollisionMenuItem_Click(object sender, EventArgs e)
        {
            string collisionTypeStr = (sender as ToolStripMenuItem).Tag as string;

            if (!string.IsNullOrEmpty(collisionTypeStr) &&
                Enum.TryParse(collisionTypeStr, out LDD.Primitives.Collisions.CollisionType collisionType))
            {
                var newCollision = PartCollision.Create(collisionType, 0.4f);

                var focusedBoneNode = GetFocusedParentElement<PartBone>();

                if (focusedBoneNode != null)
                    (focusedBoneNode.Element as PartBone).Collisions.Add(newCollision);
                else
                    CurrentProject.Collisions.Add(newCollision);

                ProjectManager.SelectElement(newCollision);
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

            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<BaseProjectNode>();

            ContextMenu_Rename.Visible = selectedNodes.Count() == 1 && 
                selectedNodes.First() is ProjectElementNode;

            var anyPojectElem = GetSelectedElements().Any();
            ContextMenu_Delete.Enabled = anyPojectElem;
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
                //TODO: show confirmation message whene deleting more than one
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

            string projectTitle = ProjectManager.GetProjectDisplayName();

            label1.Text = ProjectManager.IsProjectOpen ? projectTitle : $"<{projectTitle}> ";
            
        }

        protected override void OnProjectElementsChanged()
        {
            base.OnProjectElementsChanged();

            if (InvokeRequired)
                BeginInvoke((Action)(() => RebuildNavigation(false)));
            else
                RebuildNavigation(false);
        }

        protected override void OnElementPropertyChanged(ElementValueChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);
            if (e.PropertyName == nameof(PartElement.Name))
            {
                if (InvokeRequired)
                    BeginInvoke((Action)(() => RefreshElementName(e.Element)));
                else
                    RefreshElementName(e.Element);
            }
        }

        private void RefreshElementName(PartElement element)
        {
            var elementNodes = GetAllTreeNodes().OfType<ProjectElementNode>();
            var node = elementNodes.FirstOrDefault(x => x.Element == element);
            if (node != null)
            {
                node.Text = element.Name;
                ProjectTreeView.RefreshObject(node);
            }
        }

        public IEnumerable<PartElement> GetSelectedElements()
        {
            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<BaseProjectNode>();
            var elementNodes = selectedNodes.SelectMany(x => x.GetChildHierarchy(true)).OfType<ProjectElementNode>();
            return elementNodes.Select(x => x.Element);
        }

        public IEnumerable<BaseProjectNode> GetAllTreeNodes()
        {
            var selectedNodes = ProjectTreeView.Objects.OfType<BaseProjectNode>();
            return selectedNodes.SelectMany(x => x.GetChildHierarchy(true));
        }

        public ProjectElementNode FindElementNode(PartElement element)
        {
            return GetAllTreeNodes().OfType<ProjectElementNode>()
                .FirstOrDefault(x => x.Element == element);
        }

        private bool TreeViewItemsSelected = false;

        public void SetSelectedNodes(IEnumerable<BaseProjectNode> nodes)
        {
            using (FlagManager.UseFlag("ManualSelect"))
            {
                TreeViewItemsSelected = true;
                ProjectTreeView.SelectObjects(nodes.ToList());
            }
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

            if (FlagManager.IsSet("SelectElements"))
                return;

            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(SyncProjectSelection));
            else
                SyncProjectSelection();
        }

        private BaseProjectNode GetFirstVisibleParentNode(BaseProjectNode projectNode)
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

        private void ProjectTreeView_SelectionChanged(object sender, EventArgs e)
        {
            if (!(FlagManager.IsSet("BuildingNavTree") ||
                FlagManager.IsSet("ManualSelect") ||
                FlagManager.IsSet("DragDropping")))
            {
                if (TreeViewItemsSelected)
                {
                    TreeViewItemsSelected = false;
                    return;
                }
                using (FlagManager.UseFlag("SelectElements"))
                    ProjectManager.SelectElements(GetSelectedElements());
            }
        }

        #region Drag&Drop Handling

        public class NavigationDragHandler : BrightIdeasSoftware.SimpleDragSource
        {
            public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
            {
                var selectedNodes = olv.SelectedObjects.OfType<BaseProjectNode>();
                if (selectedNodes.Any())
                {
                    int selectionLevel = selectedNodes.First().Level;
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
                var targetNode = e.DropTargetItem.RowObject as BaseProjectNode;

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
                var elemType = elementNodes.FirstOrDefault()?.Element.GetType();

                if (elementNodes.All(x => x.Element.GetType() == elemType))
                {
                    if (elemType == typeof(ModelMeshReference))
                    {
                        var meshRefElems = elementNodes.Select(x => x.Element).OfType<ModelMeshReference>().ToList();
                        var targetNode = e.DropTargetItem.RowObject as BaseProjectNode;
                        var targetElement = (targetNode as ProjectElementNode)?.Element;

                        IElementCollection targetCollection = null;

                        if (targetNode is ProjectElementNode targetElemNode)
                        {
                            if (targetElemNode.Element is SurfaceComponent surfaceComponent)
                            {
                                targetCollection = surfaceComponent.Meshes;
                            }
                            else if (targetElemNode.Element is ModelMeshReference modelRef)
                            {
                                var parentComponent = modelRef.Parent as SurfaceComponent;
                                targetCollection = parentComponent.Meshes;

                                if (parentComponent is FemaleStudModel femaleStud &&
                                    femaleStud.ReplacementMeshes.Contains(modelRef))
                                {
                                    targetCollection = femaleStud.ReplacementMeshes;
                                }
                            }
                        }
                        else if (targetNode is ElementCollectionNode elemCollectionNode)
                        {
                            if (elemCollectionNode.CollectionType == elemType)
                                targetCollection = elemCollectionNode.Collection;
                        }

                        if (targetCollection != null)
                        {
                            using (FlagManager.UseFlag("DragDropping"))
                            {
                                ProjectManager.StartBatchChanges();

                                if (targetElement != null && e.DropTargetLocation == DropTargetLocation.AboveItem)
                                {
                                    int itemIndex = targetCollection.IndexOf(targetElement);
                                    meshRefElems.ForEach(x => x.TryRemove());
                                    targetCollection.InsertAllAt(itemIndex, meshRefElems);
                                }
                                else if (targetElement != null && e.DropTargetLocation == DropTargetLocation.BelowItem)
                                {
                                    int itemIndex = targetCollection.IndexOf(targetElement);
                                    meshRefElems.ForEach(x => x.TryRemove());
                                    targetCollection.InsertAllAt(itemIndex + 1, meshRefElems);
                                }
                                else
                                {
                                    meshRefElems.ForEach(x => x.TryRemove());
                                    targetCollection.AddRange(meshRefElems);
                                }

                                ProjectManager.EndBatchChanges();
                            }

                            SetSelectedNodes(elementNodes);
                        }
                    }
                }
            }
        }




        #endregion


    }

}
