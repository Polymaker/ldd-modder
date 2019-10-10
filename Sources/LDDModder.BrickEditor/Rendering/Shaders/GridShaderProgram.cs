using ObjectTK.Shaders.Variables;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using ObjectTK.Shaders.Sources;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    [SourceFile("LDDModder.BrickEditor.Resources.Shaders.GridShader.glsl", Embedded = true, SourceName = "GridShader")]
    [VertexShaderSource("GridShader.Vertex")]
    [FragmentShaderSource("GridShader.Fragment")]
    public class GridShaderProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib Position { get; protected set; }

        //public Uniform<bool> CenteredOrigin { get; protected set; }
        public Uniform<Color4> MajorGridColor { get; protected set; }
        public Uniform<Color4> MinorGridColor { get; protected set; }

        public float MajorLineSpacing
        {
            get => MajorSettings.Value.X;
            set
            {
                var val = MajorSettings.Value;
                MajorSettings.Set(new Vector3(value, val.Y, val.Z));
            }
        }

        public float MajorLineThickness
        {
            get => MajorSettings.Value.Y;
            set
            {
                var val = MajorSettings.Value;
                MajorSettings.Set(new Vector3(val.X, value, val.Z));
            }
        }

        public bool MajorLineOffcenter
        {
            get => MajorSettings.Value.Z > 0;
            set
            {
                var val = MajorSettings.Value;
                MajorSettings.Set(new Vector3(val.X, val.Y, value ? 1f : 0f));
            }
        }

        public float MinorLineSpacing
        {
            get => MinorSettings.Value.X;
            set
            {
                var val = MajorSettings.Value;
                MinorSettings.Set(new Vector3(value, val.Y, val.Z));
            }
        }

        public float MinorLineThickness
        {
            get => MinorSettings.Value.Y;
            set
            {
                var val = MinorSettings.Value;
                MinorSettings.Set(new Vector3(val.X, value, val.Z));
            }
        }

        public bool MinorLineOffcenter
        {
            get => MinorSettings.Value.Z > 0;
            set
            {
                var val = MinorSettings.Value;
                MinorSettings.Set(new Vector3(val.X, val.Y, value ? 1f : 0f));
            }
        }

        //public Uniform<float> MajorSpacing { get; protected set; }
        //public Uniform<float> MinorSpacing { get; protected set; }
        public Uniform<Matrix4> MVPMatrix { get; protected set; }

        public Uniform<Vector3> MajorSettings { get; protected set; }
        public Uniform<Vector3> MinorSettings { get; protected set; }



    }
}
