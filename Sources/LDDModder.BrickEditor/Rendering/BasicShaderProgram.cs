using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders;
using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    [VertexShaderSource("BasicShader.Vertex")]
    [FragmentShaderSource("BasicShader.Fragment")]
    public class BasicShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public Uniform<Vector4> MaterialColor { get; protected set; }
    }
}
