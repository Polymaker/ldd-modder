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
        public const string APP_DIR = "LEGO Company\\LEGO Digital Designer";

        public string ProgramFilesPath { get; set; }
        public string ApplicationDataPath { get; set; }

        public string AssetsPath { get; set; }

        public string DatabasePath { get; set; }

        public bool AssetsExtracted { get; private set; }

        public bool DatabaseExtracted { get; private set; }


        public static LDDEnvironment Current { get; private set; }

        public static bool HasInitialized { get; private set; }

        public static void Initialize()
        {
            var lddEnv = new LDDEnvironment()
            {
                ProgramFilesPath = FindInstallFolder(),
                ApplicationDataPath = FindAppDataFolder()
            };
            lddEnv.CheckLifStatus();
            Current = lddEnv;
            HasInitialized = true;
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

        public void CheckLifStatus()
        {
            DatabaseExtracted = false;
            if (Directory.Exists(Path.Combine(ApplicationDataPath, "db")))
                DatabaseExtracted = File.Exists(Path.Combine(ApplicationDataPath, "db", "info.xml"));

            AssetsExtracted = false;
            if (Directory.Exists(Path.Combine(ProgramFilesPath, "Assets")))
                AssetsExtracted = Directory.EnumerateFiles(Path.Combine(ProgramFilesPath, "Assets"), "*", SearchOption.AllDirectories).Any();

        }

        public string GetAppDataSubDir(string path)
        {
            return Path.Combine(ApplicationDataPath, path);
        }

        public DirectoryInfo GetAppDataSubDirInfo(string path)
        {
            return new DirectoryInfo(GetAppDataSubDir(path));
        }

        public string GetDatabaseLifPath()
        {
            return Path.Combine(ApplicationDataPath, "db.lif");
        }
    }
}
