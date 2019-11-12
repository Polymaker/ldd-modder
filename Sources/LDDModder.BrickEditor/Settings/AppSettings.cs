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
        [JsonProperty]
        public string LddProgramFilesPath { get; set; }
        [JsonProperty]
        public string LddApplicationDataPath { get; set; }
        [JsonProperty]
        public string ProjectWorkspace { get; set; }
        [JsonProperty]
        public RecentFileInfo LastOpenProject { get; set; }
        [JsonProperty]
        public List<RecentFileInfo> RecentProjectFiles { get; set; } = new List<RecentFileInfo>();

        //[JsonProperty("Display")]
        //public DisplaySettings DisplaySettings { get; set; }

        public static AppSettings CreateDefault()
        {
            var currEnv = LDD.LDDEnvironment.Current;
            return new AppSettings()
            {
                LddApplicationDataPath = currEnv.ApplicationDataPath,
                LddProgramFilesPath = currEnv.ProgramFilesPath
            };
        }
    }
}
