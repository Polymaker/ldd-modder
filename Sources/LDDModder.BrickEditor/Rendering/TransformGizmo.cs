using ObjectTK.Buffers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    public class TransformGizmo : IDisposable
    {
        public Matrix4 Transform { get; set; }
        public bool Visible { get; set; }

        public IndexedVertexBuffer<Vector3> VertexBuffer { get; private set; }

        private TranslationGizmoAxis[] Axes;

        private Vector2 DisplayScale;

        public BSphere BoundingSphere { get; private set; }

        public TransformGizmo()
        {
            Transform = Matrix4.Identity;
            DisplayScale = Vector2.One;
            Axes = new TranslationGizmoAxis[]
            {
                new TranslationGizmoAxis(GizmoAxis.X, Vector3.UnitX, new Color4(1f,0.3f,0.3f,1f)),
                new TranslationGizmoAxis(GizmoAxis.Y, Vector3.UnitY, new Color4(0.6f, 0.9f, 0, 1f)),
                new TranslationGizmoAxis(GizmoAxis.Z, Vector3.UnitZ, new Color4(0.18f,0.55f,1f,1f))
            };

            Axes[0].RotationMatrix = Matrix4.CreateRotationZ((float)Math.PI * -0.5f);
            Axes[2].RotationMatrix = Matrix4.CreateRotationX((float)Math.PI * 0.5f);
        }

        public enum GizmoAxis
        {
            None,
            X,
            Y,
            Z
        }

        class TranslationGizmoAxis
        {
            public Vector3 Direction { get; set; }

            public BBox BoundingBox { get; set; }

            public Color4 Color { get; set; }

            public Color4 InnactiveColor { get; set; }

            public bool IsOver { get; set; }

            public Matrix4 RotationMatrix { get; set; }

            public GizmoAxis Axis { get; }

            public TranslationGizmoAxis(GizmoAxis axis, Vector3 direction, Color4 color)
            {
                Axis = axis;
                Direction = direction;
                Color = color;
                RotationMatrix = Matrix4.Identity;
                InnactiveColor = new Color4(color.R * 0.9f, color.G * 0.9f, color.B * 0.9f, 0.75f);
            }
        }

        public void InitializeVertexBuffer()
        {
            VertexBuffer = new IndexedVertexBuffer<Vector3>();

            float stepAngle = (float)Math.PI * 2f / 32f;

            var indices = new List<int>();
            var vertices = new List<Vector3>();

            for (int i = 0; i < 32; i++)
            {
                var pt = new Vector3((float)Math.Cos(stepAngle * i), 0f, (float)Math.Sin(stepAngle * i)) * 0.5f;
                vertices.Add(pt);
                indices.Add(i);
                indices.Add((i + 1) % 32);
                
                indices.Add(32);
            }
            vertices.Add(Vector3.UnitY);
            VertexBuffer.SetIndices(indices);
            VertexBuffer.SetVertices(vertices);
        }

        public void Render(Camera camera)
        {
            var viewMatrix = camera.GetViewMatrix();
            var projection = camera.GetProjectionMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMatrix);

            var gizmoTrans = Transform;
            GL.MultMatrix(ref gizmoTrans);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(2f);

            var arrowScale = Matrix4.CreateScale(10 * DisplayScale.X, 20 * DisplayScale.Y, 10 * DisplayScale.X);
            var arrowTrans = Matrix4.CreateTranslation(Vector3.UnitY * DisplayScale.Y * 75);


            VertexBuffer.Bind();
            VertexBuffer.BindVertexBuffer();
            GL.VertexPointer(3, VertexPointerType.Float, 12, 0);

            for (int i = 0; i < 3; i++)
            {
                var axis = Axes[i];

                GL.Color4(axis.IsOver ? axis.Color : axis.InnactiveColor);

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Vector3.Zero);
                GL.Vertex3(axis.Direction * DisplayScale.Y * 75);
                GL.End();

                GL.Enable(EnableCap.VertexArray);

                GL.PushMatrix();


                var arrowMatrix = arrowScale * arrowTrans * axis.RotationMatrix;// axis.RotationMatrix * arrowTrans * arrowScale;
                GL.MultMatrix(ref arrowMatrix);
                VertexBuffer.DrawElements();
                GL.PopMatrix();
                GL.Disable(EnableCap.VertexArray);
            }

            GL.PopAttrib();
        }

        public bool RayIntersectsAxis(Ray ray, out GizmoAxis axis)
        {
            axis = GizmoAxis.None;

            if (RayIntersectsAxis(ray, out TranslationGizmoAxis gizmoAxis))
            {
                axis = gizmoAxis.Axis;
                return true;
            }
            return false;
        }

        private bool RayIntersectsAxis(Ray ray, out TranslationGizmoAxis hitAxis)
        {
            hitAxis = null;
            var localRay = Ray.Transform(ray, Transform.Inverted());
            float minDist = 99999f;

            for (int i = 0; i < 3; i++)
            {
                if (Ray.IntersectsBox(localRay, Axes[i].BoundingBox, out float hitDist))
                {
                    if (hitDist < minDist)
                    {
                        hitAxis = Axes[i];
                        minDist = hitDist;
                    }
                }
            }
            return hitAxis != null;
        }

        public void UpdateBoundingBoxes(Camera camera)
        {
            var gizmoPos = Vector3.TransformPosition(Vector3.Zero, Transform);
            var distFromCamera = camera.GetDistanceFromCamera(gizmoPos);
            var viewSize = camera.GetViewSize(distFromCamera);
            DisplayScale = new Vector2(viewSize.Y / camera.Viewport.Height, viewSize.X / camera.Viewport.Width);

            for (int i = 0; i < 3; i++)
            {
                var axis = Axes[i];
                var boxSize = new Vector3(DisplayScale.X * 10);
                boxSize = Vector3.ComponentMax(boxSize, axis.Direction * DisplayScale.Y * 95);
                axis.BoundingBox = BBox.FromCenterSize(axis.Direction * DisplayScale.Y * 95 * 0.5f, boxSize);
            }

            BoundingSphere = new BSphere(gizmoPos, DisplayScale.Y * 95);
        }

        public void PerformMouseOver(Ray mouseRay)
        {
            for (int i = 0; i < 3; i++)
                Axes[i].IsOver = false;

            if (RayIntersectsAxis(mouseRay, out TranslationGizmoAxis gizmoAxis))
                gizmoAxis.IsOver = true;
        }

        public void Dispose()
        {
            if (VertexBuffer != null)
            {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }
        }
    }
}
