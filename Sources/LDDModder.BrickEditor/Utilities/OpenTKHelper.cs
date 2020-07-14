using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace OpenTK
{
    public static class OpenTKHelper
    {
        private static MethodInfo SetKeyStateMethod;

        static OpenTKHelper()
        {
            SetKeyStateMethod = typeof(KeyboardState).GetMethod("SetKeyState", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void SetKeyState(this KeyboardState keyboardState, Key key, bool down)
        {
            SetKeyStateMethod.Invoke(keyboardState, new object[] { key, down });
        }

        public static Quaternion AverageQuaternion(IEnumerable<Quaternion> quaternions)
        {
            var avgQuat = Quaternion.Identity;
            bool firstItem = true;

            foreach (var quat in quaternions)
            {
                if (firstItem)
                {
                    avgQuat = quat;
                    firstItem = false;
                }
                else
                    avgQuat = Quaternion.Slerp(quat, avgQuat, 0.5f);
            }

            return avgQuat;
        }

        public static IDisposable TempEnable(EnableCap cap)
        {
            return new TempFlag(cap, true);
        }

        public static IDisposable TempDisable(EnableCap cap)
        {
            return new TempFlag(cap, false);
        }

        private class TempFlag : IDisposable
        {
            public EnableCap Flag { get; }
            public bool Enable { get; set; }
            public bool WasEnabled { get; }

            public TempFlag(EnableCap flag, bool enable)
            {
                Flag = flag;
                WasEnabled = GL.IsEnabled(flag);
                Enable = enable;
                if (enable)
                    GL.Enable(flag);
                else
                    GL.Disable(flag);
            }

            public void Dispose()
            {
                if (Enable && !WasEnabled)
                    GL.Disable(Flag);
                else if (!Enable && WasEnabled)
                    GL.Enable(Flag);
            }
        }
    }
}
