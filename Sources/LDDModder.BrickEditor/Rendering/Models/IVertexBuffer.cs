using ObjectTK.Buffers;
using ObjectTK.Shaders.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public interface IVertexBuffer
    {
        Buffer<int> IndexBuffer { get; }

        void Bind();

        void BindAttribute(VertexAttrib attribute, int offset);

        void UnbindAttribute(VertexAttrib attribute);
    }
}
