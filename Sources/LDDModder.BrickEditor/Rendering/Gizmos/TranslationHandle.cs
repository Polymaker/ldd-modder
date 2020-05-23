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

        private Plane SecondaryPlane;

        public TranslationHandle(Vector3 axis) : base(axis)
        {
            //var diag = (new Vector3(1f) - axis).Normalized();
            //Plane = new Plane(Vector3.Zero, diag, 0f);

            if (axis == Vector3.UnitX)
            {
                Plane = new Plane(Vector3.Zero, Vector3.UnitZ, 0f);
                SecondaryPlane = new Plane(Vector3.Zero, Vector3.UnitY, 0f);
            }
            else if (axis == Vector3.UnitZ)
            {
                Plane = new Plane(Vector3.Zero, Vector3.UnitX, 0f);
                SecondaryPlane = new Plane(Vector3.Zero, Vector3.UnitY, 0f);
            }
            else if (axis == Vector3.UnitY)
            {
                Plane = new Plane(Vector3.Zero, Vector3.UnitX, 0f);
                SecondaryPlane = new Plane(Vector3.Zero, Vector3.UnitZ, 0f);
            }
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

        public override Vector2 ProjectToPlane2D(Ray ray)
        {
            bool hit1 = Ray.IntersectsPlane(ray, Plane, out float dist1);
            bool hit2 = Ray.IntersectsPlane(ray, SecondaryPlane, out float dist2);
            if ((hit1 && !hit2) || (hit1 && hit2 && dist1 < dist2))
            {
                var hitPos = ray.Origin + ray.Direction * dist1;
                return Plane.ProjectPoint2D(Axis, hitPos).Yx;
            }
            else if (hit2)
            {
                var hitPos = ray.Origin + ray.Direction * dist2;
                return SecondaryPlane.ProjectPoint2D(Axis, hitPos).Yx;
            }
            return base.ProjectToPlane2D(ray);
        }

        public override Vector3 ProjectToPlane(Ray ray)
        {
            bool hit1 = Ray.IntersectsPlane(ray, Plane, out float dist1);
            bool hit2 = Ray.IntersectsPlane(ray, SecondaryPlane, out float dist2);

            if ((hit1 && !hit2) || (hit1 && hit2 && dist1 < dist2))
            {
                var hitPos = ray.Origin + ray.Direction * dist1;
                return Plane.ProjectPoint(hitPos);
            }
            else if (hit2)
            {
                var hitPos = ray.Origin + ray.Direction * dist2;
                return SecondaryPlane.ProjectPoint(hitPos);
            }

            return base.ProjectToPlane(ray);
        }

        public override void RenderHandle(TransformGizmo gizmo, Vector4 color, bool outlined = false)
        {
            base.RenderHandle(gizmo, color);

            if (gizmo.IsEditing && !IsSelected)
                return;

            var scale = Matrix4.CreateScale(GizmoSize);
            var arrowSize = GizmoSize * ArrowSize;
            var arrowScale = Matrix4.CreateScale(arrowSize / 2f, arrowSize, arrowSize / 2f);
            var arrowTrans = arrowScale * Matrix4.CreateTranslation(0, GizmoSize, 0);

            var baseTransform = Orientation * gizmo.GetActiveTransform();

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(outlined ? 4.5f : 2.5f);

            RenderHelper.BeginDrawColor(gizmo.VertexBuffer, scale * baseTransform, color);
            gizmo.VertexBuffer.DrawArrays(PrimitiveType.Lines, 32, 2);
            
            GL.PopAttrib();

            
            if (outlined)
            {
                RenderHelper.BeginDrawWireframe(gizmo.VertexBuffer, arrowTrans * baseTransform, 2f, color);
                gizmo.VertexBuffer.DrawElements(PrimitiveType.Triangles);
                RenderHelper.EndDrawWireframe(gizmo.VertexBuffer);
            }
            else
            {
                RenderHelper.BeginDrawColor(gizmo.VertexBuffer, arrowTrans * baseTransform, color);
                gizmo.VertexBuffer.DrawElements(PrimitiveType.Triangles);
            }
            

        }
    }
}
