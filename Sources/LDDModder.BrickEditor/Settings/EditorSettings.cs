using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Settings
{
    public class EditorSettings
    {
        [JsonProperty("workspaceFolder")]
        public string ProjectWorkspace { get; set; }

        [JsonProperty("backupInterval")]
        public int BackupInterval { get; set; } = -1;

        [JsonProperty("username")]
        public string Username { get; set; }

        public void InitializeDefaults()
        {
            if (BackupInterval < 0)
                BackupInterval = 60;
        }
    }
}
