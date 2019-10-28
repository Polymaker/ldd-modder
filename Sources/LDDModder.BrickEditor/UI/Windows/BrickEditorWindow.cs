using LDDModder.BrickEditor.UI.Panels;
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
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BrickEditorWindow : Form
    {
        private NavigationPanel Navigation;
        private ViewportPanel Viewport;

        public PartProject CurrentProject { get; private set; }
        //private string TemporaryFolder;

        public BrickEditorWindow()
        {
            InitializeComponent();
            visualStudioToolStripExtender1.SetStyle(menuStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, DockPanelControl.Theme);
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializePanels();
        }

        private void InitializePanels()
        {
            Navigation = new NavigationPanel();
            Viewport = new ViewportPanel();
            
            Viewport.Show(DockPanelControl, DockState.Document);
            DockPanelControl.DockLeftPortion = 250d / Width;
            Navigation.Show(DockPanelControl, DockState.DockLeft);
            
        }

        #region Main menu

        private void CreateFromBrickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SelectBrickDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var selectedBrick = dlg.SelectedBrick;
                    var project = PartProject.CreateFromLddPart(selectedBrick.PartId);
                    project.GenerateProjectXml().Save("poject.xml");
                    LoadPartProject(project);
                }
            }
        }

        private void NewProjectMenuItem_Click(object sender, EventArgs e)
        {
            var project = PartProject.CreateEmptyProject();
            LoadPartProject(project);
        }

        private void LDDEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new LddEnvironmentConfigWindow())
                dlg.ShowDialog();
        }

        private void ExportBrickMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new ModelImportExportWindow())
                frm.ShowDialog();
        }

        #endregion


        #region Project Handling

        private void LoadPartProject(PartProject project)
        {
            if (CurrentProject != project)
            {
                if (CurrentProject != null)
                {
                    //todo ask for confirmation
                }

                CurrentProject = project;
                Navigation.LoadPartProject(project);
                Viewport.LoadPartProject(project);
            }
        }

        #endregion

        

        private void BrickEditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var form in DockPanelControl.Documents.OfType<DockContent>().ToList())
            {
                form.Close();
                if (!form.IsDisposed)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void DockPanelControl_ActiveDocumentChanged(object sender, EventArgs e)
        {
            
        }

        
    }
}
