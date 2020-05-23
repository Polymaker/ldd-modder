using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using ObjectTK.Shaders.Sources;
using ObjectTK.Textures;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.UIShader.glsl", Embedded = true, SourceName = "UIShader")]
    [VertexShaderSource("UIShader.Vertex")]
    [FragmentShaderSource("UIShader.Fragment")]
    public class UIShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib TexCoord { get; protected set; }

        public Uniform<float> Opacity { get; protected set; }

        public TextureUniform<Texture2D> Texture { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
