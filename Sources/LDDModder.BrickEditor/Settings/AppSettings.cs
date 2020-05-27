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
        [JsonProperty/*("ldd.paths.programfiles")*/]
        public string LddProgramFilesPath { get; set; }
        [JsonProperty/*("ldd.paths.programfiles")*/]
        public string LddApplicationDataPath { get; set; }
        [JsonProperty/*("workspace_folder")*/]
        public string ProjectWorkspace { get; set; }

        [JsonProperty/*("build_configs")*/]
        public List<BuildConfiguration> BuildConfigurations { get; set; }

        [JsonProperty]
        public RecentFileInfo LastOpenProject { get; set; }

        [JsonProperty/*("recent_files")*/]
        public List<RecentFileInfo> RecentProjectFiles { get; set; }

        //[JsonProperty("Display")]
        //public DisplaySettings DisplaySettings { get; set; }

        public AppSettings()
        {
            BuildConfigurations = new List<BuildConfiguration>();
            RecentProjectFiles = new List<RecentFileInfo>();
        }

        public static AppSettings CreateDefault()
        {
            var currEnv = LDD.LDDEnvironment.GetInstalledEnvironment();
            return new AppSettings()
            {
                LddApplicationDataPath = currEnv?.ApplicationDataPath,
                LddProgramFilesPath = currEnv?.ProgramFilesPath
            };
        }
    }
}
