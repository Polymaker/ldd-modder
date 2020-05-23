using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Settings
{
    public class DisplaySettings
    {
        [JsonProperty]
        public Color WireframeColor { get; set; }

        [JsonProperty]
        public Color CollisionColor { get; set; }

        [JsonProperty]
        public Color DefaultPartColor { get; set; }

        public static DisplaySettings CreateDefault()
        {
            return new DisplaySettings()
            {
                WireframeColor = Color.Black,
                CollisionColor = Color.Red,
                DefaultPartColor = Color.FromArgb(155,155,155)
            };
        }
    }
}
