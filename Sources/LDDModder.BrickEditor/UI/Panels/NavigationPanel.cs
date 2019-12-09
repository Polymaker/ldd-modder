using BrightIdeasSoftware;
using LDDModder.BrickEditor.Models.Navigation;
using LDDModder.BrickEditor.ProjectHandling;
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
        private bool IsDragAndDrop;

        //private class ViewModeModel
        //{
        //    public NavigationViewMode Value { get; set; }
        //    public string Name { get; set; }
        //}

        private NavigationViewMode CurrentViewMode => (NavigationViewMode)ViewModeComboBox.SelectedIndex;

        internal NavigationPanel()
        {
            InitializeComponent();
            InitializeNavigationImageList();
        }

        public NavigationPanel(ProjectManager projectManager) : base (projectManager)
        {
            InitializeComponent();
            InitializeNavigationImageList();

            CloseButtonVisible = false;
            CloseButton = false;
            ElementsMenu_Delete.ShortcutKeys = Keys.Delete;

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

            InitializeContextMenus();

            ProjectTreeView.DropSink = new NavigationDropHandler();
            ProjectTreeView.DragSource = new NavigationDragHandler();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeViewComboBox();

            label1.Text = "<No active project>";
        }

        private void InitializeViewComboBox()
        {
            //int currentIndex = ViewModeComboBox.SelectedIndex;

            ViewModeComboBox.Items.Clear();
            ViewModeComboBox.Items.Add(ViewModeAll.Text);
            ViewModeComboBox.Items.Add(ViewModeSurfaces.Text);

            if (ProjectManager.IsProjectOpen && CurrentProject.Flexible)
            {
                ViewModeComboBox.Items.Add(ViewModeBones.Text);
            }
            else
            {
                ViewModeComboBox.Items.Add(ViewModeCollisions.Text);
                ViewModeComboBox.Items.Add(ViewModeConnections.Text);
            }
            
            
            ViewModeComboBox.SelectedIndex = 1;
        }

        private void InitializeNavigationImageList()
        {
            NavigationImageList.Images.Add("Surface_Main", Properties.Resources.MainSurfaceIcon);
            NavigationImageList.Images.Add("Surface_Decoration", Properties.Resources.DecorationSurfaceIcon);
            NavigationImageList.Images.Add("Model_MaleStud", Properties.Resources.MaleStudIcon);
            NavigationImageList.Images.Add("Mesh", Properties.Resources.MeshIcon);
            ProjectTreeView.SmallImageList = NavigationImageList;
        }

        protected override void OnProjectChanged()
        {
            base.OnProjectChanged();
            RebuildNavigation(true);
            string projectTitle = ProjectManager.GetProjectDisplayName();

            label1.Text = ProjectManager.IsProjectOpen ? projectTitle : $"<{projectTitle}> ";
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
            if (selectedNodes.Any())
            {

            }
            var anyPojectElem = GetSelectedElements().Any();
            ElementsMenu_Delete.Enabled = selectedNodes.Any(x => x is ProjectElementNode);
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
            if (!(InternalSelection || UpdatingNavigation || IsDragAndDrop))
            {
                ProjectManager.SelectElements(GetSelectedElements());
            }
        }

        public IEnumerable<PartElement> GetSelectedElements()
        {
            var selectedNodes = ProjectTreeView.SelectedObjects.OfType<BaseProjectNode>();
            var elementNodes = selectedNodes.SelectMany(x => x.GetChildHierarchy(true)).OfType<ProjectElementNode>();
            return elementNodes.Select(x => x.Element);
        }

        public void SetSelectedNodes(IEnumerable<BaseProjectNode> nodes)
        {
            InternalSelection = true;
            ProjectTreeView.SelectObjects(nodes.ToList());
            InternalSelection = false;
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

                    if (selectedNodes.All(x => x.CanDragDrop() && x.Level == selectionLevel))
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

            protected override Rectangle CalculateDropTargetRectangle(OLVListItem item, int subItem)
            {
                if (subItem > 0)
                    return item.SubItems[subItem].Bounds;

                Rectangle result = item.Bounds;// ListView.CalculateCellTextBounds(item, subItem);
                result.X += 3;
                result.Width -= 6;
                //if (item.IndentCount > 0)
                //{
                //    int width = TreeListView.TreeRenderer.PIXELS_PER_LEVEL;
                //    result.X += width * item.IndentCount;
                //    result.Width -= width * item.IndentCount;
                //}
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
                            IsDragAndDrop = true;
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
                            IsDragAndDrop = false;

                            SetSelectedNodes(elementNodes);
                        }
                    }
                }
            }
        }


        #endregion

    }
    
}
