using LDDModder.BrickEditor.Settings;
using LDDModder.Modding.Editing;
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
                if (!string.IsNullOrEmpty(SettingsManager.Current.ProjectWorkspace) &&
                    Directory.Exists(SettingsManager.Current.ProjectWorkspace))
                {
                    ofd.InitialDirectory = SettingsManager.Current.ProjectWorkspace;
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
                //if (!string.IsNullOrEmpty(SettingsManager.Current.ProjectWorkspace) &&
                //    Directory.Exists(SettingsManager.Current.ProjectWorkspace))
                //{
                //    ofd.InitialDirectory = SettingsManager.Current.ProjectWorkspace;
                //}

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
                    ProjectManager.GenerateLddFiles();
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
                    ProjectManager.ValidateProject();
                    ValidationPanel.Activate();
                    return;
                }
                else if (!ProjectManager.IsPartValid)
                {
                    ValidationPanel.Activate();
                    return;
                }
                //SettingsManager.Current.BuildSettings.GenerateOutlines
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
                    BuildConfigComboBox.SelectedIndex = 0;
                    SelectedBuildConfig = BuildConfigList[0];
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
                    ShowSettingsWindow(AppSettingsWindow.SettingTab.BuildSettings);
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
                SaveGeneratedPart(e.Result, SelectedBuildConfig);
                
            }
            else
            {
                MessageBox.Show("An error occured.");
                ValidationPanel.Activate();
                ValidationPanel.ShowBuildMessages(e.Messages);
            }
        }

        private void SaveGeneratedPart(LDD.Parts.PartWrapper part, BuildConfiguration buildConfig)
        {
            if (buildConfig.InternalFlag == BuildConfiguration.LDD_FLAG)
            {
                part.SaveToLdd(LDDEnvironment.Current);
                MessageBox.Show("Part files generated!");
                return;
            }

            string targetPath = buildConfig.OutputPath;

            if (targetPath.Contains("$"))
            {
                targetPath = ProjectManager.ExpandVariablePath(targetPath);
            }

            if (buildConfig.InternalFlag == BuildConfiguration.MANUAL_FLAG || 
                string.IsNullOrEmpty(buildConfig.OutputPath))
            {
                using (var sfd = new SaveFileDialog())
                {
                    if (!string.IsNullOrEmpty(targetPath) &&
                        FileHelper.IsValidDirectory(targetPath))
                        sfd.InitialDirectory = targetPath;

                    sfd.FileName = part.PartID.ToString();
                    //if (buildConfig.CreateZip)
                    //{
                    //    sfd.FileName += ".zip";
                    //    sfd.DefaultExt = ".zip";
                    //}

                    if (sfd.ShowDialog() != DialogResult.OK)
                    {
                        //show canceled message
                        return;
                    }

                    //if (buildConfig.CreateZip)
                    //    targetPath = sfd.FileName;
                    //else
                        targetPath = Path.GetDirectoryName(sfd.FileName);
                }
            }

            Directory.CreateDirectory(targetPath);
            part.SavePrimitive(targetPath);

            if (!buildConfig.LOD0Subdirectory)
                part.SaveMeshes(targetPath);
            else
            {
                targetPath = Path.Combine(targetPath, "LOD0");
                Directory.CreateDirectory(targetPath);
                part.SaveMeshes(targetPath);
            }

            MessageBox.Show("Part files generated!");

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
