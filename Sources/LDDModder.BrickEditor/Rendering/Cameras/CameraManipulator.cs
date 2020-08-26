using OpenTK;
using OpenTK.Input;
using System;

namespace LDDModder.BrickEditor.Rendering
{
    public class CameraManipulator
    {
        public Camera Camera { get; }

        private Vector3 _Gimbal;

        public Vector3 Gimbal
        {
            get => _Gimbal;
            set
            {
                if (value != _Gimbal)
                {
                    _Gimbal = value;
                    Camera.LookAt(_Gimbal, Vector3.UnitY);
                }
            }
        }

        public float MinimumZoom { get; set; }

        public float CameraDistance
        {
            get => (Camera.Position - Gimbal).Length;
        }

        public MouseButton RotationButton { get; set; }

        public CameraManipulator(Camera camera)
        {
            Camera = camera;
            MinimumZoom = 0.1f;
            RotationButton = MouseButton.Middle;
        }

        public void Initialize(Vector3 cameraPosition, Vector3 gimbalPosition)
        {
            Camera.Position = cameraPosition;
            Camera.LookAt(gimbalPosition, Vector3.UnitY);
            _Gimbal = gimbalPosition;
        }

        public void Initialize(Vector3 cameraPosition, Vector3 gimbalPosition, Vector3 upVector)
        {
            Camera.Position = cameraPosition;
            Camera.LookAt(gimbalPosition, upVector);
            _Gimbal = gimbalPosition;
        }

        public void ProcessInput(InputManager input)
        {
            if (!input.ContainsMouse)
                return;

            //var viewSize = new Vector2(Camera.Viewport.Width, Camera.Viewport.Height);
            var mouseDelta = input.MousePos - input.LastMousePos;

            if (mouseDelta.LengthFast > 1)
            {
                if (input.IsButtonDown(RotationButton))
                {
                    var gimbalTrans = Matrix4.CreateTranslation(Gimbal);
                    var localTrans = Camera.Transform * gimbalTrans.Inverted();

                    if (input.IsKeyDown(Key.ControlLeft) || input.IsKeyDown(Key.ControlRight))
                    {
                        PerformCameraZooming(localTrans, gimbalTrans, mouseDelta / 500f);
                    }
                    else if (input.IsKeyDown(Key.ShiftLeft) || input.IsKeyDown(Key.ShiftRight))
                    {
                        PerformCameraPanning(localTrans, gimbalTrans, mouseDelta);
                    }
                    else
                    {
                        PerformCameraOrbit(localTrans, gimbalTrans, mouseDelta / 500f);
                    }
                }
            }

            var mouseWheelDelta = input.MouseState.WheelPrecise - input.LastMouseState.WheelPrecise;
            bool isZoomingByRotation = input.IsButtonDown(RotationButton) && input.IsControlDown();
            
            if (mouseWheelDelta != 0 && !isZoomingByRotation)
            {
                if (Camera.IsPerspective)
                {
                    float zoomAmount = mouseWheelDelta * (CameraDistance / 10f);
                    var travelAmount = Camera.Forward * zoomAmount;

                    Camera.Position += travelAmount;
                    if (input.IsShiftDown())
                    {
                        Gimbal += travelAmount;
                    }
                }
                else
                {
                    float zoomAmount = mouseWheelDelta * (Camera.OrthographicSize / 10f);
                    Camera.OrthographicSize -= zoomAmount;
                }
            }
        }

        private void PerformCameraZooming(Matrix4 cameraTransform, Matrix4 gimbalTransform, Vector2 mouseDelta)
        {
            if (Camera.IsPerspective)
            {
                var zoomAmount = mouseDelta.Y * CameraDistance;
                var newPos = Camera.Position + (Camera.Forward * zoomAmount * -1);
                if ((Gimbal - newPos).Length < MinimumZoom)
                    newPos = Gimbal + (Camera.Forward * MinimumZoom * -1f);
                Camera.Position = newPos;
            }
            else
            {
                float zoomAmount = mouseDelta.Y * (Camera.OrthographicSize / 2f);
                Camera.OrthographicSize += zoomAmount;
            }
        }

        private void PerformCameraPanning(Matrix4 cameraTransform, Matrix4 gimbalTransform, Vector2 mouseDelta)
        {
            var viewSizeAtGimbal = Camera.GetViewSize(CameraDistance);

            if (Camera.IsPerspective)
            {
                var scaledDelta = Vector2.Multiply(mouseDelta / 500f, viewSizeAtGimbal / 2f);
                //var panning = Camera.Up * scaledDelta.Y +
                //    Camera.Right * scaledDelta.X;
                //var translation = Matrix4.CreateTranslation(scaledDelta.X, scaledDelta.Y, 0);
                //var testTrans = translation * cameraTransform;

                var panning = Vector3.TransformVector(new Vector3(scaledDelta.X, scaledDelta.Y, 0), cameraTransform);
                _Gimbal += panning;
                Camera.Position += panning;
            }
            else
            {
                var viewportSize = new Vector2(Camera.Viewport.Width, Camera.Viewport.Height);
                //does not work properly?? shoud follow the cursor exactly
                var scaledDelta = Vector2.Multiply(Vector2.Divide(mouseDelta, viewportSize), viewSizeAtGimbal / 2f);
                var panning = Vector3.TransformVector(new Vector3(scaledDelta.X, scaledDelta.Y, 0), cameraTransform);
                _Gimbal += panning;
                Camera.Position += panning;
            }
        }

        private void PerformCameraOrbit(Matrix4 cameraTransform, Matrix4 gimbalTransform, Vector2 mouseDelta)
        {
            var pitchRotation = Matrix4.CreateFromAxisAngle(Camera.Right, mouseDelta.Y * (float)Math.PI);
            var yawRotation = Matrix4.CreateFromAxisAngle(Vector3.UnitY, mouseDelta.X * (float)Math.PI * -1f);

            var combinedRot = pitchRotation * yawRotation; //!Important, this order is less prone to induce roll

            var finalTranform = cameraTransform * (combinedRot * gimbalTransform);

            Camera.Transform = finalTranform;
        }
    }
}
