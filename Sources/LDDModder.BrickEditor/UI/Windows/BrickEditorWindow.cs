using LDDModder.BrickEditor.Native;
using LDDModder.BrickEditor.ProjectHandling;
using LDDModder.BrickEditor.ProjectHandling.ViewInterfaces;
using LDDModder.BrickEditor.Rendering;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Panels;
using LDDModder.BrickEditor.Utilities;
using LDDModder.LDD;
using LDDModder.LDD.Parts;
using LDDModder.Modding;
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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("Main Window");

        public ProjectManager ProjectManager { get; private set; }

        public PartProject CurrentProject => ProjectManager.CurrentProject;

        protected FlagManager FlagManager { get; }

        private bool IsInitializing;

        public BrickEditorWindow()
        {
            InitializeComponent();

            DockPanelControl.Theme = new CustomDockTheme();

            visualStudioToolStripExtender1.SetStyle(menuStrip1, 
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                DockPanelControl.Theme);

            DockPanelControl.Theme.Extender.DockPaneStripFactory = new VS2015DockPaneStripFactory();
            DockPanelControl.Theme.Extender.DockPaneCaptionFactory = new VS2015DockPaneCaptionFactory();

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
            SettingsManager.Initialize();

            RestoreSavedPosition();
            InitializeProjectManager();
            menuStrip1.Enabled = false;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Logger.Info("Main window shown");

            IsInitializing = true;
            
            UpdateMenuItemStates();
            UpdateWindowTitle();

            this.InvokeWithDelay(200, BeginLoadingUI);

        }

        private void RestoreSavedPosition()
        {
            if (SettingsManager.Current.DisplaySettings.LastPosition != default)
            {
                var savedBounds = SettingsManager.Current.DisplaySettings.LastPosition;
                var tlCorner = SettingsManager.Current.DisplaySettings.LastPosition.Location;
                var trCorner = new Point(savedBounds.Right, savedBounds.Top);
                var blCorner = new Point(savedBounds.Left, savedBounds.Bottom);
                var brCorner = new Point(savedBounds.Right, savedBounds.Bottom);
                var corners = new Point[] { tlCorner, trCorner, blCorner, brCorner };

                var screen = Screen.FromPoint(tlCorner);

                if (screen != null && corners.Any(x => screen.Bounds.Contains(x)))
                {
                    Bounds = savedBounds;
                    if (SettingsManager.Current.DisplaySettings.IsMaximized)
                        WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void LoadAndValidateSettings()
        {
            if (!SettingsManager.HasInitialized)
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
                if (!Models.BrickListCache.IsInitialized || Models.BrickListCache.CheckIfConfigurationChanged())
                {
                    Task.Factory.StartNew(() => Models.BrickListCache.Initialize());
                }
            }

            AutoSaveTimer.Interval = SettingsManager.Current.EditorSettings.BackupInterval * 1000;
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
        public ProjectInfoPanel InfoPanel { get; private set; }
        public ProgressPopupWindow WaitPopup { get; private set; }

        private IDockContent[] DockPanels => new IDockContent[]
        {
            NavigationPanel,
            ViewportPanel,
            ValidationPanel,
            PropertiesPanel,
            DetailPanel,
            StudConnectionPanel,
            ConnectionPanel,
            InfoPanel
        };

        private void InitializePanels()
        {
            Logger.Info("Initializing Panels");

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

            InfoPanel = new ProjectInfoPanel(ProjectManager);
            WaitPopup.UpdateProgress(8, 10);

        }

        private void LayoutDockPanels()
        {
            Logger.Info("Loading Panels");

            var savedLayout = SettingsManager.GetSavedUserLayout();
            if (!(savedLayout != null && LoadCustomLayout(savedLayout)))
                LoadDefaultLayout();

            if (WaitPopup != null && WaitPopup.Visible)
                WaitPopup.UpdateProgress(9, 10);

            ValidateAllPanelsLoaded();

            if (WaitPopup != null && WaitPopup.Visible)
                WaitPopup.UpdateProgress(10, 10);

            ViewportPanel.Activate();//must be visible to properly init GL resource

            //if (PropertiesPanel.Pane == DetailPanel.Pane)
            //{
            //    PropertiesPanel.Activate();
            //}
            //else
            //{
            //    DetailPanel.Activate();
            //}

            foreach (IDockContent dockPanel in DockPanelControl.Contents)
            {
                if (dockPanel is ProjectDocumentPanel documentPanel)
                    documentPanel.Enabled = false;
            }
        }

        private void ValidateAllPanelsLoaded()
        {
            var panels = DockPanels;
            var loadedPanels = DockPanelControl.Contents.OfType<ProjectDocumentPanel>().ToList();

            var pane = PropertiesPanel?.Pane;
            if (pane == null)
            {
                Logger.Warn("PropertiesPanel.Pane is NULL!");
                pane = loadedPanels.FirstOrDefault()?.Pane;
            }

            if (pane == null)
            {
                Logger.Error("Could not find DockPane!");
                return;
            }

            for (int i = 0; i < panels.Length; i++)
            {
                if (!loadedPanels.Contains(panels[i]))
                {
                    (panels[i] as DockContent).Show(pane, null);
                }
            }
        }

        private IDockContent DockContentLoadingHandler(string str)
        {
            var panels = DockPanels;

            string panelClass = str;
            string layoutArgs = null;

            if (panelClass.Contains(":"))
            {
                panelClass = str.Substring(0, str.IndexOf(":"));
                layoutArgs = str.Substring(str.IndexOf(":") + 1);
            }

            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].GetType().FullName == panelClass)
                {
                    if (!string.IsNullOrEmpty(layoutArgs) && 
                        panels[i] is ProjectDocumentPanel documentPanel)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(100);
                            BeginInvoke((Action)(() =>
                            {
                                documentPanel.ApplyLayoutArgs(layoutArgs);
                            }));
                        });
                    }

                    return panels[i];
                }
            }
            return null;
        }

        private bool LoadCustomLayout(UserUILayout layout)
        {
            if (!File.Exists(layout.Path))
                return false;

            Logger.Info("Loading custom layout");

            MemoryStream tmpMs = null;

            if (DockPanelControl.Contents.Count > 0)
            {
                tmpMs = new MemoryStream();
                DockPanelControl.SaveAsXml(tmpMs, Encoding.UTF8);
            }

            foreach (var content in DockPanelControl.Contents.ToArray())
                content.DockHandler.DockPanel = null;

            try
            {
                DockPanelControl.LoadFromXml(layout.Path, DockContentLoadingHandler);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error while loading custom layout xml");
            }

            if (tmpMs != null)
            {
                DockPanelControl.LoadFromXml(tmpMs, DockContentLoadingHandler);
            }

            return false;
        }

        private void LoadDefaultLayout()
        {
            foreach (var content in DockPanelControl.Contents.ToArray())
                content.DockHandler.DockPanel = null;

            try
            {
                var layoutStream = ResourceHelper.GetResourceStream("DefaultLayout.xml");
                if (layoutStream != null)
                    DockPanelControl.LoadFromXml(layoutStream, DockContentLoadingHandler);
                return;
            }
            catch { }

            ViewportPanel.Show(DockPanelControl, DockState.Document);

            StudConnectionPanel.Show(DockPanelControl, DockState.Document);

            ViewportPanel.Activate();

            DockPanelControl.DockLeftPortion = 250;

            NavigationPanel.Show(DockPanelControl, DockState.DockLeft);

            DockPanelControl.DockWindows[DockState.DockBottom].BringToFront();
            DockPanelControl.DockBottomPortion = 250;

            PropertiesPanel.Show(DockPanelControl, DockState.DockBottom);

            DetailPanel.Show(PropertiesPanel.Pane, null);

            ConnectionPanel.Show(PropertiesPanel.Pane, null);

            ValidationPanel.Show(PropertiesPanel.Pane, null);

            InfoPanel.Show(PropertiesPanel.Pane, null);
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
            WaitPopup.Shown -= OnInitializationPopupLoaded;
            

            this.InvokeWithDelay(100, ()=>
            {
                try
                {
                    InitializePanels();
                    LayoutDockPanels();
                    InitializeAfterShown();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error while initializing UI");
                    MessageBoxEX.ShowException(this, Messages.Caption_UnexpectedError, Messages.Caption_UnexpectedError, ex);
                }
                
            });
            
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(100);
            //    BeginInvoke((Action)(() =>
            //    {
            //        FlagManager.Set("OnLoadAsync");
            //        AutoSaveTimer.Interval = 500;
            //        AutoSaveTimer.Start();
            //        WaitPopup.Shown -= OnInitializationPopupLoaded;
            //        InitializePanels();
            //        LayoutDockPanels();
            //    }));
            //});
        }

        private void InitializeAfterShown()
        {
            WaitPopup.Message = Messages.Message_InitializingResources;
            WaitPopup.UpdateProgress(0, 0);
            
            Application.DoEvents();

            ResourceHelper.LoadResources();

            var documentPanels = DockPanelControl.Contents.OfType<ProjectDocumentPanel>().ToList();

            foreach (var documentPanel in documentPanels)
            {
                documentPanel.Enabled = true;
                documentPanel.DefferedInitialization();
            }

            if (!UIRenderHelper.LoadFreetype6())
            {
                MessageBoxEX.Show(this, Messages.Message_CouldNotLoadFreetype6, 
                    Messages.Caption_UnexpectedError, 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
            WaitPopup.Close();

            LoadAndValidateSettings();
            UpdateMenuItemStates();
            RebuildLayoutMenu();
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
            ProjectManager.ObjectPropertyChanged += ProjectManager_ObjectPropertyChanged;
        }

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            UpdateMenuItemStates();

            UpdateWindowTitle();
        }

        private void ProjectManager_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
        {
            if (e.Object is PartProperties && 
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

        private bool OpenPartProjectFile(string projectFilePath)
        {
            if (!CloseCurrentProject())
                return false;

            if (MultiInstanceManager.InstanceCount > 1)
            {
                if (MultiInstanceManager.CheckFileIsOpen(projectFilePath))
                {
                    MessageBoxEX.ShowDetails(this,
                        Messages.Error_OpeningProject,
                        Messages.Caption_OpeningProject,
                        "The file is already opened in another instance."); //TODO: translate
                    return false;
                }
            }

            PartProject loadedProject = null;
            try
            {
                loadedProject = PartProject.Open(projectFilePath);
            }
            catch (Exception ex)
            {
                MessageBoxEX.ShowDetails(this, 
                    Messages.Error_OpeningProject, 
                    Messages.Caption_OpeningProject, ex.ToString());
            }

            if (loadedProject != null)
            {
                LoadPartProject(loadedProject);
                
            }

            return loadedProject != null;
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
                
                if (!string.IsNullOrWhiteSpace(SettingsManager.Current.EditorSettings.Username))
                {
                    string username = SettingsManager.Current.EditorSettings.Username;
                    if (string.IsNullOrWhiteSpace(project.ProjectInfo.Authors)/* || 
                        !project.ProjectInfo.Authors.Contains(username)*/)
                    {
                        //if (!string.IsNullOrWhiteSpace(project.ProjectInfo.Authors))
                        //    username = ", " + username;

                        //project.ProjectInfo.Authors += username;
                        project.ProjectInfo.Authors = username;
                    }
                    
                }
                LoadPartProject(project);
            }
            catch (Exception ex)
            {
                MessageBoxEX.ShowDetails(this,
                    Messages.Error_CreatingProject,
                    Messages.Caption_OpeningProject, ex.ToString());
            }
        }

        private bool LoadPartProject(PartProject project, string tempPath = null)
        {
            if (!CloseCurrentProject())
                return false;

            if (string.IsNullOrEmpty(project.ProjectPath))
                Logger.Info($"Loading project new empty project");
            else
                Logger.Info($"Loading project {project.ProjectPath}");

            ViewportPanel.ForceRender();
            ProjectManager.SetCurrentProject(project, tempPath);

            if (project != null)
            {
                if (!ProjectManager.IsNewProject && string.IsNullOrEmpty(tempPath))
                {
                    RebuildRecentFilesMenu();
                    SettingsManager.AddRecentProject(ProjectManager.GetCurrentProjectInfo(), true);
                }
                
                AutoSaveTimer.Start();
            }

            return project != null;
        }

        public bool CloseCurrentProject()
        {
            if (ProjectManager.IsProjectOpen)
            {
                Logger.Info("Closing current project");
                AutoSaveTimer.Stop();

                if (ProjectManager.IsModified || ProjectManager.IsNewProject)
                {
                    var messageText = ProjectManager.IsNewProject ? 
                        Messages.Message_SaveNewProject : 
                        Messages.Message_SaveChanges;

                    var result = MessageBox.Show(messageText, Messages.Caption_SaveBeforeClose, MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                        SaveProject();
                    else if (result == DialogResult.Cancel)
                    {
                        AutoSaveTimer.Start();
                        return false;
                    }
                }

                ProjectManager.CloseCurrentProject();
                
            }

            return true;
        }

        public void SaveProject(bool selectPath = false)
        {
            var project = ProjectManager.CurrentProject;
            string targetPath = ProjectManager.CurrentProjectPath;
            Logger.Info($"Saving current project");

            if (selectPath || ProjectManager.IsNewProject)
            {
                using (var sfd = new SaveFileDialog())
                {
                    if (!string.IsNullOrEmpty(targetPath))
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(targetPath);
                        sfd.FileName = Path.GetFileName(targetPath);
                    }
                    else
                    {
                        if (SettingsManager.IsWorkspaceDefined)
                            sfd.InitialDirectory = SettingsManager.Current.EditorSettings.ProjectWorkspace;

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

            string oldPath = ProjectManager.CurrentProjectPath;

            ProjectManager.SaveProject(targetPath);

            if (oldPath != targetPath)
                RebuildRecentFilesMenu();
        }

        private void CheckCanRecoverProject()
        {
            SettingsManager.CleanUpFilesHistory();

            if (SettingsManager.Current.OpenedProjects.Count > 0)
            {
                bool projectWasLoaded = false;

                foreach(var fileInfo in SettingsManager.Current.OpenedProjects.ToArray())
                {
                    //project was not correctly closed
                    if (File.Exists(fileInfo.TemporaryPath))
                    {
                        if (projectWasLoaded)
                            continue;

                        if (MultiInstanceManager.InstanceCount > 1)
                        {
                            bool isOpenInOtherInstance = MultiInstanceManager.CheckFileIsOpen(fileInfo.TemporaryPath);
                            if (isOpenInOtherInstance)
                                return;
                        }

                        bool projectRestored = false;

                        if (MessageBoxEX.Show(this,
                            Messages.Message_RecoverProject,
                            Messages.Caption_RecoverLastProject, 
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            PartProject loadedProject = null;
                            try
                            {
                                loadedProject = PartProject.Open(fileInfo.TemporaryPath);
                                loadedProject.ProjectPath = fileInfo.ProjectFile;

                                if (LoadPartProject(loadedProject, fileInfo.TemporaryPath))
                                {
                                    projectRestored = true;
                                    projectWasLoaded = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBoxEX.ShowDetails(this,
                                    Messages.Error_OpeningProject,
                                    Messages.Caption_OpeningProject, ex.ToString());
                                //exceptionThrown = true;
                            }
                            
                        }

                        if (!projectRestored)
                            ProjectManager.DeleteTemporaryProject(fileInfo.TemporaryPath);

                        break;
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
            if (IsInitializing)
            {
                e.Cancel = true;
                return;
            }

            Logger.Info("Exiting Brick Studio");

            SaveCurrentUILayout();

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
                return ProjectManager.CurrentProjectPath == filepath || 
                    ProjectManager.TemporaryProjectPath == filepath;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (!MultiInstanceManager.ProcessMessage(ref m))
                base.WndProc(ref m);
        }

        private void SaveCurrentUILayout()
        {
            SettingsManager.SaveCurrentUILayout(DockPanelControl);

            User32.WINDOWPLACEMENT posInfo = default;
            if (User32.GetWindowPlacement(Handle, ref posInfo))
            {
                SettingsManager.LoadSettings();
                SettingsManager.Current.DisplaySettings.LastPosition = posInfo.NormalPosition;
                SettingsManager.Current.DisplaySettings.IsMaximized = posInfo.ShowCmd.HasFlag(User32.ShowWindowCommands.Maximize);
                SettingsManager.SaveSettings();
            }
        }

        
    }
}
