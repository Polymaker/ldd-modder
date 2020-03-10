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

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class ConnectorEditor : UserControl
    {
        public Connector CurrentObject { get; private set; }


        public ConnectorEditor()
        {
            InitializeComponent();
        }

        public void UpdateBindings(Connector connector)
        {
            SubTypeComboBox.DataBindings.Clear();
            LengthTextBox.DataBindings.Clear();
            StartCappedCheckBox.DataBindings.Clear();
            EndCappedCheckBox.DataBindings.Clear();

            LengthTextBox.Visible = false;
            StartCappedCheckBox.Visible = false;
            EndCappedCheckBox.Visible = false;

            if (connector != null)
            {
                FillSubTypeComboBox(connector.Type);

                SubTypeComboBox.DataBindings.Add(
                    new Binding("SelectedValue", connector, "SubType", 
                    false, DataSourceUpdateMode.OnValidation));

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
                
            }
        }

        private void FillSubTypeComboBox(ConnectorType connectorType)
        {

        }
    }
}
