using LDDModder.LifExtractor.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor
{
    static class NativeMethods
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public static int MakeWord(int wLow, int wHigh)
        {
            return (wHigh << 16) + (wLow & 0xFFFF);
        }

        #region Dark Theme Tests

        [Flags]
        public enum LoadLibraryFlags : uint
        {
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, uint procName);

        private delegate void AllowDarkModeForWindowDelegate(IntPtr handle, bool flag);

        public static void AllowDarkModeForWindow(IntPtr handle, bool flag)
        {
            var hModule = NativeMethods.LoadLibraryEx(@"C:\Windows\System32\uxtheme.dll", IntPtr.Zero, NativeMethods.LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
            var procAddress = NativeMethods.GetProcAddress(hModule, 133);
            var funcDelegate = (AllowDarkModeForWindowDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(AllowDarkModeForWindowDelegate));
            funcDelegate(handle, flag);
            FreeLibrary(hModule);
        }

        #endregion

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        public static string FormatFileSize(long fileSize)
        {
            var sb = new StringBuilder(255);
            StrFormatByteSize(fileSize, sb, sb.Capacity);
            return sb.ToString();
        }

        #region ListView

        public const int WM_CHANGEUISTATE = 0x0127;
        public const int UIS_SET = 1;
        public const int UIS_CLEAR = 2;
        public const int UISF_HIDEFOCUS = 0x1;

        #endregion

        public static bool CopyFiles(IEnumerable<string> items, string destination)
        {
            try
            {
                var fs = new Shell32.SHFILEOPSTRUCT
                {
                    wFunc = Shell32.FileOperationType.FO_COPY,
                    pFrom = string.Join("\0", items.ToArray()) + '\0' + '\0',
                    pTo = destination + '\0' + '\0'
                };
                int result = Shell32.SHFileOperation(ref fs);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteFileOrFolder(string destination)
        {
            try
            {
                var fs = new Shell32.SHFILEOPSTRUCT
                {
                    wFunc = Shell32.FileOperationType.FO_DELETE,
                    pFrom = destination + '\0' + '\0',
                    fFlags = Shell32.FileOperationFlags.FOF_NOCONFIRMATION | 
                    Shell32.FileOperationFlags.FOF_NOERRORUI | 
                    Shell32.FileOperationFlags.FOF_SILENT
                };
                Shell32.SHFileOperation(ref fs);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
