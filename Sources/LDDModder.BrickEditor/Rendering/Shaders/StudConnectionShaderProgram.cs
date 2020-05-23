using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using ObjectTK.Shaders.Sources;
using ObjectTK.Textures;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.StudConnectionShader.glsl", Embedded = true, SourceName = "StudConnectionShader")]
    [VertexShaderSource("StudConnectionShader.Vertex")]
    [GeometryShaderSource("StudConnectionShader.Geometry")]
    [FragmentShaderSource("StudConnectionShader.Fragment")]
    public class StudConnectionShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Values { get; protected set; }

        public TextureUniform<Texture2D> Texture { get; protected set; }

        public Uniform<Vector2> CellSize { get; protected set; }

        public Uniform<bool> IsMale { get; protected set; }

        public Uniform<Matrix4> ModelMatrix { get; protected set; }

        public Uniform<Matrix4> ViewMatrix { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
