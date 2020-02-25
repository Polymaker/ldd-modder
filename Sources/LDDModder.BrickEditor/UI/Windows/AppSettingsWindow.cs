using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Controls;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class AppSettingsWindow : Form
    {
        public AppSettingsWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillSettings(LDD.LDDEnvironment.Current);
        }

        private void FillSettings(LDD.LDDEnvironment environment)
        {
            PrgmFilePathTextBox.Value = environment?.ProgramFilesPath;
            AppDataPathTextBox.Value = environment?.ApplicationDataPath;
            UserCreationPathTextBox.Value = environment?.UserCreationPath;

            if (environment != null && environment.DirectoryExists(LDD.LddDirectory.ApplicationData))
            {
                
                DbStatusLabel.ForeColor = environment.DatabaseExtracted ? Color.Green : Color.Red;
                DbStatusLabel.Text = environment.DatabaseExtracted ? LifExtractedMessage : LifNotExtractedMessage;
                ExtractDBButton.Enabled = !environment.DatabaseExtracted;
            }
            else
            {
                DbStatusLabel.ForeColor = Color.Black;
                DbStatusLabel.Text = LifNotFoundMessage;
                ExtractDBButton.Enabled = false;
            }

            if (environment != null && environment.DirectoryExists(LDD.LddDirectory.ProgramFiles))
            {
                AssetsStatusLabel.ForeColor = environment.AssetsExtracted ? Color.Green : Color.Red;
                AssetsStatusLabel.Text = environment.AssetsExtracted ? LifExtractedMessage : LifNotExtractedMessage;
                ExtractAssetsButton.Enabled = !environment.AssetsExtracted;
            }
            else
            {
                AssetsStatusLabel.ForeColor = Color.Black;
                AssetsStatusLabel.Text = LifNotFoundMessage;
                ExtractAssetsButton.Enabled = false;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ExtractDBButton_Click(object sender, EventArgs e)
        {

        }

        private void ExtractAssetsButton_Click(object sender, EventArgs e)
        {

        }

        private void PrgmFilePathTextBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            var sourceBox = sender as BrowseTextBox;
            string oldValue = sourceBox.Value;
            using (var fbd = new FolderBrowserDialog())
            {
                if (FileHelper.IsValidDirectory(sourceBox.Value , true))
                    fbd.SelectedPath = sourceBox.Value;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    sourceBox.Value = fbd.SelectedPath;

                    if (sourceBox == PrgmFilePathTextBox && !ValidateProgramPath())
                    {
                        MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
                        sourceBox.Value = oldValue;
                    }
                }
            }
        }


        private void FindEnvironmentButton_Click(object sender, EventArgs e)
        {
            var defaultEnv = LDD.LDDEnvironment.GetInstalledEnvironment();

            if (defaultEnv != null)
            {
                defaultEnv.CheckLifStatus();
                FillSettings(defaultEnv);
            }
            else
                MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
        }

        private bool ValidateProgramPath()
        {
            if (!FileHelper.IsValidDirectory(PrgmFilePathTextBox.Value, true))
                return false;

            string exePath = Path.Combine(PrgmFilePathTextBox.Value, LDD.LDDEnvironment.EXE_NAME);
            return File.Exists(exePath);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PrgmFilePathTextBox.Value))
            {
                if (!ValidateProgramPath())
                    MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
                else
                    SettingsManager.Current.LddProgramFilesPath = PrgmFilePathTextBox.Value;
            }

            SettingsManager.SaveSettings();
        }
    }
}
