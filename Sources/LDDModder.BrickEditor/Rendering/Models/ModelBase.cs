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
            get => _Visible;
            set
            {
                if (value != _Visible)
                {
                    _Visible = value;
                    OnVisibilityChanged();
                }
            }
        }

        public BBox BoundingBox { get; set; }

        private Matrix4 _Transform;

        public Matrix4 Transform
        {
            get => _Transform;
            set
            {
                if (value != _Transform)
                {
                    _Transform = value;
                    if (!IsEditingTransform)
                        OnTransformChanged();
                }
            }
        }

        public bool IsEditingTransform { get; private set; }

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

        public abstract bool RayIntersects(Ray ray, out float distance);

        public virtual BBox GetWorldBoundingBox()
        {
            var corners = BoundingBox.GetCorners();
            for (int i = 0; i < 8; i++)
                corners[i] = Vector3.TransformPosition(corners[i], Transform);
            return BBox.FromVertices(corners);
        }

        #endregion

        protected void OnVisibilityChanged()
        {
            VisibilityChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void RenderModel(Camera camera)
        {

        }

        #region Transform Handling

        private Matrix4 OriginalTrans;

        protected void SetTransform(Matrix4 transform, bool fireChange)
        {
            if (_Transform != transform)
            {
                _Transform = transform;
                if (fireChange)
                {
                    TransformChanged?.Invoke(this, EventArgs.Empty);
                    OnTransformChanged();
                }
            }
        }

        public void BeginEditTransform()
        {
            OriginalTrans = Transform;
            IsEditingTransform = true;
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
                    TransformChanged?.Invoke(this, EventArgs.Empty);
                    OnTransformChanged();
                }
            }
        }

        public virtual void ApplyTransform(Matrix4 transform)
        {
            _Transform = transform;
        }

        protected virtual void OnTransformChanged()
        {

        }

        #endregion

        public virtual void Dispose()
        {
            
        }
    }
}
