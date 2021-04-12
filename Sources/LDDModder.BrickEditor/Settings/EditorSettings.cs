using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Settings
{
    public class EditorSettings : ISettingsClass
    {
        [JsonProperty("workspaceFolder")]
        public string ProjectWorkspace { get; set; }

        [JsonProperty("backupInterval")]
        public int BackupInterval { get; set; } = -1;

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("defaultFlexAttributes")]
        public double[] DefaultFlexAttributes { get; set; }

        public void InitializeDefaults()
        {
            if (BackupInterval < 0)
                BackupInterval = 60;
            if (DefaultFlexAttributes == null)
                DefaultFlexAttributes = new double[] { -0.06, 0.06, 20, 10, 10 };
        }
    }
}
