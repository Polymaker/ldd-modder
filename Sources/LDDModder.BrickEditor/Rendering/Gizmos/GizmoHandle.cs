using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public abstract class GizmoHandle
    {
        public Matrix4 Orientation { get; private set; }



        public abstract bool HitTest(Ray ray, out float distance);
    }
}
