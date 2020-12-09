using LDDModder.BrickEditor.UI.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

            public static AppInstanceInfo Parse(string text)
            {
                var parts = text.Split(';');
                if (parts.Length < 2)
                    return null;

                if (!int.TryParse(parts[0], out int procID))
                    return null;
                if (!int.TryParse(parts[1], out int handleID))
                    return null;

                return new AppInstanceInfo()
                {
                    ProcessID = procID,
                    WindowHandle = (IntPtr)handleID,
                    InstanceID = parts.Length > 2 ? parts[2] : string.Empty
                };
            }
        }

        public static AppInstanceInfo Current { get; private set; }

        public static string ProcessName { get; private set; }

        public static BrickEditorWindow MainWindow { get; set; }

        public static string InstanceID => Current?.InstanceID;

        public static List<AppInstanceInfo> AppInstances { get; private set; }

        public static int InstanceCount => AppInstances.Count + 1;

        private static Dictionary<string, Action<AppInstanceInfo, string>> MessageHandlers;

        static MultiInstanceManager()
        {
            AppInstances = new List<AppInstanceInfo>();
            MessageHandlers = new Dictionary<string, Action<AppInstanceInfo, string>>();
        }

        public static void Initialize(BrickEditorWindow mainWindow)
        {
            var myProc = Process.GetCurrentProcess();
            ProcessName = myProc.ProcessName;

            Current = new AppInstanceInfo()
            {
                WindowHandle = myProc.MainWindowHandle,
                ProcessID = myProc.Id,
                InstanceID = Guid.NewGuid().ToString("N")
            };

            MainWindow = mainWindow;

            if (Current.WindowHandle == IntPtr.Zero)
                Current.WindowHandle = mainWindow.Handle;

            MessageHandlers.Add(MSG_NOTIFY, OnNotify);
            MessageHandlers.Add(MSG_QUERY_FILE_OPEN, OnCheckFileOpen);


            PollActiveInstances();
        }

        public const string MSG_HEADER = "MULTI#";
        public const string MSG_NOTIFY = "NOTIFY";
        public const string MSG_QUERY_FILE_OPEN = "QFO";
        public const string MSG_RETURN_FILE_OPEN = "RFO";

        public static bool ProcessMessage(ref Message m)
        {
            if (m.Msg == WM_COPYDATA)
            {
                var dataStruct = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                var messageStr = Marshal.PtrToStringUni(dataStruct.lpData, dataStruct.cbData / 2);
                if (!messageStr.StartsWith(MSG_HEADER))
                    return false;

                var sender = AppInstances.FirstOrDefault(x => x.WindowHandle == dataStruct.dwData);

                messageStr = messageStr.Substring(MSG_HEADER.Length);
                int actionIndex = messageStr.IndexOf('#');

                if (actionIndex > 0)
                {
                    string actionName = messageStr.Substring(0, actionIndex);
                    messageStr = messageStr.Substring(actionIndex + 1);

                    if (MessageHandlers.ContainsKey(actionName))
                        MessageHandlers[actionName](sender, messageStr);
                }
                return true;
            }
            return false;
        }

        #region MyRegion

        public static void PollActiveInstances()
        {
            AppInstances.Clear();
            var otherProcs = Process.GetProcessesByName(ProcessName);
            
            if (otherProcs.Length > 1)
            {
                foreach (var proc in otherProcs)
                {
                    if (proc.Id == Current.ProcessID)
                        continue;

                    var instanceInfo = new AppInstanceInfo()
                    {
                        ProcessID = proc.Id,
                        WindowHandle = proc.MainWindowHandle
                    };
                    AppInstances.Add(instanceInfo);
                    SendAction(instanceInfo, MSG_NOTIFY, Current.ToString());
                    break;
                }
            }
        }

        private static bool CheckIsActive(AppInstanceInfo instance)
        {
            bool isActive = false;
            try
            {
                var instanceProc = Process.GetProcessById(instance.ProcessID);
                isActive = (instanceProc != null && instanceProc.ProcessName == ProcessName);
            }
            catch { }

            if (!isActive)
                AppInstances.Remove(instance);

            return isActive;
        }

        #endregion

        #region IPC External methods

        //public static async Task<bool> CheckFileIsOpen(string filepath)
        //{
        //    bool isOpen = false;
        //    foreach (var appInstance in AppInstances)
        //    {
        //        GetActionResult(appInstance, MSG_QUERY_FILE_OPEN, MSG_RETURN_FILE_OPEN, filepath);

        //        var responseTask = AwaitResponse(MSG_RETURN_FILE_OPEN);
        //        SendAction(appInstance, MSG_QUERY_FILE_OPEN, filepath);
        //        string result = await responseTask;
        //        bool.TryParse(result, out isOpen);
        //        Trace.WriteLine($"PID {appInstance.ProcessID} CheckFileIsOpen() => {isOpen}");

        //        if (bool.TryParse(result, out isOpen) && isOpen)
        //            break;
        //    }
        //    return isOpen;
        //}

        public static bool CheckFileIsOpen(string filepath)
        {
            bool isOpen = false;
            var task = Task.Factory.StartNew(() =>
            {
                foreach (var appInstance in AppInstances)
                {
                    string result = GetActionResult(appInstance, MSG_QUERY_FILE_OPEN, MSG_RETURN_FILE_OPEN, filepath);
                    //bool.TryParse(result, out isOpen);
                    //Trace.WriteLine($"PID {appInstance.ProcessID} CheckFileIsOpen() => {isOpen}");

                    if (bool.TryParse(result, out isOpen) && isOpen)
                        break;
                }
            });
            task.Wait();
            return isOpen;
        }

        #endregion

        #region IPC Invokable methods

        private static void OnNotify(AppInstanceInfo sender, string data)
        {
            var instanceInfo = AppInstanceInfo.Parse(data);

            if (!AppInstances.Any(a => a.ProcessID == instanceInfo.ProcessID))
                AppInstances.Add(instanceInfo);
        }

        private static void OnCheckFileOpen(AppInstanceInfo sender, string data)
        {
            bool isFileOpen = MainWindow?.IsFileOpen(data) ?? false;
            Trace.WriteLine($"PID: {Current.ProcessID} IsFileOpen('{data}') => {isFileOpen}");
            SendAction(sender, MSG_RETURN_FILE_OPEN, isFileOpen.ToString());
        }

        #endregion

        //private static async Task<string> AwaitResponse(string action)
        //{
        //    var mre = new ManualResetEvent(false);
        //    string actionResult = null;

        //    MessageHandlers.Add(action, (AppInstanceInfo sender, string msg) =>
        //    {
        //        mre.Set();
        //        actionResult = msg;
        //    });

        //    var waitTask = Task.Factory.StartNew(() =>
        //    {
        //        mre.WaitOne(2000);
        //        Trace.WriteLine("ManualResetEvent is set!");
        //    });

        //    await waitTask;

        //    mre.Dispose();
        //    MessageHandlers.Remove(action);
        //    return actionResult;
        //}

        private static void SendAction(AppInstanceInfo instance, string actionName, string message)
        {
            try
            {
                SendDataMessage(Current.WindowHandle, instance.WindowHandle, $"{MSG_HEADER}{actionName}#{message}");
            }
            catch
            {
                CheckIsActive(instance);
            }
        }

        private static string GetActionResult(AppInstanceInfo instance, string actionName, string resultName, string data)
        {
            var mre = new ManualResetEvent(false);
            string actionResult = null;

            MessageHandlers.Add(resultName, (AppInstanceInfo sender, string msg) =>
            {
                mre.Set();
                actionResult = msg;
            });

            SendAction(instance, actionName, data);

            mre.WaitOne(1500);
            mre.Dispose();
            MessageHandlers.Remove(resultName);

            return actionResult;
        }

        //private class IPCMessage
        //{
        //    public string Action { get; set; }
        //    public 
        //}

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

        public static void SendDataMessage(IntPtr sourceProc, IntPtr targetProc, string msg)
        {
            IntPtr messagePtr = Marshal.StringToHGlobalUni(msg);

            var copyData = new COPYDATASTRUCT
            {
                dwData = sourceProc,
                lpData = messagePtr,
                cbData = msg.Length * 2
            };

            IntPtr dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<COPYDATASTRUCT>());
            Marshal.StructureToPtr(copyData, dataPtr, false);

            SendMessage(targetProc, WM_COPYDATA, IntPtr.Zero, dataPtr);

            copyData = Marshal.PtrToStructure<COPYDATASTRUCT>(dataPtr);

            //if (copyData.lpData != messagePtr)
            //{

            //    MessageBox.Show("TEST");
            //}
            Marshal.FreeHGlobal(dataPtr);
            Marshal.FreeHGlobal(messagePtr);
        }

        #endregion
    }
}
