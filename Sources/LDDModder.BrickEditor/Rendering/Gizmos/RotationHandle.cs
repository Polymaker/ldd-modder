using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public class RotationHandle : GizmoHandle
    {
        public override GizmoStyle HandleType => GizmoStyle.Rotation;

        public RotationHandle(Vector3 axis) : base(axis)
        {
            Plane = new Plane(Vector3.Zero, axis, 0f);
        }

        public override bool HitTest(Ray ray, out float distance)
        {
            distance = float.NaN;
            if (Ray.IntersectsPlane(ray, Plane, out float hitDist))
            {
                var hitPos = ray.Origin + ray.Direction * hitDist;
                float distanceFromCenter = (hitPos - Plane.Origin).Length;
                distance = Math.Abs(GizmoSize - distanceFromCenter); //distance from radius
                if (distance <= Tolerence)
                    return true;

            }
            return false;
        }

        public override Vector2 ProjectToPlane(Ray ray)
        {
            if (Ray.IntersectsPlane(ray, Plane, out float dist))
            {
                var hitPos = ray.Origin + ray.Direction * dist;
                var planeAxis = Axis == Vector3.UnitY ? Vector3.UnitX : Vector3.UnitY;
                return Plane.ProjectPoint2D(planeAxis, hitPos).Yx;
            }
            return base.ProjectToPlane(ray);
        }

        public override void RenderHandle(TransformGizmo gizmo, Vector4 color)
        {
            var scale = Matrix4.CreateScale(GizmoSize * 2f);
            var baseTransform = Orientation * gizmo.GetActiveTransform();

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(3.5f);

            RenderHelper.BeginDrawColor(gizmo.VertexBuffer, scale * baseTransform, color);
            gizmo.VertexBuffer.DrawArrays(PrimitiveType.LineLoop, 0, 32);

            GL.PopAttrib();
        }
    }
}
