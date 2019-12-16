using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering.Gizmos
{
    public class TransformGizmo : IDisposable
    {
        private RotationHandle[] RotationHandles;
        private TranslationHandle[] TranslationHandles;

        private float _GizmoSize;
        private Matrix4 _Transform;
        private bool RecalculateBounds;
        private Matrix4 _Position;
        private Matrix4 _Orientation;

        public float UIScale { get; private set; }

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
                    RecalculateBounds = true;
                }
            }
        }

        /// <summary>
        /// The position of the gizmo.
        /// </summary>
        public Vector3 Position
        {
            get => _Position.Row3.Xyz;
            set
            {
                if (_Position.Row3.Xyz != value)
                {
                    _Position = Matrix4.CreateTranslation(value);
                    _Transform = _Orientation * _Position;
                    RecalculateBounds = true;
                }
            }
        }

        /// <summary>
        /// The orientation matrix of the gizmo.
        /// </summary>
        public Matrix4 Orientation
        {
            get => _Orientation;
            set
            {
                if (_Orientation != value)
                {
                    _Orientation = value;
                    _Transform = _Orientation * _Position;
                    RecalculateBounds = true;
                    SetupElementsMatrices();
                }
            }
        }

        /// <summary>
        /// The transformation matrix of the gizmo.
        /// </summary>
        public Matrix4 Transform
        {
            get => _Transform;
            set
            {
                if (_Transform != value)
                {
                    _Transform = value;
                    _Position = Matrix4.CreateTranslation(_Transform.ExtractTranslation());
                    _Orientation = Matrix4.CreateFromQuaternion(_Transform.ExtractRotation());
                    RecalculateBounds = true;

                }
            }
        }

        public bool Visible { get; set; }

        public bool IsHovering { get; private set; }

        public GizmoHandle SelectedHandle { get; private set; }

        public bool Selected => SelectedHandle != null;

        public bool IsEditing { get; private set; }

        #region MyRegion

        private GizmoStyle _DisplayStyle;
        private OrientationMode _OrientationMode;
        private PivotPointMode _PivotPointMode;

        public GizmoStyle DisplayStyle
        {
            get => _DisplayStyle;
            set
            {
                if (!(IsEditing || Selected) && value != _DisplayStyle)
                {
                    _DisplayStyle = value;
                    RecalculateBounds = true;
                    ClearOver();

                    if (Visible && ActiveElements.Any())
                        RepositionGizmo();

                    DisplayStyleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public OrientationMode OrientationMode
        {
            get => _OrientationMode;
            set
            {
                if (!(IsEditing || Selected) && value != _OrientationMode)
                {
                    _OrientationMode = value;
                    ClearOver();
                    if (Visible && ActiveElements.Any())
                        RepositionGizmo();
                }
            }
        }

        public PivotPointMode PivotPointMode
        {
            get => _PivotPointMode;
            set
            {
                if (!(IsEditing || Selected) && value != _PivotPointMode)
                {
                    _PivotPointMode = value;
                    ClearOver();
                    if (Visible && ActiveElements.Any())
                        RepositionGizmo();
                }
            }
        }

        public event EventHandler DisplayStyleChanged;

        #endregion

        public float TranslationSnap { get; set; }

        public float RotationSnap { get; set; }

        public BSphere BoundingSphere { get; private set; }

        public Vector4[] HandleColors { get; set; }

        public IndexedVertexBuffer<Vector3> VertexBuffer { get; private set; }

        public TransformGizmo()
        {
            _GizmoSize = 75f;
            _Transform = Matrix4.Identity;
            _Position = Matrix4.Identity;
            _Orientation = Matrix4.Identity;

            RotationSnap = 5f * (float)Math.PI / 180f;
            TranslationSnap = 0.4f;

            EditTransform = Matrix4.Identity;

            _DisplayStyle = GizmoStyle.Translation;
            
            HandleColors = new Vector4[]
            {
                new Vector4(1f,0.09f,0.26f,1f),
                new Vector4(0.58f, 0.898f, 0.156f, 1f),
                new Vector4(0.156f,0.564f,1f,1f)
            };

            EditedElements = new List<TransformFollower>();
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

        public void UpdateBounds(Camera camera)
        {
            var distFromCamera = camera.GetDistanceFromCamera(Position);
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

            BoundingSphere = new BSphere(Position, scaledGizmoSize + padding);
            RecalculateBounds = false;
        }

        private GizmoHandle GetHandle(int index)
        {
            if (DisplayStyle == GizmoStyle.Translation)
                return TranslationHandles[index];
            else if (DisplayStyle == GizmoStyle.Rotation)
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

        #region HitTesting

        public bool IsOverGizmo(Ray ray)
        {
            return Ray.IntersectsSphere(ray, BoundingSphere, out _);
        }

        public bool IsOverHandle(Ray ray, out GizmoHandle handle)
        {
            handle = null;
            if (RecalculateBounds)
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

        private void ClearOver()
        {
            for (int i = 0; i < 3; i++)
            {
                var transAxis = TranslationHandles[i];
                transAxis.IsOver = false;

                var rotAxis = RotationHandles[i];
                rotAxis.IsOver = false;
            }
            IsHovering = false;
        }

        private void ClearSelection()
        {
            if (SelectedHandle != null)
            {
                SelectedHandle.IsSelected = false;
                SelectedHandle = null;
            }
        }

        public void PerformMouseOver(Ray ray)
        {
            ClearOver();
            if (IsOverHandle(ray, out GizmoHandle handle))
            {
                handle.IsOver = true;
                IsHovering = true;
            }
        }

        #endregion

        public void ProcessInput(Camera camera, InputManager input)
        {
            if (!Visible)
                return;

            bool recalculatedBounds = RecalculateBounds;
            if (RecalculateBounds)
                UpdateBounds(camera);

            if (IsEditing && input.IsKeyPressed(OpenTK.Input.Key.Escape))
            {
                CancelEdit();
                ClearSelection();
                ClearOver();
            }

            var mouseDelta = input.MousePos - input.LastMousePos;

            if (recalculatedBounds || mouseDelta.LengthFast > 1)
            {
                var mouseRay = camera.RaycastFromScreen(input.LocalMousePos);

                if (!Selected && (IsOverGizmo(mouseRay) || IsHovering))
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
                    EditCurrentPos = SelectedHandle.ProjectToPlane(localRay);

                    if (DisplayStyle == GizmoStyle.Translation)
                    {
                        TransformedAmount = GetComponent(EditCurrentPos - EditStartPos, SelectedHandle.Axis);
                        if (input.IsControlDown())
                            TransformedAmount = SnapValue(TransformedAmount, TranslationSnap);

                        EditTransform = Matrix4.CreateTranslation(SelectedHandle.Axis * TransformedAmount);

                    }
                    else if (DisplayStyle == GizmoStyle.Rotation)
                    {
                        var v1 = EditStartPos.Normalized();
                        var v2 = EditCurrentPos.Normalized();
                        var angle = Vector3.CalculateAngle(v1, v2);

                        if (angle >= 0.001)
                        {
                            var v3 = Vector3.Cross(EditStartPos, EditCurrentPos).Normalized();
                            var counterClockwise = Vector3.Distance(v3, SelectedHandle.Axis) <= 0.1;
                            TransformedAmount = angle * (counterClockwise ? 1 : -1);
                            if (input.IsControlDown())
                                TransformedAmount = SnapValue(TransformedAmount, RotationSnap);
                            EditTransform = Matrix4.CreateFromAxisAngle(SelectedHandle.Axis, TransformedAmount);
                        }
                    }

                    ApplyTransformToElements();
                }
            }

            if (IsHovering && input.IsButtonPressed(OpenTK.Input.MouseButton.Left))
            {
                SelectedHandle = GetHoveredHandle();
                if (SelectedHandle != null)
                    SelectedHandle.IsSelected = true;
            }

            if (Selected && !input.IsButtonDown(OpenTK.Input.MouseButton.Left))
            {
                input.MouseClickHandled = true;

                if (IsEditing)
                {
                    if (TransformedAmount == 0)
                        CancelEdit();
                    else
                        EndEditGizmo();
                }
                ClearSelection();
                UpdateBounds(camera);
            }
        }

        private static float SnapValue(float value, float snapIncrement)
        {
            return (float)Math.Round(Math.Abs(value) / snapIncrement) * snapIncrement * Math.Sign(value);
        }

        public static float GetComponent(Vector3 value, Vector3 axis)
        {
            for (int i = 0; i < 3; i++)
                if (axis[i] == 1)
                    return value[i];
            return 0;
        }

        #region Rendering

        public void Render()
        {
            switch (DisplayStyle)
            {
                case GizmoStyle.Plain:
                    RenderPlainGizmo();
                    break;
                case GizmoStyle.Translation:
                    RenderTranslationGizmo();
                    break;
                case GizmoStyle.Rotation:
                    RenderRotationGizmo();
                    break;
                case GizmoStyle.Scaling:
                    break;
            }
        }

        private void RenderPlainGizmo()
        {
            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(2f);

            for (int i = 0; i < 3; i++)
            {
                var color = HandleColors[i];
                
                RenderHelper.BeginDrawColor(VertexBuffer, Transform, color);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Vector3.Zero);
                GL.Vertex3(TranslationHandles[i].Axis * UIScale * GizmoSize);
                GL.End();
            }
            
            GL.PopAttrib();
        }

        private void RenderTranslationGizmo()
        {
            if (IsEditing)
            {
                var color = HandleColors[SelectedHandle.Index];
                RenderHelper.BeginDrawColor(VertexBuffer, Transform, color);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(SelectedHandle.Axis * -100f);
                GL.Vertex3(SelectedHandle.Axis * 100f);
                GL.End();
            }

            RenderHandles();
        }

        private void RenderRotationGizmo()
        {
            if (IsEditing)
            {
                var color = HandleColors[SelectedHandle.Index];
                RenderHelper.BeginDrawColor(VertexBuffer, Transform, color);

                var p1 = EditStartPos.Normalized();
                var rot = Matrix4.CreateFromAxisAngle(SelectedHandle.Axis, TransformedAmount);
                var p2 = Vector3.TransformPosition(p1, rot);

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Vector3.Zero);
                GL.Vertex3(p2.Normalized() * UIScale * GizmoSize);
                GL.End();
            }

            RenderHandles();
        }

        private void RenderHandles()
        {
            RenderHelper.EnableStencilTest();

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

                    RenderHelper.EnableStencilMask();

                    gizmoHandle.RenderHandle(this, color);
                    
                    var outlineColor = (gizmoHandle.IsOver || gizmoHandle.IsSelected) ? new Vector4(1f) : new Vector4(0, 0, 0, 1f);
                    RenderHelper.ApplyStencilMask();

                    gizmoHandle.RenderHandle(this, outlineColor, true);

                    RenderHelper.RemoveStencilMask();
                    RenderHelper.ClearStencil();
                }
            }

            RenderHelper.DisableStencilTest();
        }

        #endregion

        #region Transform Editing

        public Matrix4 EditTransform { get; private set; }

        private Vector3 EditStartPos;

        private Vector3 EditCurrentPos;

        public event EventHandler TransformFinished;

        public event EventHandler TransformFinishing;

        private List<TransformFollower> EditedElements { get; }

        public IEnumerable<ITransformableElement> ActiveElements => EditedElements.Select(x => x.Element);

        public float TransformedAmount { get; private set; }

        private bool ApplyingTransform;

        private void BeginEditGizmo(Ray ray)
        {
            TransformedAmount = 0;
            IsEditing = true;
            EditTransform = Matrix4.Identity;
            var localRay = Ray.Transform(ray, Transform.Inverted());
            EditStartPos = SelectedHandle.ProjectToPlane(localRay);

            foreach (var element in ActiveElements)
                element.BeginEditTransform();
        }

        public void CancelEdit()
        {
            if (IsEditing)
            {
                IsEditing = false;
                EditTransform = Matrix4.Identity;
                ClearSelection();

                foreach (var element in ActiveElements)
                    element.EndEditTransform(true);
            }
        }

        private void EndEditGizmo()
        {
            TransformFinishing?.Invoke(this, EventArgs.Empty);

            ApplyTransformToElements();

            ApplyingTransform = true;

            foreach (var element in ActiveElements)
                element.EndEditTransform(false);

            TransformFinished?.Invoke(this, EventArgs.Empty);

            ApplyingTransform = false;

            Transform = EditTransform * Transform;

            SetupElementsMatrices();
           
            IsEditing = false;
            EditTransform = Matrix4.Identity;
            ClearSelection();
        }

        public Matrix4 GetActiveTransform()
        {
            if (!IsEditing)
                return Transform;
            return EditTransform * Transform;
        }

        public void SetActiveElements(IEnumerable<ITransformableElement> elements)
        {
            DetachActiveElements();
            
            foreach (var element in elements)
            {
                EditedElements.Add(new TransformFollower(element));
                element.TransformChanged += Element_TransformChanged;
            }

            RepositionGizmo();
            Visible = ActiveElements.Any();
        }

        private void Element_TransformChanged(object sender, EventArgs e)
        {
            if (!ApplyingTransform)
                RepositionGizmo();
        }

        private void DetachActiveElements()
        {
            foreach (var follower in EditedElements)
                follower.Element.TransformChanged -= Element_TransformChanged;
            EditedElements.Clear();
        }

        public void RepositionGizmo()
        {
            Vector3 transformPosition = Vector3.Zero;
            switch (PivotPointMode)
            {
                case PivotPointMode.BoundingBox:
                    var allboxes = ActiveElements.Select(x => x.GetWorldBoundingBox());
                    transformPosition = BBox.Combine(allboxes).Center;
                    break;
                case PivotPointMode.MedianCenter:
                    var allCenters = ActiveElements.Select(x => x.GetWorldBoundingBox().Center).ToList();
                    foreach (var center in allCenters)
                        transformPosition += center;
                    transformPosition /= allCenters.Count;
                    break;
                case PivotPointMode.MedianOrigin:
                    var allOrigins = ActiveElements.Select(x => x.Origin).ToList();
                    foreach (var origin in allOrigins)
                        transformPosition += origin;
                    transformPosition /= allOrigins.Count;
                    break;
                case PivotPointMode.ActiveElement:
                    transformPosition = ActiveElements.Last().Origin;
                    break;

                case PivotPointMode.Cursor:
                    //TODO
                    break;
            }

            _Position = Matrix4.CreateTranslation(transformPosition);
            _Orientation = Matrix4.Identity;

            if (OrientationMode == OrientationMode.Local)
            {
                var allRot = ActiveElements.Select(x => x.Transform.ExtractRotation());
                var avgRot = OpenTKHelper.AverageQuaternion(allRot);
                _Orientation = Matrix4.CreateFromQuaternion(avgRot);
            }

            //_SelectionOrientation = _Orientation;
            _Transform = _Orientation * _Position;
            RecalculateBounds = true;

            if (ActiveElements.Any())
                SetupElementsMatrices();
        }

        private void SetupElementsMatrices()
        {
            var invTrans = Transform.Inverted();

            foreach (var follower in EditedElements)
            {
                follower.OriginalMatrix = follower.Element.Transform;

                var localPos = Vector3.TransformPosition(follower.OriginalMatrix.ExtractTranslation(), invTrans);
                var localRot = Quaternion.Multiply(Orientation.ExtractRotation().Inverted(),
                    follower.OriginalMatrix.ExtractRotation());

                var localMatrix = Matrix4.Mult(Matrix4.CreateFromQuaternion(localRot),
                    Matrix4.CreateTranslation(localPos));

                //var localMatrix = Matrix4.Mult(invTrans, model.OriginalMatrix);
                follower.LocalMatrix = localMatrix;
            }
        }

        public void Deactivate()
        {
            if (IsEditing)
                CancelEdit();
            
            DetachActiveElements();
            Visible = false;
        }

        private void ApplyTransformToElements()
        {
            var localTrans = GetActiveTransform();
            foreach (var modelTrans in EditedElements)
                modelTrans.ApplyTransform(localTrans);
        }

        private class TransformFollower
        {
            public ITransformableElement Element { get; set; }

            public Matrix4 OriginalMatrix { get; set; }

            public Matrix4 LocalMatrix { get; set; }

            public TransformFollower(ITransformableElement model)
            {
                Element = model;
                OriginalMatrix = model.Transform;
            }

            public void ApplyTransform(Matrix4 transform)
            {
                Element.ApplyTransform(LocalMatrix * transform);
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
