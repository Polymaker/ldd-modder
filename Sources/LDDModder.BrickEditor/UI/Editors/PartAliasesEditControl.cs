using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.Modding.Editing;

namespace LDDModder.BrickEditor.UI.Controls
{
    public partial class PartAliasesEditControl : UserControl
    {
        public PartProperties PartProperties { get; set; }

        private BindingList<int> PartAliases { get; set; }

        public PartAliasesEditControl()
        {
            InitializeComponent();
            PartAliases = new BindingList<int>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            AliasesListBox.DataSource = PartAliases;
        }

        public void ReloadAliases()
        {
            AliasesListBox.DataSource = null;
            PartAliases = new BindingList<int>();
            
            if (PartProperties != null)
            {
                foreach (var aliasID in PartProperties.Aliases)
                    PartAliases.Add(aliasID);
            }

            AliasesListBox.DataSource = PartAliases;
            AliasIDTextBox.Text = string.Empty;
        }

        private void AliasesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveButton.Enabled = AliasesListBox.SelectedItem != null;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (AliasesListBox.SelectedItem is int selectedAlias)
                PartAliases.Remove(selectedAlias);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AliasIDTextBox.Text) && 
                int.TryParse(AliasIDTextBox.Text, out int partAlias))
            {
                if (!PartAliases.Contains(partAlias))
                {
                    PartAliases.Add(partAlias);
                    AliasIDTextBox.Text = string.Empty;
                }
            }
        }

        private void AliasIDTextBox_TextChanged(object sender, EventArgs e)
        {
            AddButton.Enabled = !string.IsNullOrEmpty(AliasIDTextBox.Text);
        }

        private void CancelEditButton_Click(object sender, EventArgs e)
        {
            if (Parent is ToolStripDropDown dropDown)
            {
                dropDown.Close();
            }
        }

        private void ApplyEditButton_Click(object sender, EventArgs e)
        {
            if (Parent is ToolStripDropDown dropDown)
            {
                PartProperties.Aliases = PartAliases.ToList();
                dropDown.Close();
            }
        }
    }
}
