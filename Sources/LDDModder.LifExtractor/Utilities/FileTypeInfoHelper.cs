using LDDModder.LifExtractor.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Utilities
{
    static class FileTypeInfoHelper
    {
        private static Dictionary<string, string> TypeDescriptions;

        static FileTypeInfoHelper()
        {
            TypeDescriptions = new Dictionary<string, string>();
        }

        public static string GetFileTypeDescription(string fileNameOrExtension)
        {
            var fileExt = fileNameOrExtension;
            if (fileNameOrExtension.IndexOf(".") > 0)
                fileExt = Path.GetExtension(fileNameOrExtension);
            fileExt = fileExt.ToUpper();

            if (TypeDescriptions.ContainsKey(fileExt))
                return TypeDescriptions[fileExt];

            var result = Shell32.SHGetFileInfo(fileNameOrExtension, Shell32.FILE_ATTRIBUTE.NORMAL, out SHFILEINFO shfi, 
                (uint)Marshal.SizeOf<SHFILEINFO>(), 
                Shell32.SHGFI.USEFILEATTRIBUTES | Shell32.SHGFI.TYPENAME);

            TypeDescriptions[fileExt] = result != IntPtr.Zero ? shfi.szTypeName : string.Empty;

            return TypeDescriptions[fileExt];
        }

        public static Icon GetFileTypeIconNormal(string fileNameOrExtension)
        {
            var result = Shell32.SHGetFileInfo(fileNameOrExtension, Shell32.FILE_ATTRIBUTE.NORMAL, out SHFILEINFO shfi,
                (uint)Marshal.SizeOf<SHFILEINFO>(),
                Shell32.SHGFI.USEFILEATTRIBUTES | Shell32.SHGFI.ICON);

            return result != IntPtr.Zero && shfi.hIcon != IntPtr.Zero ? Icon.FromHandle(shfi.hIcon) : null;
        }

        public static Icon GetFileTypeIconSmall(string fileNameOrExtension)
        {
            return GetFileTypeIconSmall(fileNameOrExtension, out _);
        }

        public static Icon GetFileTypeIconSmall(string fileNameOrExtension, out bool isKnowExtension)
        {
            var result = Shell32.SHGetFileInfo(fileNameOrExtension, Shell32.FILE_ATTRIBUTE.NORMAL, out SHFILEINFO shfi,
                (uint)Marshal.SizeOf<SHFILEINFO>(),
                Shell32.SHGFI.USEFILEATTRIBUTES | Shell32.SHGFI.ICON | Shell32.SHGFI.SMALLICON);
            isKnowExtension = result != IntPtr.Zero && shfi.iIcon > 0;
            return result != IntPtr.Zero && shfi.hIcon != IntPtr.Zero ? Icon.FromHandle(shfi.hIcon) : null;
        }

        public static Icon GetFileTypeIconLarge(string fileNameOrExtension)
        {
            var result = Shell32.SHGetFileInfo(fileNameOrExtension, Shell32.FILE_ATTRIBUTE.NORMAL, out SHFILEINFO shfi,
                (uint)Marshal.SizeOf<SHFILEINFO>(),
                Shell32.SHGFI.USEFILEATTRIBUTES | Shell32.SHGFI.ICON | Shell32.SHGFI.LARGEICON);

            return result != IntPtr.Zero && shfi.hIcon != IntPtr.Zero ? Icon.FromHandle(shfi.hIcon) : null;
        }
    }
}
