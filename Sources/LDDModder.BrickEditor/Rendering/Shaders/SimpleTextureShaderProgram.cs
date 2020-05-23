using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using ObjectTK.Shaders.Sources;
using ObjectTK.Textures;


namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.SimpleTextureShader.glsl", Embedded = true, SourceName = "SimpleTextureShader")]
    [VertexShaderSource("SimpleTextureShader.Vertex")]
    [FragmentShaderSource("SimpleTextureShader.Fragment")]
    public class SimpleTextureShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib TexCoord { get; protected set; }

        public TextureUniform<Texture2D> Texture { get; protected set; }

        public Uniform<Matrix4> ModelMatrix { get; protected set; }

        public Uniform<Matrix4> ViewMatrix { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
