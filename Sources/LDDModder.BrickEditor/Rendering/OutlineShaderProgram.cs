using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.BrickOutlineShader.glsl", Embedded = true, SourceName = "BrickOutlineShader")]
    [VertexShaderSource("BrickOutlineShader.Vertex")]
    [FragmentShaderSource("BrickOutlineShader.Fragment")]
    public class OutlineShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib vRECoord0 { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib vRECoord1 { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib vRECoord2 { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib vRECoord3 { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib vRECoord4 { get; protected set; }
        [VertexAttrib(2, VertexAttribPointerType.Float)]
        public VertexAttrib vRECoord5 { get; protected set; }
        //[VertexAttrib(2, VertexAttribPointerType.Float)]
        //public VertexAttrib OutlineDataIndex { get; protected set; }

        public Uniform<Matrix4> MVPMatrix { get; protected set; }
        public Uniform<Color4> MaterialColor { get; protected set; }

        public Uniform<int> PairToDisplay { get; protected set; }

        //public TextureUniform<Texture2D> PackedRoundEdgeDataTexture { get; protected set; }

        public TextureUniform<Texture2D> Texture0 { get; protected set; }

        public TextureUniform<Texture2D> Texture1 { get; protected set; }

        public TextureUniform<Texture2D> Texture2 { get; protected set; }
    }
}
