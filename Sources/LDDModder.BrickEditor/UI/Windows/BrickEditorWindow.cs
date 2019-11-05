using LDDModder.BrickEditor.Native;
using LDDModder.BrickEditor.Settings;
using LDDModder.BrickEditor.UI.Panels;
using LDDModder.Modding.Editing;
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
using WeifenLuo.WinFormsUI.Docking;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BrickEditorWindow : Form
    {
        private NavigationPanel Navigation;
        private ViewportPanel Viewport;

        public PartProject CurrentProject { get; private set; }
        //private string TemporaryFolder;

        public BrickEditorWindow()
        {
            InitializeComponent();
            visualStudioToolStripExtender1.SetStyle(menuStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, DockPanelControl.Theme);
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SettingsManager.Initialize();
            InitializePanels();
            RebuildRecentFilesMenu();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (SettingsManager.Current.LastOpenProject != null)
            {
                var fileInfo = SettingsManager.Current.LastOpenProject;
                if (Directory.Exists(fileInfo.WorkingDirectory))
                {
                    //project was not correctly close/saved
                    OpenProjectWorkingDir(fileInfo.WorkingDirectory);
                }
            }
        }

        private void InitializePanels()
        {
            Navigation = new NavigationPanel();
            Viewport = new ViewportPanel();
            
            Viewport.Show(DockPanelControl, DockState.Document);
            DockPanelControl.DockLeftPortion = 250;
            Navigation.Show(DockPanelControl, DockState.DockLeft);
            
        }

        #region Main menu

        private void LDDEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new LddEnvironmentConfigWindow())
                dlg.ShowDialog();
        }

        private void ExportBrickMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new ModelImportExportWindow())
                frm.ShowDialog();
        }

        #endregion


        #region Project Handling

        public string GetTemporaryWorkingDir()
        {
            return Path.Combine(Path.GetTempPath(), Utilities.StringUtils.GenerateUID(16)); ;
        }

        private void OpenPartProjectFile(string projectPath)
        {
            CloseCurrentProject();

            string tmpProjectDir = GetTemporaryWorkingDir();

            try
            {
                using (var fs = File.OpenRead(projectPath))
                {
                    var project = PartProject.ExtractAndOpen(fs, tmpProjectDir);
                    project.ProjectPath = projectPath;
                    project.ProjectWorkingDir = tmpProjectDir;
                    SettingsManager.Current.LastOpenProject = new RecentFileInfo(project, true);
                    SettingsManager.AddRecentProject(project);
                    LoadPartProject(project);
                    RebuildRecentFilesMenu();
                }
            }
            catch
            {

            }
        }

        private void OpenProjectWorkingDir(string projectPath)
        {
            CloseCurrentProject();

            try
            {
                var project = PartProject.LoadFromDirectory(projectPath);
                LoadPartProject(project);
            }
            catch { }
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

            CurrentProject = project;
            Navigation.LoadPartProject(project);
            Viewport.LoadPartProject(project);

            File_SaveMenu.Enabled = project != null;
            File_SaveAsMenu.Enabled = project != null;

        }

        public bool CloseCurrentProject()
        {
            if (CurrentProject != null)
            {
                if (string.IsNullOrEmpty(CurrentProject.ProjectPath))
                {
                    //project not saved

                }

                if (!string.IsNullOrEmpty(CurrentProject.ProjectWorkingDir))
                    NativeMethods.DeleteFileOrFolder(CurrentProject.ProjectWorkingDir, true, false);

                SettingsManager.Current.LastOpenProject = null;
                SettingsManager.SaveSettings();
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

        #endregion

        

        private void BrickEditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseCurrentProject())
            {
                e.Cancel = true;
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

        private void DockPanelControl_ActiveDocumentChanged(object sender, EventArgs e)
        {
            
        }

        
    }
}
