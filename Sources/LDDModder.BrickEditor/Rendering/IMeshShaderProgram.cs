using ObjectTK.Shaders.Variables;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public interface IMeshShaderProgram
    {
        Uniform<Vector3> LightPosition { get; }

        Uniform<Matrix4> ModelMatrix { get; }

        Uniform<Matrix4> ViewMatrix { get; }

        Uniform<Matrix4> ModelViewProjectionMatrix { get; }

        Uniform<Vector4> MaterialColor { get; }

        Uniform<bool> DisplayWireframe { get; }
    }
}
