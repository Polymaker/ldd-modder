using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Controls;
using LDDModder.LDD.Files;
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
            LddEnvironment,
            BuildSettings,
            //ProjectSettings
        }

        public SettingTab StartupTab { get; set; }

        public AppSettingsWindow()
        {
            InitializeComponent();
            Icon = Properties.Resources.BrickStudioIcon;
            StartupTab = SettingTab.LddEnvironment;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings();
            switch (StartupTab)
            {
                case SettingTab.BuildSettings:
                    tabControl1.SelectedTab = BuildSettingsTabPage;
                    break;
            }
        }

        private void LoadSettings()
        {
            FillLddSettings(LDD.LDDEnvironment.Current);
            FillBuildSettingsList();
        }

        #region LDD Settings

        private void FillLddSettings(LDD.LDDEnvironment environment)
        {
            environment.CheckLifStatus();

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

        #endregion

        #region Build Settings

        private void FillBuildSettingsList()
        {
            var buildConfigs = SettingsManager.GetBuildConfigurations();

            foreach (var config in buildConfigs)
            {
                var lvi = new ListViewItem(config.Name)
                {
                    Tag = config
                };
                BuildConfigListView.Items.Add(lvi);
            }

            BuildConfigListView.Items[0].Selected = true;
        }


        private void BuildConfigListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BuildConfigListView.SelectedItems.Count == 1
                && BuildConfigListView.SelectedItems[0].Tag is BuildConfiguration config)
            {
                FillBuildConfigDetails(config);
            }
        }

        private void FillBuildConfigDetails(BuildConfiguration config)
        {
            BuildCfg_NameBox.DataBindings.Clear();
            BuildCfg_PathBox.DataBindings.Clear();
            BuildCfg_Lod0Chk.DataBindings.Clear();
            BuildCfg_OverwriteChk.DataBindings.Clear();

            if (config != null)
            {
                BuildCfg_NameBox.DataBindings.Add(new Binding(
                    "Text",
                    config,
                    nameof(BuildConfiguration.Name),
                    true,
                    DataSourceUpdateMode.OnValidation
                    )
                );

                BuildCfg_NameBox.Enabled = true;
                BuildCfg_NameBox.ReadOnly = config.IsInternalConfig;

                if (!config.IsInternalConfig)
                {
                    BuildCfg_PathBox.DataBindings.Add(new Binding(
                        "Value",
                        config,
                        nameof(BuildConfiguration.OutputPath),
                        true,
                        DataSourceUpdateMode.OnValidation
                        )
                    );
                }

                BuildCfg_PathBox.Enabled = !config.IsInternalConfig;

                BuildCfg_Lod0Chk.DataBindings.Add(new Binding(
                    "Checked",
                    config,
                    nameof(BuildConfiguration.LOD0Subdirectory),
                    true,
                    DataSourceUpdateMode.OnPropertyChanged
                    )
                );

                BuildCfg_Lod0Chk.Enabled = config.InternalFlag != 1;

                BuildCfg_OverwriteChk.DataBindings.Add(new Binding(
                    "Checked",
                    config,
                    nameof(BuildConfiguration.ConfirmOverwrite),
                    true,
                    DataSourceUpdateMode.OnPropertyChanged
                    )
                );
                BuildCfg_OverwriteChk.Enabled = true;
            }
            else
            {
                BuildCfg_NameBox.Enabled = false;
                BuildCfg_PathBox.Enabled = false;
                BuildCfg_Lod0Chk.Enabled = false;
                BuildCfg_OverwriteChk.Enabled = false;
            }
        }

        #endregion


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

        private void BuildCfg_PathBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            using(var fbd = new FolderBrowserDialog())
            {
                //if (!string.IsNullOrEmpty(BuildCfg_PathBox.Value) && Dire)
                if (fbd.ShowDialog() == DialogResult.OK)
                    BuildCfg_PathBox.Value = fbd.SelectedPath;
            }
        }
    }
}
