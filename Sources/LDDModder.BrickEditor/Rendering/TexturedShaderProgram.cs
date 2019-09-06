using ObjectTK.Shaders.Variables;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using ObjectTK.Shaders.Sources;
using ObjectTK.Textures;

namespace LDDModder.BrickEditor.Rendering
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.TexturedShader.glsl", Embedded = true, SourceName = "TexturedShader")]
    [VertexShaderSource("TexturedShader.Vertex")]
    [GeometryShaderSource("TexturedShader.Geometry")]
    [FragmentShaderSource("TexturedShader.Fragment")]
    public class TexturedShaderProgram : ObjectTK.Shaders.Program, IMeshShaderProgram
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib InTexCoord { get; protected set; }

        public Uniform<Vector3> LightPosition { get; protected set; }
        public Uniform<Matrix4> ModelMatrix { get; protected set; }
        public Uniform<Matrix4> ViewMatrix { get; protected set; }
        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public Uniform<Vector4> MaterialColor { get; protected set; }
        public TextureUniform<Texture2D> Texture { get; protected set; }
        public Uniform<bool> DisplayWireframe { get; protected set; }
    }
}
