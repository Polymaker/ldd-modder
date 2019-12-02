using LDDModder.BrickEditor.EditModels;
using LDDModder.BrickEditor.Native;
using LDDModder.BrickEditor.Resources;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Panels;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class BrickEditorWindow : Form
    {
        
        private Assimp.AssimpContext AssimpContext;

        public ProjectManager ProjectManager { get; private set; }

        public PartProject CurrentProject => ProjectManager.CurrentProject;

        private bool ProjectCreatedFromBrick;

        //private string TemporaryFolder;

        public BrickEditorWindow()
        {
            InitializeComponent();
            visualStudioToolStripExtender1.SetStyle(menuStrip1, 
                VisualStudioToolStripExtender.VsVersion.Vs2015,
                DockPanelControl.Theme);

            ProjectManager = new ProjectManager();
            ProjectManager.ProjectChanged += ProjectManager_ProjectChanged;
            ProjectManager.UndoHistoryChanged += ProjectManager_UndoHistoryChanged;
        }

        private void ProjectManager_UndoHistoryChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(UpdateUndoRedoMenus));
            else
                UpdateUndoRedoMenus();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SettingsManager.Initialize();
            ResourceHelper.LoadPlatformsAndCategories();
            InitializePanels();
            RebuildRecentFilesMenu();
            UpdateMenuItemStates();
            AssimpContext = new Assimp.AssimpContext();
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

        #region UI Layout

        private NavigationPanel NavigationPanel;
        private ViewportPanel ViewportPanel;
        private ValidationPanel ValidationPanel;
        private ProjectPropertiesPanel PropertiesPanel;

        private void InitializePanels()
        {
            NavigationPanel = new NavigationPanel(ProjectManager);
            ViewportPanel = new ViewportPanel(ProjectManager);
            ValidationPanel = new ValidationPanel(ProjectManager);
            PropertiesPanel = new ProjectPropertiesPanel(ProjectManager);


            ViewportPanel.Show(DockPanelControl, DockState.Document);

            DockPanelControl.DockLeftPortion = 250;
            NavigationPanel.Show(DockPanelControl, DockState.DockLeft);

            DockPanelControl.DockWindows[DockState.DockBottom].BringToFront();
            DockPanelControl.DockBottomPortion = 200;

            PropertiesPanel.Show(DockPanelControl, DockState.DockBottom);
            ValidationPanel.Show(PropertiesPanel.Pane, null);

            PropertiesPanel.Activate();

        }

        #endregion

        #region Project Handling

        private void ProjectManager_ProjectChanged(object sender, EventArgs e)
        {
            UpdateMenuItemStates();
            ProjectCreatedFromBrick = false;
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

            try
            {
                using (var fs = File.OpenRead(projectFilePath))
                    loadedProject = PartProject.ExtractAndOpen(fs, tmpProjectDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "There was an error:\r\n" + ex.ToString(), "Error opening file");
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
                MessageBox.Show(this, "There was an error:\r\n" + ex.ToString(), "Error opening project");
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
            catch { }
        }

        private void LoadPartProject(PartProject project)
        {
            if (!CloseCurrentProject())
                return;

            ProjectManager.SetCurrentProject(project);
        }

        public bool CloseCurrentProject()
        {
            if (ProjectManager.IsProjectOpen)
            {
                if (ProjectManager.IsModified || ProjectManager.IsNewProject)
                {
                    var result = MessageBox.Show("Do you want to save?", "", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                        SaveProject(CurrentProject);
                    else if (result == DialogResult.Cancel)
                        return false;
                }

                string workingDirPath = CurrentProject?.ProjectWorkingDir;
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
                            sfd.FileName = $"new part.lpp";
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
            project.Save(targetPath);
            
            project.ProjectPath = targetPath;
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
            using (var imd = new ImportModelsDialog())
            {
                imd.Project = CurrentProject;
                imd.SelectFileOnStart = true;
                if (imd.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }
        /*
        private void ImportAssimpModel(Assimp.Scene scene, PartSurface preferredSurface)
        {
            using(var imd = new ImportModelsDialog())
            {
                imd.Project = CurrentProject;
                imd.SceneToImport = scene;
                imd.PreferredSurfaceID = preferredSurface?.SurfaceID ?? -1;
                imd.SelectFileOnStart = true;

                if (imd.ShowDialog() == DialogResult.OK)
                {

                    foreach (var model in imd.ModelsToImport)
                    {
                        var geom = Meshes.MeshConverter.AssimpToLdd(imd.SceneToImport, model.Mesh);
                        var surface = CurrentProject.Surfaces.FirstOrDefault(x => x.SurfaceID == model.SurfaceID);

                        if (surface == null)
                        {
                            surface = new PartSurface(model.SurfaceID, CurrentProject.Surfaces.Max(x=>x.SubMaterialIndex) + 1);
                            CurrentProject.Surfaces.Add(surface);
                        }

                        var partModel = surface.Components.FirstOrDefault(x=>x.ComponentType == ModelComponentType.Part);

                        if (partModel == null)
                        {
                            partModel = new PartModel();
                            surface.Components.Add(partModel);
                        }

                        var modelMesh = CurrentProject.AddMeshGeometry(geom);
                        partModel.Meshes.Add(new ModelMeshReference(modelMesh));

                    }
                }
            }
        }
        */
        private void BrickEditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentProject != null)
            {
                e.Cancel = true;
                BeginInvoke(new MethodInvoker(TryCloseProjectAndExit));
                return;
            }
            

            foreach (var form in DockPanelControl.Documents.OfType<DockContent>().ToList())
            {
                form.Close();
                if (!form.IsDisposed)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void TryCloseProjectAndExit()
        {
            if (CloseCurrentProject())
            {
                //Application.DoEvents();
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(100);
                    BeginInvoke(new MethodInvoker(Close));
                });
                //Close();
            }
        }

        
    }
}
