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

        public GizmoStyle DisplayStyle { get; set; }

        public IndexedVertexBuffer<Vector3> VertexBuffer { get; private set; }

        private TranslationManipulator[] TranslationAxes;

        private RotationManipulator[] RotationAxes;
        
        public float GizmoSize { get; set; }

        private float DisplayScale { get; set; }


        public BSphere BoundingSphere { get; private set; }

        public bool IsMouseOver { get; private set; }

        public bool IsSelected { get; set; }

        public bool IsDragging { get; set; }

        public TransformGizmo()
        {
            Transform = Matrix4.Identity;
            DisplayScale = 1f;
            GizmoSize = 75f;
            DisplayStyle = GizmoStyle.Translation;
            InitializeManipulators();
        }

        public void InitializeManipulators()
        {
            TranslationAxes = new TranslationManipulator[]
            {
                new TranslationManipulator(GizmoAxis.X, new Color4(1f,0.09f,0.26f,1f)),
                new TranslationManipulator(GizmoAxis.Y, new Color4(0.58f, 0.898f, 0.156f, 1f)),
                new TranslationManipulator(GizmoAxis.Z, new Color4(0.156f,0.564f,1f,1f))
            };

            RotationAxes = new RotationManipulator[]
            {
                new RotationManipulator(GizmoAxis.X, new Color4(1f,0.09f,0.26f,1f)),
                new RotationManipulator(GizmoAxis.Y, new Color4(0.58f, 0.898f, 0.156f, 1f)),
                new RotationManipulator(GizmoAxis.Z, new Color4(0.156f,0.564f,1f,1f))
            };
        }


        public enum GizmoAxis
        {
            None,
            X,
            Y,
            Z
        }

        public enum GizmoStyle
        {
            Translation,
            Rotation
        }

        abstract class GizmoAxisManipulator
        {
            public Vector3 Direction { get; set; }

            public Color4 Color { get; set; }

            public Color4 InnactiveColor { get; set; }

            public GizmoAxis Axis { get; protected set; }

            public Matrix4 AxisRotation { get; set; }

            public bool IsOver { get; set; }

            public bool IsSelected { get; set; }

            protected GizmoAxisManipulator(GizmoAxis axis, Color4 color)
            {
                Axis = axis;
                Color = color;
                InnactiveColor = new Color4(color.R * 0.95f, color.G * 0.95f, color.B * 0.95f, 0.8f);

                switch (axis)
                {
                    case GizmoAxis.X:
                        AxisRotation = Matrix4.CreateRotationZ((float)Math.PI * -0.5f);
                        Direction = Vector3.UnitX;
                        break;
                    case GizmoAxis.Y:
                        AxisRotation = Matrix4.Identity;
                        Direction = Vector3.UnitY;
                        break;
                    case GizmoAxis.Z:
                        AxisRotation = Matrix4.CreateRotationX((float)Math.PI * 0.5f);
                        Direction = Vector3.UnitZ;
                        break;
                }
            }

            public abstract bool Hittest(Ray ray, out float distance);
        }

        class TranslationManipulator : GizmoAxisManipulator
        {
            public BBox BoundingBox { get; set; }

            public TranslationManipulator(GizmoAxis axis, Color4 color) : base(axis, color)
            {
                
            }

            public override bool Hittest(Ray ray, out float distance)
            {
                return Ray.IntersectsBox(ray, BoundingBox, out distance);
            }
        }

        class RotationManipulator : GizmoAxisManipulator
        {
            public Plane Plane { get; set; }

            public float GizmoRadius { get; set; }

            public float Tolerence { get; set; }

            public RotationManipulator(GizmoAxis axis, Color4 color) : base(axis, color)
            {
                Plane = new Plane()
                {
                    Normal = Direction
                };
            }

            public override bool Hittest(Ray ray, out float distance)
            {
                if (Ray.IntersectsPlane(ray, Plane, out distance))
                {
                    var hitPos = ray.Origin + ray.Direction * distance;
                    var v = Vector3.Dot(hitPos, hitPos);
                    distance = (float)Math.Sqrt(v);
                    distance = Math.Abs(GizmoRadius - distance);

                    if (distance <= Tolerence)
                        return true;
                }
                return false;
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
                
                indices.Add((i + 1) % 32);
                indices.Add(i);
                indices.Add(32);
            }
            vertices.Add(Vector3.UnitY);//cone top

            //for drawing a line
            vertices.Add(Vector3.Zero);
            indices.Add(33); indices.Add(32);

            VertexBuffer.SetIndices(indices);
            VertexBuffer.SetVertices(vertices);
        }


        public bool RayIntersectsAxis(Ray ray, out GizmoAxis axis)
        {
            axis = GizmoAxis.None;

            if (RayIntersectsAxis(ray, out GizmoAxisManipulator gizmoAxis))
            {
                axis = gizmoAxis.Axis;
                return true;
            }
            return false;
        }

        private bool RayIntersectsAxis(Ray ray, out GizmoAxisManipulator hitAxis)
        {
            hitAxis = null;
            var localRay = Ray.Transform(ray, Transform.Inverted());
            float minDist = 99999f;

            for (int i = 0; i < 3; i++)
            {
                var axis = (DisplayStyle == GizmoStyle.Translation) ? 
                    (GizmoAxisManipulator)TranslationAxes[i] : RotationAxes[i];

                if (axis.Hittest(localRay, out float hitDist))
                {
                    if (hitDist < minDist)
                    {
                        hitAxis = axis;
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
            DisplayScale = viewSize.Y / camera.Viewport.Height;

            float scaledGizmoSize = DisplayScale * GizmoSize;
            float arrowSize = scaledGizmoSize * 0.2666666666666667f;
            float handleLength = scaledGizmoSize + arrowSize;

            for (int i = 0; i < 3; i++)
            {
                var transAxis = TranslationAxes[i];
                var boxSize = new Vector3(DisplayScale * 10);

                boxSize = Vector3.ComponentMax(boxSize, transAxis.Direction * handleLength);

                transAxis.BoundingBox = BBox.FromCenterSize(transAxis.Direction * handleLength * 0.5f, boxSize);

                RotationAxes[i].GizmoRadius = scaledGizmoSize;
                RotationAxes[i].Tolerence = 10 * DisplayScale;
            }

            BoundingSphere = new BSphere(gizmoPos, handleLength);
        }

        public void PerformMouseOver(Ray mouseRay)
        {
            for (int i = 0; i < 3; i++)
            {
                TranslationAxes[i].IsOver = false;
                RotationAxes[i].IsOver = false;
            }
            IsMouseOver = false;

            if (RayIntersectsAxis(mouseRay, out GizmoAxisManipulator gizmoAxis))
            {
                gizmoAxis.IsOver = true;
                IsMouseOver = true;
            }
        }

        #region Rendering

        public void Render()
        {
            GL.PushMatrix();
            var gizmoTrans = Transform;
            GL.MultMatrix(ref gizmoTrans);

            float scaledGizmoSize = DisplayScale * GizmoSize;
            float arrowSize = scaledGizmoSize * 0.2666666666666667f;

            var gizmoScale = Matrix4.CreateScale(scaledGizmoSize * 2);

            var arrowScale = Matrix4.CreateScale(arrowSize /2f, arrowSize, arrowSize / 2f);
            var arrowTrans = Matrix4.CreateTranslation(Vector3.UnitY * scaledGizmoSize);
            var arrowTransform = arrowScale * arrowTrans;

            RenderHelper.EnableStencilTest();

            VertexBuffer.BindVertexPointer();
            GL.VertexPointer(3, VertexPointerType.Float, 12, 0);

            for (int i = 0; i < 3; i++)
            {
                var currentAxis = DisplayStyle == GizmoStyle.Translation ? 
                    (GizmoAxisManipulator)TranslationAxes[i] : RotationAxes[i];

                if (currentAxis.IsOver)
                    RenderHelper.EnableStencilMask();

                if (DisplayStyle == GizmoStyle.Translation)
                    RenderGizmoAxis(TranslationAxes[i], arrowTransform);
                else
                    RenderGizmoAxis(RotationAxes[i], gizmoScale);

                if (currentAxis.IsOver)
                {
                    RenderHelper.ApplyStencilMask();
                    if (DisplayStyle == GizmoStyle.Translation)
                        RenderGizmoAxis(TranslationAxes[i], arrowTransform, true);
                    else
                        RenderGizmoAxis(RotationAxes[i], gizmoScale, true);
                    RenderHelper.RemoveStencilMask();
                }
            }

            GL.PopMatrix();
            RenderHelper.DisableStencilTest();
        }

        private void RenderGizmoAxis(TranslationManipulator gizmoAxis, Matrix4 transform, bool outlined = false)
        {
            var arrowMatrix = transform * gizmoAxis.AxisRotation;
            var axisColor = gizmoAxis.IsOver ? gizmoAxis.Color : gizmoAxis.InnactiveColor;

            if (outlined)
                axisColor = new Color4(1f, 1f, 1f, 1f);

            GL.Color4(axisColor);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(outlined ? 3.5f : 2f);

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(Vector3.Zero);
            GL.Vertex3(gizmoAxis.Direction * DisplayScale * 75);
            GL.End();

            GL.PopAttrib();

            if (outlined)
            {
                GL.PushAttrib(AttribMask.LineBit);
                GL.LineWidth(1.5f);
            }

            GL.PushMatrix();
            GL.MultMatrix(ref arrowMatrix);

            GL.Enable(EnableCap.VertexArray);
            VertexBuffer.DrawElements(outlined ? PrimitiveType.LineLoop : PrimitiveType.Triangles);
            GL.Disable(EnableCap.VertexArray);

            GL.PopMatrix();
            if (outlined)
                GL.PopAttrib();
        }

        private void RenderGizmoAxis(RotationManipulator gizmoAxis, Matrix4 transform, bool outlined = false)
        {
            var axisColor = gizmoAxis.IsOver ? gizmoAxis.Color : gizmoAxis.InnactiveColor;

            if (outlined)
                axisColor = new Color4(1f, 1f, 1f, 1f);

            GL.Color4(axisColor);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(outlined ? 4f : 2.5f);

            var finalMatrix = transform * gizmoAxis.AxisRotation;

            GL.PushMatrix();
            GL.MultMatrix(ref finalMatrix);

            GL.Enable(EnableCap.VertexArray);
            VertexBuffer.DrawArrays(PrimitiveType.LineLoop, 0, 32);
            GL.Disable(EnableCap.VertexArray);

            GL.PopMatrix();

            GL.PopAttrib();
        }

        #endregion



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
