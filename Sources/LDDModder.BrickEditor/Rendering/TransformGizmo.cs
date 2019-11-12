using ObjectTK.Buffers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class TransformGizmo
    {
        public Matrix4 Transform { get; set; }
        public bool Visible { get; set; }

        public GizmoAxis SelectedAxis { get; set; }

        public enum GizmoAxis
        {   None,
            X,
            Y,
            Z
        }

        public enum GizmoStyle
        {
            Plain,
            Translation,
            Rotation,
            Scaling
        }

        public void Draw()
        {
            
        }

        public bool RayIntersectsAxis(Ray ray, Camera camera)
        {
            var gizmoPos = Vector3.TransformPosition(Vector3.Zero, Transform);
            var localRay = Ray.Transform(ray, Transform.Inverted());
            var distFromCamera = camera.GetDistanceFromCamera(gizmoPos);
            var viewSize = camera.GetViewSize(distFromCamera);

            float axisLen = 80 * viewSize.Y / camera.Viewport.Height;
            float axisWidth = 10 * viewSize.X / camera.Viewport.Width;

            var upBox = BBox.FromCenterSize(Vector3.UnitY * axisLen * 0.5f, 
                new Vector3(axisWidth,axisLen, axisWidth));

            if (Ray.IntersectsBox(localRay, upBox, out float hitDist))
            {

            }
            return false;
        }

        public void DrawGizmo(GizmoStyle style, Matrix4 transform)
        {

        }
    }
}
