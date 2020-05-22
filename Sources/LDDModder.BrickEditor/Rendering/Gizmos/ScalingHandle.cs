using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public class ScalingHandle : GizmoHandle
    {
        public BBox BoundingBox { get; set; }

        public const float CubeSize = 0.1333333333333334f;

        public override GizmoStyle HandleType => GizmoStyle.Scaling;

        private Plane SecondaryPlane;

        public ScalingHandle(Vector3 axis) : base(axis)
        {
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
            var boxLength = Axis * (GizmoSize * (1f + CubeSize));
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

            var editOffset = 0f;
            var renderScale = new Vector3(GizmoSize);

            if (gizmo.IsEditing && IsSelected)
            {
                editOffset = gizmo.TransformedAmount / 2f; 
                renderScale += new Vector3(0, editOffset, 0);
            }

            var scaleMatrix = Matrix4.CreateScale(renderScale);
            var cubeRenderSize = GizmoSize * CubeSize;

            if (gizmo.IsEditing)
                cubeRenderSize *= IsSelected ? 1.25f : 0.75f;

            var cubeScale = Matrix4.CreateScale(cubeRenderSize);
            var cubeTrans = cubeScale * Matrix4.CreateTranslation(0, GizmoSize + editOffset, 0);

            var baseTransform = Orientation * gizmo.GetActiveTransform();

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(outlined ? 4.5f : 2.5f);

            RenderHelper.BeginDrawColor(gizmo.VertexBuffer, scaleMatrix * baseTransform, color);
            gizmo.VertexBuffer.DrawArrays(PrimitiveType.Lines, 32, 2);

            GL.PopAttrib();


            if (outlined)
            {
                RenderHelper.BeginDrawWireframe(ModelManager.CubeModel.VertexBuffer, cubeTrans * baseTransform, 2f, color);
                ModelManager.CubeModel.DrawElements();
                //gizmo.VertexBuffer.DrawElements(PrimitiveType.Triangles);
                RenderHelper.EndDrawWireframe(gizmo.VertexBuffer);
            }
            else
            {
                RenderHelper.BeginDrawColor(ModelManager.CubeModel.VertexBuffer, cubeTrans * baseTransform, color);
                ModelManager.CubeModel.DrawElements();
                //gizmo.VertexBuffer.DrawElements(PrimitiveType.Triangles);
            }


        }
    }
}
