using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    /// <summary>
    /// TODO: re-implement this elsewhere
    /// </summary>
    public class RenderOptions
    {
        public bool Hidden { get; set; }
        public bool DrawWireframe { get; set; }
        public bool DrawTextured { get; set; }
        public bool DrawShaded { get; set; }
        public bool DrawTransparent { get; set; }

        public Vector4 WireframeColor { get; set; }

        public Vector4 WireframeColorAlt { get; set; }

        public Vector4 OutlineColor { get; set; }

        public RenderOptions()
        {
            WireframeColor = new Vector4(0, 0, 0, 1f);
            WireframeColorAlt = new Vector4(0.85f, 0.85f, 0.85f, 1f);// new Vector4(0.956f, 0.6f, 0.168f, 1f);
            OutlineColor = new Vector4(1f);
        }
    }
}
