using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }
}
