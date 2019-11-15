using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Native
{
    static class NativeMethods
    {
        public static bool DeleteFileOrFolder(string path, bool silent = false, bool sendToRecycleBin = true)
        {
            try
            {
                var fs = new Shell32.SHFILEOPSTRUCT
                {
                    wFunc = Shell32.FileOperationType.FO_DELETE,
                    pFrom = path + '\0' + '\0',
                };

                if (silent)
                {
                    fs.fFlags |= Shell32.FileOperationFlags.FOF_NOCONFIRMATION |
                        Shell32.FileOperationFlags.FOF_SILENT |
                        Shell32.FileOperationFlags.FOF_NOERRORUI;
                }
                if (sendToRecycleBin)
                    fs.fFlags |= Shell32.FileOperationFlags.FOF_ALLOWUNDO;

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
