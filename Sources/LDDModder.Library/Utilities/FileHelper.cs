﻿using System;
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

        public static bool IsValidDirectory(string directory)
        {
            if (directory.ContainsAny(Path.GetInvalidPathChars()))
                return false;
            return true;
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

        public static string GetTempDirectory()
        {
            return Path.Combine(Path.GetTempPath(), LDDModder.Utilities.StringUtils.GenerateUID(8));
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
    }
}
