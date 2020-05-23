using ObjectTK.Shaders.Variables;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ObjectTK.Shaders.Sources;
using ObjectTK.Textures;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.ModelShader.glsl", Embedded = true, SourceName = "ModelShader")]
    [VertexShaderSource("ModelShader.Vertex")]
    [FragmentShaderSource("ModelShader.Fragment")]
    public class ModelShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Normal { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib TexCoord { get; protected set; }

        public Uniform<Vector3> ViewPosition { get; protected set; }

        public ArrayUniform<LightInfo> Lights { get; protected set; }

        public Uniform<int> LightCount { get; protected set; }

        public StructUniform<MaterialInfo> Material { get; protected set; }

        public Uniform<bool> UseTexture { get; protected set; }

        public Uniform<bool> IsSelected { get; protected set; }

        public TextureUniform<Texture2D> Texture { get; protected set; }

        public Uniform<Matrix4> ModelMatrix { get; protected set; }

        public Uniform<Matrix4> ViewMatrix { get; protected set; }

        public Uniform<Matrix4> Projection { get; protected set; }
    }
}
