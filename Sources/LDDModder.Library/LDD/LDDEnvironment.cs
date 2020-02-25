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

        public string ProgramFilesPath { get; private set; }
        public string ApplicationDataPath { get; private set; }
        public string UserCreationPath { get; private set; }

        public bool AssetsExtracted { get; private set; }

        public bool DatabaseExtracted { get; private set; }

        public static LDDEnvironment Current { get; private set; }

        public static bool IsInstalled => !string.IsNullOrEmpty(Current?.ProgramFilesPath);

        public static bool HasInitialized { get; private set; }

        protected LDDEnvironment()
        {
        }

        public LDDEnvironment(string programFilesPath, string applicationDataPath)
        {
            ProgramFilesPath = programFilesPath;
            ApplicationDataPath = applicationDataPath;
            CheckLifStatus();
        }

        public static void Initialize()
        {
            var lddEnv = GetInstalledEnvironment();

            if (lddEnv != null)
            {
                lddEnv.CheckLifStatus();
                Current = lddEnv;
            }
            
            HasInitialized = true;
        }

        public static LDDEnvironment GetInstalledEnvironment()
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

                return lddEnv;
            }

            return null;
        }

        public static void SetEnvironment(LDDEnvironment environment)
        {
            Current = environment;
        }

        public static void SetEnvironmentPaths(string programFilesPath, string applicationDataPath)
        {
            if (Current == null)
                Current = new LDDEnvironment();

            string exePath = Path.Combine(programFilesPath, EXE_NAME);
            if (!File.Exists(exePath))
                programFilesPath = string.Empty;

            Current.ProgramFilesPath = programFilesPath;
            Current.ApplicationDataPath = applicationDataPath;
        }

        public static string FindInstallFolder()
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            programFilesPath = programFilesPath.Substring(programFilesPath.IndexOf(Path.VolumeSeparatorChar) + 2);

            foreach (string volume in Environment.GetLogicalDrives())
            {
                string installPath = Path.Combine(volume + programFilesPath, APP_DIR);
                string exePath = Path.Combine(installPath, EXE_NAME);
                if (File.Exists(exePath))
                    return installPath;
            }

            return string.Empty;
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
            DatabaseExtracted = false;
            if (Directory.Exists(Path.Combine(ApplicationDataPath, "db")))
                DatabaseExtracted = File.Exists(Path.Combine(ApplicationDataPath, "db", "info.xml"));

            AssetsExtracted = false;
            if (Directory.Exists(Path.Combine(ProgramFilesPath, "Assets")))
                AssetsExtracted = Directory.EnumerateFiles(Path.Combine(ProgramFilesPath, "Assets"), "*", SearchOption.AllDirectories).Any();

        }

        public bool IsLifExtracted(LddLif lif)
        {
            switch (lif)
            {
                case LddLif.Assets:
                    return AssetsExtracted;
                case LddLif.DB:
                    return DatabaseExtracted;
                default:
                    return false;
            }
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
                    return Path.Combine(ProgramFilesPath, ASSETS_LIF);
                case LddLif.DB:
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
                    return Path.Combine(ProgramFilesPath, "Assets");
                case LddLif.DB:
                    return Path.Combine(ApplicationDataPath, "db");
                default:
                    return null;
            }
        }

        public bool DirectoryExists(LddDirectory directory)
        {
            string path = GetLddDirectoryPath(directory);
            return Utilities.FileHelper.IsValidDirectory(path) && Directory.Exists(path);
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
            return Path.Combine(GetLddDirectoryPath(directory), subfolder);
        }

        public DirectoryInfo GetLddSubdirectoryInfo(LddDirectory directory, string subfolder)
        {
            return new DirectoryInfo(GetLddSubdirectory(directory, subfolder));
        }

        public string GetAppDataSubDir(string subfolder)
        {
            return GetLddSubdirectory(LddDirectory.ApplicationData, subfolder);
        }

        public DirectoryInfo GetAppDataSubDirInfo(string subfolder)
        {
            return GetLddSubdirectoryInfo(LddDirectory.ApplicationData, subfolder);
        }
    }
}
