﻿using LDDModder.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD
{
    public class LDDEnvironment
    {
        public const string EXE_NAME = "LDD.exe";
        public const string ASSETS_LIF = "Assets.lif";
        public const string DB_LIF = "db.lif";
        public const string APP_DIR = "LEGO Company\\LEGO Digital Designer";
        public const string USER_CREATION_FOLDER = "LEGO Creations";

        private static LDDEnvironment _InstalledEnvironment;
        private int LifStatusFlags;
        //private string _CustomAssetsPath;
        //private string _CustomDatabasePath;

        public string ProgramFilesPath { get; private set; }
        public string ApplicationDataPath { get; private set; }
        public string UserCreationPath { get; private set; }

        public bool AssetsExtracted => IsLifExtracted(LddLif.Assets);

        public bool DatabaseExtracted => IsLifExtracted(LddLif.DB);

        public static LDDEnvironment InstalledEnvironment
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return _InstalledEnvironment;
            }
        }

        public static LDDEnvironment CustomEnvironment { get; set; }

        public static LDDEnvironment Current => CustomEnvironment ?? InstalledEnvironment;

        public bool IsValidInstall { get; private set; }

        public static bool IsInstalled => InstalledEnvironment?.IsValidInstall ?? false;

        public static bool HasInitialized { get; private set; }

        private static readonly object InitializationLock = new object();

        protected LDDEnvironment()
        {
        }

        public LDDEnvironment(string programFilesPath, string applicationDataPath)
        {
            ProgramFilesPath = programFilesPath;
            ApplicationDataPath = applicationDataPath;
            CheckLifStatus();
        }

        public static LDDEnvironment Create(string programFilesPath, string applicationDataPath)
        {
            var lddEnv = new LDDEnvironment()
            {
                ProgramFilesPath = programFilesPath,
                ApplicationDataPath = applicationDataPath,
            };
            lddEnv.Validate();
            return lddEnv;
        }

        public static void Initialize()
        {
            lock (InitializationLock)
            {
                if (HasInitialized)
                    return;

                _InstalledEnvironment = FindInstalledEnvironment();
                HasInitialized = true;
            }
        }

        public void Validate()
        {
            IsValidInstall = false;

            if (FileHelper.IsValidPath(ProgramFilesPath))
                IsValidInstall = File.Exists(Path.Combine(ProgramFilesPath, EXE_NAME));

            CheckLifStatus();
        }

        public static LDDEnvironment FindInstalledEnvironment()
        {
            string installDir = FindInstallFolder();

            if (!string.IsNullOrEmpty(installDir))
            {
                var lddEnv = new LDDEnvironment()
                {
                    ProgramFilesPath = installDir,
                    ApplicationDataPath = FindAppDataFolder(),
                    UserCreationPath = FindUserFolder()
                };
                lddEnv.Validate();
                return lddEnv;
            }

            return null;
        }

        public static void SetOverride(LDDEnvironment environment)
        {
            CustomEnvironment = environment;
        }

        public void SetEnvironmentPaths(string programFilesPath, string applicationDataPath)
        {
            ProgramFilesPath = programFilesPath;
            ApplicationDataPath = applicationDataPath;
            IsValidInstall = File.Exists(Path.Combine(programFilesPath, EXE_NAME));
            LifStatusFlags = 0;
        }

        public static string FindInstallFolder()
        {
            bool ValidateInstall(string path)
            {
                return File.Exists(Path.Combine(path, EXE_NAME));
            }

            if (FindInstallByRegistry(out string lddInstallPath) && 
                ValidateInstall(lddInstallPath))
                return lddInstallPath;

            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            programFilesPath = programFilesPath.Substring(programFilesPath.IndexOf(Path.VolumeSeparatorChar) + 2);

            foreach (string volume in Environment.GetLogicalDrives())
            {
                string installPath = Path.Combine(volume + programFilesPath, APP_DIR);
                if (ValidateInstall(installPath))
                    return installPath;

                installPath = Path.Combine(volume, APP_DIR);
                if (ValidateInstall(installPath))
                    return installPath;
            }

            return string.Empty;
        }

        private static bool FindInstallByRegistry(out string lddInstallPath)
        {
            lddInstallPath = string.Empty;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {

                    var installKey = hklm.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\New LEGO Digital Designer");

                    if (installKey != null)
                    {
                        string uninstallPath = installKey.GetValue("UninstallString", string.Empty) as string;
                        if (!string.IsNullOrEmpty(uninstallPath))
                        {
                            lddInstallPath = Path.GetDirectoryName(uninstallPath);
                            return true;
                        }
                        installKey.Dispose();
                    }
                }
            }
            catch { }

            return false;
        }

        public static string FindAppDataFolder()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            localAppData = Path.Combine(localAppData, APP_DIR);
            if (Directory.Exists(localAppData))
                return localAppData;
            return string.Empty;
        }

        public static string FindUserFolder()
        {
            string userDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            userDocuments = Path.Combine(userDocuments, USER_CREATION_FOLDER);
            if (Directory.Exists(userDocuments))
                return userDocuments;
            return string.Empty;
        }

        public void CheckLifStatus()
        {
            LifStatusFlags = 0;

            foreach (LddLif lif in Enum.GetValues(typeof(LddLif)))
            {
                if (File.Exists(GetLifFilePath(lif)))
                    LifStatusFlags |= 1 << ((int)lif * 3); //.lif file exist

                string lifFolder = GetLifFolderPath(lif);

                if (string.IsNullOrEmpty(lifFolder) || !Directory.Exists(lifFolder))
                    continue;

                LifStatusFlags |= 2 << ((int)lif * 3); //extracted folder exist
                bool contentPresent = false;

                if (lif == LddLif.DB)
                {
                    contentPresent = File.Exists(Path.Combine(lifFolder, "info.xml"));
                }
                else if (lif == LddLif.Assets)
                {
                    contentPresent = Directory.EnumerateFiles(lifFolder, "*", SearchOption.AllDirectories).Any();
                }

                if (contentPresent)
                    LifStatusFlags |= 4 << ((int)lif * 3); //the folder has content
            }
        }

        public bool IsLifPresent(LddLif lif)
        {
            return ((LifStatusFlags >> ((int)lif * 3)) & 1) == 1;
        }

        public bool IsLifExtracted(LddLif lif)
        {
            return ((LifStatusFlags >> ((int)lif * 3)) & 4) == 4;
        }

        public string GetExecutablePath()
        {
            return Path.Combine(ProgramFilesPath, EXE_NAME);
        }

        public string GetLifFilePath(LddLif lif)
        {
            switch (lif)
            {
                case LddLif.Assets:
                    if (string.IsNullOrEmpty(ProgramFilesPath))
                        return string.Empty;
                    return Path.Combine(ProgramFilesPath, ASSETS_LIF);
                case LddLif.DB:
                    if (string.IsNullOrEmpty(ApplicationDataPath))
                        return string.Empty;
                    return Path.Combine(ApplicationDataPath, DB_LIF);
                default:
                    return null;
            }
        }

        public string GetLifFolderPath(LddLif lif)
        {
            switch (lif)
            {
                case LddLif.Assets:
                    if (string.IsNullOrEmpty(ProgramFilesPath))
                        return string.Empty;
                    return Path.Combine(ProgramFilesPath, "Assets");
                case LddLif.DB:
                    if (string.IsNullOrEmpty(ApplicationDataPath))
                        return string.Empty;
                    return Path.Combine(ApplicationDataPath, "db");
                default:
                    return null;
            }
        }

        public bool DirectoryExists(LddDirectory directory)
        {
            string path = GetLddDirectoryPath(directory);
            return FileHelper.IsValidDirectory(path) && Directory.Exists(path);
        }

        public string GetLddDirectoryPath(LddDirectory directory)
        {
            switch (directory)
            {
                case LddDirectory.ProgramFiles:
                    return ProgramFilesPath;
                case LddDirectory.ApplicationData:
                    return ApplicationDataPath;
                case LddDirectory.UserDocuments:
                    return UserCreationPath;
                default:
                    return null;
            }
        }
   
        public string GetLddSubdirectory(LddDirectory directory, string subfolder)
        {
            string lddDir = GetLddDirectoryPath(directory);
            if (string.IsNullOrEmpty(lddDir))
                return string.Empty;

            return Path.Combine(GetLddDirectoryPath(directory), subfolder);
        }

        public DirectoryInfo GetLddSubdirectoryInfo(LddDirectory directory, string subfolder)
        {
            string lddSubDir = GetLddSubdirectory(directory, subfolder);
            if (string.IsNullOrEmpty(lddSubDir))
                return null;
            return new DirectoryInfo(lddSubDir);
        }

        public string GetAppDataSubDir(string subfolder)
        {
            return GetLddSubdirectory(LddDirectory.ApplicationData, subfolder);
        }

        public DirectoryInfo GetAppDataSubDirInfo(string subfolder)
        {
            return GetLddSubdirectoryInfo(LddDirectory.ApplicationData, subfolder);
        }

        public bool IsEqual(LDDEnvironment other)
        {
            return ProgramFilesPath.Equals(other.ProgramFilesPath, StringComparison.InvariantCultureIgnoreCase)
                && ApplicationDataPath.Equals(other.ApplicationDataPath, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
