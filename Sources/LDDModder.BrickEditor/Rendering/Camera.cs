using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class Camera
    {
        private Matrix4 _Transform;
        private bool _IsPerspective;
        private float _FieldOfView;

        private float _FarClipDistance;
        private float _NearClipDistance;
        private float _OrthographicSize;

        private bool isViewMatrixDirty;
        private Matrix4 _ViewMatrix;

        private bool isProjectionMatrixDirty;
        private Matrix4 _ProjectionMatrix;
        private RectangleF _Viewport;

        public Matrix4 Transform
        {
            get => _Transform;
            set
            {
                if (_Transform != value)
                {
                    _Transform = value;
                    isViewMatrixDirty = true;
                }
            }
        }

        public Vector3 Forward => Vector3.TransformNormal(Vector3.UnitZ, _Transform);

        public Vector3 Up => Vector3.TransformNormal(Vector3.UnitY, _Transform);

        public Vector3 Right => Vector3.TransformNormal(Vector3.UnitX, _Transform);

        public Vector3 Position
        {
            get => Vector3.TransformPosition(Vector3.Zero, _Transform);
            set
            {
                var curPos = Position;
                if (curPos != value)
                {
                    var translation = Matrix4.CreateTranslation(value - curPos);
                    _Transform *= translation;
                    isViewMatrixDirty = true;
                }
            }
        }

        public RectangleF Viewport
        {
            get => _Viewport;
            set
            {
                if (value != _Viewport)
                {
                    _Viewport = value;
                    isProjectionMatrixDirty = true;
                }
            }
        }

        public float AspectRatio => _Viewport.Width / _Viewport.Height;

        public float FieldOfView
        {
            get => _FieldOfView;
            set
            {
                if (value != _FieldOfView)
                {
                    _FieldOfView = value;
                    isProjectionMatrixDirty = true;
                }
            }
        }

        public bool IsPerspective
        {
            get => _IsPerspective;
            set
            {
                if (value != _IsPerspective)
                {
                    _IsPerspective = value;
                    isViewMatrixDirty = true;
                    isProjectionMatrixDirty = true;
                }
            }
        }

        public float NearClipDistance
        {
            get { return _NearClipDistance; }
            set
            {
                value = Math.Max(0.0001f, Math.Min(FarClipDistance - 0.01f, value));
                if (_NearClipDistance == value)
                    return;
                _NearClipDistance = value;
                isProjectionMatrixDirty = true;
            }
        }

        public float FarClipDistance
        {
            get { return _FarClipDistance; }
            set
            {
                value = Math.Max(NearClipDistance + 0.01f, value);
                if (_FarClipDistance == value)
                    return;
                _FarClipDistance = value;
                isProjectionMatrixDirty = true;
            }
        }

        public float OrthographicSize
        {
            get { return _OrthographicSize; }
            set
            {
                if (value < 0.01f || _OrthographicSize == value)
                    return;
                _OrthographicSize = value;
                if (!_IsPerspective)
                    isProjectionMatrixDirty = true;
            }
        }

        public Camera()
        {
            _Transform = Matrix4.Identity;
            isProjectionMatrixDirty = true;
            isViewMatrixDirty = true;
            _FieldOfView = 0.7853982f; //45°
            _NearClipDistance = 0.3f;
            _FarClipDistance = 1000f;
            _OrthographicSize = 10f;
            _IsPerspective = true;
        }


        public void LookAt(Vector3 target, Vector3 up)
        {
            var viewDir = (target - Position).Normalized();
            var rot = Matrix4.LookAt(viewDir, Vector3.Zero, up).ExtractRotation();
            var rotMat = Matrix4.CreateFromQuaternion(rot.Inverted());
            _Transform = rotMat * Matrix4.CreateTranslation(Position);
            isViewMatrixDirty = true;
        }

        public Matrix4 GetViewMatrix()
        {
            if (isViewMatrixDirty)
            {
                var pos = Position;
                if (!IsPerspective)
                    pos -= Forward * 20;
                _ViewMatrix = Matrix4.LookAt(pos, pos + Forward * 4, Up);
                isViewMatrixDirty = false;
            }
            return _ViewMatrix;
        }

        public Matrix4 GetProjectionMatrix()
        {
            if (isProjectionMatrixDirty)
            {
                if (_IsPerspective)
                {
                    _ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                        FieldOfView, AspectRatio, 
                        NearClipDistance, FarClipDistance);
                }
                else
                {
                    _ProjectionMatrix = Matrix4.CreateOrthographic(
                        OrthographicSize * AspectRatio, OrthographicSize, 
                        NearClipDistance, FarClipDistance);
                }
                isProjectionMatrixDirty = false;
            }
            return _ProjectionMatrix;
        }

        /// <summary>
        /// Transforms position from screen space into viewport space.
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        public Vector2 ScreenPointToViewport(Vector2 screenPoint)
        {
            return new Vector2(
                (screenPoint.X - Viewport.X) / Viewport.Width, //left to right = 0 to 1
                (Viewport.Height - (screenPoint.Y - Viewport.Y)) / Viewport.Height);//up to bottom = 0 to 1 (invert Y)
        }

        #region Viewport 

        public static Vector4 ViewportPointToFrustum(Vector3 viewportPoint)
        {
            return new Vector4(
                viewportPoint.X * 2f - 1f,
                viewportPoint.Y * 2f - 1f,
                viewportPoint.Z * 2f - 1f,
                1f);
        }

        private static Vector3 FrustumPointToWorld(Vector4 frustumPoint, Matrix4 cameraMatrix)
        {
            var result = Vector4.Transform(frustumPoint, cameraMatrix);
            if (result.W == 0)
                throw new Exception("Problems");
            return result.Xyz / result.W;
        }

        public Ray RaycastFromScreen(Vector2 point)
        {
            var viewPoint = ScreenPointToViewport(point);

            var cameraMatrix = Matrix4.Mult(GetViewMatrix(), GetProjectionMatrix()).Inverted();

            try
            {
                var nearFrustumPoint = ViewportPointToFrustum(new Vector3(viewPoint.X, viewPoint.Y, 0));
                var farFrustumPoint = ViewportPointToFrustum(new Vector3(viewPoint.X, viewPoint.Y, 1));

                var origin = FrustumPointToWorld(nearFrustumPoint, cameraMatrix);
                var target = FrustumPointToWorld(farFrustumPoint, cameraMatrix);

                return Ray.FromPoints(origin, target);
            }
            catch
            {
                return null;
            }
        }

        #endregion


        public float GetViewHeight(float distFromCamera)
        {
            if (IsPerspective)
                return (float)Math.Tan(FieldOfView / 2f) * distFromCamera * 2f;

            return OrthographicSize;
        }

        public float GetDistanceForSize(Vector2 size)
        {
            float value1 = (size.Y / 2f) / (float)Math.Tan(FieldOfView / 2f);
            float value2 = (size.X / AspectRatio / 2f) / (float)Math.Tan(FieldOfView / 2f);
            return Math.Max(value1, value2);
        }

        public float GetDistanceFromCamera(Vector3 worldPos)
        {
            var posOffset = worldPos - Position;

            var angleFromFwrd = Vector3.CalculateAngle(posOffset.Normalized(), Forward);

            if (float.IsNaN(angleFromFwrd))
                return posOffset.Length;

            //distance from camera is equal to adjacent side on the triangle formed by camera, specified pos and a point along camera foward axis
            return (float)Math.Cos(angleFromFwrd) * posOffset.Length;
        }

        public void FitOrtographicSize(Vector2 size)
        {
            float width = size.Y * AspectRatio;
            if (width < size.X)
                OrthographicSize = size.X / AspectRatio;
            else
                OrthographicSize = size.Y;

        }

        public Vector2 GetViewSize(float distFromCamera)
        {
            var vh = GetViewHeight(distFromCamera);
            return new Vector2(vh * AspectRatio, vh);
        }
    }
}
