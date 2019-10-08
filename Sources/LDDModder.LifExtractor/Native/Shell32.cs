using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LifExtractor.Native
{
    static class Shell32
    {

        [DllImport("shell32")]
        public static extern IntPtr SHGetFileInfo(string pszPath, FILE_ATTRIBUTE dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, SHGFI flags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        public enum FILE_ATTRIBUTE : uint
        {
            READONLY = 0x00000001,
            HIDDEN = 0x00000002,
            SYSTEM = 0x00000004,
            DIRECTORY = 0x00000010,
            ARCHIVE = 0x00000020,
            DEVICE = 0x00000040,
            NORMAL = 0x00000080,
            TEMPORARY = 0x00000100,
            SPARSE_FILE = 0x00000200,
            REPARSE_POINT = 0x00000400,
            COMPRESSED = 0x00000800,
            OFFLINE = 0x00001000,
            NOT_CONTENT_INDEXED = 0x00002000,
            ENCRYPTED = 0x00004000,
            VIRTUAL = 0x00010000
        }

        public enum SHGFI : uint
        {
            ICON = 0x000000100,     // get icon
            DISPLAYNAME = 0x000000200,     // get display name
            TYPENAME = 0x000000400,     // get type name
            ATTRIBUTES = 0x000000800,     // get attributes
            ICONLOCATION = 0x000001000,     // get icon location
            EXETYPE = 0x000002000,     // return exe type
            SYSICONINDEX = 0x000004000,     // get system icon index
            LINKOVERLAY = 0x000008000,     // put a link overlay on icon
            SELECTED = 0x000010000,     // show icon in selected state
            ATTR_SPECIFIED = 0x000020000,     // get only specified attributes
            LARGEICON = 0x000000000,     // get large icon
            SMALLICON = 0x000000001,     // get small icon
            OPENICON = 0x000000002,     // get open icon
            SHELLICONSIZE = 0x000000004,     // get shell size icon
            PIDL = 0x000000008,     // pszPath is a pidl
            USEFILEATTRIBUTES = 0x000000010,     // use passed dwFileAttribute
        }

        [Flags]
        public enum FileOperationFlags : ushort
        {
            /// <summary>
            /// Do not show a dialog during the process
            /// </summary>
            FOF_SILENT = 0x0004,
            /// <summary>
            /// Do not ask the user to confirm selection
            /// </summary>
            FOF_NOCONFIRMATION = 0x0010,
            /// <summary>
            /// Delete the file to the recycle bin.  (Required flag to send a file to the bin
            /// </summary>
            FOF_ALLOWUNDO = 0x0040,
            /// <summary>
            /// Do not show the names of the files or folders that are being recycled.
            /// </summary>
            FOF_SIMPLEPROGRESS = 0x0100,
            /// <summary>
            /// Surpress errors, if any occur during the process.
            /// </summary>
            FOF_NOERRORUI = 0x0400,
            /// <summary>
            /// Warn if files are too big to fit in the recycle bin and will need
            /// to be deleted completely.
            /// </summary>
            FOF_WANTNUKEWARNING = 0x4000,
        }

        public enum FileOperationType : uint
        {
            /// <summary>
            /// Move the objects
            /// </summary>
            FO_MOVE = 0x0001,
            /// <summary>
            /// Copy the objects
            /// </summary>
            FO_COPY = 0x0002,
            /// <summary>
            /// Delete (or recycle) the objects
            /// </summary>
            FO_DELETE = 0x0003,
            /// <summary>
            /// Rename the object(s)
            /// </summary>
            FO_RENAME = 0x0004,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SHFILEOPSTRUCT
        {

            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public FileOperationType wFunc;
            public string pFrom;
            public string pTo;
            public FileOperationFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }


    }
}
