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
        public BrickEditorWindow()
        {
            InitializeComponent();
            visualStudioToolStripExtender1.SetStyle(toolStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, dockPanel1.Theme);
            InitializePanels();
        }

        private void InitializePanels()
        {
            var navigation = new DockContent
            {
                Text = "Navigation",
                CloseButtonVisible = false,
                CloseButton = false,
                DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float
            };

            navigation.Show(dockPanel1, DockState.DockLeft);
        }
    }
}
