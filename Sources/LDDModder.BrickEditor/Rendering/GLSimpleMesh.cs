using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLSimpleMesh : GLMeshBase<VertVN>
    {
        protected override void BindShaderAttributes(ObjectTK.Shaders.Program program)
        {
            if (program is BasicShaderProgram shader)
            {
                BindVertexAttribute(shader.InPosition);
                BindVertexAttribute(shader.InNormal, 12);
            }
        }
    }
}
