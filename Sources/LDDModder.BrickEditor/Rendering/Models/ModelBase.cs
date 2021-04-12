using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class ModelBase : ITransformableElement, IDisposable
    {
        private bool _Visible;

        public bool Visible
        {
            get => VisibleOverride() ?? _Visible;
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    var valueOverride = VisibleOverride();

                    if (!valueOverride.HasValue || valueOverride.Value != value)
                        OnVisibilityChanged();
                }
            }
        }

        public MaterialInfo ModelMaterial { get; set; }

        public BBox BoundingBox { get; set; }

        private Matrix4 _Transform;
        private Matrix4? _TemporaryTransform;
        protected readonly object TransformLock = new object();

        public Matrix4 Transform
        {
            get
            {
                lock (TransformLock)
                {
                    return _TemporaryTransform.HasValue ? _TemporaryTransform.Value : _Transform;
                }
                
            }
            set
            {
                if (_TemporaryTransform.HasValue)
                    return;
                if (value != _Transform)
                {
                    _Transform = value;
                    if (!IsEditingTransform)
                        OnTransformChanged();
                }
            }
        }


        public bool IsEditingTransform { get; private set; }

        public float ZDepth { get; set; }

        public bool IsSelectable { get; set; } = true;

        public bool IsSelected { get; set; }

        public int RenderLayer { get; set; }

        public Vector3 Origin => Vector3.TransformPosition(Vector3.Zero, Transform);

        public event EventHandler VisibilityChanged;

        public event EventHandler TransformChanged;

        public ModelBase()
        {
            Visible = true;
            _Transform = Matrix4.Identity;
        }

        #region HitTest

        public virtual bool RayIntersectsBoundingBox(Ray ray, out float distance)
        {
            if (BoundingBox.IsEmpty)
            {
                distance = float.NaN;
                return false;
            }

            var localRay = Ray.Transform(ray, Transform.Inverted());
            return Ray.IntersectsBox(localRay, BoundingBox, out distance);
        }

        public bool RayIntersectsGizmo(Ray ray, float axisLength, float axisThickness, out float distance)
        {
            var xBox = BBox.FromCenterSize(new Vector3(axisLength / 2f, 0, 0), new Vector3(axisLength, axisThickness, axisThickness));
            var yBox = BBox.FromCenterSize(new Vector3(0, axisLength / 2f, 0), new Vector3(axisThickness, axisLength, axisThickness));
            var zBox = BBox.FromCenterSize(new Vector3(0, 0, axisLength / 2f), new Vector3(axisThickness, axisThickness, axisLength));
            distance = float.MaxValue;

            var localRay = Ray.Transform(ray, Transform.Inverted());

            float result;
            if (Ray.IntersectsBox(localRay, xBox, out result) && result < distance)
                distance = result;
            if (Ray.IntersectsBox(localRay, yBox, out result) && result < distance)
                distance = result;
            if (Ray.IntersectsBox(localRay, zBox, out result) && result < distance)
                distance = result;

            return distance != float.MaxValue;
        }

        public abstract bool RayIntersects(Ray ray, out float distance);

        public virtual BBox GetWorldBoundingBox()
        {
            var corners = BoundingBox.GetCorners();
            for (int i = 0; i < 8; i++)
                corners[i] = Vector3.TransformPosition(corners[i], Transform);
            return BBox.FromVertices(corners);
        }

        #endregion

        protected virtual bool? VisibleOverride()
        {
            return null;
        }

        protected virtual void OnVisibilityChanged()
        {
            VisibilityChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {

        }

        public virtual void RenderUI(Camera camera)
        {

        }

        #region Transform Handling

        private Matrix4 OriginalTrans;

        public void SetTransform(Matrix4 transform, bool fireChange = true)
        {
            if (_Transform != transform)
            {
                _Transform = transform;
                if (fireChange)
                    OnTransformChanged();
            }
        }

        public Matrix4 GetBaseTranform()
        {
            return _Transform;
        }

        public void SetTemporaryTransform(Matrix4? transform)
        {
            lock (TransformLock)
            {
                _TemporaryTransform = transform;
            }
        }

        public void BeginEditTransform()
        {
            OriginalTrans = _Transform;
            IsEditingTransform = true;
            OnBeginEditTransform();
        }

        protected virtual void OnBeginEditTransform()
        {

        }

        public void EndEditTransform(bool canceled)
        {
            IsEditingTransform = false;

            if (OriginalTrans != Transform)
            {
                if (canceled)
                    _Transform = OriginalTrans;
                else
                {
                    OnTransformChanged();
                }
            }

            OnEndEditTransform(canceled);
        }

        protected virtual void OnEndEditTransform(bool canceled)
        {

        }

        public virtual void ApplyTransform(Matrix4 transform)
        {
            _Transform = transform;
        }

        protected virtual void OnTransformChanged()
        {
            TransformChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public virtual void Dispose()
        {
            
        }
    }
}
