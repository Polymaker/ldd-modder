using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLSimpleMesh : GLMeshBase<VertVN>
    {
        public void BindToShader(BasicShaderProgram shader)
        {
            BindToProgram(shader);
            Vao.Bind();
            BindVertexAttribute(shader.InPosition);
            BindVertexAttribute(shader.InNormal, 12);
            if (IndexBuffer != null)
                Vao.BindElementBuffer(IndexBuffer);
        }
    }
}
