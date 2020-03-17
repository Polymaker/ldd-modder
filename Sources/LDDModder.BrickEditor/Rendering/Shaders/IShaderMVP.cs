using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Shaders
{
    public interface IShaderMVP
    {
        void SetMatrices(Matrix4 v, Matrix4 p);
        void SetMatrices(Matrix4 m, Matrix4 v, Matrix4 p);
    }
}
