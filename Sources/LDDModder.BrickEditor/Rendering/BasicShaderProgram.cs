using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders;
using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.BasicShader.glsl", Embedded = true, SourceName = "BasicShader")]
    [VertexShaderSource("BasicShader.Vertex")]
    [GeometryShaderSource("BasicShader.Geometry")]
    [FragmentShaderSource("BasicShader.Fragment")]
    public class BasicShaderProgram : ObjectTK.Shaders.Program, IMeshShaderProgram
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }

        public Uniform<Vector3> LightPosition { get; protected set; }
        public Uniform<Matrix4> ModelMatrix { get; protected set; }
        public Uniform<Matrix4> ViewMatrix { get; protected set; }
        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public Uniform<Vector4> MaterialColor { get; protected set; }
        public Uniform<bool> DisplayWireframe { get; protected set; }
    }
}
