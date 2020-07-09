using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL;
using ObjectTK.Shaders.Sources;
using OpenTK;
using OpenTK.Graphics;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.WireframeShader2.glsl", Embedded = true, SourceName = "WireframeShader2")]
    [VertexShaderSource("WireframeShader2.Vertex")]
    [GeometryShaderSource("WireframeShader2.Geometry")]
    [FragmentShaderSource("WireframeShader2.Fragment")]
    public class WireframeShader2Program : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }

        public Uniform<Vector4> Color { get; protected set; }

        public Uniform<float> Size { get; protected set; }

        public Uniform<Matrix4> ModelMatrix { get; protected set; }

        public Uniform<Matrix4> ViewMatrix { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
