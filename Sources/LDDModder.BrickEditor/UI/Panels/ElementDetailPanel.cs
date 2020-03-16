using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.LDD.Primitives.Connectors;
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

namespace LDDModder.BrickEditor.UI.Panels
{
    public partial class ElementDetailPanel : ProjectDocumentPanel
    {
        public ElementDetailPanel()
        {
            InitializeComponent();
        }

        public ElementDetailPanel(ProjectManager projectManager) : base(projectManager)
        {
            InitializeComponent();
            SelectedElementComboBox.ComboBox.DrawItem += SelectedElementComboBox_DrawItem;
        }

        protected override void OnElementSelectionChanged()
        {
            base.OnElementSelectionChanged();
 
            ExecuteOnThread(() =>
            {
                FillSelectionDetails();
            });
        }

        private void FillSelectionDetails()
        {
            //if (SyncSelectionCheckBox.Checked)
            //{


            //}

            if (studGridControl1.Tag != null)
            {
                if (studGridControl1.IsEditingNode)
                    studGridControl1.CancelEditNode();
                studGridControl1.StudConnector = null;
                studGridControl1.Visible = false;
                studGridControl1.Tag = null;
            }

            if (transformEditor1.Tag != null)
            {
                transformEditor1.BindPhysicalElement(null);
                transformEditor1.Visible = false;
            }
            
            if (ProjectManager.SelectedElement is IPhysicalElement physicalElement)
            {
                transformEditor1.BindPhysicalElement(physicalElement);
                transformEditor1.Tag = physicalElement;
                transformEditor1.Visible = true;
            }

            if (ProjectManager.SelectedElement is PartConnection partConnection)
            {
                if (partConnection.ConnectorType == ConnectorType.Custom2DField)
                {
                    studGridControl1.StudConnector = partConnection.GetConnector<Custom2DFieldConnector>();
                    studGridControl1.Visible = true;
                    studGridControl1.Tag = partConnection;
                }
            }
        }

        protected override void OnProjectElementsChanged()
        {
            base.OnProjectElementsChanged();
            UpdateSelectedElementComboBox();
        }

        private void SelectedElementComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("UpdateSelectedElementComboBox"))
                return;

            if (SyncSelectionCheckBox.Checked)
            {
                
            }
        }

        private void UpdateSelectedElementComboBox()
        {
            using (FlagManager.UseFlag("UpdateSelectedElementComboBox"))
            {
                var combo = SelectedElementComboBox.ComboBox;

                var elementList = new List<LDDModder.Modding.Editing.PartElement>();
                //elementList.AddRange(CurrentProject.su)
            }
        }

        private void SelectedElementComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {

        }
    }
}
