using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public interface ITransformableElement
    {
        bool IsEditingTransform { get; }

        Matrix4 Transform { get; set; }

        void BeginEditTransform();

        void EndEditTransform(bool canceled);
    }
}
