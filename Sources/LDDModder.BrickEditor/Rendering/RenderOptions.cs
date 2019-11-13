using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class RenderOptions
    {
        public bool Hidden { get; set; }
        public bool DrawWireframe { get; set; }
        public bool DrawTextured { get; set; }
        public bool DrawShaded { get; set; }
        public bool DrawTransparent { get; set; }
    }
}
