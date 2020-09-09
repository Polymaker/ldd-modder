using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.ProjectHandling.ViewInterfaces;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Panels;
using LDDModder.BrickEditor.Utilities;
using LDDModder.LDD;
using LDDModder.LDD.Parts;
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

        private bool IsInitializing;

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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MultiInstanceManager.Initialize(this);

            InitializeProjectManager();
            menuStrip1.Enabled = false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            IsInitializing = true;

            Task.Factory.StartNew(() =>
            {
                ResourceHelper.LoadResources();
            });
            
            UpdateMenuItemStates();
            UpdateWindowTitle();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(200);
                BeginInvoke(new MethodInvoker(BeginLoadingUI));
            });
        }
       

        private void LoadAndValidateSettings()
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

            if (!FlagManager.IsSet("OnLoadAsync"))
            {
                AutoSaveTimer.Interval = SettingsManager.Current.AutoSaveInterval * 1000;
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
        public ConnectionEditorPanel ConnectionPanel { get; private set; }

        public ProgressPopupWindow WaitPopup { get; private set; }

        private void InitializePanels()
        {
            WaitPopup.UpdateProgress(0, 10);

            NavigationPanel = new NavigationPanel(ProjectManager);
            WaitPopup.UpdateProgress(1, 10);

            ViewportPanel = new ViewportPanel(ProjectManager);
            WaitPopup.UpdateProgress(2, 10);

            ValidationPanel = new ValidationPanel(ProjectManager);
            WaitPopup.UpdateProgress(3, 10);

            PropertiesPanel = new PartPropertiesPanel(ProjectManager);
            WaitPopup.UpdateProgress(4, 10);

            DetailPanel = new ElementDetailPanel(ProjectManager);
            WaitPopup.UpdateProgress(5, 10);

            StudConnectionPanel = new StudConnectionPanel(ProjectManager);
            WaitPopup.UpdateProgress(6, 10);

            ConnectionPanel = new ConnectionEditorPanel(ProjectManager);
            WaitPopup.UpdateProgress(7, 10);

            ViewportPanel.Show(DockPanelControl, DockState.Document);

            StudConnectionPanel.Show(DockPanelControl, DockState.Document);

            ViewportPanel.Activate();

            WaitPopup.UpdateProgress(8, 10);

            DockPanelControl.DockLeftPortion = 250;

            NavigationPanel.Show(DockPanelControl, DockState.DockLeft);

            DockPanelControl.DockWindows[DockState.DockBottom].BringToFront();
            DockPanelControl.DockBottomPortion = 250;

            WaitPopup.UpdateProgress(9, 10);

            PropertiesPanel.Show(DockPanelControl, DockState.DockBottom);

            DetailPanel.Show(PropertiesPanel.Pane, null);

            ConnectionPanel.Show(PropertiesPanel.Pane, null);

            ValidationPanel.Show(PropertiesPanel.Pane, null);

            PropertiesPanel.Activate();

            WaitPopup.UpdateProgress(10, 10);

            foreach (IDockContent dockPanel in DockPanelControl.Contents)
            {
                if (dockPanel is ProjectDocumentPanel documentPanel)
                    documentPanel.Enabled = false;
            }
        }

        private void BeginLoadingUI()
        {
            WaitPopup = new ProgressPopupWindow();
            WaitPopup.Message = Messages.Message_InitializingUI;
            WaitPopup.Shown += OnInitializationPopupLoaded;
            WaitPopup.ShowCenter(this);
        }

        private void OnInitializationPopupLoaded(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                BeginInvoke((Action)(() =>
                {
                    FlagManager.Set("OnLoadAsync");
                    AutoSaveTimer.Interval = 500;
                    AutoSaveTimer.Start();
                    DockPanelControl.Layout += DockPanelControl_Layout;
                    WaitPopup.Shown -= OnInitializationPopupLoaded;
                    InitializePanels();
                }));
            });
        }

        private void DockPanelControl_Layout(object sender, LayoutEventArgs e)
        {
            AutoSaveTimer.Stop();
            AutoSaveTimer.Start();
        }

        private void InitializeAfterShown()
        {
            WaitPopup.Message = Messages.Message_InitializingResources;
            WaitPopup.UpdateProgress(0, 0);
            Application.DoEvents();

            var documentPanels = DockPanelControl.Contents.OfType<ProjectDocumentPanel>().ToList();

            foreach (var documentPanel in documentPanels)
            {
                documentPanel.Enabled = true;
                documentPanel.DefferedInitialization();
            }

            Task.Factory.StartNew(() =>
            {
                ViewportPanel.InitGlResourcesAsync();
                BeginInvoke(new MethodInvoker(OnInitializationFinished));
            });
        }

        private void OnInitializationFinished()
        {
            menuStrip1.Enabled = true;
            WaitPopup.Hide();

            LoadAndValidateSettings();
            UpdateMenuItemStates();
            RebuildRecentFilesMenu();

            var documentPanels = DockPanelControl.Contents.OfType<ProjectDocumentPanel>().ToList();

            foreach (var documentPanel in documentPanels)
                documentPanel.OnInitializationFinished();

            IsInitializing = false;

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(200);
                Invoke(new MethodInvoker(CheckCanRecoverProject));
            });
        }

        #endregion

        #region Project Handling

        private void InitializeProjectManager()
        {
            ProjectManager = new ProjectManager();
            ProjectManager.MainWindow = this;
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.UndoHistoryChanged += ProjectManager_UndoHistoryChanged;
            ProjectManager.ValidationFinished += ProjectManager_ValidationFinished;
            ProjectManager.GenerationFinished += ProjectManager_GenerationFinished;
            ProjectManager.ElementPropertyChanged += ProjectManager_ElementPropertyChanged;
        }

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
                MessageBoxEX.ShowDetails(this, 
                    Messages.Error_OpeningProject, 
                    Messages.Caption_OpeningProject, ex.ToString());
                exceptionThrown = true;
            }

            if (loadedProject != null)
            {
                loadedProject.ProjectPath = projectFilePath;
                loadedProject.ProjectWorkingDir = tmpProjectDir;
                SettingsManager.AddOpenedFile(loadedProject);
                SettingsManager.AddRecentProject(loadedProject);
                LoadPartProject(loadedProject);
                RebuildRecentFilesMenu();
            }
            else if (!exceptionThrown)
            {
                MessageBoxEX.ShowDetails(this,
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
                MessageBoxEX.ShowDetails(this,
                    Messages.Error_OpeningProject,
                    Messages.Caption_OpeningProject, ex.ToString());
                
            }

            if (loadedProject != null)
                LoadPartProject(loadedProject);
        }

        private void OpenPartFromFiles(string primitiveFilepath)
        {

            string filename = Path.GetFileNameWithoutExtension(primitiveFilepath);
            string fileType = Path.GetExtension(primitiveFilepath);

            if (!int.TryParse(filename, out int partID))
            {
                //TODO: Show message
                return;
            }

            LDD.Primitives.Primitive primitive = null;
            
            if (fileType.ToLower() == ".xml")
            {
                try
                {
                    primitive = LDD.Primitives.Primitive.Load(primitiveFilepath);
                }
                catch
                {
                    MessageBoxEX.ShowDetails(this,
                        Messages.Error_OpeningFile, //TODO: change
                        Messages.Caption_OpeningProject,
                        "File is not an LDD Primitive."); //TODO: translate

                    return;
                }
            }
            else if (fileType.ToLower().StartsWith(".g"))
            {

            }

            string fileDir = Path.GetDirectoryName(primitiveFilepath);
            string fileDirLod0 = Path.Combine(fileDir, "Lod0");

            var meshFiles = Directory.GetFiles(fileDir, primitive.ID + ".g*");

            if (meshFiles.Length == 0 && Directory.Exists(fileDirLod0))
            {
                meshFiles = Directory.GetFiles(fileDirLod0, primitive.ID + ".g*");
            }

            if (meshFiles.Length == 0)
            {
                MessageBoxEX.ShowDetails(this,
                    Messages.Error_OpeningFile, //TODO: change
                    Messages.Caption_OpeningProject,
                    "No mesh file found."); //TODO: translate
                return;
            }
        }

        private void LoadNewPartProject(PartProject project)
        {
            try
            {
                string tmpProjectDir = GetTemporaryWorkingDir();
                project.SaveExtracted(tmpProjectDir);
                SettingsManager.AddOpenedFile(project);
                LoadPartProject(project);
            }
            catch (Exception ex)
            {
                MessageBoxEX.ShowDetails(this,
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

                SettingsManager.RemoveOpenedFile(CurrentProject);
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
            if (SettingsManager.Current.OpenedProjects.Count > 0)
            {

                foreach(var fileInfo in SettingsManager.Current.OpenedProjects.ToArray())
                {
                    //project was not correctly closed
                    if (Directory.Exists(fileInfo.WorkingDirectory))
                    {
                        if (MultiInstanceManager.InstanceCount > 1)
                        {
                            bool isOpenInOtherInstance = MultiInstanceManager.CheckFileIsOpen(fileInfo.WorkingDirectory);
                            if (isOpenInOtherInstance)
                                return;
                        }

                        if (MessageBoxEX.Show(this,
                            Messages.Message_RecoverProject,
                            Messages.Caption_RecoverLastProject, 
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            OpenPartProjectDirectory(fileInfo.WorkingDirectory);
                        }
                        else
                        {
                            SettingsManager.RemoveOpenedFile(fileInfo);
                            Task.Factory.StartNew(() => FileHelper.DeleteFileOrFolder(fileInfo.WorkingDirectory, true, true));
                        }
                        break;
                    }
                    else
                        SettingsManager.RemoveOpenedFile(fileInfo);
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
            if (IsInitializing)
            {
                e.Cancel = true;
                return;
            }

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
            if (FlagManager.IsSet("OnLoadAsync"))
            {
                FlagManager.Unset("OnLoadAsync");
                DockPanelControl.Layout += DockPanelControl_Layout;
                AutoSaveTimer.Stop();

                if (SettingsManager.HasInitialized)
                    AutoSaveTimer.Interval = SettingsManager.Current.AutoSaveInterval * 1000;
                else
                    AutoSaveTimer.Interval = 15000;

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(200);
                    BeginInvoke(new MethodInvoker(InitializeAfterShown));
                });

                return;
            }

            if (ProjectManager.IsProjectOpen)
                ProjectManager.SaveWorkingProject();
        }

        public DockPanel GetDockPanelControl()
        {
            return DockPanelControl;
        }

        public bool IsFileOpen(string filepath)
        {
            if (ProjectManager.IsProjectOpen)
            {
                return ProjectManager.CurrentProject.ProjectWorkingDir == filepath;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (!MultiInstanceManager.ProcessMessage(ref m))
                base.WndProc(ref m);
        }
    }
}
