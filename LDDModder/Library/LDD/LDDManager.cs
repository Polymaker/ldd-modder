using ICSharpCode.SharpZipLib.Zip;
using LDDModder.Configuration;
using LDDModder.LDD.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace LDDModder.LDD
{
    public static class LDDManager
    {
        private static bool _HasInitialized;
        private static bool _IsInstalled;
        private static bool _IsLibraryDownloaded;
        private static bool isInitializing;

        public const string EXE_NAME = "LDD.exe";
        public const string APP_DIR = "LEGO Company\\LEGO Digital Designer";
        public const string USER_MODELS_DIR = "LEGO Creations\\Models";

        public const string LDD_SETTINGS_FILENAME = "preferences.ini";

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
            get
            {
                InitializeOnce();
                return SettingsManager.LddInstallDirectory;
            }
        }

        /// <summary>
        /// Gets the user LDD AppData (roaming) directory.
        /// </summary>
        public static string ApplicationDataPath
        {
            get
            {
                InitializeOnce();
                return SettingsManager.LddAppDataDirectory;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not LDD is installed.
        /// </summary>
        public static bool IsInstalled
        {
            get
            {
                InitializeOnce();
                return _IsInstalled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not LDD has been started once. 
        /// </summary>
        public static bool IsLibraryDownloaded
        {
            get
            {
                InitializeOnce();
                return _IsLibraryDownloaded;
            }
        }

        public static bool HasInitialized
        {
            get { return _HasInitialized; }
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

        private static void InitializeOnce()
        {
            if (!HasInitialized && !isInitializing)
                Initialize();
        }

        public static void Initialize()
        {
            if (isInitializing)
                return;

            isInitializing = true;
            _IsInstalled = false;

            _IsLibraryDownloaded = false;

            if (!SettingsManager.HasLoadedOnce)
                SettingsManager.Load();

            _IsInstalled = FindInstallPath();

            if (_IsInstalled && FindAppDataPath())
                _IsLibraryDownloaded = Directory.GetFiles(ApplicationDataPath, "*.lif").Length > 0;

            if (SettingsManager.HasChanges)
                SettingsManager.Save();

            isInitializing = false;
            _HasInitialized = true;
        }

        private static bool FindInstallPath()
        {
            if (!string.IsNullOrEmpty(SettingsManager.LddInstallDirectory) &&
                ValidateInstallDirectory(SettingsManager.LddInstallDirectory))
                return true;
            
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            programFilesPath = programFilesPath.Split(Path.VolumeSeparatorChar)[1].Substring(1);

            foreach (string volume in Environment.GetLogicalDrives())
            {
                string installPath = Path.Combine(volume + programFilesPath, APP_DIR);

                if (ValidateInstallDirectory(installPath))
                {
                    SettingsManager.LddInstallDirectory.Value = installPath;
                    return true;
                }
            }

            SettingsManager.LddInstallDirectory.Value = string.Empty;
            return false;
        }

        public static void SetApplicationPath(string filepath)
        {
            if (ValidateInstallDirectory(filepath))
            {
                SettingsManager.LddInstallDirectory.Value = filepath;
            }
        }

        public static bool ValidateInstallDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                return false;
            return File.Exists(Path.Combine(directory, EXE_NAME));
        }

        private static bool FindAppDataPath()
        {
            if (!string.IsNullOrEmpty(SettingsManager.LddAppDataDirectory) &&
                Directory.Exists(SettingsManager.LddAppDataDirectory))
                return true;

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            localAppData = Path.Combine(localAppData, APP_DIR);

            if (Directory.Exists(localAppData))
            {
                SettingsManager.LddAppDataDirectory.Value = localAppData;
                return true;
            }

            SettingsManager.LddAppDataDirectory.Value = string.Empty;
            return false;
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

            DiscardOfLif(discardMethod, GetLifPath(lif));
        }

        public static void DiscardOfLif(LifInstance lif)
        {
            DiscardOfLif(DefaultDiscardMethod, GetLifPath(lif));
        }

        public static void DiscardOfLif(LifInstance lif, LifDiscardMethod discardMethod)
        {
            DiscardOfLif(discardMethod, GetLifPath(lif));
        }

        internal static void DiscardOfLif(string lifFilePath)
        {
            DiscardOfLif(DefaultDiscardMethod, lifFilePath);
        }

        internal static void DiscardOfLif(LifDiscardMethod discardMethod, string lifFilePath)
        {
            if (discardMethod == LifDiscardMethod.Compress)
                CompressLif(lifFilePath);
            else// if(discardMethod == LifDiscardMethod.Rename)
                RenameLif(lifFilePath);
        }

        internal static void RenameLif(string lifFilePath)
        {
            if (!File.Exists(lifFilePath))
                return;
            string newName = Path.Combine(Path.GetDirectoryName(lifFilePath), "_" + Path.GetFileName(lifFilePath));
            File.Move(lifFilePath, newName);
        }

        internal static void CompressLif(string lifFilePath)
        {
            if (!File.Exists(lifFilePath))
                return;
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

        public static string GetDirectory(LDDLocation directory)
        {
            return directory == LDDLocation.ProgramFiles ? ApplicationPath : ApplicationDataPath;
        }

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

        #region LDD Settings (preferences.ini)
        //TODO: move to a separate class

        public static string GetSettingsFilePath(LDDLocation source)
        {
            switch (source)
            {
                case LDDLocation.ProgramFiles:
                    return Path.Combine(ApplicationPath, LDD_SETTINGS_FILENAME);
                default:
                case LDDLocation.AppData:
                    return Path.Combine(ApplicationDataPath, LDD_SETTINGS_FILENAME);
            }
        }

        public static string GetSettingValue(string key, LDDLocation source)
        {
            string settingValue;
            GetSettingValue(key, source, out settingValue);
            return settingValue;
        }

        public static bool GetSettingBoolean(string key, LDDLocation source, bool defaultValue = false)
        {
            string settingValue;
            if (!GetSettingValue(key, source, out settingValue) || 
                string.IsNullOrEmpty(settingValue))
                return defaultValue;
            settingValue = settingValue.Trim().ToLower();
            return settingValue == "1" || settingValue == "yes" || settingValue == "true";
        }

        public static bool GetSettingValue(string key, LDDLocation source, out string value)
        {
            value = string.Empty;

            string settingsPath = GetSettingsFilePath(source);

            if (!File.Exists(settingsPath))
            {
                Trace.WriteLine("Warning! LDD preference.ini file not found.");
                return false;
            }

            try
            {
                using (var fs = File.OpenRead(settingsPath))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.Contains('='))
                                continue;

                            string keyName = line.Substring(0, line.IndexOf('='));

                            if (keyName.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                            {
                                value = line.Substring(line.IndexOf('=') + 1);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
	        {
                Trace.WriteLine("Warning! Exception while reading LDD preferences.ini" + Environment.NewLine + ex.ToString());
            }
            
            return !string.IsNullOrEmpty(value);
        }

        public static void SetSetting(string key, string value, LDDLocation source)
        {
            string settingsPath = GetSettingsFilePath(source);

            if (!File.Exists(settingsPath))
            {
                Trace.WriteLine("Warning! LDD preference.ini file not found.");
                return;
            }

            List<string> settingsLines;

            try
            {
                settingsLines = File.ReadAllLines(settingsPath).ToList();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Warning! Exception while reading LDD preferences.ini" + Environment.NewLine + ex.ToString());
                return;
            }

            settingsLines.RemoveAll(s => string.IsNullOrWhiteSpace(s));

            bool keyFound = false;

            for (int i = 0; i < settingsLines.Count; i++)
            {
                if (settingsLines[i].StartsWith(key + "=", StringComparison.InvariantCultureIgnoreCase))
                {
                    settingsLines[i] = key + "=" + value;
                    keyFound = true;
                    break;
                }
            }

            if (!keyFound)
            {
                settingsLines.Add(key + "=" + value);
            }

            //LDD always add two blank lines
            settingsLines.Add(String.Empty); settingsLines.Add(String.Empty);
            try
            {
                using (var fs = File.Create(settingsPath))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        foreach (var line in settingsLines)
                            sw.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Warning! Exception while writing LDD preferences.ini"+ Environment.NewLine + ex.ToString());
            }
        }

        #endregion

        public enum LifDiscardMethod
        {
            Rename,
            Compress
        }
    }
}