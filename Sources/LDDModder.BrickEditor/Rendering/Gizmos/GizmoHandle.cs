using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public abstract class GizmoHandle
    {
        public Matrix4 Orientation { get; private set; }

        public Vector3 Axis { get; private set; }

        public int Index { get; private set; }

        public Plane Plane { get; set; }

        /// <summary>
        /// Gizmo size in world space.
        /// </summary>
        public float GizmoSize { get; set; }

        public float Tolerence { get; set; }

        public bool IsOver { get; set; }

        public abstract GizmoStyle HandleType { get; }

        public abstract bool HitTest(Ray ray, out float distance);

        protected GizmoHandle(Vector3 axis)
        {
            Axis = axis;
            
            if (axis.X == 1)
            {
                Index = 0;
                Orientation = Matrix4.CreateRotationZ((float)Math.PI * -0.5f);
            }
            else if (axis.Y == 1)
            {
                Index = 1;
                Orientation = Matrix4.Identity;
            }
            else if (axis.Z == 1)
            {
                Index = 2;
                Orientation = Matrix4.CreateRotationX((float)Math.PI * 0.5f);
            }
        }

        public virtual void UpdateBounds() { }

        public virtual void RenderHandle(TransformGizmo gizmo, Vector4 color)
        {

        }

        public virtual Vector2 ProjectToPlane2D(Ray ray)
        {
            if (Ray.IntersectsPlane(ray, Plane, out float dist))
            {
                var hitPos = ray.Origin + ray.Direction * dist;
                return Plane.ProjectPoint2D(Axis, hitPos).Yx;
            }

            return Vector2.Zero;
        }

        public virtual Vector3 ProjectToPlane(Ray ray)
        {
            if (Ray.IntersectsPlane(ray, Plane, out float dist))
            {
                var hitPos = ray.Origin + ray.Direction * dist;
                return hitPos;
            }
            return Vector3.Zero;
        }
    }
}
