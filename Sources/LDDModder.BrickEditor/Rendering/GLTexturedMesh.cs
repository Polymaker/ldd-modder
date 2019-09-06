using ObjectTK.Shaders;
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

        public GLTexturedMesh()
        {
            TextureUnit = OpenTK.Graphics.OpenGL.TextureUnit.Texture0;
        }

        protected override void BindShaderAttributes(ObjectTK.Shaders.Program program)
        {
            if (program is TexturedShaderProgram shader)
            {
                BindVertexAttribute(shader.InPosition);
                BindVertexAttribute(shader.InNormal, 12);
                BindVertexAttribute(shader.InTexCoord, 24);
            }
        }

        protected override void SetProgramUniforms()
        {
            base.SetProgramUniforms();

            if (BoundProgram is TexturedShaderProgram program)
            {
                if (Texture != null)
                    program.Texture.BindTexture(TextureUnit, Texture);
            }
        }
    }
}
