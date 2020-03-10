using LDDModder.BrickEditor.ProjectHandling;
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
            if (SyncSelectionCheckBox.Checked)
            {

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
