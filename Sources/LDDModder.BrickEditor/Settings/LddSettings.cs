using LDDModder.LDD;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class LddSettings : ISettingsClass
    {
        [JsonProperty("programFilesPath")]
        public string ProgramFilesPath { get; set; }
        [JsonProperty("appDataPath")]
        public string ApplicationDataPath { get; set; }

        public void InitializeDefaults()
        {
            if (LDDEnvironment.HasInitialized && LDDEnvironment.IsInstalled)
            {
                if (string.IsNullOrEmpty(ProgramFilesPath))
                    ProgramFilesPath = LDDEnvironment.InstalledEnvironment.ProgramFilesPath;

                if (string.IsNullOrEmpty(ApplicationDataPath))
                    ApplicationDataPath = LDDEnvironment.InstalledEnvironment.ApplicationDataPath;
            }
        }
    }
}
