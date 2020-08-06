using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.ProjectHandling.ViewInterfaces;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Panels;
using LDDModder.LDD;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BrickEditorWindow : Form, IMainWindow
    {

        public ProjectManager ProjectManager { get; private set; }

        public PartProject CurrentProject => ProjectManager.CurrentProject;

        protected FlagManager FlagManager { get; }

        //private string TemporaryFolder;

        public BrickEditorWindow()
        {
            InitializeComponent();
            visualStudioToolStripExtender1.SetStyle(menuStrip1, 
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                DockPanelControl.Theme);
            this.vS2015LightTheme1.Extender.DockPaneStripFactory = new VS2015DockPaneStripFactory();
            FlagManager = new FlagManager();

            Icon = Properties.Resources.BrickStudioIcon;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ProjectManager = new ProjectManager();
            ProjectManager.MainWindow = this;
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.UndoHistoryChanged += ProjectManager_UndoHistoryChanged;
            ProjectManager.ValidationFinished += ProjectManager_ValidationFinished;
            ProjectManager.GenerationFinished += ProjectManager_GenerationFinished;
            ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;

            InitialCheckUp();

            ResourceHelper.LoadPlatformsAndCategories();
            ResourceHelper.LoadConnectors();

            InitializePanels();
            RebuildRecentFilesMenu();
            UpdateMenuItemStates();
            UpdateWindowTitle();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(200);
                Invoke(new MethodInvoker(CheckCanRecoverProject));
            });
        }

        private void InitialCheckUp()
        {
            SettingsManager.Initialize();

            if (LDDEnvironment.Current == null || !LDDEnvironment.Current.IsValidInstall)
            {
                if (!LDDEnvironment.IsInstalled)
                    MessageBox.Show(Messages.LddInstallNotFound, Messages.Caption_StartupValidations, MessageBoxButtons.OK);
                else
                    MessageBox.Show(Messages.LddConfigInvalid, Messages.Caption_StartupValidations, MessageBoxButtons.OK);
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Models.BrickListCache.Initialize();
                });
            }
        }

        private void UpdateWindowTitle()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(UpdateWindowTitle));
                return;
            }

            if (CurrentProject != null)
            {
                string projectDesc = ProjectManager.GetProjectDisplayName();
                
                Text = $"{projectDesc}";
            }
            else
                Text = WindowTitle.Text;
        }

        #region UI Layout

        public NavigationPanel NavigationPanel { get; private set; }
        public ViewportPanel ViewportPanel { get; private set; }
        public ValidationPanel ValidationPanel { get; private set; }
        public PartPropertiesPanel PropertiesPanel { get; private set; }
        public ElementDetailPanel DetailPanel { get; private set; }
        public StudConnectionPanel StudConnectionPanel { get; private set; }

        private void InitializePanels()
        {
            NavigationPanel = new NavigationPanel(ProjectManager);
            ViewportPanel = new ViewportPanel(ProjectManager);
            ValidationPanel = new ValidationPanel(ProjectManager);
            PropertiesPanel = new PartPropertiesPanel(ProjectManager);
            DetailPanel = new ElementDetailPanel(ProjectManager);
            StudConnectionPanel = new StudConnectionPanel(ProjectManager);

            ViewportPanel.Show(DockPanelControl, DockState.Document);
            StudConnectionPanel.Show(DockPanelControl, DockState.Document);
            ViewportPanel.Activate();

            DockPanelControl.DockLeftPortion = 250;
            NavigationPanel.Show(DockPanelControl, DockState.DockLeft);

            DockPanelControl.DockWindows[DockState.DockBottom].BringToFront();
            DockPanelControl.DockBottomPortion = 230;

            PropertiesPanel.Show(DockPanelControl, DockState.DockBottom);
            DetailPanel.Show(PropertiesPanel.Pane, null);
            ValidationPanel.Show(PropertiesPanel.Pane, null);

            PropertiesPanel.Activate();
        }

        #endregion

        #region Project Handling

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            UpdateMenuItemStates();

            UpdateWindowTitle();
        }

        private void ProjectManager_ElementPropertyChanged(object sender, ElementValueChangedEventArgs e)
        {
            if (e.Element is PartProperties && 
                (e.PropertyName == nameof(PartProperties.ID) || e.PropertyName == nameof(PartProperties.Description))
                )
            {
                UpdateWindowTitle();
            }
        }

        private void ProjectManager_UndoHistoryChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(UpdateUndoRedoMenus));
            else
                UpdateUndoRedoMenus();
        }

        public string GetTemporaryWorkingDir()
        {
            return Path.Combine(Path.GetTempPath(), StringUtils.GenerateUID(16));
        }

        private void OpenPartProjectFile(string projectFilePath)
        {
            if (!CloseCurrentProject())
                return;

            string tmpProjectDir = GetTemporaryWorkingDir();

            PartProject loadedProject = null;

            bool exceptionThrown = false;
            try
            {
                using (var fs = File.OpenRead(projectFilePath))
                    loadedProject = PartProject.ExtractAndOpen(fs, tmpProjectDir);
            }
            catch (Exception ex)
            {
                ErrorMessageBox.Show(this, 
                    Messages.Error_OpeningProject, 
                    Messages.Caption_OpeningProject, ex.ToString());
                exceptionThrown = true;
            }

            if (loadedProject != null)
            {
                loadedProject.ProjectPath = projectFilePath;
                loadedProject.ProjectWorkingDir = tmpProjectDir;
                SettingsManager.Current.LastOpenProject = new RecentFileInfo(loadedProject, true);
                SettingsManager.AddRecentProject(loadedProject);
                LoadPartProject(loadedProject);
                RebuildRecentFilesMenu();
            }
            else if (!exceptionThrown)
            {
                ErrorMessageBox.Show(this,
                    Messages.Error_OpeningProject,
                    Messages.Caption_OpeningProject, "Invalid or corrupted project file. Missing \"project.xml\" file.");
            }
        }

        private void OpenPartProjectDirectory(string projectPath)
        {
            if (!CloseCurrentProject())
                return;

            PartProject loadedProject = null;

            try
            {
                loadedProject = PartProject.LoadFromDirectory(projectPath);
            }
            catch (Exception ex)
            {
                ErrorMessageBox.Show(this,
                    Messages.Error_OpeningProject,
                    Messages.Caption_OpeningProject, ex.ToString());
                
            }

            if (loadedProject != null)
                LoadPartProject(loadedProject);
        }

        private void LoadNewPartProject(PartProject project)
        {
            try
            {
                string tmpProjectDir = GetTemporaryWorkingDir();
                project.SaveExtracted(tmpProjectDir);
                SettingsManager.Current.LastOpenProject = new RecentFileInfo(project, true);
                SettingsManager.SaveSettings();
                LoadPartProject(project);
            }
            catch (Exception ex)
            {
                ErrorMessageBox.Show(this,
                    Messages.Error_CreatingProject,
                    Messages.Caption_OpeningProject, ex.ToString());
            }
        }

        private void LoadPartProject(PartProject project)
        {
            if (!CloseCurrentProject())
                return;

            ProjectManager.SetCurrentProject(project);
            if (project != null)
                AutoSaveTimer.Start();
        }

        public bool CloseCurrentProject()
        {
            if (ProjectManager.IsProjectOpen)
            {
                AutoSaveTimer.Stop();

                if (ProjectManager.IsModified || ProjectManager.IsNewProject)
                {
                    var messageText = ProjectManager.IsNewProject ? 
                        Messages.Message_SaveNewProject : 
                        Messages.Message_SaveChanges;

                    var result = MessageBox.Show(messageText, Messages.Caption_SaveBeforeClose, MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                        SaveProject(CurrentProject);
                    else if (result == DialogResult.Cancel)
                    {
                        AutoSaveTimer.Start();
                        return false;
                    }
                }

                string workingDirPath = CurrentProject?.ProjectWorkingDir;

                //Delete temporary project working directory
                if (!string.IsNullOrEmpty(workingDirPath) && Directory.Exists(workingDirPath))
                {
                    Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(workingDirPath, true, true));
                }

                SettingsManager.Current.LastOpenProject = null;
                SettingsManager.SaveSettings();

                ProjectManager.CloseCurrentProject();
                
            }

            return true;
        }

        public void SaveProject(PartProject project, bool selectPath = false)
        {
            bool isNew = string.IsNullOrEmpty(project.ProjectPath);
            string targetPath = project.ProjectPath;

            if (selectPath || isNew)
            {
                using (var sfd = new SaveFileDialog())
                {
                    if (!string.IsNullOrEmpty(project.ProjectPath))
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(project.ProjectPath);
                        sfd.FileName = Path.GetFileName(project.ProjectPath);
                    }
                    else
                    {
                        if (SettingsManager.IsWorkspaceDefined)
                            sfd.InitialDirectory = SettingsManager.Current.ProjectWorkspace;

                        if (project.PartID > 0)
                            sfd.FileName = $"{project.PartID}.lpp";
                        else
                            sfd.FileName = $"New part.lpp";
                    }

                    sfd.Filter = "LDD Part Project|*.lpp|All Files|*.*";
                    sfd.DefaultExt = ".lpp";

                    if (sfd.ShowDialog() == DialogResult.OK)
                        targetPath = sfd.FileName;
                    else
                        return;
                }
            }

            string oldPath = project.ProjectPath;

            ProjectManager.SaveProject(targetPath);

            SettingsManager.AddRecentProject(project, true);
            if (oldPath != targetPath)
                RebuildRecentFilesMenu();
        }

        private void CheckCanRecoverProject()
        {
            if (SettingsManager.Current.LastOpenProject != null)
            {
                var fileInfo = SettingsManager.Current.LastOpenProject;
                //project was not correctly closed
                if (Directory.Exists(fileInfo.WorkingDirectory))
                {
                    if (MessageBox.Show("Do you want to recover the project?","", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        OpenPartProjectDirectory(fileInfo.WorkingDirectory);
                    }
                    else
                    {
                        Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(fileInfo.WorkingDirectory, true, true));
                    }
                }
            }
        }

        #endregion

        private void ImportMeshFile()
        {
            using (var imd = new ImportModelsDialog(ProjectManager))
            {
                imd.SelectFileOnStart = true;
                imd.ShowDialog();
            }
        }
        
        private void BrickEditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentProject != null)
            {
                e.Cancel = true;
                BeginInvoke(new MethodInvoker(TryCloseProjectAndExit));
                return;
            }

            Debug.WriteLine($"FormClosing started at {DateTime.Now:HH:mm:ss.ff}");

            foreach (var form in DockPanelControl.Documents.OfType<DockContent>().ToList())
            {
                Debug.WriteLine($"Closing Form '{form.Text}' at {DateTime.Now:HH:mm:ss.ff}");
                form.Close();
                if (!form.IsDisposed)
                {
                    e.Cancel = true;
                    break;
                }
            }

            Debug.WriteLine($"FormClosing finished at {DateTime.Now:HH:mm:ss.ff}");
        }

        private void TryCloseProjectAndExit()
        {
            Debug.WriteLine($"TryCloseProjectAndExit at {DateTime.Now:HH:mm:ss.ff}");
            if (CloseCurrentProject())
            {
                ViewportPanel.StopRenderingLoop();
                ViewportPanel.UnloadModels();

                //Application.DoEvents();
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    Debug.WriteLine($"Invoke Close at {DateTime.Now:HH:mm:ss.ff}");
                    BeginInvoke(new MethodInvoker(Close));
                });
            }
        }

        private void BrickEditorWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine($"FormClosed at {DateTime.Now:HH:mm:ss.ff}");
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            ProjectManager.SaveWorkingProject();
        }

        
    }
}
