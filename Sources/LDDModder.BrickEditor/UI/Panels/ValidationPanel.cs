using LDDModder.BrickEditor.EditModels;
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

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ValidationPanel : ProjectDocumentPanel
    {
        internal ValidationPanel()
        {
            InitializeComponent();
        }

        public ValidationPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
        }


    }
}
