using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Native
{
    static class UxTheme
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, uint procName);


        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);


        [System.Flags]
        enum LoadLibraryFlags : uint
        {
            None = 0,
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
            LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
            LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
            LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }

        private static Dictionary<string, IntPtr> ProcAddresses;

        static UxTheme()
        {
            IntPtr hModule = LoadLibraryEx("uxtheme.dll", IntPtr.Zero, LoadLibraryFlags.LOAD_LIBRARY_SEARCH_SYSTEM32 | LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);

            ProcAddresses = new Dictionary<string, IntPtr>();

            ProcAddresses.Add("AllowDarkModeForWindow", GetProcAddress(hModule, 133));
        }

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private delegate bool AllowDarkModeForWindowDelegate(IntPtr hWnd, bool allow);


        public static bool AllowDarkModeForWindow(IntPtr hWnd, bool allow)
        {
            var procDelegate = GetProcDelegate<AllowDarkModeForWindowDelegate>("AllowDarkModeForWindow");
            return procDelegate(hWnd, allow);
        }

        private static T GetProcDelegate<T>(string procName)
        {
            var procPtr = ProcAddresses[procName];
            return Marshal.GetDelegateForFunctionPointer<T>(procPtr);
        }
    }
}
