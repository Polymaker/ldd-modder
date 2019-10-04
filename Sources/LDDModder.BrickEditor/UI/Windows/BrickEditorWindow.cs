using LDDModder.BrickEditor.UI.Panels;
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
            Navigation.Show(DockPanelControl, DockState.DockLeft);
            Viewport.Show(DockPanelControl, DockState.Document);
        }


        private void LDDEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new LddEnvironmentConfigWindow())
                dlg.ShowDialog();
        }

        private void LddLocalizationsMenuItem_Click(object sender, EventArgs e)
        {
            var existingPanel = DockPanelControl.Documents.OfType<LocalisationEditorPanel>().FirstOrDefault();
            if (existingPanel != null)
            {
                existingPanel.Activate();
                return;
            }

            var locEditPanel = new LocalisationEditorPanel();
            locEditPanel.Show(DockPanelControl, DockState.Document);
        }

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
            if (!(DockPanelControl.ActiveDocument is ViewportPanel))
            {
                Navigation.Hide();
            }
            else
            {
                Navigation.Show();
            }
        }

        private void CreateFromBrickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new SelectBrickDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }
    }
}
