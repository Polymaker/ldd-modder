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
using LDDModder.Modding.Editing;
using LDDModder.Utilities;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class ConnectorEditor : UserControl
    {
        public Connector CurrentObject { get; private set; }

        private SortableBindingList<ConnectorInfo> SubTypeList { get; set; }

        private FlagManager FlagManager;

        public ConnectorEditor()
        {
            InitializeComponent();
            SubTypeList = new SortableBindingList<ConnectorInfo>();
            SubTypeList.ApplySort("SubType", ListSortDirection.Ascending);
            FlagManager = new FlagManager();
        }

        public void UpdateBindings(Connector connector)
        {
            ConnectionSubTypeCombo.DataBindings.Clear();
            LengthTextBox.DataBindings.Clear();
            StartCappedCheckBox.DataBindings.Clear();
            EndCappedCheckBox.DataBindings.Clear();
            
            if (CurrentObject != null)
            {
                CurrentObject.PropertyValueChanged -= CurrentObject_PropertyValueChanged;
                CurrentObject = null;
            }

            using (FlagManager.UseFlag("UpdateBindings"))
            {
                if (connector != null)
                {
                    FillSubTypeComboBox(connector.Type);

                    ConnectionTypeValueLabel.Text = connector.Type.ToString();

                    SetSubTypeComboValue(connector.SubType);

                    if (connector is AxelConnector ||
                        connector is SliderConnector ||
                        connector is RailConnector)
                    {
                        LengthTextBox.Visible = true;
                        LengthLabel.Visible = true;
                        LengthTextBox.DataBindings.Add(
                            new Binding("Value", connector, "Length",
                            false, DataSourceUpdateMode.OnPropertyChanged));
                    }
                    else
                    {
                        LengthTextBox.Visible = false;
                        LengthLabel.Visible = false;
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
                    else
                    {
                        StartCappedCheckBox.Visible = false;
                        EndCappedCheckBox.Visible = false;
                    }

                    OpenStudPanelButton.Visible = connector is Custom2DFieldConnector;

                    CurrentObject = connector;
                    CurrentObject.PropertyValueChanged += CurrentObject_PropertyValueChanged;
                }
                else
                {
                    ConnectionSubTypeCombo.DataSource = null;
                    LengthTextBox.Visible = false;
                    LengthLabel.Visible = false;
                    StartCappedCheckBox.Visible = false;
                    EndCappedCheckBox.Visible = false;
                    OpenStudPanelButton.Visible = false;
                    ConnectionTypeValueLabel.Text = string.Empty;
                }
            }

            
        }

        private void CurrentObject_PropertyValueChanged(object sender, System.ComponentModel.PropertyValueChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PartConnection.SubType) && CurrentObject != null)
            {
                using (FlagManager.UseFlag("SetSubTypeCombo"))
                    SetSubTypeComboValue(CurrentObject.SubType);
            }
        }

        private void FillSubTypeComboBox(ConnectorType connectorType)
        {
            ConnectionSubTypeCombo.DataSource = null;
            SubTypeList.Clear();

            SubTypeList.AddRange(Resources.ResourceHelper.Connectors
                .Where(x => x.Type == connectorType));
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

        private void ConnectionSubTypeCombo_Validating(object sender, CancelEventArgs e)
        {
            if (ConnectionSubTypeCombo.SelectedItem == null)
            {
                if (!int.TryParse(ConnectionSubTypeCombo.Text, out _))
                    e.Cancel = true;
            }
        }

        private void ConnectionSubTypeCombo_Validated(object sender, EventArgs e)
        {
            if (ConnectionSubTypeCombo.SelectedItem == null &&
                int.TryParse(ConnectionSubTypeCombo.Text, out int subTypeID))
            {
                SetSubTypeComboValue(subTypeID);
            }
        }

        private void SetSubTypeComboValue(int subTypeID)
        {
            var connInfo = SubTypeList.FirstOrDefault(x => x.SubType == subTypeID);
            if (connInfo == null)
            {
                connInfo = new ConnectorInfo()
                {
                    SubType = subTypeID,
                    //Type = 
                };
                SubTypeList.AddSorted(connInfo);
            }
            ConnectionSubTypeCombo.SelectedItem = connInfo;
        }


        private void ConnectionSubTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("UpdateBindings") || FlagManager.IsSet("SetSubTypeCombo"))
                return;

            if (CurrentObject != null && 
                ConnectionSubTypeCombo.SelectedItem is ConnectorInfo connInfo)
            {
                CurrentObject.SubType = connInfo.SubType;
            }
        }
    }
}
