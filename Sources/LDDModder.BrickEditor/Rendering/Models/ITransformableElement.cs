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

        BBox BoundingBox { get; }

        Vector3 Origin { get; }

        bool Visible { get; }

        event EventHandler TransformChanged;

        event EventHandler VisibilityChanged;

        Matrix4 GetBaseTranform();

        void BeginEditTransform();

        void ApplyTransform(Matrix4 transform);

        void EndEditTransform(bool canceled);

        BBox GetWorldBoundingBox();
    }
}
