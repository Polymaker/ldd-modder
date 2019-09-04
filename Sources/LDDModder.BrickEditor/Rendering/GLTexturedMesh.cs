using ObjectTK.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLTexturedMesh : GLMeshBase<VertVNT>
    {
        public Texture2D Texture { get; set; }
        public OpenTK.Graphics.OpenGL.TextureUnit TextureUnit { get; set; }

        public void BindToShader(TexturedShaderProgram shader)
        {
            BindToProgram(shader);
            Vao.Bind();
            BindVertexAttribute(shader.InPosition);
            BindVertexAttribute(shader.InNormal, 12);
            BindVertexAttribute(shader.InTexCoord, 24);
            Vao.BindElementBuffer(IndexBuffer);
        }

        public override void AssignShaderValues()
        {
            base.AssignShaderValues();

            if (BoundProgram is TexturedShaderProgram program)
            {
                program.Texture.Set(TextureUnit);
            }
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            Texture.Bind(TextureUnit);
        }
    }
}
