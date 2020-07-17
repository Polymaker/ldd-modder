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
using LDDModder.BrickEditor.UI.Panels;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class ConnectorEditor : UserControl
    {
        public Connector CurrentObject { get; private set; }
        private List<ConnectorInfo> SubTypeList { get; set; }

        public ConnectorEditor()
        {
            InitializeComponent();
            SubTypeList = new List<ConnectorInfo>();
        }

        public void UpdateBindings(Connector connector)
        {
            ConnectionSubTypeCombo.DataBindings.Clear();
            LengthTextBox.DataBindings.Clear();
            StartCappedCheckBox.DataBindings.Clear();
            EndCappedCheckBox.DataBindings.Clear();

            LengthTextBox.Visible = false;
            StartCappedCheckBox.Visible = false;
            EndCappedCheckBox.Visible = false;
            OpenStudPanelButton.Visible = false;
            ConnectionTypeValueLabel.Text = string.Empty;

            if (connector != null)
            {
                FillSubTypeComboBox(connector.Type);
                ConnectionTypeValueLabel.Text = connector.Type.ToString();

                //ConnectionSubTypeCombo.DataBindings.Add(
                //    new Binding("SelectedValue", connector, "SubType", 
                //    true, DataSourceUpdateMode.OnPropertyChanged));

                if (SubTypeList.Any(x => x.SubType == connector.SubType))
                    ConnectionSubTypeCombo.SelectedValue = connector.SubType;
                else
                    ConnectionSubTypeCombo.Text = connector.SubType.ToString();

                if (connector is AxelConnector || 
                    connector is SliderConnector || 
                    connector is RailConnector)
                {
                    LengthTextBox.Visible = true;

                    LengthTextBox.DataBindings.Add(
                        new Binding("Value", connector, "Length",
                        false, DataSourceUpdateMode.OnPropertyChanged));
                }

                if (connector is AxelConnector ||
                    connector is SliderConnector)
                {
                    StartCappedCheckBox.Visible = true;
                    EndCappedCheckBox.Visible = true;

                    StartCappedCheckBox.DataBindings.Add(
                        new Binding("Checked", connector, "StartCapped",
                        false, DataSourceUpdateMode.OnPropertyChanged));

                    EndCappedCheckBox.DataBindings.Add(
                        new Binding("Checked", connector, "EndCapped",
                        false, DataSourceUpdateMode.OnPropertyChanged));
                }

                OpenStudPanelButton.Visible = connector is Custom2DFieldConnector;
            }
            else
            {
                ConnectionSubTypeCombo.DataSource = null;
            }
        }

        private void FillSubTypeComboBox(ConnectorType connectorType)
        {
            ConnectionSubTypeCombo.DataSource = null;
            SubTypeList = Resources.ResourceHelper.Connectors
                .Where(x => x.Type == connectorType).ToList();
            ConnectionSubTypeCombo.DataSource = SubTypeList;
            ConnectionSubTypeCombo.DisplayMember = "SubTypeDisplayText";
            ConnectionSubTypeCombo.ValueMember = "SubType";
        }

        private void OpenStudPanelButton_Click(object sender, EventArgs e)
        {
            var test = (ParentForm as ElementDetailPanel);
            test.EditorWindow.StudConnectionPanel.Activate();
            //test.EditorWindow.StudConnectionPanel.Show(test.EditorWindow.DockPanelControl, DockState.Document);
        }
    }
}
