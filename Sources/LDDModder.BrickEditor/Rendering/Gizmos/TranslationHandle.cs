using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public class TranslationHandle : GizmoHandle
    {
        public BBox BoundingBox { get; set; }

        public override GizmoStyle HandleType => GizmoStyle.Translation;

        public const float ArrowSize = 0.2666666666666667f;

        public TranslationHandle(Vector3 axis) : base(axis)
        {
            var diag = (new Vector3(1f) - axis).Normalized();
            Plane = new Plane(Vector3.Zero, diag, 0f);
        }

        public override void UpdateBounds()
        {
            var boxSize = new Vector3(Tolerence) * (Vector3.One - Axis);
            var boxLength = Axis * (GizmoSize * (1f + ArrowSize));
            boxSize += boxLength;

            BoundingBox = BBox.FromCenterSize(boxLength / 2f, boxSize);
        }

        public override bool HitTest(Ray ray, out float distance)
        {
            return Ray.IntersectsBox(ray, BoundingBox, out distance);
        }

        public override void RenderHandle(TransformGizmo gizmo, Vector4 color)
        {
            base.RenderHandle(gizmo, color);

            var scale = Matrix4.CreateScale(GizmoSize);
            var arrowSize = GizmoSize * ArrowSize;
            var arrowScale = Matrix4.CreateScale(arrowSize / 2f, arrowSize, arrowSize / 2f);
            var arrowTrans = arrowScale * Matrix4.CreateTranslation(0, GizmoSize, 0);

            var baseTransform = Orientation * gizmo.GetActiveTransform();

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(2.5f);

            RenderHelper.BeginDrawColor(gizmo.VertexBuffer, scale * baseTransform, color);
            gizmo.VertexBuffer.DrawArrays(PrimitiveType.Lines, 32, 2);

            GL.PopAttrib();

            RenderHelper.BeginDrawColor(gizmo.VertexBuffer, arrowTrans * baseTransform, color);
            gizmo.VertexBuffer.DrawElements(PrimitiveType.Triangles);

        }
    }
}
