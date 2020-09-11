﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public class BuildConfiguration
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("outputPath", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputPath { get; set; }
        
        [JsonProperty("useLOD0Subdirectory")]
        public bool LOD0Subdirectory { get; set; }

        [JsonProperty("confirmOverwrite")]
        public bool ConfirmOverwrite { get; set; }

        

        //[JsonProperty("compress", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(false)]
        //public bool CreateZip { get; set; }

        [JsonIgnore]
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public int InternalFlag { get; set; }

        [JsonIgnore]
        public bool IsInternalConfig => InternalFlag > 0;

        [JsonIgnore]
        public string UniqueID { get; set; }


        public const int LDD_FLAG = 1;

        public const int MANUAL_FLAG = 2;

        public bool ShouldSerializeName()
        {
            return InternalFlag == 0;
        }

        public bool ShouldSerializeOutputPath()
        {
            return InternalFlag == 0;
        }

        public bool ShouldSerializeLOD0Subdirectory()
        {
            return InternalFlag != LDD_FLAG;
        }

        public BuildConfiguration Clone()
        {
            return new BuildConfiguration()
            {
                Name = Name,
                OutputPath = OutputPath,
                LOD0Subdirectory = LOD0Subdirectory,
                ConfirmOverwrite = ConfirmOverwrite,
                InternalFlag = InternalFlag,
                IsDefault = IsDefault,
                UniqueID = UniqueID
            };
        }

        public void GenerateUniqueID()
        {
            if (string.IsNullOrEmpty(UniqueID))
            {
                UniqueID = Guid.NewGuid().ToString();
            }
        }
    }
}
