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
        private bool recalculateBounds;
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
                    recalculateBounds = true;
                }
            }
        }

        public Vector3 Position
        {
            get => _Position.Row3.Xyz;
            set
            {
                if (_Position.Row3.Xyz != value)
                {
                    _Position = Matrix4.CreateTranslation(value);
                    _Transform = _Orientation * _Position;
                    recalculateBounds = true;
                }
            }
        }

        public Matrix4 Orientation
        {
            get => _Orientation;
            set
            {
                if (_Orientation != value)
                {
                    _Orientation = value;
                    _Transform = _Orientation * _Position;
                    recalculateBounds = true;
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
                    _Position = Matrix4.CreateTranslation(_Transform.ExtractTranslation());
                    _Orientation = Matrix4.CreateFromQuaternion(_Transform.ExtractRotation());
                    recalculateBounds = true;
                }
            }
        }

        public bool Visible { get; set; }

        public bool IsHovering { get; private set; }

        public GizmoHandle SelectedHandle { get; private set; }

        public bool Selected => SelectedHandle != null;

        public bool IsEditing { get; private set; }

        public GizmoStyle TransformMode { get; set; }

        public OrientationMode OrientationMode { get; set; }

        public PivotPointMode PivotPointMode { get; set; }

        public BSphere BoundingSphere { get; private set; }

        public Vector4[] HandleColors { get; set; }

        public IndexedVertexBuffer<Vector3> VertexBuffer { get; private set; }

        public TransformGizmo()
        {
            _GizmoSize = 75f;
            _Transform = Matrix4.Identity;
            _Position = Matrix4.Identity;
            _Orientation = Matrix4.Identity;

            EditTransform = Matrix4.Identity;

            TransformMode = GizmoStyle.Translation;
            
            HandleColors = new Vector4[]
            {
                new Vector4(1f,0.09f,0.26f,1f),
                new Vector4(0.58f, 0.898f, 0.156f, 1f),
                new Vector4(0.156f,0.564f,1f,1f)
            };

            EditedElements = new List<ModelEditInfo>();
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
            recalculateBounds = false;
        }

        private GizmoHandle GetHandle(int index)
        {
            if (TransformMode == GizmoStyle.Translation)
                return TranslationHandles[index];
            else if (TransformMode == GizmoStyle.Rotation)
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
            if (recalculateBounds)
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

            if (recalculateBounds)
                UpdateBounds(camera);

            bool updateMouseOver = false;

            if (!(IsEditing || Selected))
            {
                if (input.IsKeyPressed(OpenTK.Input.Key.R) && TransformMode != GizmoStyle.Rotation)
                {
                    TransformMode = GizmoStyle.Rotation;
                    ClearOver();
                    updateMouseOver = true;
                }
                else if (input.IsKeyPressed(OpenTK.Input.Key.T) && TransformMode != GizmoStyle.Translation)
                {
                    TransformMode = GizmoStyle.Translation;
                    ClearOver();
                    updateMouseOver = true;
                }
            }

            if (IsEditing && input.IsKeyPressed(OpenTK.Input.Key.Escape))
            {
                CancelEdit();
                SelectedHandle = null;
                ClearOver();
            }

            var mouseDelta = input.MousePos - input.LastMousePos;

            if (updateMouseOver || mouseDelta.LengthFast > 1)
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
                    EditCurrentPos = SelectedHandle.ProjectToPlane(localRay);

                    if (TransformMode == GizmoStyle.Translation)
                    {
                        EditAmount = GetComponent(EditCurrentPos - EditStartPos, SelectedHandle.Axis);
                        EditTransform = Matrix4.CreateTranslation(SelectedHandle.Axis * EditAmount);

                    }
                    else if (TransformMode == GizmoStyle.Rotation)
                    {
                        var v1 = EditStartPos.Normalized();
                        var v2 = EditCurrentPos.Normalized();
                        var angle = Vector3.CalculateAngle(v1, v2);

                        if (angle >= 0.001)
                        {
                            var v3 = Vector3.Cross(EditStartPos, EditCurrentPos).Normalized();
                            var counterClockwise = Vector3.Distance(v3, SelectedHandle.Axis) <= 0.1;
                            EditAmount = angle * (counterClockwise ? 1 : -1);
                            EditTransform = Matrix4.CreateFromAxisAngle(SelectedHandle.Axis, EditAmount);
                        }
                    }

                    ApplyTransformToElements();
                }
            }

            if (IsHovering && input.IsButtonPressed(OpenTK.Input.MouseButton.Left))
            {
                SelectedHandle = GetHoveredHandle();
            }

            if (Selected && !input.IsButtonDown(OpenTK.Input.MouseButton.Left))
            {
                if (IsEditing)
                    EndEditGizmo();
                SelectedHandle = null;
                UpdateBounds(camera);
            }
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
            if (IsEditing && TransformMode == GizmoStyle.Translation)
            {
                var color = HandleColors[SelectedHandle.Index];
                RenderHelper.BeginDrawColor(VertexBuffer, Transform, color);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(SelectedHandle.Axis * -100f);
                GL.Vertex3(SelectedHandle.Axis * 100f);
                GL.End();
            }

            if (IsEditing && TransformMode == GizmoStyle.Rotation)
            {

                var color = HandleColors[SelectedHandle.Index];
                RenderHelper.BeginDrawColor(VertexBuffer, Transform, color);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Vector3.Zero);
                GL.Vertex3(EditCurrentPos.Normalized() * UIScale * GizmoSize);
                GL.End();
            }

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

        #region Transform Editing

        public Matrix4 EditTransform { get; private set; }

        private Vector3 EditStartPos;

        private Vector3 EditCurrentPos;

        private List<ModelEditInfo> EditedElements { get; }

        public float EditAmount { get; private set; }

        private void BeginEditGizmo(Ray ray)
        {
            EditAmount = 0;
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

                if (EditedElements.Any())
                {
                    EditedElements.ForEach(x => x.Model.Transform = x.OriginalTrans);
                    EditedElements.Clear();
                }
            }
        }

        private void EndEditGizmo()
        {
            ApplyTransformToElements();
            Transform = EditTransform * Transform;
            InitEditedElements();

            IsEditing = false;
            EditTransform = Matrix4.Identity;
            SelectedHandle = null;
        }

        public Matrix4 GetActiveTransform()
        {
            if (!IsEditing)
                return Transform;
            return EditTransform * Transform;
        }


        public void ActivateForModels(IEnumerable<ModelBase> models, OrientationMode orientation, PivotPointMode pivotPoint)
        {
            Vector3 transformPosition = Vector3.Zero;

            EditedElements.Clear();
            EditedElements.AddRange(models.Select(x => new ModelEditInfo(x)));
            

            switch (pivotPoint)
            {
                case PivotPointMode.BoundingBox:
                    var allboxes = models.Select(x => x.GetWorldBoundingBox());
                    transformPosition = BBox.Combine(allboxes).Center;
                    break;
                case PivotPointMode.MedianCenter:
                    var allCenters = models.Select(x => x.GetWorldBoundingBox().Center).ToList();
                    foreach (var center in allCenters)
                        transformPosition += center;
                    transformPosition /= allCenters.Count;
                    break;
                case PivotPointMode.MedianOrigin:
                    var allOrigins = models.Select(x => x.Origin).ToList();
                    foreach (var origin in allOrigins)
                        transformPosition += origin;
                    transformPosition /= allOrigins.Count;
                    break;
                case PivotPointMode.ActiveElement:
                    transformPosition = models.Last().Origin;
                    break;

                case PivotPointMode.Cursor:
                    //TODO
                    break;
            }

            _Position = Matrix4.CreateTranslation(transformPosition);
            _Orientation = Matrix4.Identity;

            if (orientation == OrientationMode.Local)
            {
                var allRot = models.Select(x => x.Transform.ExtractRotation());
                var avgRot = OpenTKHelper.AverageQuaternion(allRot);
                _Orientation = Matrix4.CreateFromQuaternion(avgRot);
            }

            _Transform = _Orientation * _Position;

            InitEditedElements();

            recalculateBounds = true;
            Visible = true;
        }


        private void InitEditedElements()
        {
            var invTrans = Transform.Inverted();
            foreach (var model in EditedElements)
            {
                model.OriginalTrans = model.Model.Transform;
                //TODO: improve this logic!!
                var localPos = Vector3.TransformPosition(model.OriginalTrans.ExtractTranslation(), invTrans);
                var localRot = Quaternion.Multiply(Orientation.ExtractRotation().Inverted(), 
                    model.OriginalTrans.ExtractRotation());

                model.LocalMatrix = Matrix4.Mult(Matrix4.CreateFromQuaternion(localRot), 
                    Matrix4.CreateTranslation(localPos));
            }
        }

        public void Deactivate()
        {
            Visible = false;
            EditedElements.Clear();
        }

        private void ApplyTransformToElements()
        {
            //var localTrans = EditTransform * Orientation;
            var localTrans = GetActiveTransform();
            foreach (var modelTrans in EditedElements)
            {
                var baseTrans = modelTrans.LocalMatrix * localTrans;
                
                
                modelTrans.Model.Transform = baseTrans;
            }
        }

        private class ModelEditInfo
        {
            public ModelBase Model { get; set; }
            public Matrix4 OriginalTrans { get; set; }

            public Matrix4 LocalMatrix { get; set; }

            public Matrix4 InvertMat2 { get; set; }

            public ModelEditInfo(ModelBase model)
            {
                Model = model;
                OriginalTrans = model.Transform;
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
