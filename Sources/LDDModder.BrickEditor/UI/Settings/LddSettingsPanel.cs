using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.BrickEditor.Settings;
using System.IO;
using LDDModder.Utilities;
using LDDModder.BrickEditor.UI.Controls;
using LDDModder.LDD.Files;
using LDDModder.LDD;

namespace LDDModder.BrickEditor.UI.Settings
{
    public partial class LddSettingsPanel : SettingsPanelBase
    {
        private LDDEnvironment SettingsEnvironment;
        private LddSettings LddSettings;
        public LddSettingsPanel()
        {
            InitializeComponent();
        }

        public override void FillSettings(AppSettings settings)
        {
            LddSettings = settings.LddSettings;
            SettingsEnvironment = new LDDEnvironment(
                settings.LddSettings.ProgramFilesPath,
                settings.LddSettings.ApplicationDataPath);
            
            FillLddSettings(SettingsEnvironment);
            HasSettingsChanged = false;
        }

        private void FillLddSettings(LDD.LDDEnvironment environment)
        {
            environment?.CheckLifStatus();

            using (FlagManager.UseFlag(nameof(FillLddSettings)))
            {
                PrgmFilePathTextBox.Value = environment?.ProgramFilesPath;
                AppDataPathTextBox.Value = environment?.ApplicationDataPath;
                UpdateLifsStatus(environment);
            }
        }

        public override void ApplySettings(AppSettings settings)
        {
            base.ApplySettings(settings);

            settings.LddSettings.ProgramFilesPath = PrgmFilePathTextBox.Value;
            settings.LddSettings.ApplicationDataPath = AppDataPathTextBox.Value;
        }

        public override bool ValidateSettings()
        {
            if (!string.IsNullOrEmpty(PrgmFilePathTextBox.Value))
            {
                string selectedDir = PrgmFilePathTextBox.Value;
                if (!ValidateProgramPath(ref selectedDir))
                {
                    MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
                    return false;
                }
                else if (PrgmFilePathTextBox.Value != selectedDir)
                    PrgmFilePathTextBox.Value = selectedDir;
            }

            if (!string.IsNullOrEmpty(AppDataPathTextBox.Value))
            {
                string selectedDir = AppDataPathTextBox.Value;
                if (!ValidateAppDataPath(ref selectedDir))
                {
                    MessageBox.Show(this, AppDataDbNotFound, this.Text, MessageBoxButtons.OK);
                    return false;
                }
                else if (AppDataPathTextBox.Value != selectedDir)
                    AppDataPathTextBox.Value = selectedDir;
            }

            return true;
        }

        private void UpdateLifsStatus(LDD.LDDEnvironment environment)
        {
            string tmpPath = environment.ApplicationDataPath;
            if (environment != null && environment.DirectoryExists(LDD.LddDirectory.ApplicationData)
                && ValidateAppDataPath(ref tmpPath))
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
            tmpPath = environment.ProgramFilesPath;

            if (environment != null && environment.DirectoryExists(LDD.LddDirectory.ProgramFiles)
                && ValidateProgramPath(ref tmpPath))
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
            if (FlagManager.IsSet(nameof(FillLddSettings)))
                return;

            var tmpEnv = new LDD.LDDEnvironment(PrgmFilePathTextBox.Value, AppDataPathTextBox.Value);
            tmpEnv.CheckLifStatus();
            UpdateLifsStatus(tmpEnv);
            HasSettingsChanged = !SettingsEnvironment.IsEqual(tmpEnv);
        }

        private void ExtractDBButton_Click(object sender, EventArgs e)
        {
            ExtractLif(LDD.LddLif.DB);
        }

        private void ExtractAssetsButton_Click(object sender, EventArgs e)
        {
            ExtractLif(LDD.LddLif.Assets);
        }

        private void ExtractLif(LDD.LddLif lif)
        {
            try
            {
                var tmpEnv = new LDD.LDDEnvironment(PrgmFilePathTextBox.Value, AppDataPathTextBox.Value);
                string lifFilePath = tmpEnv.GetLifFilePath(lif);
                string lifFolderPath = tmpEnv.GetLifFolderPath(lif);
                if (!string.IsNullOrEmpty(lifFilePath) && File.Exists(lifFilePath))
                {

                    using (var lifFile = LifFile.Open(lifFilePath))
                        lifFile.ExtractTempTo(lifFolderPath);
                }
                //LifFile.Open()
            }
            catch { }
        }

        private void PrgmFilePathTextBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            var sourceBox = sender as BrowseTextBox;

            using (var fbd = new FolderBrowserDialog())
            {
                if (FileHelper.IsValidDirectory(sourceBox.Value, true))
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
                    else if (sourceBox == AppDataPathTextBox && !ValidateAppDataPath(ref selectedDir))
                    {
                        MessageBox.Show(this, AppDataDbNotFound, this.Text, MessageBoxButtons.OK);
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
                FillLddSettings(defaultEnv);
                HasSettingsChanged = !SettingsEnvironment.IsEqual(defaultEnv);
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

        private bool ValidateAppDataPath(ref string appDataPath)
        {
            if (!FileHelper.IsValidDirectory(appDataPath, true))
                return false;

            var dirInfo = new DirectoryInfo(appDataPath);
            if (dirInfo.Name == "LEGO Company")
                appDataPath = Path.Combine(appDataPath, "LEGO Digital Designer");

            string dbLif = Path.Combine(appDataPath, LDD.LDDEnvironment.DB_LIF);
            string dbFolder = Path.Combine(appDataPath, Path.GetFileNameWithoutExtension(LDD.LDDEnvironment.DB_LIF));
            return File.Exists(dbLif) || Directory.Exists(dbFolder);
        }
    }
}
