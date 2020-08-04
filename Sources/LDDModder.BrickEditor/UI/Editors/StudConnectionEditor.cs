using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.LDD.Primitives.Connectors;

namespace LDDModder.BrickEditor.UI.Editors
{
    public partial class StudConnectionEditor : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Custom2DFieldConnector StudConnector
        {
            get => studGridControl1.StudConnector;
            set => studGridControl1.StudConnector = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StudGridControl GridEditor => studGridControl1;

        public StudConnectionEditor()
        {
            InitializeComponent();
        }

        private void studGridControl1_ConnectorChanged(object sender, EventArgs e)
        {
            LoadConnectorInfo();
        }

        private void studGridControl1_ConnectorSizeChanged(object sender, EventArgs e)
        {
            LoadConnectorInfo();
        }

        private void LoadConnectorInfo()
        {
            GridHeightBox.Value = StudConnector?.StudHeight ?? 1;
            GridWidthBox.Value = StudConnector?.StudWidth ?? 1;
            GidSizeBox_ValueChanged(null, EventArgs.Empty);
        }

        private void ApplySizeButton_Click(object sender, EventArgs e)
        {
            if (StudConnector != null)
            {
                StudConnector.StudHeight = (int)GridHeightBox.Value;
                StudConnector.StudWidth = (int)GridWidthBox.Value;
            }
        }

        private void GidSizeBox_ValueChanged(object sender, EventArgs e)
        {
            if (StudConnector != null)
            {
                ApplySizeButton.Enabled = StudConnector.StudWidth != (int)GridWidthBox.Value || StudConnector.StudHeight != (int)GridHeightBox.Value;
            }
        }
    }
}
