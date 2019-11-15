using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public abstract class ModelBase
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
                    OnTransformChanged();
                }
            }
        }

        public Vector3 Origin => Vector3.TransformPosition(Vector3.Zero, Transform);

        public event EventHandler VisibilityChanged;

        public ModelBase()
        {
            Visible = true;
            _Transform = Matrix4.Identity;
        }

        public virtual bool RayIntersectsBoundingBox(Ray ray, out float distance)
        {
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

        protected virtual void OnTransformChanged()
        {
            
        }

        protected void OnVisibilityChanged()
        {
            VisibilityChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void SetTransform(Matrix4 transform)
        {
            _Transform = transform;
        }

        public virtual void RenderModel(Camera camera)
        {

        }
    }
}
