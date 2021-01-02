using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

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

        private static Type xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");

        public static IWindowInfo GetWindowInfo(IntPtr handle, bool isControl = false)
        {
            if (Configuration.RunningOnWindows)
                return OpenTK.Platform.Utilities.CreateWindowsWindowInfo(handle);

            if (Configuration.RunningOnMacOS)
                return OpenTK.Platform.Utilities.CreateMacOSCarbonWindowInfo(handle, false, isControl);

            if (Configuration.RunningOnSdl2)
                return OpenTK.Platform.Utilities.CreateSdl2WindowInfo(handle);

            if (Configuration.RunningOnX11)
            {
                try
                {
                    if (xplatui != null)
                    {
                        var display = (IntPtr)xplatui.GetField("DisplayHandle", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                        var rootWindow = (IntPtr)xplatui.GetField("RootWindow", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                        int screen = (int)xplatui.GetField("ScreenNo", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                        return Utilities.CreateX11WindowInfo(display, screen, handle, rootWindow, IntPtr.Zero);
                    }
                }
                catch { }
            }

            return null;
        }
    }
}
