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
        [JsonProperty("ldd.programFilesPath")]
        public string LddProgramFilesPath { get; set; }
        [JsonProperty("ldd.appDataPath")]
        public string LddApplicationDataPath { get; set; }

        [JsonProperty("workspace.folder")]
        public string ProjectWorkspace { get; set; }

        [JsonProperty("build.configurations")]
        public ProjectBuildSettings BuildSettings { get; set; }

        //[JsonProperty("viewport.default")]
        //public ViewportDisplaySettings ViewportSettings { get; set; }
        //[JsonProperty("file.history")]
        //public List<RecentFileInfo> OpenedProjects { get; set; }

        [JsonProperty("currentProjectPath")]
        public RecentFileInfo LastOpenProject { get; set; }

        [JsonProperty("file.history")]
        public List<RecentFileInfo> RecentProjectFiles { get; set; }

        [JsonProperty("autosave.interval")]
        public int AutoSaveInterval { get; set; }

        //[JsonProperty("Display")]
        //public DisplaySettings DisplaySettings { get; set; }

        public AppSettings()
        {
            RecentProjectFiles = new List<RecentFileInfo>();
            BuildSettings = new ProjectBuildSettings();
            AutoSaveInterval = -1;
            //ViewportSettings = new ViewportDisplaySettings()
            //{
            //    PartRenderMode = Rendering.MeshRenderMode.SolidWireframe,
            //    ShowPartModels = true
            //};
        }

        public static AppSettings CreateDefault(LDD.LDDEnvironment lddEnvironment)
        {
            var settings = new AppSettings()
            {
                LddApplicationDataPath = lddEnvironment?.ApplicationDataPath ?? string.Empty,
                LddProgramFilesPath = lddEnvironment?.ProgramFilesPath ?? string.Empty,
                AutoSaveInterval = 15
            };
            return settings;
        }

        public static AppSettings CreateDefault()
        {
            return CreateDefault(LDD.LDDEnvironment.InstalledEnvironment);
        }

        public void InitializeDefaultValues()
        {
            if (string.IsNullOrEmpty(LddApplicationDataPath) ||
                string.IsNullOrEmpty(LddProgramFilesPath))
            {
                var installedEnv = LDD.LDDEnvironment.InstalledEnvironment;
                LddApplicationDataPath = installedEnv?.ApplicationDataPath ?? string.Empty;
                LddProgramFilesPath = installedEnv?.ProgramFilesPath ?? string.Empty;
            }

            if (AutoSaveInterval < 0)
                AutoSaveInterval = 15;

            if (BuildSettings == null)
                BuildSettings = new ProjectBuildSettings();
            BuildSettings.InitializeDefaults();
        }
    }
}
