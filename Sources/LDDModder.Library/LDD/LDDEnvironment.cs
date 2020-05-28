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

        private static object InitializationLock = new object();

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
            LifStatusFlags = 0;

            foreach (LddLif lif in Enum.GetValues(typeof(LddLif)))
            {
                if (File.Exists(GetLifFilePath(lif)))
                    LifStatusFlags |= 1 << ((int)lif * 3); //.lif file exist

                string lifFolder = GetLifFolderPath(lif);

                if (Directory.Exists(lifFolder))
                {
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
                    
                    if(contentPresent)
                        LifStatusFlags |= 4 << ((int)lif * 3); //the folder has content
                }
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
