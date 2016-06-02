using ICSharpCode.SharpZipLib.Zip;
using LDDModder.LDD.Files;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.LDD
{
    public static class LDDManager
    {
        private static string _ApplicationDataPath;
        private static string _ApplicationPath;

        public const string EXE_NAME = "LDD.exe";
        public const string APP_DIR = "LEGO Company\\LEGO Digital Designer";
        public const string USER_MODELS_DIR = "LEGO Creations\\Models";

        public const string LIF_EXT = ".lif";
        public const string LIF_ASSET_NAME = "Assets";
        public const string LIF_DB_NAME = "db";
        public const string LIF_ASSET_FILENAME = LIF_ASSET_NAME + LIF_EXT;
        public const string LIF_DB_FILENAME = LIF_DB_NAME + LIF_EXT;

        public static LifDiscardMethod DefaultDiscardMethod = LifDiscardMethod.Compress;

        /// <summary>
        /// Gets the LDD installation directory.
        /// </summary>
        public static string ApplicationPath
        {
            get { return _ApplicationPath; }
        }

        /// <summary>
        /// Gets the user LDD AppData directory.
        /// </summary>
        public static string ApplicationDataPath
        {
            get { return _ApplicationDataPath; }
        }

        /// <summary>
        /// Gets a value indicating whether or not LDD is installed.
        /// </summary>
        public static bool IsInstalled
        {
            get
            {
                if (!string.IsNullOrEmpty(ApplicationPath))
                    return File.Exists(Path.Combine(ApplicationPath, EXE_NAME));//TODO: maybe convert to private field to prevent always accessing the disk.
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not LDD has been started once. 
        /// </summary>
        public static bool IsLibraryDownloaded
        {
            get
            {
                if (!IsInstalled)
                    return false;
                if (!string.IsNullOrEmpty(ApplicationDataPath))
                    return Directory.GetFiles(ApplicationDataPath, "*.lif").Length > 0;
                return false;
            }
        }

        /// <summary>
        /// Gets the directory for the current user models.
        /// </summary>
        public static string UserModelsDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), USER_MODELS_DIR);
            }
        }

        static LDDManager()
        {
            _ApplicationPath = FindApplicationPath();
            _ApplicationDataPath = GetLocalAppDataPath();
        }

        static string FindApplicationPath()
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            programFilesPath = programFilesPath.Split(Path.VolumeSeparatorChar)[1].Substring(1);
            foreach (string volume in Environment.GetLogicalDrives())
            {
                string installPath = Path.Combine(volume + programFilesPath, APP_DIR);

                if (Directory.Exists(installPath))
                    return installPath;
            }
            //TODO: maybe search registry
            return string.Empty;
        }

        static string GetLocalAppDataPath()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            localAppData = Path.Combine(localAppData, APP_DIR);
            if (Directory.Exists(localAppData))
                return localAppData;
            return string.Empty;
        }

        #region Lifs SubDirectories enum

        public enum DbDirectories
        {
            Assemblies,
            Decorations,
            MainGroupDividers,
            MaterialNames,
            Primitives,
            LOD0
        }

        public enum AssetDirectories
        {
            Graphics,
            Palettes,
            Scripts,
            Shaders,
            StartModels,
            Templates
        }

        #endregion

        #region Lifs path related methods

        /// <summary>
        /// Returns the full path of the specified Lif file on disk.
        /// </summary>
        /// <param name="lif"></param>
        /// <returns></returns>
        public static string GetLifPath(LifInstance lif)
        {
            switch (lif)
            {
                case LifInstance.Assets:
                    return Path.Combine(ApplicationPath, LIF_ASSET_FILENAME);
                default:
                case LifInstance.Database:
                    return Path.Combine(ApplicationDataPath, LIF_DB_FILENAME);
                case LifInstance.AssetsDatabase:
                    return Path.Combine(ApplicationPath, LIF_ASSET_NAME, LIF_DB_FILENAME);
            }
        }

        /// <summary>
        /// Returns the directory of the specified (extracted) Lif file.
        /// </summary>
        /// <param name="lif"></param>
        /// <returns></returns>
        public static string GetLifDirectory(LifInstance lif)
        {
            string lifDir = GetLifPath(lif);//TODO: refactor inline when debugging is no longer necessary
            return Path.Combine(Path.GetDirectoryName(lifDir), Path.GetFileNameWithoutExtension(lifDir));
        }

        #endregion

        #region Lifs extraction verification

        public static bool IsLifExtracted(LifInstance lif)
        {
            string lifDir = GetLifDirectory(lif);
            if (!Directory.Exists(lifDir))
                return false;

            return Directory.EnumerateFileSystemEntries(lifDir).Any();
        }

        public static void ExtractLif(LifInstance lif)
        {
            ExtractLif(lif, DefaultDiscardMethod);
        }

        public static void ExtractLif(LifInstance lif, LifDiscardMethod discardMethod)
        {
            if (IsLifExtracted(lif))
                return;

            /* UNCOMMENT WHEN LIFEXTRACTOR IS COMPLETED/WORKING
            //LifInstance.Assets and LifInstance.AssetsDatabase are located in the ProgramFiles directory
            //and will generally require Admin privilege to create directories and files
            if (lif != LifInstance.Database && !SecurityHelper.IsUserAdministrator)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "LifExtractor.exe",
                    WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    Arguments = string.Format("-i {0} -m {1}", (int)lif, (int)discardMethod),
                    Verb = "runas"
                });
                return;
            }
            */
            string lifDir = GetLifDirectory(lif);

            Directory.CreateDirectory(lifDir);

            using (var lifFile = OpenLif(lif))
                lifFile.Extract(lifDir);

            if (discardMethod == LifDiscardMethod.Compress)
                CompressLif(GetLifPath(lif));
            else// if(discardMethod == LifDiscardMethod.Rename)
                RenameLif(GetLifPath(lif));
        }

        static void RenameLif(string lifFilePath)
        {
            string newName = Path.Combine(Path.GetDirectoryName(lifFilePath), "_" + Path.GetFileName(lifFilePath));
            File.Move(lifFilePath, newName);
        }

        static void CompressLif(string lifFilePath)
        {
            string zippedLifPath = Path.ChangeExtension(lifFilePath, "zip");
            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zippedLifPath)))
            {
                var zipEntry = new ZipEntry(Path.GetFileName(lifFilePath));
                zipStream.PutNextEntry(zipEntry);
                using (var fs = File.OpenRead(lifFilePath))
                {
                    byte[] buffer = new byte[4096];
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        zipStream.Write(buffer, 0, sourceBytes);
                    }
                    while (sourceBytes > 0);
                }
                zipStream.Finish();
                zipStream.Close();
            }
        }

        public static LifFile OpenLif(LifInstance source)
        {
            return LifFile.Open(GetLifPath(source));
        }

        #endregion

        #region GetDirectory

        private static string GetDirectoryName(DbDirectories directory)
        {
            string dirName = directory.ToString();
            if (directory == DbDirectories.LOD0)
                dirName = Path.Combine(DbDirectories.Primitives.ToString(), dirName);
            return dirName;
        }

        /// <summary>
        /// Get the full path for the specified directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fromAssets">Specify if the directory from AppData or from de DB.LIF in the Assets</param>
        /// <returns></returns>
        public static string GetDirectory(DbDirectories directory, bool fromAssets = false)//on-disk location (extracted)
        {
            return Path.Combine(GetLifDirectory(fromAssets ? LifInstance.AssetsDatabase : LifInstance.Database), GetDirectoryName(directory));
        }

        public static string GetDirectory(AssetDirectories directory)
        {
            return Path.Combine(GetLifDirectory(LifInstance.Assets), directory.ToString());
        }

        #endregion


        public enum LifDiscardMethod
        {
            Rename,
            Compress
        }
    }
}