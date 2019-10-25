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
        public NavigationPanel()
        {
            InitializeComponent();
            CloseButtonVisible = false;
            CloseButton = false;
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float;

            treeListView1.CanExpandGetter += (model) =>
            {
                if (model is ProjectItemNode node)
                    return node.HasChildrens();
                return false;
            };

            treeListView1.ChildrenGetter += (model) =>
            {
                if (model is ProjectItemNode node)
                    return new ArrayList(node.Childrens);
                return new ArrayList();
            };
        }

        public void LoadPartProject(PartProject project)
        {
            treeListView1.ClearObjects();
            
            olvColumn1.Text = project.PartID > 0 ? 
                ModelLocalizations.Label_PartProjectName.Replace("%partid%", project.PartID.ToString()) : 
                ModelLocalizations.Label_NewPartProject;

            var surfacesNode = new SurfacesGroupNode(project);
            treeListView1.AddObject(surfacesNode);

            var connectorsNode = new ConnectionsGroupNode(project);
            treeListView1.AddObject(connectorsNode);
            
            treeListView1.Expand(surfacesNode);
            treeListView1.Expand(connectorsNode);
        }
    }
}
