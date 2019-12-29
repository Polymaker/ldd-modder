using LDDModder.BrickEditor.Settings;
using LDDModder.Modding.Editing;
using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BrickEditorWindow
    {

        public void UpdateMenuItemStates()
        {
            File_SaveMenu.Enabled = ProjectManager.IsProjectOpen;
            File_SaveAsMenu.Enabled = ProjectManager.IsProjectOpen;
            File_CloseProjectMenu.Enabled = ProjectManager.IsProjectOpen;
            Edit_ImportMeshMenu.Enabled = ProjectManager.IsProjectOpen;
            Edit_ValidatePartMenu.Enabled = ProjectManager.IsProjectOpen;
            Edit_GenerateFilesMenu.Enabled = ProjectManager.IsProjectOpen;
            UpdateUndoRedoMenus();
        }

        #region Main menu

        private void LDDEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new LddEnvironmentConfigWindow())
                dlg.ShowDialog();
        }

        private void ExportBrickMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new ExportPartModelWindow())
            {
                if (ProjectCreatedFromBrick && CurrentProject != null)
                    frm.PartIDToExport = CurrentProject.PartID;
                frm.ShowDialog();
            }
        }

        #endregion

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
                    if (CurrentProject == project)
                        ProjectCreatedFromBrick = true;
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

        private void RebuildRecentFilesMenu()
        {
            File_OpenRecentMenu.DropDownItems.Clear();

            int currentFileIndex = 1;
            foreach (var recentProject in SettingsManager.Current.RecentProjectFiles.ToArray())
            {
                if (!File.Exists(recentProject.ProjectFile))
                {
                    SettingsManager.Current.RecentProjectFiles.Remove(recentProject);
                    continue;
                }
                string projectName = $"{currentFileIndex++} - {Path.GetFileName(recentProject.ProjectFile)}";
                
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
                    MessageBox.Show("Could not generate LDD Part. See validation panel.");
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

                ProjectManager.GenerateLddFiles();
            }
        }

        private void ProjectManager_GenerationFinished(object sender, EventArgs e)
        {
            MessageBox.Show("LDD Part files generated.");
        }

        #endregion


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
