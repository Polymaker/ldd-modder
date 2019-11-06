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
                if (model is ProjectItemNode node)
                    return node.HasChildrens();
                else if (model is PartElementTreeNode treeNode)
                    return treeNode.HasChildrens();
                return false;
            };

            treeListView1.ChildrenGetter += (model) =>
            {
                if (model is ProjectItemNode node)
                    return new ArrayList(node.Childrens);
                else if (model is PartElementTreeNode treeNode)
                    return treeNode.Childrens;
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
            RebuildNavigation();
        }

        private void ViewModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectManager.IsProjectOpen)
                RebuildNavigation();
        }

        #region TreeListView Handling

        private void RebuildNavigation()
        {
            treeListView1.ClearObjects();

            if (ProjectManager.IsProjectOpen)
            {
                switch (CurrentViewMode)
                {
                    case NavigationViewMode.All:
                        {
                            var surfacesNode = new PartElementTreeNode()
                            {
                                Text = ModelLocalizations.Label_Surfaces
                            };
                            surfacesNode.SetChildrens(GenerateSurfaceNodes());
                            treeListView1.AddObject(surfacesNode);

                            var collisionsNode = new PartElementTreeNode()
                            {
                                Text = ModelLocalizations.Label_Collisions
                            };
                            collisionsNode.SetChildrens(GenerateCollisionNodes());
                            treeListView1.AddObject(collisionsNode);
                        }
                        break;
                    case NavigationViewMode.Surfaces:
                        treeListView1.AddObjects(GenerateSurfaceNodes().ToList());
                        break;
                    case NavigationViewMode.Collisions:
                        treeListView1.AddObjects(GenerateCollisionNodes().ToList());
                        break;
                    case NavigationViewMode.Connections:
                        break;
                    case NavigationViewMode.Bones:
                        break;
                }
            }
        }

        private IEnumerable<PartElementTreeNode> GenerateSurfaceNodes()
        {
            if (CurrentProject != null)
            {
                foreach (var surface in CurrentProject.Surfaces)
                {
                    string nodeText = surface.SurfaceID == 0 ? ModelLocalizations.Label_MainSurface :
                            string.Format(ModelLocalizations.Label_DecorationSurfaceNumber, surface.SurfaceID);

                    yield return new PartElementTreeNode(surface, nodeText);
                }
            }
            yield break;
        }

        private IEnumerable<PartElementTreeNode> GenerateCollisionNodes()
        {
            if (CurrentProject != null)
            {
                foreach (var collision in CurrentProject.Collisions)
                {
                    var collisionNode = new PartElementTreeNode()
                    {
                        Project = CurrentProject,
                        Element = collision,
                        Text = collision.Name
                    };
                    yield return collisionNode;
                }
            }
            yield break;
        }

        //private static 

        #endregion

        protected override void OnProjectElementsChanged(CollectionChangedEventArgs e)
        {
            base.OnProjectElementsChanged(e);
            switch (CurrentViewMode)
            {
                case NavigationViewMode.All:
                    break;
                case NavigationViewMode.Surfaces:
                    
                    break;
                case NavigationViewMode.Collisions:
                    if (e.ElementType == typeof(PartBoxCollision) ||
                        e.ElementType == typeof(PartSphereCollision))
                        RebuildNavigation();
                    break;
                case NavigationViewMode.Connections:
                    if (e.ElementType == typeof(PartConnection))
                        RebuildNavigation();
                    break;
                case NavigationViewMode.Bones:
                    break;
            }
        }

        class PartElementTreeNode
        {
            public PartProject Project { get; set; }
            public PartElement Element { get; set; }

            public string Text { get; set; }

            public bool IsDirty { get; set; } = true;

            public List<PartElementTreeNode> Childrens { get; set; } = new List<PartElementTreeNode>();

            public PartElementTreeNode()
            {
            }

            public PartElementTreeNode(PartElement element, string text)
            {
                Element = element;
                Project = element.Project;
                Text = text;
            }

            public void SetChildrens(IEnumerable<PartElementTreeNode> childrens)
            {
                Childrens.Clear();
                Childrens.AddRange(childrens);
                IsDirty = false;
            }

            public bool HasChildrens()
            {
                if (IsDirty)
                    RebuildChildrens();
                return Childrens.Any();
            }

            public void RebuildChildrens()
            {
                if (Element != null)
                {
                    Childrens.Clear();

                    if (Element is PartSurface surface)
                    {
                        foreach (ModelComponentType componentType in Enum.GetValues(typeof(ModelComponentType)))
                        {
                            var components = surface.Components.Where(x => x.ComponentType == componentType);
                            var compNodes = components.Select(x =>
                                new PartElementTreeNode(x, x.Name)
                            ).ToList();

                            if (compNodes.Count <= 4)
                            {
                                Childrens.AddRange(compNodes);
                            }
                            else
                            {
                                var typeNode = new PartElementTreeNode()
                                {
                                    Text = componentType.ToString()
                                };
                                typeNode.SetChildrens(compNodes);
                                Childrens.Add(typeNode);
                            }
                        }
                    }
                }
                IsDirty = false;
            }
        }
    }
}
