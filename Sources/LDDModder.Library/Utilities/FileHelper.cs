using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public static class FileHelper
    {
        public static Regex FileNameCleaner;

        static FileHelper()
        {
            var invalidCharsStr = new string(Path.GetInvalidFileNameChars());
            FileNameCleaner = new Regex("[" + Regex.Escape(invalidCharsStr) + "]");
        }

        public static bool IsValidPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            string directory = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            if (!string.IsNullOrEmpty(fileName) && !IsValidFileName(fileName))
                return false;
            if (!string.IsNullOrEmpty(directory) && !IsValidDirectory(directory))
                return false;

            if (string.IsNullOrEmpty(directory) && 
                string.IsNullOrEmpty(fileName) && 
                !string.IsNullOrEmpty(path))
                return false;

            return true;
        }

        public static bool IsValidFileName(string fileName)
        {
            if (fileName.ContainsAny(Path.GetInvalidFileNameChars()))
                return false;
            return true;
        }

        public static bool IsValidDirectory(string directory, bool checkExists = false)
        {
            if (directory.ContainsAny(Path.GetInvalidPathChars()))
                return false;
            return !checkExists || Directory.Exists(directory);
        }

        public static bool IsValidDirectoryName(string directory)
        {
            if (directory.ContainsAny(Path.GetInvalidPathChars()))
                return false;
            if (directory.Contains(Path.PathSeparator))
                return false;
            if (directory.Contains(Path.VolumeSeparatorChar))
                return false;
            return true;
        }

        public static string GetSafeFileName(string fileName)
        {
            return FileNameCleaner.Replace(fileName, string.Empty);
        }

        public static string GetTempDirectory(int nameLength = 8)
        {
            return Path.Combine(Path.GetTempPath(), LDDModder.Utilities.StringUtils.GenerateUID(nameLength));
        }

        public static bool IsInTempFolder(string filepath)
        {
            filepath = Path.GetFileName(filepath).ToUpper();
            return filepath.StartsWith(Path.GetTempPath().ToUpper());
        }

        public static bool DeleteFileOrFolder(string destination, bool permanent, bool silent)
        {
            try
            {
                var fs = new Native.Shell32.SHFILEOPSTRUCT
                {
                    wFunc = Native.Shell32.FileOperationType.FO_DELETE,
                    pFrom = destination + '\0' + '\0',
                };

                if (!permanent)
                    fs.fFlags |= Native.Shell32.FileOperationFlags.FOF_ALLOWUNDO;

                if (silent)
                {
                    fs.fFlags |= Native.Shell32.FileOperationFlags.FOF_NOCONFIRMATION |
                        Native.Shell32.FileOperationFlags.FOF_NOERRORUI |
                        Native.Shell32.FileOperationFlags.FOF_SILENT;
                }

                Native.Shell32.SHFileOperation(ref fs);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CopyFiles(IEnumerable<string> items, string destination, bool silent)
        {
            try
            {
                var fs = new Native.Shell32.SHFILEOPSTRUCT
                {
                    wFunc = Native.Shell32.FileOperationType.FO_COPY,
                    pFrom = string.Join("\0", items.ToArray()) + '\0' + '\0',
                    pTo = destination + '\0' + '\0'
                };

                if (silent)
                {
                    fs.fFlags |= Native.Shell32.FileOperationFlags.FOF_NOCONFIRMATION |
                        Native.Shell32.FileOperationFlags.FOF_NOERRORUI |
                        Native.Shell32.FileOperationFlags.FOF_SILENT;
                }

                int result = Native.Shell32.SHFileOperation(ref fs);

                return result == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool MoveFile(string sourceFileName, string destFileName, bool silent)
        {
            return MoveFiles(new string[] { sourceFileName }, destFileName, silent);
        }

        public static bool MoveFiles(IEnumerable<string> items, string destination, bool silent)
        {
            try
            {
                var fs = new Native.Shell32.SHFILEOPSTRUCT
                {
                    wFunc = Native.Shell32.FileOperationType.FO_MOVE,
                    pFrom = string.Join("\0", items.ToArray()) + '\0' + '\0',
                    pTo = destination + '\0' + '\0'
                };

                if (silent)
                {
                    fs.fFlags |= Native.Shell32.FileOperationFlags.FOF_NOCONFIRMATION |
                        Native.Shell32.FileOperationFlags.FOF_NOERRORUI |
                        Native.Shell32.FileOperationFlags.FOF_SILENT;
                }

                int result = Native.Shell32.SHFileOperation(ref fs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
