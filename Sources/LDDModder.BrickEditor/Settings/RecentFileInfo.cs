using LDDModder.Modding.Editing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class RecentFileInfo
    {
        [JsonProperty]
        public int PartID { get; set; }
        [JsonProperty]
        public string PartName { get; set; }
        [JsonProperty]
        public string ProjectFile { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TemporaryPath { get; set; }

        public RecentFileInfo() { }

        public RecentFileInfo(PartProject project, bool includeWorkingDir = false)
        {
            PartID = project.PartID;
            PartName = project.PartDescription;
            ProjectFile = project.ProjectPath;

            //if (includeWorkingDir)
            //    WorkingDirectory = project.TemporaryProjectPath;
        }

        public RecentFileInfo(PartProject project, string temporaryPath)
        {
            PartID = project.PartID;
            PartName = project.PartDescription;
            ProjectFile = project.ProjectPath;
            TemporaryPath = temporaryPath;
        }
    }
}
