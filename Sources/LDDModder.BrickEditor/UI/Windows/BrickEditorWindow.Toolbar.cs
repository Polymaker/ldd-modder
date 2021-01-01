using LDDModder.BrickEditor.Settings;
using LDDModder.Modding;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using LDDModder.LDD;
using LDDModder.BrickEditor.Resources;
using System.Collections.Generic;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.Utilities;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BrickEditorWindow
    {

        public void UpdateMenuItemStates()
        {
            using (FlagManager.UseFlag("UpdateMenuItemStates"))
            {
                if (SettingsManager.HasInitialized)
                {
                    File_CreateFromBrickMenu.Enabled = LDDEnvironment.Current?.IsValidInstall ?? false;
                    ExportBrickMenuItem.Enabled = LDDEnvironment.Current?.IsValidInstall ?? false;
                }
                else
                {
                    File_CreateFromBrickMenu.Enabled = false;
                    ExportBrickMenuItem.Enabled = false;
                }
                
                File_SaveMenu.Enabled = ProjectManager.IsProjectOpen;
                File_SaveAsMenu.Enabled = ProjectManager.IsProjectOpen;
                File_CloseProjectMenu.Enabled = ProjectManager.IsProjectOpen;
                Edit_ImportMeshMenu.Enabled = ProjectManager.IsProjectOpen;
                Edit_ValidatePartMenu.Enabled = ProjectManager.IsProjectOpen;
                Edit_GenerateFilesMenu.Enabled = ProjectManager.IsProjectOpen;
            }

            UpdateUndoRedoMenus();

            if (SettingsManager.HasInitialized)
                UpdateBuildConfigs();
            
        }
 
        #region File Menu

        private void File_NewProjectMenu_Click(object sender, EventArgs e)
        {
            var project = PartProject.CreateEmptyProject();
            LoadNewPartProject(project);
        }

        private void File_CreateFromBrickMenu_Click(object sender, EventArgs e)
        {
            using (var dlg = new SelectBrickDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var selectedBrick = dlg.SelectedBrick;
                    var project = PartProject.CreateFromLddPart(selectedBrick.PartId);
                    LoadNewPartProject(project);
                }
            }
        }

        private void File_OpenProjectMenu_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (SettingsManager.IsWorkspaceDefined &&
                    Directory.Exists(SettingsManager.Current.EditorSettings.ProjectWorkspace))
                {
                    ofd.InitialDirectory = SettingsManager.Current.EditorSettings.ProjectWorkspace;
                }

                ofd.Filter = "LDD Part Project (*.lpp)|*.lpp";
                if (ofd.ShowDialog() == DialogResult.OK)
                    OpenPartProjectFile(ofd.FileName);
            }
        }

        private void FileMenu_OpenPartFiles_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {

                ofd.Filter = "LDD Primitive file|*.xml|LDD Mesh file|*.g|LDD Textured Mesh file|*.g*|All|*.xml;*.g;*.g*";
                if (ofd.ShowDialog() == DialogResult.OK)
                    OpenPartFromFiles(ofd.FileName);
            }
        }

        private void RebuildRecentFilesMenu()
        {
            File_OpenRecentMenu.DropDownItems.Clear();

            if (!SettingsManager.HasInitialized)
                return;

            int currentFileIndex = 1;
            foreach (var recentProject in SettingsManager.Current.RecentProjectFiles.ToArray())
            {
                if (!File.Exists(recentProject.ProjectFile))
                {
                    SettingsManager.Current.RecentProjectFiles.Remove(recentProject);
                    continue;
                }
                string projectName = $"{currentFileIndex++}: {Path.GetFileName(recentProject.ProjectFile)}";
                
                //if (recentProject.PartID > 0)
                //    projectName = recentProject.PartID.ToString();
                //else
                //    projectName = Path.GetFileName(recentProject.ProjectFile);

                if (!string.IsNullOrEmpty(recentProject.PartName))
                    projectName += $" ({recentProject.PartName})";

                var recentFileMenuEntry = File_OpenRecentMenu.DropDownItems.Add(projectName);
                recentFileMenuEntry.Tag = recentProject;
                recentFileMenuEntry.ToolTipText = recentProject.PartName;
                recentFileMenuEntry.Click += RecentFileMenuEntry_Click;
            }

            File_OpenRecentMenu.Enabled = File_OpenRecentMenu.DropDownItems.Count > 0;
        }

        private void File_OpenRecentMenu_DropDownOpening(object sender, EventArgs e)
        {
            if (File_OpenRecentMenu.DropDown is ToolStripDropDownMenu downMenu)
            {
                downMenu.ShowImageMargin = false;
            }
        }

        private void RecentFileMenuEntry_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripItem;
            string filePath = (menuItem.Tag as RecentFileInfo).ProjectFile;
            OpenPartProjectFile(filePath);
        }


        private void File_SaveMenu_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null)
                SaveProject(CurrentProject, false);
        }

        private void File_SaveAsMenu_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null)
                SaveProject(CurrentProject, true);
        }

        private void File_CloseProjectMenu_Click(object sender, EventArgs e)
        {
            CloseCurrentProject();
        }

        #endregion

        #region Edit Menu

        private void EditMenu_Undo_Click(object sender, EventArgs e)
        {
            ProjectManager.Undo();
        }

        private void EditMenu_Redo_Click(object sender, EventArgs e)
        {
            ProjectManager.Redo();
        }

        private void UpdateUndoRedoMenus()
        {
            EditMenu_Undo.Enabled = ProjectManager.CanUndo;
            EditMenu_Redo.Enabled = ProjectManager.CanRedo;
        }

        private void Edit_ImportMeshMenu_Click(object sender, EventArgs e)
        {
            ImportMeshFile();
        }

        private bool GenerateFileAfterValidation;

        private void ProjectManager_ValidationFinished(object sender, EventArgs e)
        {
            if (GenerateFileAfterValidation)
            {
                GenerateFileAfterValidation = false;
                if (!ProjectManager.IsPartValid)
                {
                    MessageBox.Show(Messages.Message_FileGenerationError, Messages.Caption_LddPartGeneration);
                }
                else
                {
                    ProjectManager.GenerateLddFiles();
                }
            }
        }

        private void Edit_ValidatePartMenu_Click(object sender, EventArgs e)
        {
            if (ProjectManager.IsValidatingProject || ProjectManager.IsGeneratingFiles)
                return;

            ProjectManager.ValidateProject();
            ValidationPanel.Activate();
        }

        private void Edit_GenerateFilesMenu_Click(object sender, EventArgs e)
        {
            if (CurrentProject != null)
            {
                if (ProjectManager.IsValidatingProject || ProjectManager.IsGeneratingFiles)
                    return;

                if (!ProjectManager.IsPartValidated)
                {
                    GenerateFileAfterValidation = true;
                    ValidationPanel.Activate();
                    ProjectManager.ValidateProject();
                    return;
                }
                else if (!ProjectManager.IsPartValid)
                {
                    ValidationPanel.Activate();
                    return;
                }
                ProjectManager.GenerateLddFiles();
            }
        }

        #endregion

        #region Tools Menu

        private void Settings_EnvironmentMenu_Click(object sender, EventArgs e)
        {
            ShowSettingsWindow();
        }

        public void ShowSettingsWindow(AppSettingsWindow.SettingTab defaultTab = AppSettingsWindow.SettingTab.LddEnvironment)
        {
            using (var dlg = new AppSettingsWindow())
            {
                dlg.StartupTab = defaultTab;
                dlg.ShowDialog();
            }

            UpdateMenuItemStates();
            RebuildLayoutMenu();
        }

        private void ExportBrickMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new ExportPartModelWindow())
            {
                frm.CurrentProject = CurrentProject;
                frm.ShowDialog();
            }
        }

        private void StartLddMenuItem_Click(object sender, EventArgs e)
        {
            if (LDD.LDDEnvironment.Current != null)
            {
                try
                {
                    var currentProc = GetRunningLDDProcess();
                    if (currentProc != null)
                    {
                        currentProc.Kill();
                        currentProc.WaitForExit(5000);
                    }
                }
                catch { }


                var exePath = LDD.LDDEnvironment.Current.GetExecutablePath();
                if (File.Exists(exePath))
                    Process.Start(exePath);
            }
        }

        private Process GetRunningLDDProcess()
        {
            var lddProcs = Process.GetProcessesByName("LDD", Environment.MachineName);
            return lddProcs.Length > 0 ? lddProcs[0] : null;
        }

        private void ToolsMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (LDD.LDDEnvironment.Current != null)
            {
                var lddProc = GetRunningLDDProcess();
                if (lddProc != null)
                    StartLddMenuItem.Text = RestartLddText;
                else
                    StartLddMenuItem.Text = StartLddText;
            }
            else
                StartLddMenuItem.Enabled = false;
        }

        #endregion

        #region Window Menu


        #endregion

        private void RebuildLayoutMenu()
        {
            WindowMenu_ApplyLayout.DropDownItems.Clear();
            var userLayouts = SettingsManager.GetUserUILayouts().ToList();
            if (!userLayouts.Any())
            {
                WindowMenu_ApplyLayout.DropDownItems.Add(new ToolStripMenuItem
                {
                    Text = "No Saved Layouts",
                    Enabled = false
                });
            }
            else
            {
                foreach (var userLayout in userLayouts)
                {
                    var tsb = WindowMenu_ApplyLayout.DropDownItems.Add(userLayout.Name);
                    tsb.Tag = userLayout;
                    tsb.Click += ApplyLayoutMenu_ItemClick;
                }
            }
        }

        private void ApplyLayoutMenu_ItemClick(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            if (tsmi.Tag is UserUILayout layout)
            {
                LoadCustomLayout(layout);
            }
        }

        private void WindowMenu_SaveLayout_Click(object sender, EventArgs e)
        {
            string layoutName = InputDialog.Show(Messages.Message_SaveWindowLayout, Messages.Caption_SaveWindowLayout);

            if (!string.IsNullOrWhiteSpace(layoutName))
            {
                if (SettingsManager.GetUserUILayouts().Any(x => StringUtils.EqualsIC(x.Name, layoutName)))
                {
                    MessageBox.Show(Messages.Message_LayoutNameAlreadyExist, Messages.Caption_Warning, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SettingsManager.SaveUILayout(DockPanelControl, layoutName);
                RebuildLayoutMenu();
            }
        }

        private void WindowMenu_ManageLayouts_Click(object sender, EventArgs e)
        {
            ShowSettingsWindow(AppSettingsWindow.SettingTab.LayoutSettings);
        }

        private void WindowMenu_ResetLayout_Click(object sender, EventArgs e)
        {

        }

        private void HelpMenu_About_Click(object sender, EventArgs e)
        {
            using (var dlg = new AboutWindow())
                dlg.ShowDialog(this);
        }


        #region Build Configs & Project Compilation

        private List<BuildConfiguration> BuildConfigList;
        private BuildConfiguration SelectedBuildConfig;

        private void UpdateBuildConfigs()
        {
            if (!SettingsManager.HasInitialized)
                return;

            using (FlagManager.UseFlag("UpdateBuildConfig"))
            {
                var currentSelection = SelectedBuildConfig;
                BuildConfigComboBox.ComboBox.DataSource = null;
                BuildConfigList = SettingsManager.GetBuildConfigurations().ToList();

                BuildConfigList.Add(new BuildConfiguration()
                {
                    Name = Messages.BuildConfig_Manage,
                    InternalFlag = 3
                });

                BuildConfigComboBox.ComboBox.DataSource = BuildConfigList;
                BuildConfigComboBox.ComboBox.DisplayMember = "Name";

                if (currentSelection != null && currentSelection.InternalFlag > 0)
                    currentSelection = BuildConfigList.FirstOrDefault(x => x.InternalFlag == currentSelection.InternalFlag);

                if (BuildConfigList.Contains(currentSelection))
                {
                    SelectedBuildConfig = currentSelection;
                    BuildConfigComboBox.SelectedIndex = BuildConfigList.IndexOf(currentSelection);
                }
                else if (BuildConfigList.Count > 1)
                {
                    var defaultCfg = BuildConfigList.FirstOrDefault(x => x.IsDefault);
                    if (defaultCfg != null)
                    {
                        BuildConfigComboBox.SelectedItem = defaultCfg;
                        SelectedBuildConfig = defaultCfg;
                    }
                    else
                    {
                        BuildConfigComboBox.SelectedIndex = 0;
                        SelectedBuildConfig = BuildConfigList[0];
                    }
                }
                else
                {
                    BuildConfigComboBox.SelectedIndex = -1;
                    SelectedBuildConfig = null;
                }
            }
        }

        private void BuildConfigComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FlagManager.IsSet("UpdateBuildConfig"))
                return;

            if (BuildConfigComboBox.SelectedItem is BuildConfiguration buildConfig)
            {
                if (buildConfig.InternalFlag == 3)
                {
                    using (FlagManager.UseFlag("UpdateBuildConfig"))
                        BuildConfigComboBox.SelectedIndex = BuildConfigList.IndexOf(SelectedBuildConfig);
                    ShowSettingsWindow(AppSettingsWindow.SettingTab.EditorSettings);
                }
                else
                {
                    SelectedBuildConfig = buildConfig;
                }
            }
        }

        private void ProjectManager_GenerationFinished(object sender, ProjectBuildEventArgs e)
        {
            //TODO: localize and improve messages
            if (e.Successful)
            {
                ProjectManager.SaveGeneratedPart(e.Result, SelectedBuildConfig);
            }
            else
            {
                MessageBox.Show("An error occured.");
                ValidationPanel.Activate();
                ValidationPanel.ShowBuildMessages(e.Messages);
            }
        }

        #endregion



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!IsEditControlFocused())
            {
                if (keyData.HasFlag(Keys.Z) && keyData.HasFlag(Keys.Control))
                {
                    ProjectManager.Undo();
                    return true;
                }
                else if (keyData.HasFlag(Keys.Y) && keyData.HasFlag(Keys.Control))
                {
                    ProjectManager.Redo();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public bool IsEditControlFocused()
        {
            var focusedControl = GetFocusedControl(ActiveControl);
            if (focusedControl is TextBox)
                return true;

            if (focusedControl is ComboBox cbo)
            {
                if (cbo.DroppedDown)
                    return true;
                
                return cbo.DropDownStyle != ComboBoxStyle.DropDownList;
            }
            return false;
        }

        public static Control GetFocusedControl(Control parent)
        {
            if (parent is ContainerControl container && container.ActiveControl != null)
                return GetFocusedControl(container.ActiveControl);
            return parent;
        }
    }
}
