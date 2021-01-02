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
        [JsonProperty("lddEnvironment")]
        public LddSettings LddSettings { get; set; }

        [JsonProperty("editorSettings")]
        public EditorSettings EditorSettings { get; set; }

        [JsonProperty("buildConfigurations")]
        public ProjectBuildSettings BuildSettings { get; set; }

        [JsonProperty("displaySettings")]
        public DisplaySettings DisplaySettings { get; set; }

        [JsonProperty("file.opened")]
        public List<RecentFileInfo> OpenedProjects { get; set; }

        [JsonProperty("file.history")]
        public List<RecentFileInfo> RecentProjectFiles { get; set; }

        public AppSettings()
        {
            RecentProjectFiles = new List<RecentFileInfo>();
            BuildSettings = new ProjectBuildSettings();
            EditorSettings = new EditorSettings();
            LddSettings = new LddSettings();
            DisplaySettings = new DisplaySettings();
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

            if (DisplaySettings == null)
                DisplaySettings = new DisplaySettings();

            DisplaySettings.InitializeDefaults();
        }

        public IEnumerable<BuildConfiguration> GetBuildConfigurations()
        {
            if (BuildSettings == null)
                yield break;

            if (BuildSettings.LDD != null)
                yield return BuildSettings.LDD;

            if (BuildSettings.Manual != null)
                yield return BuildSettings.Manual;
            
            foreach (var cfg in BuildSettings.UserDefined)
            {
                cfg.GenerateUniqueID();
                yield return cfg;
            }
        }
    }
}
