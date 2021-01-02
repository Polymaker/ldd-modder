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

namespace LDDModder.BrickEditor.UI.Settings
{
    public partial class EditorSettingsPanel : SettingsPanelBase
    {
        private BuildConfiguration SelectedBuildConfig;
        private ProjectBuildSettings BuildSettings;
        private EditorSettings EditorSettings;
        private bool IsEditingConfig;

        public EditorSettingsPanel()
        {
            InitializeComponent();
        }

        public override void FillSettings(AppSettings settings)
        {
            base.FillSettings(settings);
            BuildSettings = settings.BuildSettings;
            EditorSettings = settings.EditorSettings;
            UsernameTextbox.Text = EditorSettings.Username;
            WorkspaceBrowseBox.Value = EditorSettings.ProjectWorkspace;
            numericUpDown1.Value = EditorSettings.BackupInterval;

            FillBuildSettingsList();
        }

        public override void ApplySettings(AppSettings settings)
        {
            base.ApplySettings(settings);
            settings.EditorSettings.Username = UsernameTextbox.Text;
            settings.EditorSettings.BackupInterval = (int)numericUpDown1.Value;
            settings.BuildSettings.LDD.Update(BuildSettings.LDD);
            settings.BuildSettings.Manual.Update(BuildSettings.Manual);
            settings.BuildSettings.UserDefined.Clear();
            settings.BuildSettings.UserDefined.AddRange(BuildSettings.UserDefined);
        }

        private void FillBuildSettingsList()
        {
            var buildConfigs = BuildSettings.GetAllConfigurations().ToList();

            using (FlagManager.UseFlag(nameof(FillBuildSettingsList)))
            {

                BuildConfigListView.Items.Clear();

                foreach (var config in buildConfigs)
                {
                    var lvi = new ListViewItem(config.Name)
                    {
                        Tag = config.Clone()
                    };

                    if (config.IsDefault)
                        lvi.Text += "*";

                    BuildConfigListView.Items.Add(lvi);
                }

                BuildConfiguration configToSelect = null;

                if (SelectedBuildConfig != null)
                {
                    configToSelect = buildConfigs.FirstOrDefault(c => c.UniqueID == SelectedBuildConfig.UniqueID);
                }

                if (configToSelect == null)
                    configToSelect = buildConfigs.FirstOrDefault(c => c.InternalFlag == 1); 

                SetSelectedBuildConfig(configToSelect, true);
            }
            
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
                FillBuildConfigDetails();

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
            if (FlagManager.IsSet(nameof(FillSettings)))
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
            //else if (SelectedBuildConfig != null)
            //{
            //    SetSelectedBuildConfig(SelectedBuildConfig, false);
            //}
        }

        private void UpdateDeleteButtonState()
        {
            DelBuildCfgBtn.Enabled = SelectedBuildConfig != null &&
                SelectedBuildConfig.InternalFlag == 0;
        }

        private void FillBuildConfigDetails()
        {
            using (FlagManager.UseFlag(nameof(FillBuildConfigDetails)))
            {
                BuildCfg_NameBox.DataBindings.Clear();
                BuildCfg_PathBox.DataBindings.Clear();
                BuildCfg_Lod0Chk.DataBindings.Clear();
                BuildCfg_OverwriteChk.DataBindings.Clear();
                BuildCfg_PathBox.Value = string.Empty;

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
                if (isNewConfig)
                    BeginEditBuildConfig();
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

            BeginEditBuildConfig();
        }

        private void SaveBuildCfgBtn_Click(object sender, EventArgs e)
        {
            var buildConfigs = BuildSettings.GetAllConfigurations();
            var existingCfg = buildConfigs.FirstOrDefault(c => c.UniqueID == SelectedBuildConfig.UniqueID);

            if (existingCfg == null)
            {
                BuildSettings.UserDefined.Add(SelectedBuildConfig);
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
            HasSettingsChanged = true;
            EndEditBuildConfig();
            FillBuildSettingsList();
        }

        private void CancelBuildCfgBtn_Click(object sender, EventArgs e)
        {
            var buildConfigs = BuildSettings.GetAllConfigurations();
            var configToSelect = buildConfigs.FirstOrDefault(c => c.UniqueID == SelectedBuildConfig.UniqueID);
            SetSelectedBuildConfig(configToSelect, true);
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
            if (IsEditingConfig)
                return;
            SetSelectedBuildConfig(new BuildConfiguration(), true);
        }

        private void DelBuildCfgBtn_Click(object sender, EventArgs e)
        {
            if (SelectedBuildConfig != null && !IsEditingConfig)
            {
                BuildSettings.UserDefined.RemoveAll(c => c.UniqueID == SelectedBuildConfig.UniqueID);
                FillBuildSettingsList();
            }
        }

        private void BuildConfigListView_SizeChanged(object sender, EventArgs e)
        {
            BuildCfgNameColumn.Width = BuildConfigListView.Width - 3;
        }
    }
}
