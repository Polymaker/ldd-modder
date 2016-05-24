using System;
using System.Collections.Generic;
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
    }
}