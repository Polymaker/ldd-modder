using LDDModder.LDD;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Settings
{
    public static class SettingsManager
    {
        public static string AppDataFolder { get; set; }

        public const string AppSettingsFileName = "settings.json";

        public static AppSettings Current { get; private set; }

        public static LDDEnvironment LddInstall { get; private set; }

        public static int MaximumRecentFiles { get; set; } = 10;

        static SettingsManager()
        {
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataFolder = Path.Combine(AppDataFolder, "LDDModder", "BrickEditor");
        }

        public static void Initialize()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            if (LddInstall == null)
                LddInstall = LDDEnvironment.InstalledEnvironment;

            LoadSettings();
        }

        public static void LoadSettings()
        {
            string settingsPath = Path.Combine(AppDataFolder, AppSettingsFileName);

            if (File.Exists(settingsPath))
            {
                try
                {
                    Current = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(settingsPath));
                }
                catch
                {

                }
            }


            if (Current == null)
                Current = AppSettings.CreateDefault();
            
            Current.InitializeDefaultValues();

            ValidateLddPaths();

            SaveSettings();
        }

        private static void ValidateLddPaths()
        {
            bool sameAsInstalled = false;

            if (LDDEnvironment.IsInstalled)
            {
                sameAsInstalled = StringUtils.EqualsIC(
                        Current.LddApplicationDataPath,
                        LDDEnvironment.InstalledEnvironment.ApplicationDataPath
                    ) &&
                    StringUtils.EqualsIC(
                        Current.LddProgramFilesPath,
                        LDDEnvironment.InstalledEnvironment.ProgramFilesPath
                    );
            }

            if (sameAsInstalled)
                LDDEnvironment.SetOverride(null);
            else
            {
                var custom = LDDEnvironment.Create(Current.LddProgramFilesPath, Current.LddApplicationDataPath);
                LDDEnvironment.SetOverride(custom);
            }
        }

        public static void SaveSettings()
        {
            string settingsPath = Path.Combine(AppDataFolder, AppSettingsFileName);


            using (var fs = File.Open(settingsPath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
                sw.Write(JsonConvert.SerializeObject(Current, Formatting.Indented));

            //LDDEnvironment.SetEnvironmentPaths(
            //    Current.LddProgramFilesPath, 
            //    Current.LddApplicationDataPath);
        }

        public static void AddRecentProject(PartProject project, bool isSavedFile = false)
        {
            if (Current.RecentProjectFiles == null)
                Current.RecentProjectFiles = new List<RecentFileInfo>();

            if (isSavedFile && !Current.RecentProjectFiles.Any(x => x.ProjectFile == project.ProjectPath))
            {
                Current.RecentProjectFiles.Insert(0, new RecentFileInfo(project));

                while (Current.RecentProjectFiles.Count > MaximumRecentFiles)
                    Current.RecentProjectFiles.RemoveAt(Current.RecentProjectFiles.Count - 1);

                SaveSettings();
            }
            else if (!isSavedFile)
            {
                Current.RecentProjectFiles.RemoveAll(x => x.ProjectFile == project.ProjectPath);
                Current.RecentProjectFiles.Insert(0, new RecentFileInfo(project));

                while (Current.RecentProjectFiles.Count > MaximumRecentFiles)
                    Current.RecentProjectFiles.RemoveAt(Current.RecentProjectFiles.Count - 1);

                SaveSettings();
            }
        }

        #region Project Configuration

        public static IEnumerable<BuildConfiguration> GetBuildConfigurations()
        {
            if (LddInstall.IsValidInstall)
                yield return Current.BuildSettings.LDD;

            yield return Current.BuildSettings.Manual;

            foreach (var cfg in Current.BuildSettings.UserDefined)
                yield return cfg;
        }


        #endregion


        public static bool IsWorkspaceDefined => !string.IsNullOrEmpty(Current.ProjectWorkspace);
    }
}
