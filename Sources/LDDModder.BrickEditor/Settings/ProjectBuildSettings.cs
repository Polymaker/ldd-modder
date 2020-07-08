using LDDModder.BrickEditor.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class ProjectBuildSettings
    {
        [JsonProperty("ldd")]
        public BuildConfiguration LDD { get; set; }

        [JsonProperty("manual")]
        public BuildConfiguration Manual { get; set; }

        [JsonProperty("userDefined")]
        public List<BuildConfiguration> UserDefined { get; set; }

        public ProjectBuildSettings()
        {
            UserDefined = new List<BuildConfiguration>();
        }

        public void InitializeDefaults()
        {
            if (LDD == null)
            {
                LDD = new BuildConfiguration()
                {
                    ConfirmOverwrite = true
                };
            }

            LDD.OutputPath = "$(LddAppData)\\db\\";
            LDD.Name = Messages.BuildConfig_LDD;
            LDD.InternalFlag = 1;
            LDD.LOD0Subdirectory = true;

            if (Manual == null)
            {
                Manual = new BuildConfiguration()
                {
                    ConfirmOverwrite = true,
                    LOD0Subdirectory = true
                };
            }

            Manual.OutputPath = string.Empty;
            Manual.Name = Messages.BuildConfig_Browse;
            Manual.InternalFlag = 2;

            if (UserDefined == null)
                UserDefined = new List<BuildConfiguration>();
        }

        
    }
}
