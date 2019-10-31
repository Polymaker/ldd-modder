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

        public bool HasInitialized { get; private set; }

        public bool ContainsMouse { get; set; }

        public void HandleInputs()
        {
            LastMouseState = MouseState;
            LastKeyboardState = KeyboardState;
        }

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
        }

        public bool IsKeyDown(Key key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Key key)
        {
            return KeyboardState.IsKeyUp(key);
        }

        public bool HasKeyChanged(Key key)
        {
            return LastKeyboardState.IsKeyDown(key) != KeyboardState.IsKeyDown(key);
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

    }
}
