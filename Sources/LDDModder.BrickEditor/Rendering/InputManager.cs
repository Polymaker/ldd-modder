using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class InputManager
    {
        public Vector2 LastMousePos { get; set; }
        public MouseState LastMouseState { get; set; }
        public KeyboardState LastKeyboardState { get; set; }

        public Vector2 MousePos { get; set; }
        public MouseState MouseState { get; set; }
        public KeyboardState KeyboardState { get; set; }

        public Vector2 LocalMousePos { get; private set; }

        public bool HasInitialized { get; private set; }

        public bool ContainsMouse { get; set; }

        public Vector2[] MouseDownPositions { get; set; }

        public Vector2[] MouseUpPositions { get; set; }

        public float ClickTolerence { get; set; }

        public bool MouseClickHandled { get; set; }

        public InputManager()
        {
            MouseDownPositions = new Vector2[13];
            MouseUpPositions = new Vector2[13];
            ClickedButtons = new bool[13];
            ClickTolerence = 2f;
        }

        private readonly bool[] ClickedButtons;

        public void UpdateInputStates()
        {
            LastMouseState = MouseState;
            LastKeyboardState = KeyboardState;
            LastMousePos = MousePos;
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            MousePos = new Vector2(MouseState.X, MouseState.Y);

            if (!HasInitialized)
            {
                LastMouseState = MouseState;
                LastKeyboardState = KeyboardState;
                LastMousePos = MousePos;
                HasInitialized = true;
            }

            MouseClickHandled = false;

            for (int i = 0; i < (int)MouseButton.LastButton; i++)
            {
                ClickedButtons[i] = false;
                if (IsButtonPressed((MouseButton)i))
                    MouseDownPositions[i] = ContainsMouse ? MousePos : new Vector2(99999,99999);

                if (IsButtonReleased((MouseButton)i))
                {
                    MouseUpPositions[i] = MousePos;
                    var dist = (MouseUpPositions[i] - MouseDownPositions[i]).Length;
                    if (dist <= ClickTolerence && ContainsMouse)
                        ClickedButtons[i] = true;
                }
            }
        }

        public void ProcessMouseMove(System.Windows.Forms.MouseEventArgs mouseEvent)
        {
            LocalMousePos = new Vector2(mouseEvent.X, mouseEvent.Y);
        }

        #region State functions

        public bool IsKeyDown(Key key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public bool IsControlDown()
        {
            return IsKeyDown(Key.ControlLeft) || IsKeyDown(Key.ControlRight);
        }

        public bool IsShiftDown()
        {
            return IsKeyDown(Key.ShiftLeft) || IsKeyDown(Key.ShiftRight);
        }

        public bool IsKeyUp(Key key)
        {
            return KeyboardState.IsKeyUp(key);
        }

        public bool HasKeyChanged(Key key)
        {
            return LastKeyboardState.IsKeyDown(key) != KeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Key key)
        {
            return KeyboardState.IsKeyDown(key) && !LastKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Key key)
        {
            return KeyboardState.IsKeyUp(key) && !LastKeyboardState.IsKeyUp(key);
        }

        public bool IsButtonDown(MouseButton button)
        {
            return MouseState.IsButtonDown(button);
        }

        public bool IsButtonUp(MouseButton button)
        {
            return MouseState.IsButtonUp(button);
        }

        public bool HasButtonChanged(MouseButton button)
        {
            return LastMouseState.IsButtonDown(button) != MouseState.IsButtonDown(button);
        }

        public bool IsButtonPressed(MouseButton button)
        {
            return MouseState.IsButtonDown(button) && !LastMouseState.IsButtonDown(button);
        }

        public bool IsButtonReleased(MouseButton button)
        {
            return !MouseState.IsButtonDown(button) && LastMouseState.IsButtonDown(button);
        }

        public bool IsButtonClicked(MouseButton button)
        {
            return ClickedButtons[(int)button];
        }

        #endregion

    }
}
