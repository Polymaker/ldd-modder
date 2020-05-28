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
                    ConfirmOverwrite = true,
                    InternalFlag = 1,
                    OutputPath = "$(LddAppData)\\db\\",
                    Name = Messages.BuildConfig_LDD
                };
            }
            else
                LDD.InternalFlag = 1;

            if (Manual == null)
            {
                Manual = new BuildConfiguration()
                {
                    ConfirmOverwrite = true,
                    InternalFlag = 2,
                    OutputPath = string.Empty,
                    Name = Messages.BuildConfig_Browse
                };
            }
            else
                Manual.InternalFlag = 2;
        }

        
    }
}
