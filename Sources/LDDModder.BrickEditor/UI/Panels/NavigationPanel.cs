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
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float;
            
            treeListView1.CanExpandGetter += (model) =>
            {
                if (model is BaseProjectNode node)
                    return node.HasChildrens();
                return false;
            };

            treeListView1.ChildrenGetter += (model) =>
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
        }

        private void ViewModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectManager.IsProjectOpen)
                FilterNavigation();
        }

        #region TreeListView Handling

        private void RebuildNavigation(bool recreate)
        {
            if (recreate)
                treeListView1.ClearObjects();

            if (ProjectManager.IsProjectOpen)
            {
                if (recreate)
                {
                    treeListView1.AddObject(new ProjectGroupNode(
                        CurrentProject.Surfaces,
                        ModelLocalizations.Label_Surfaces));

                    treeListView1.AddObject(new ProjectGroupNode(
                        CurrentProject.Collisions,
                        ModelLocalizations.Label_Collisions));

                    treeListView1.AddObject(new ProjectGroupNode(
                        CurrentProject.Connections,
                        ModelLocalizations.Label_Connections));

                    foreach (ProjectGroupNode node in treeListView1.Roots)
                        treeListView1.Expand(node);
                }
                else
                {
                    var expandedNodes = treeListView1.ExpandedObjects
                        .OfType<BaseProjectNode>().Select(x => x.NodeID).ToList();

                    foreach (BaseProjectNode node in treeListView1.Roots)
                    {
                        node.IsDirty = true;
                        treeListView1.UpdateObject(node);
                    }

                    ExpandNodes(treeListView1.Roots, expandedNodes);
                }

                //treeListView1.AddObject(new ProjectGroupNode(
                //    CurrentProject.Bones,
                //    ModelLocalizations.Label_Bones));
            }

            FilterNavigation();
        }

        private void ExpandNodes(IEnumerable nodes, List<string> nodeIDs)
        {
            foreach (BaseProjectNode node in nodes)
            {
                if(nodeIDs.Contains(node.NodeID))
                {
                    treeListView1.Expand(node);
                    ExpandNodes(node.Childrens, nodeIDs);
                }
            }
        }


        private void FilterNavigation()
        {

        }


        #endregion

        protected override void OnProjectElementsChanged(CollectionChangedEventArgs e)
        {
            base.OnProjectElementsChanged(e);
            RebuildNavigation(false);
        }

        private void treeListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!InternalSelection && treeListView1.SelectedObject is ProjectElementNode elementNode)
            {
                ProjectManager.SelectedElement = elementNode.Element;
            }
        }

        protected override void OnSelectedElementChanged(PartElement selectedElement)
        {
            base.OnSelectedElementChanged(selectedElement);
            InternalSelection = true;

            
            InternalSelection = false;
        }
    }
}
