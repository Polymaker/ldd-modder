using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using ObjectTK.Shaders.Sources;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.ColorShader.glsl", Embedded = true, SourceName = "ColorShader")]
    [VertexShaderSource("ColorShader.Vertex")]
    [FragmentShaderSource("ColorShader.Fragment")]
    public class ColorShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }

        public Uniform<Vector4> Color { get; protected set; }

        public Uniform<Matrix4> ModelMatrix { get; protected set; }

        public Uniform<Matrix4> ViewMatrix { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
