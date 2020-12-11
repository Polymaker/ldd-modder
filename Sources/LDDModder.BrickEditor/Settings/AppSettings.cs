using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class AppSettings
    {
        //[JsonProperty("ldd.programFilesPath")]
        //public string LddProgramFilesPath { get; set; }

        //[JsonProperty("ldd.appDataPath")]
        //public string LddApplicationDataPath { get; set; }

        //[JsonProperty("workspace.folder")]
        //public string ProjectWorkspace { get; set; }

        [JsonProperty("lddEnvironment")]
        public LddSettings LddSettings { get; set; }

        public EditorSettings EditorSettings { get; set; }

        [JsonProperty("buildConfigurations")]
        public ProjectBuildSettings BuildSettings { get; set; }

        [JsonProperty("file.opened")]
        public List<RecentFileInfo> OpenedProjects { get; set; }

        //[JsonProperty("currentProjectPath")]
        //public RecentFileInfo LastOpenProject { get; set; }

        [JsonProperty("file.history")]
        public List<RecentFileInfo> RecentProjectFiles { get; set; }

        //[JsonProperty("autosave.interval")]
        //public int AutoSaveInterval { get; set; }

        public AppSettings()
        {
            RecentProjectFiles = new List<RecentFileInfo>();
            BuildSettings = new ProjectBuildSettings();
            EditorSettings = new EditorSettings();
            LddSettings = new LddSettings();
            OpenedProjects = new List<RecentFileInfo>();
        }

        public void InitializeDefaultValues()
        {
            if (BuildSettings == null)
                BuildSettings = new ProjectBuildSettings();

            BuildSettings.InitializeDefaults();

            if (EditorSettings == null)
                EditorSettings = new EditorSettings();

            EditorSettings.InitializeDefaults();

            if (LddSettings == null)
                LddSettings = new LddSettings();

            LddSettings.InitializeDefaults();
        }
    }
}
