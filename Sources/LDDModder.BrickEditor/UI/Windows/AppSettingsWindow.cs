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
        private FlagManager FlagManager;
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
            FlagManager = new FlagManager();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            switch (StartupTab)
            {
                case SettingTab.BuildSettings:
                    SettingsTabControl.SelectedTab = BuildSettingsTabPage;
                    break;
            }

            LoadSettings();
        }

        private void LoadSettings()
        {
            FillLddSettings(LDD.LDDEnvironment.Current);
            FillBuildSettingsList();
            SaveBuildCfgBtn.Visible = false;
            CancelBuildCfgBtn.Visible = false;
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

        private void SaveLddEnvironmentSettings()
        {
            if (!string.IsNullOrEmpty(PrgmFilePathTextBox.Value))
            {
                string selectedDir = PrgmFilePathTextBox.Value;
                if (!ValidateProgramPath(ref selectedDir))
                    MessageBox.Show(this, LddExeNotFound, this.Text, MessageBoxButtons.OK);
                else
                    SettingsManager.Current.LddProgramFilesPath = selectedDir;
            }
            if (!string.IsNullOrEmpty(AppDataPathTextBox.Value))
            {
                string selectedDir = AppDataPathTextBox.Value;
                if (!ValidateAppDataPath(ref selectedDir))
                    MessageBox.Show(this, AppDataDbNotFound, this.Text, MessageBoxButtons.OK);
                else
                    SettingsManager.Current.LddApplicationDataPath = selectedDir;
            }
            SettingsManager.SaveSettings();
        }

        #endregion

        #region Build Settings

        private BuildConfiguration SelectedBuildConfig;
        private bool BuildConfigsShown;
        private bool IsEditingConfig;

        private void FillBuildSettingsList()
        {
            var buildConfigs = SettingsManager.GetBuildConfigurations();

            using (FlagManager.UseFlag(nameof(FillBuildSettingsList)))
            {
                
                BuildConfigListView.Items.Clear();

                foreach (var config in buildConfigs)
                {
                    var lvi = new ListViewItem(config.Name)
                    {
                        Tag = config.Clone()
                    };
                    BuildConfigListView.Items.Add(lvi);
                }
            }

            BuildConfiguration configToSelect = null;

            if (SelectedBuildConfig != null)
            {
                configToSelect = buildConfigs.FirstOrDefault(c => c.UniqueID == SelectedBuildConfig.UniqueID);
            }

            configToSelect = configToSelect ?? buildConfigs.First(c => c.InternalFlag == 1);

            SetSelectedBuildConfig(configToSelect, true);
        }

        private void SetSelectedBuildConfig(BuildConfiguration config, bool fillInterface = false)
        {
            using (FlagManager.UseFlag(nameof(SetSelectedBuildConfig)))
            {
                var listConfig = GetSelectedBuildConfigFromList();

                SelectedBuildConfig = config;

                if (listConfig != SelectedBuildConfig)
                {
                    BuildConfigListView.SelectedItems.Clear();

                    if (SelectedBuildConfig != null)
                    {
                        foreach (ListViewItem lvi in BuildConfigListView.Items)
                        {
                            if (lvi.Tag is BuildConfiguration itemConfig)
                            {
                                if (itemConfig.UniqueID == config.UniqueID)
                                {
                                    lvi.Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (fillInterface)
                FillBuildConfigDetails(config);

            UpdateDeleteButtonState();
        }

        private BuildConfiguration GetSelectedBuildConfigFromList()
        {
            if (BuildConfigListView.SelectedItems.Count == 1
               && BuildConfigListView.SelectedItems[0].Tag is BuildConfiguration config)
            {
                return config;
            }

            return null;
        }

        private void BuildConfigListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet(nameof(FillBuildSettingsList)))
                return;

            if (FlagManager.IsSet(nameof(SetSelectedBuildConfig)))
                return;

            if (IsEditingConfig)
            {
                SetSelectedBuildConfig(SelectedBuildConfig, false);
                return;
            }

            if (BuildConfigListView.SelectedItems.Count == 1
                && BuildConfigListView.SelectedItems[0].Tag is BuildConfiguration config)
            {
                SetSelectedBuildConfig(config, true);
            }

            UpdateDeleteButtonState();
        }

        private void UpdateDeleteButtonState()
        {
            DelBuildCfgBtn.Enabled = SelectedBuildConfig != null && 
                SelectedBuildConfig.InternalFlag == 0;
        }

        private void FillBuildConfigDetails(BuildConfiguration config)
        {
            using (FlagManager.UseFlag(nameof(FillBuildConfigDetails)))
            {
                BuildCfg_NameBox.DataBindings.Clear();
                BuildCfg_PathBox.DataBindings.Clear();
                BuildCfg_Lod0Chk.DataBindings.Clear();
                BuildCfg_OverwriteChk.DataBindings.Clear();
                bool isNewConfig = false;

                if (SelectedBuildConfig != null && 
                    string.IsNullOrEmpty(SelectedBuildConfig.UniqueID))
                {
                    SelectedBuildConfig.GenerateUniqueID();
                    isNewConfig = true;
                }

                if (SelectedBuildConfig != null)
                {
                    BuildCfg_NameBox.DataBindings.Add(new Binding(
                        "Text",
                        SelectedBuildConfig,
                        nameof(BuildConfiguration.Name),
                        true,
                        DataSourceUpdateMode.OnValidation
                        )
                    );

                    BuildCfg_NameBox.Enabled = true;
                    BuildCfg_NameBox.ReadOnly = SelectedBuildConfig.IsInternalConfig;
                    if (!SelectedBuildConfig.IsInternalConfig)
                    {
                        BuildCfg_PathBox.DataBindings.Add(new Binding(
                            "Value",
                            SelectedBuildConfig,
                            nameof(BuildConfiguration.OutputPath),
                            true,
                            DataSourceUpdateMode.OnValidation
                            )
                        );
                    }

                    BuildCfg_PathBox.Enabled = !SelectedBuildConfig.IsInternalConfig;

                    BuildCfg_Lod0Chk.DataBindings.Add(new Binding(
                        "Checked",
                        SelectedBuildConfig,
                        nameof(BuildConfiguration.LOD0Subdirectory),
                        true,
                        DataSourceUpdateMode.OnPropertyChanged
                        )
                    );

                    BuildCfg_Lod0Chk.Enabled = SelectedBuildConfig.InternalFlag != 1;

                    BuildCfg_OverwriteChk.DataBindings.Add(new Binding(
                        "Checked",
                        SelectedBuildConfig,
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

                SaveBuildCfgBtn.Visible = isNewConfig;
                CancelBuildCfgBtn.Visible = isNewConfig;
            }
        }

        private void BuildCfg_PathBox_BrowseButtonClicked(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                //if (!string.IsNullOrEmpty(BuildCfg_PathBox.Value) && Dire)
                if (fbd.ShowDialog() == DialogResult.OK)
                    BuildCfg_PathBox.Value = fbd.SelectedPath;
            }
        }

        private void BuildCfgProperty_ValueChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet(nameof(FillBuildConfigDetails)))
                return;

            if (!BuildConfigsShown)
                return;

            BeginEditBuildConfig();
        }

        private void SaveBuildCfgBtn_Click(object sender, EventArgs e)
        {
            var buildConfigs = SettingsManager.GetBuildConfigurations();
            var existingCfg = buildConfigs.FirstOrDefault(c => c.UniqueID == SelectedBuildConfig.UniqueID);

            if (existingCfg == null)
            {
                SettingsManager.Current.BuildSettings.UserDefined.Add(SelectedBuildConfig);
            }
            else
            {
                if (!existingCfg.IsInternalConfig)
                {
                    existingCfg.OutputPath = SelectedBuildConfig.OutputPath;
                    existingCfg.Name = SelectedBuildConfig.Name;
                }
                existingCfg.LOD0Subdirectory = SelectedBuildConfig.LOD0Subdirectory;
                existingCfg.ConfirmOverwrite = SelectedBuildConfig.ConfirmOverwrite;
            }

            SettingsManager.SaveSettings();
            EndEditBuildConfig();
            FillBuildSettingsList();
        }

        private void CancelBuildCfgBtn_Click(object sender, EventArgs e)
        {
            var buildConfigs = SettingsManager.GetBuildConfigurations();
            var cfg = buildConfigs.FirstOrDefault(c => c.UniqueID == SelectedBuildConfig.UniqueID);
            //if (cfg != null)
            FillBuildConfigDetails(cfg);
            EndEditBuildConfig();
        }

        private void BeginEditBuildConfig()
        {
            if (!IsEditingConfig)
            {
                IsEditingConfig = true;
                SaveBuildCfgBtn.Visible = true;
                CancelBuildCfgBtn.Visible = true;
                AddBuildCfgBtn.Enabled = false;
                DelBuildCfgBtn.Enabled = false;
            }
        }

        private void EndEditBuildConfig()
        {
            if (IsEditingConfig)
            {
                IsEditingConfig = false;
                SaveBuildCfgBtn.Visible = false;
                CancelBuildCfgBtn.Visible = false;
                AddBuildCfgBtn.Enabled = true;
                DelBuildCfgBtn.Enabled = !(SelectedBuildConfig?.IsInternalConfig ?? false);
            }
        }

        private void AddBuildCfgBtn_Click(object sender, EventArgs e)
        {
            var newConfig = new BuildConfiguration();
            //newConfig.GenerateUniqueID();
            FillBuildConfigDetails(newConfig);
        }

        private void DelBuildCfgBtn_Click(object sender, EventArgs e)
        {
            if (SelectedBuildConfig != null)
            {
                SettingsManager.Current.BuildSettings.UserDefined.RemoveAll(c => c.UniqueID == SelectedBuildConfig.UniqueID);
                SettingsManager.SaveSettings();
                FillBuildSettingsList();
            }
        }

        private void BuildConfigListView_SizeChanged(object sender, EventArgs e)
        {
            BuildCfgNameColumn.Width = BuildConfigListView.Width - 3;
        }


        #endregion


        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveLddEnvironmentSettings();

        }

        private void SettingsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SettingsTabControl.SelectedTab == BuildSettingsTabPage)
                BuildConfigsShown = true;
        }

        
    }
}
