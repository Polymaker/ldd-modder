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

        public bool IsDirty => isViewMatrixDirty || isProjectionMatrixDirty;

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

        #region Space Convertion Functions (World, Viewport, Screen)

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

        /// <summary>
        /// Transforms position from viewport space into screen space.
        /// </summary>
        /// <param name="viewportPoint">Viewport point. The bottom-left is (0,0); the top-right is (1,1)</param>
        /// <returns>Returns a screen space coordinate.</returns>
        public Vector2 ViewportPointToScreen(Vector2 viewportPoint)
        {
            var viewRect = Viewport;
            return new Vector2(
                (int)(viewRect.X + viewRect.Width * viewportPoint.X),
                (int)(viewRect.Y + viewRect.Height * (1f - viewportPoint.Y)));
        }

        /// <summary>
        /// Transforms position from viewport space into screen space.
        /// </summary>
        /// <param name="viewportPoint">Viewport point. The bottom-left is (0,0); the top-right is (1,1); front is 0; back is 1</param>
        /// <returns></returns>
        public Vector2 ViewportPointToScreen(Vector3 viewportPoint)
        {
            return ViewportPointToScreen(viewportPoint.Xy); //we discard Z as it doesn't affect screen space, only world space
        }

        /// <summary>
        /// Converts Viewport point (0.0 to 1.0) to 'Frustum' (3D) point (-1.0 to +1.0)
        /// </summary>
        /// <param name="viewportPoint"></param>
        /// <param name="z"></param>
        /// <returns>Returns a frustum space coordinate.</returns>
        public static Vector4 ViewportPointToFrustum(Vector3 viewportPoint)
        {
            return new Vector4(
                viewportPoint.X * 2f - 1f,
                viewportPoint.Y * 2f - 1f,
                viewportPoint.Z * 2f - 1f,
                1f);
        }

        private static Vector3 FrustumPointToViewPort(Vector4 frustumPoint)
        {
            var pos = frustumPoint.Xyz / frustumPoint.W;
            return new Vector3((pos.X + 1f) / 2f, (pos.Y + 1f) / 2f, (pos.Z + 1f) / 2f);
        }

        public Vector4 WorldPointToFrustum(Vector3 worldPoint)
        {
            var transformMatrix = Matrix4.Mult(GetViewMatrix(), GetProjectionMatrix());
            var result = Vector4.Transform(new Vector4(worldPoint, 1), transformMatrix);
            //if result.Z < 0, point is behind camera (I think, not tested)
            return result;
        }

        private static Vector3 FrustumPointToWorld(Vector4 frustumPoint, Matrix4 cameraMatrix)
        {
            var result = Vector4.Transform(frustumPoint, cameraMatrix);
            if (result.W == 0)
                throw new Exception("Problems");
            return result.Xyz / result.W;
        }

        public Vector3 WorldPointToViewport(Vector3 worldPoint)
        {
            return FrustumPointToViewPort(WorldPointToFrustum(worldPoint));
        }

        public Vector2 WorldPointToScreen(Vector3 worldPoint)
        {
            return ViewportPointToScreen(WorldPointToViewport(worldPoint));
        }

        #endregion

        #region Raycast 


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

        #region Viewport size and functions

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

        #endregion

        public float DistanceFromCamera(Matrix4 transform)
        {
            var pos = Vector3.TransformPosition(Vector3.Zero, transform);
            return Vector3.Distance(pos, Position);
        }
    }
}
