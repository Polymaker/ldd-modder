using LDDModder.BrickEditor.UI.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.Utilities
{
    static class MultiInstanceManager
    {
        public class AppInstanceInfo
        {
            public int ProcessID { get; set; }
            public IntPtr WindowHandle { get; set; }
            public string InstanceID { get; set; }

            public override string ToString()
            {
                return $"{ProcessID};{WindowHandle};{InstanceID}";
            }
        }

        public static AppInstanceInfo Current { get; private set; }

        public static BrickEditorWindow MainWindow { get; set; }

        public static string InstanceID => Current?.InstanceID;

        public static List<AppInstanceInfo> AppInstances { get; private set; }

        static MultiInstanceManager()
        {
            AppInstances = new List<AppInstanceInfo>();
        }

        public static void CheckInstances()
        {
            var myProc = Process.GetCurrentProcess();

            Current = new AppInstanceInfo()
            {
                WindowHandle = myProc.MainWindowHandle,
                ProcessID = myProc.Id,
                InstanceID = Guid.NewGuid().ToString("N")
            };

            if (Current.WindowHandle == IntPtr.Zero)
                Current.WindowHandle = MainWindow.Handle;

            var otherProcs = Process.GetProcessesByName(myProc.ProcessName);

            if (otherProcs.Length > 1)
            {
                foreach (var proc in otherProcs)
                {
                    if (proc.Id == myProc.Id)
                        continue;

                    var instanceInfo = new AppInstanceInfo()
                    {
                        ProcessID = proc.Id,
                        WindowHandle = proc.MainWindowHandle
                    };
                    AppInstances.Add(instanceInfo);
                    break;
                }
            }
        }

        public const string MSG_HEADER = "MULTI#";

        public static bool ProcessMessage(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                var dataStruct = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                var messageStr = Marshal.PtrToStringUni(dataStruct.lpData, dataStruct.cbData / 2);
                if (!messageStr.StartsWith(MSG_HEADER))
                    return false;

                messageStr = messageStr.Substring(MSG_HEADER.Length);
                int actionIndex = messageStr.IndexOf('#');

                if (actionIndex > 0)
                {
                    string actionName = messageStr.Substring(0, actionIndex);
                    messageStr = messageStr.Substring(actionIndex + 1);

                    //Marshal.FreeHGlobal(dataStruct.lpData);
                    //dataStruct.lpData = Marshal.StringToHGlobalUni(Current.ToString());
                    //Marshal.StructureToPtr(dataStruct, m.LParam, false);
                }
                return true;
            }
            return false;
        }

        #region MyRegion



        #endregion

        private static void SendAction(IntPtr procHandle, string actionName, string message)
        {
            SendDataMessage(procHandle, $"{MSG_HEADER}{actionName}#{message}");

        }

        #region Native methods

        public const int WM_COPYDATA = 0x004A;
        public const int WM_USER = 0x0400;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;    // Any value the sender chooses.  Perhaps its main window handle?
            public int cbData;       // The count of bytes in the message.
            public IntPtr lpData;    // The address of the message.
        }

        [DllImport("user32", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr Hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public static void SendDataMessage(IntPtr procHandle, string msg)
        {
            IntPtr messagePtr = Marshal.StringToHGlobalUni(msg);

            var copyData = new COPYDATASTRUCT
            {
                dwData = IntPtr.Zero,
                lpData = messagePtr,
                cbData = msg.Length * 2
            };

            IntPtr dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<COPYDATASTRUCT>());
            Marshal.StructureToPtr(copyData, dataPtr, false);

            SendMessage(procHandle, WM_COPYDATA, IntPtr.Zero, dataPtr);

            copyData = Marshal.PtrToStructure<COPYDATASTRUCT>(dataPtr);

            if (copyData.lpData != messagePtr)
            {

                MessageBox.Show("TEST");
            }
            Marshal.FreeHGlobal(dataPtr);
            Marshal.FreeHGlobal(messagePtr);
        }

        #endregion
    }
}
