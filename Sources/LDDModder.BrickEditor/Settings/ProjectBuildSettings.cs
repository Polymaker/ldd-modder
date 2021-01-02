using LDDModder.BrickEditor.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class ProjectBuildSettings : ISettingsClass
    {
        [JsonProperty("ldd")]
        public BuildConfiguration LDD { get; set; }

        [JsonProperty("manual")]
        public BuildConfiguration Manual { get; set; }

        [JsonProperty("userDefined")]
        public List<BuildConfiguration> UserDefined { get; set; }

        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string DefaultConfigName { get; set; }

        [JsonIgnore]
        public BuildConfiguration DefaultConfiguration
        {
            get
            {
                if (string.IsNullOrEmpty(DefaultConfigName))
                    return null;

                if (DefaultConfigName == "ldd")
                    return LDD;
                if (DefaultConfigName == "manual")
                    return Manual;

                return UserDefined.FirstOrDefault(x => x.Name == DefaultConfigName);
            }
        }

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

            LDD.OutputPath = "$(LddAppData)\\db\\Primitives";
            LDD.Name = Messages.BuildConfig_LDD;
            LDD.InternalFlag = 1;
            LDD.LOD0Subdirectory = true;
            LDD.GenerateUniqueID();

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
            Manual.GenerateUniqueID();

            if (UserDefined == null)
                UserDefined = new List<BuildConfiguration>();


            if (DefaultConfiguration != null)
                DefaultConfiguration.IsDefault = true;
            else
                DefaultConfigName = string.Empty;
        }

        public IEnumerable<BuildConfiguration> GetAllConfigurations()
        {
            if (LDD != null)
                yield return LDD;

            if (Manual != null)
                yield return Manual;

            foreach (var cfg in UserDefined)
                yield return cfg;
        }
    }
}
