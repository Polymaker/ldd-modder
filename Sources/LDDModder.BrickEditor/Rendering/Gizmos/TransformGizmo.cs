using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public class TransformGizmo : IDisposable
    {
        private RotationHandle[] RotationHandles;
        private TranslationHandle[] TranslationHandles;

        private float _GizmoSize;
        private Matrix4 _Transform;
        private bool isDirty;

        /// <summary>
        /// Gizmo size in pixels.
        /// </summary>
        public float GizmoSize
        {
            get => _GizmoSize;
            set
            {
                if (_GizmoSize != value)
                {
                    _GizmoSize = value;
                    isDirty = true;
                }
            }
        }

        public Matrix4 Transform
        {
            get => _Transform;
            set
            {
                if (_Transform != value)
                {
                    _Transform = value;
                    isDirty = true;
                }
            }
        }

        public Matrix4 EditTransform { get; private set; }

        public float UIScale { get; private set; }

        public bool Visible { get; set; }

        public bool IsOver { get; private set; }

        public GizmoHandle SelectedHandle { get; private set; }

        public bool Selected => SelectedHandle != null;

        public bool IsEditing { get; private set; }

        public GizmoStyle CurrentMode { get; set; }

        public BSphere BoundingSphere { get; private set; }

        public Vector4[] HandleColors { get; set; }

        public IndexedVertexBuffer<Vector3> VertexBuffer { get; private set; }

        public TransformGizmo()
        {
            _GizmoSize = 75f;
            _Transform = Matrix4.Identity;
            EditTransform = Matrix4.Identity;

            CurrentMode = GizmoStyle.Translation;
            
            HandleColors = new Vector4[]
            {
                new Vector4(1f,0.09f,0.26f,1f),
                new Vector4(0.58f, 0.898f, 0.156f, 1f),
                new Vector4(0.156f,0.564f,1f,1f)
            };

            InitializeHandles();
        }

        private void InitializeHandles()
        {
            TranslationHandles = new TranslationHandle[3];
            RotationHandles = new RotationHandle[3];
            for (int i = 0; i < 3; i++)
            {
                var axis = new Vector3(i == 0 ? 1 : 0, i == 1 ? 1 : 0, i == 2 ? 1 : 0);
                TranslationHandles[i] = new TranslationHandle(axis);
                RotationHandles[i] = new RotationHandle(axis);
            }
        }

        public void InitializeVBO()
        {
            VertexBuffer = new IndexedVertexBuffer<Vector3>();

            float stepAngle = (float)Math.PI * 2f / 32f;

            var indices = new List<int>();
            var vertices = new List<Vector3>();

            for (int i = 0; i < 32; i++)
            {
                var pt = new Vector3((float)Math.Cos(stepAngle * i), 0f, (float)Math.Sin(stepAngle * i)) * 0.5f;
                vertices.Add(pt);
                indices.Add((i + 1) % 32); indices.Add(i); indices.Add(32);
            }

            vertices.Add(Vector3.UnitY);//cone top

            //for drawing a line
            vertices.Add(Vector3.Zero);
            //indices.Add(33); indices.Add(32);

            VertexBuffer.SetIndices(indices);
            VertexBuffer.SetVertices(vertices);
        }

        public Vector3 GetPosition()
        {
            return Vector3.TransformPosition(Vector3.Zero, Transform);
        }

        public void UpdateBounds(Camera camera)
        {
            var position = GetPosition();
            var distFromCamera = camera.GetDistanceFromCamera(position);
            var viewSize = camera.GetViewSize(distFromCamera);

            UIScale = viewSize.Y / camera.Viewport.Height;
            
            float scaledGizmoSize = GizmoSize * UIScale;
            float padding = scaledGizmoSize * 0.3f;

            for (int i = 0; i < 3; i++)
            {
                var transAxis = TranslationHandles[i];
                transAxis.GizmoSize = scaledGizmoSize;
                transAxis.Tolerence = UIScale * 10f; //10 pixel
                transAxis.UpdateBounds();

                var rotAxis = RotationHandles[i];
                rotAxis.GizmoSize = scaledGizmoSize;
                rotAxis.Tolerence = UIScale * 10f; //10 pixel
            }

            BoundingSphere = new BSphere(position, scaledGizmoSize + padding);
            isDirty = false;
        }

        private GizmoHandle GetHandle(int index)
        {
            if (CurrentMode == GizmoStyle.Translation)
                return TranslationHandles[index];
            else if (CurrentMode == GizmoStyle.Rotation)
                return RotationHandles[index];
            return null;
        }

        private GizmoHandle GetHoveredHandle()
        {
            for (int i = 0; i < 3; i++)
            {
                var gizmoHandle = GetHandle(i);
                if (gizmoHandle?.IsOver ?? false)
                    return gizmoHandle;
            }
            return null;
        }

        public bool IsOverGizmo(Ray ray)
        {
            return Ray.IntersectsSphere(ray, BoundingSphere, out _);
        }

        public bool IsOverHandle(Ray ray, out GizmoHandle handle)
        {
            handle = null;
            if (isDirty)
                return false;

            var localRay = Ray.Transform(ray, Transform.Inverted());
            float closestDist = 99999f;

            for (int i = 0; i < 3; i++)
            {
                var gizmoHandle = GetHandle(i);
                if (gizmoHandle != null && gizmoHandle.HitTest(localRay, out float hitDist))
                {
                    if (hitDist < closestDist)
                    {
                        closestDist = hitDist;
                        handle = gizmoHandle;
                    }
                }
            }

            return handle != null;
        }

        private void ClearSelection()
        {
            for (int i = 0; i < 3; i++)
            {
                var transAxis = TranslationHandles[i];
                transAxis.IsOver = false;

                var rotAxis = RotationHandles[i];
                rotAxis.IsOver = false;
            }
            IsOver = false;
        }

        public void PerformMouseOver(Ray ray)
        {
            ClearSelection();
            if (IsOverHandle(ray, out GizmoHandle handle))
            {
                handle.IsOver = true;
                IsOver = true;
            }
        }

        public void ProcessInput(Camera camera, InputManager input)
        {
            if (!Visible)
                return;

            if (!(IsEditing || Selected))
            {
                if (input.IsKeyPressed(OpenTK.Input.Key.R)
                    && CurrentMode != GizmoStyle.Rotation)
                {
                    CurrentMode = GizmoStyle.Rotation;
                    ClearSelection();
                }
                else if (input.IsKeyPressed(OpenTK.Input.Key.T)
                    && CurrentMode != GizmoStyle.Translation)
                {
                    CurrentMode = GizmoStyle.Translation;
                    ClearSelection();
                }
            }

            if (IsEditing && input.IsKeyPressed(OpenTK.Input.Key.Escape))
            {
                CancelEdit();
                SelectedHandle = null;
                ClearSelection();
            }

            var mouseDelta = input.MousePos - input.LastMousePos;

            if (mouseDelta.LengthFast > 1)
            {
                var mouseRay = camera.RaycastFromScreen(input.LocalMousePos);

                if (!Selected && IsOverGizmo(mouseRay))
                {
                    PerformMouseOver(mouseRay);
                }
                else if (Selected && !IsEditing)
                {
                    BeginEditGizmo(mouseRay);
                }
                else if (IsEditing)
                {
                    var localRay = Ray.Transform(mouseRay, Transform.Inverted());
                    var curEditPos = SelectedHandle.ProjectToPlane(localRay);

                    if (CurrentMode == GizmoStyle.Translation)
                    {
                        var delta = curEditPos - EditStartPos;
                        EditTransform = Matrix4.CreateTranslation(SelectedHandle.Axis * delta.Y);
                    }
                    else if (CurrentMode == GizmoStyle.Rotation)
                    {
                        var v1 = new Vector3(EditStartPos).Normalized();
                        var v2 = new Vector3(curEditPos).Normalized();
                        var angle = Vector3.CalculateAngle(v1, v2);
                        EditTransform = Matrix4.CreateFromAxisAngle(SelectedHandle.Axis, angle);
                    }
                }
            }

            if (IsOver && input.IsButtonPressed(OpenTK.Input.MouseButton.Left))
            {
                SelectedHandle = GetHoveredHandle();
            }

            if (Selected && !input.IsButtonDown(OpenTK.Input.MouseButton.Left))
            {
                if (IsEditing)
                    EndEditGizmo();
                SelectedHandle = null;
            }
        }

        private Vector2 EditStartPos;

        private void BeginEditGizmo(Ray ray)
        {
            IsEditing = true;
            EditTransform = Matrix4.Identity;
            var localRay = Ray.Transform(ray, Transform.Inverted());
            EditStartPos = SelectedHandle.ProjectToPlane(localRay);

        }

        public void CancelEdit()
        {
            if (IsEditing)
            {
                IsEditing = false;
                EditTransform = Matrix4.Identity;
                SelectedHandle = null;
            }
        }

        private void EndEditGizmo()
        {
            IsEditing = false;
            EditTransform = Matrix4.Identity;
            SelectedHandle = null;
        }

        public Matrix4 GetActiveTransform()
        {
            return EditTransform * Transform;
        }

        #region Rendering

        public void Render()
        {
            for (int i = 0; i < 3; i++)
            {
                var gizmoHandle = GetHandle(i);
                if (gizmoHandle != null)
                {
                    var color = HandleColors[i];
                    if (!gizmoHandle.IsOver)
                    {
                        color.Xyz *= 0.95f;
                        color.W = 0.8f;
                    }
                    gizmoHandle.RenderHandle(this, color);
                }
            }
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
