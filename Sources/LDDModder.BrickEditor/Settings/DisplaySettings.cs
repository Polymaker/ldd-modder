using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Settings
{
    public class DisplaySettings : ISettingsClass
    {
        //[JsonProperty("viewport")]
        //public ViewportDisplaySettings DefaultVisibility { get; set; }

        //[JsonProperty("defaultLayout")]
        //public string DefaultLayout { get; set; }

        [JsonProperty("lastPosition")]
        public Rectangle LastPosition { get; set; }

        [JsonProperty("isMaximized")]
        public bool IsMaximized { get; set; }

        //[JsonProperty("maximizedLayout")]
        //public string MaximizedLayout { get; set; }

        [JsonProperty("colors")]
        public ColorSettings Colors { get; set; }

        public void InitializeDefaults()
        {
            if (Colors == null)
                Colors = new ColorSettings();

            bool IsNotSet(Color color) => color == default || color.IsEmpty;

            if (IsNotSet(Colors.Wireframe))
                Colors.Wireframe = Color.Black;

            if (IsNotSet(Colors.WireframeAlt))
                Colors.WireframeAlt = Color.FromArgb(217, 217, 217);

            if (IsNotSet(Colors.Collision))
                Colors.Collision = Color.FromArgb(255, 13, 13);

            if (IsNotSet(Colors.Connection))
                Colors.Connection = Color.FromArgb(64, 64, 242);

            if (IsNotSet(Colors.ConnectionAlt))
                Colors.ConnectionAlt = Color.FromArgb(13, 242, 13);

            if (IsNotSet(Colors.Selection))
                Colors.Selection = Color.White;
        }

        public class ColorSettings
        {
            [JsonProperty("wireframe")]
            public Color Wireframe { get; set; }

            [JsonProperty("wireframeAlt")]
            public Color WireframeAlt { get; set; }

            [JsonProperty("selection")]
            public Color Selection { get; set; }

            [JsonProperty("collision")]
            public Color Collision { get; set; }

            [JsonProperty("connection")]
            public Color Connection { get; set; }

            [JsonProperty("connectionAlt")]
            public Color ConnectionAlt { get; set; }

            [JsonProperty("part")]
            public Color Part { get; set; }
        }

    }
}
