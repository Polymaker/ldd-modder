using LDDModder.BrickEditor.ProjectHandling;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var conn = ProjectManager.SelectedElements.OfType<PartConnection>().FirstOrDefault();
            if (conn != null)
            {
                if (conn.ConnectorType == LDD.Primitives.Connectors.ConnectorType.Axel)
                {
                    var test = conn.ConnectorProxy;
                    test.Length += 2;
                }
            }
        }
    }
}
