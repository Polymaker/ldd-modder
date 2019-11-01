using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CameraManipulator(Camera camera)
        {
            Camera = camera;
            MinimumZoom = 0.1f;
        }

        public void HandleCamera(InputManager input)
        {
            if (!input.ContainsMouse)
                return;

            //var viewSize = new Vector2(Camera.Viewport.Width, Camera.Viewport.Height);
            var mouseDelta = input.MousePos - input.LastMousePos;


            if (mouseDelta.LengthFast > 1)
            {
                var scaledMouseDelta = mouseDelta / 500f;//  Vector2.Divide(mouseDelta, viewSize);

                if (input.IsButtonDown(MouseButton.Middle))
                {
                    var gimbalTrans = Matrix4.CreateTranslation(Gimbal);
                    var localTrans = Camera.Transform * gimbalTrans.Inverted();

                    if (input.IsKeyDown(Key.ControlLeft) || input.IsKeyDown(Key.ControlRight))
                    {
                        if (Camera.IsPerspective)
                        {
                            var scrollAmount = scaledMouseDelta.Y * CameraDistance;
                            var newPos = Camera.Position + (Camera.Forward * scrollAmount * -1);
                            if ((Gimbal - newPos).Length < MinimumZoom)
                                newPos = Gimbal + (Camera.Forward * MinimumZoom * -1f);
                            Camera.Position = newPos;
                        }

                        //var rollRotation = Matrix4.CreateFromAxisAngle(Camera.Forward, scaledMouseDelta.X * (float)Math.PI);
                        //Camera.Transform = localTrans * rollRotation;
                    }
                    else if (input.IsKeyDown(Key.ShiftLeft) || input.IsKeyDown(Key.ShiftRight))
                    {
                        var viewSizeAtGimbal = Camera.GetViewSize(CameraDistance) / 2f;
                        var panning = Camera.Up * scaledMouseDelta.Y * viewSizeAtGimbal.Y + 
                            Camera.Right * scaledMouseDelta.X * viewSizeAtGimbal.X;
                        _Gimbal += panning;
                        Camera.Position += panning;
                    }
                    else
                    {
                        

                        var pitchRotation = Matrix4.CreateFromAxisAngle(Camera.Right, scaledMouseDelta.Y * (float)Math.PI);
                        var yawRotation = Matrix4.CreateFromAxisAngle(Vector3.UnitY, scaledMouseDelta.X * (float)Math.PI * -1f);

                        var combinedRot = pitchRotation * yawRotation; //!Important, this order is less prone to induce roll

                        var finalTranform = localTrans * (combinedRot * gimbalTrans);

                        Camera.Transform = finalTranform;

                        //var newPos = Vector3.TransformPosition(camOffset, yawRotation);
                        //newPos = Vector3.TransformPosition(newPos, pitchRotation);
                        //Camera.Position = newPos + Gimbal;
                        //Camera.LookAt(Gimbal, Vector3.UnitY);
                    }
                }
            }

            var mouseWheelDelta = input.MouseState.WheelPrecise - input.LastMouseState.WheelPrecise;

            if (mouseWheelDelta != 0)
            {
                if (Camera.IsPerspective)
                {
                    float zoomAmount = mouseWheelDelta * (CameraDistance / 10f);
                    Camera.Position += Camera.Forward * zoomAmount;
                }
                else
                {

                }
            }
        }
    }
}
