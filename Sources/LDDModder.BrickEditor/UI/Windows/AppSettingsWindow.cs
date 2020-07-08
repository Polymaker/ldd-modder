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
        private bool LoadingSettings;

        public enum SettingTab
        {
            LddPaths,
            ProjectSettings
        }

        public SettingTab StartupTab { get; set; }

        public AppSettingsWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.BrickStudioIcon;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FillSettings(LDD.LDDEnvironment.Current);
        }

        private void FillSettings(LDD.LDDEnvironment environment)
        {
            LoadingSettings = true;

            PrgmFilePathTextBox.Value = environment?.ProgramFilesPath;
            AppDataPathTextBox.Value = environment?.ApplicationDataPath;
            UserCreationPathTextBox.Value = environment?.UserCreationPath;

            UpdateLifsStatus(environment);

            LoadingSettings = false;
        }

        private void UpdateLifsStatus(LDD.LDDEnvironment environment)
        {
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

        private void LddPathTextBoxes_ValueChanged(object sender, EventArgs e)
        {
            if (LoadingSettings)
                return;
            var tmpEnv = new LDD.LDDEnvironment(PrgmFilePathTextBox.Value, AppDataPathTextBox.Value);
            tmpEnv.CheckLifStatus();
            UpdateLifsStatus(tmpEnv);
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

            using (var fbd = new FolderBrowserDialog())
            {
                if (FileHelper.IsValidDirectory(sourceBox.Value , true))
                    fbd.SelectedPath = sourceBox.Value;
                else
                {
                    if (sourceBox == PrgmFilePathTextBox)
                        fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                    else if (sourceBox == AppDataPathTextBox)
                        fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    
                    string selectedDir = fbd.SelectedPath;

                    if (sourceBox == PrgmFilePathTextBox && !ValidateProgramPath(ref selectedDir))
                    {
                        MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
                    }
                    else
                        sourceBox.Value = selectedDir;
                }
            }
        }


        private void FindEnvironmentButton_Click(object sender, EventArgs e)
        {
            var defaultEnv = LDD.LDDEnvironment.FindInstalledEnvironment();

            if (defaultEnv != null)
            {
                defaultEnv.CheckLifStatus();
                FillSettings(defaultEnv);
            }
            else
                MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
        }

        private bool ValidateProgramPath(ref string programDir)
        {
            //string programDir = PrgmFilePathTextBox.Value;
            if (!FileHelper.IsValidDirectory(programDir, true))
                return false;

            var dirInfo = new DirectoryInfo(programDir);
            if (dirInfo.Name == "LEGO Company")
                programDir = Path.Combine(programDir, "LEGO Digital Designer");

            string exePath = Path.Combine(programDir, LDD.LDDEnvironment.EXE_NAME);

            return File.Exists(exePath);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PrgmFilePathTextBox.Value))
            {
                string selectedDir = PrgmFilePathTextBox.Value;
                if (!ValidateProgramPath(ref selectedDir))
                    MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
                else
                    SettingsManager.Current.LddProgramFilesPath = selectedDir;
            }

            SettingsManager.SaveSettings();

        }

        
    }
}
