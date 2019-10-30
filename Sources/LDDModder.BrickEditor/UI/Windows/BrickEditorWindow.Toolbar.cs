using LDDModder.BrickEditor.Settings;
using LDDModder.Modding.Editing;
using System;
using System.IO;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.UI.Windows
{
    public partial class BrickEditorWindow
    {
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

        private void RebuildRecentFilesMenu()
        {
            File_OpenRecentMenu.DropDownItems.Clear();

            foreach (var recentProjectInfo in SettingsManager.Current.RecentProjectFiles.ToArray())
            {
                if (!File.Exists(recentProjectInfo.ProjectFile))
                {
                    SettingsManager.Current.RecentProjectFiles.Remove(recentProjectInfo);
                    continue;
                }
                var recentFileMenuEntry = File_OpenRecentMenu.DropDownItems.Add(Path.GetFileName(recentProjectInfo.ProjectFile));
                recentFileMenuEntry.Tag = recentProjectInfo;
                recentFileMenuEntry.Click += RecentFileMenuEntry_Click;
            }

            File_OpenRecentMenu.Enabled = File_OpenRecentMenu.DropDownItems.Count > 0;
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
    }
}
