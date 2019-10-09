using LDDModder.LDD.Files;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.LifExtractor.Windows
{
    public partial class CreateFolderDialog : Form
    {
        public LifFile.FolderEntry ParentFolder { get; set; }

        public string FolderName { get; set; }

        public CreateFolderDialog()
        {
            InitializeComponent();
            FolderName = string.Empty;
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            if (ValidateFolderName(FolderNameTextBox.Text))
            {
                FolderName = FolderNameTextBox.Text.Trim();
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateFolderName(string foldername)
        {
            foldername = foldername.Trim();

            if (string.IsNullOrWhiteSpace(foldername))
            {
                MessageBox.Show(this, "The name cannot be empty.", "Validation");
                return false;
            }

            if (!FileHelper.IsValidDirectoryName(foldername))
            {
                MessageBox.Show(this, "The name contains invalid characters.", "Validation");
                return false;
            }

            if (ParentFolder.ContainsEntryName(foldername))
            {
                MessageBox.Show(this, "An entry with the same name already exist.", "Validation");
                return false;
            }

            return true;
        }
    }
}
