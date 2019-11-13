using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL;
using ObjectTK.Shaders.Sources;
using OpenTK;
using OpenTK.Graphics;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.WireframeShader.glsl", Embedded = true, SourceName = "WireframeShader")]
    [VertexShaderSource("WireframeShader.Vertex")]
    [GeometryShaderSource("WireframeShader.Geometry")]
    [FragmentShaderSource("WireframeShader.Fragment")]
    public class WireframeShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Normal { get; protected set; }

        public Uniform<float> Thickness { get; protected set; }

        public Uniform<Vector4> Color { get; protected set; }

        public Uniform<Matrix4> ModelMatrix { get; protected set; }

        public Uniform<Matrix4> ViewMatrix { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
