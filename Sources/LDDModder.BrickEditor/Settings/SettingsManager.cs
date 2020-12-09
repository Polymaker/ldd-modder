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

        public static int MaximumRecentFiles { get; set; } = 10;

        public static bool HasInitialized { get; private set; }

        public static int AppInstanceID { get; set; }

        static SettingsManager()
        {
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataFolder = Path.Combine(AppDataFolder, "LDDModder", "BrickEditor");
        }

        public static void Initialize()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            if (!LDDEnvironment.HasInitialized)
                LDDEnvironment.Initialize();

            LoadSettings();

            HasInitialized = true;
        }

        public static void LoadSettings()
        {
            Current = LoadAppSettings();

            if (Current == null)
                Current = AppSettings.CreateDefault();
            
            Current.InitializeDefaultValues();

            ValidateLddPaths();

            SaveSettings();
        }

        public static AppSettings LoadAppSettings()
        {
            string settingsPath = Path.Combine(AppDataFolder, AppSettingsFileName);

            if (File.Exists(settingsPath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(settingsPath));
                }
                catch { }
            }

            return null;
        }

        public static void ReloadFilesHistory()
        {
            var currentSettings = LoadAppSettings();
            if (currentSettings == null)
                return;
            Current.RecentProjectFiles = currentSettings.RecentProjectFiles;
            Current.OpenedProjects = currentSettings.OpenedProjects;
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
                if (LDDEnvironment.IsInstalled)
                {
                    if (string.IsNullOrEmpty(Current.LddProgramFilesPath))
                        Current.LddProgramFilesPath = LDDEnvironment.InstalledEnvironment.ProgramFilesPath;
                    if (string.IsNullOrEmpty(Current.LddApplicationDataPath))
                        Current.LddApplicationDataPath = LDDEnvironment.InstalledEnvironment.ApplicationDataPath;
                }
                
                if (!string.IsNullOrEmpty(Current.LddProgramFilesPath) || 
                    !string.IsNullOrEmpty(Current.LddApplicationDataPath))
                {
                    var custom = LDDEnvironment.Create(Current.LddProgramFilesPath, Current.LddApplicationDataPath);
                    LDDEnvironment.SetOverride(custom);
                }
                else
                    LDDEnvironment.SetOverride(null);
            }
        }

        public static void SaveSettings()
        {
            SaveSettings(Current);
        }

        private static void SaveSettings(AppSettings settings)
        {
            string settingsPath = Path.Combine(AppDataFolder, AppSettingsFileName);

            using (var fs = File.Open(settingsPath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
                sw.Write(JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        public static void SaveFilesHistory()
        {
            var currentSettings = LoadAppSettings();
            if (currentSettings == null)
                return;
            currentSettings.OpenedProjects = Current.OpenedProjects;
            currentSettings.RecentProjectFiles = Current.RecentProjectFiles;
            SaveSettings(currentSettings);
        }

        public static void CleanUpFilesHistory()
        {
            ReloadFilesHistory();

            foreach (var fileinfo in Current.RecentProjectFiles.ToArray())
            {
                if (!File.Exists(fileinfo.ProjectFile))
                    Current.RecentProjectFiles.Remove(fileinfo);
            }

            foreach (var fileinfo in Current.OpenedProjects.ToArray())
            {
                if (!File.Exists(fileinfo.TemporaryPath))
                    Current.OpenedProjects.Remove(fileinfo);
            }

            SaveFilesHistory();
        }

        public static void AddRecentProject(PartProject project, bool isSavedFile = false)
        {
            if (Current.RecentProjectFiles == null)
                Current.RecentProjectFiles = new List<RecentFileInfo>();

            ReloadFilesHistory();

            if (isSavedFile && !Current.RecentProjectFiles.Any(x => x.ProjectFile == project.ProjectPath))
            {
                Current.RecentProjectFiles.Insert(0, new RecentFileInfo(project));

                while (Current.RecentProjectFiles.Count > MaximumRecentFiles)
                    Current.RecentProjectFiles.RemoveAt(Current.RecentProjectFiles.Count - 1);

            }
            else if (!isSavedFile)
            {
                Current.RecentProjectFiles.RemoveAll(x => x.ProjectFile == project.ProjectPath);
                Current.RecentProjectFiles.Insert(0, new RecentFileInfo(project));

                while (Current.RecentProjectFiles.Count > MaximumRecentFiles)
                    Current.RecentProjectFiles.RemoveAt(Current.RecentProjectFiles.Count - 1);

            }

            SaveFilesHistory();
        }

        public static void AddRecentProject(RecentFileInfo fileInfo, bool moveToTop = false)
        {
            if (Current.RecentProjectFiles == null)
                Current.RecentProjectFiles = new List<RecentFileInfo>();

            ReloadFilesHistory();

            if (moveToTop || !Current.RecentProjectFiles.Any(x => x.ProjectFile == fileInfo.ProjectFile))
            {
                Current.RecentProjectFiles.RemoveAll(x => x.ProjectFile == fileInfo.ProjectFile);
                Current.RecentProjectFiles.Insert(0, fileInfo);
            }

            while (Current.RecentProjectFiles.Count > MaximumRecentFiles)
                Current.RecentProjectFiles.RemoveAt(Current.RecentProjectFiles.Count - 1);

            SaveFilesHistory();
        }

        //public static void AddOpenedFile(PartProject project, string temporaryPath)
        //{
        //    ReloadFilesHistory();
        //    var fileInfo = new RecentFileInfo(project, temporaryPath);

        //    if (!Current.OpenedProjects.Any(x => x.TemporaryPath == temporaryPath))
        //    {
        //        Current.OpenedProjects.Add(fileInfo);
        //        SaveFilesHistory();
        //    }
        //}

        public static void AddOpenedFile(RecentFileInfo fileInfo)
        {
            ReloadFilesHistory();

            if (!Current.OpenedProjects.Any(x => x.TemporaryPath == fileInfo.TemporaryPath))
            {
                Current.OpenedProjects.Add(fileInfo);
                SaveFilesHistory();
            }
        }

        public static void UpdateOpenedFile(string tempPath, string projectPath)
        {
            ReloadFilesHistory();
            var openedProject = Current.OpenedProjects.FirstOrDefault(p => p.TemporaryPath == tempPath);

            if (openedProject != null)
            {
                openedProject.ProjectFile = projectPath;
                SaveFilesHistory();
            }
        }

        public static void RemoveOpenedFile(string path)
        {
            ReloadFilesHistory();
            int removed = Current.OpenedProjects.RemoveAll(x => x.TemporaryPath == path /*|| x.ProjectFile == path*/);
            if (removed > 0)
                SaveFilesHistory();
        }

        public static void RemoveRecentFile(string path)
        {
            ReloadFilesHistory();
            int removed = Current.OpenedProjects.RemoveAll(x => x.ProjectFile == path);
            if (removed > 0)
                SaveFilesHistory();
        }

        #region Project Configuration

        public static IEnumerable<BuildConfiguration> GetBuildConfigurations()
        {
            if (LDDEnvironment.Current != null &&
                LDDEnvironment.Current.IsValidInstall && 
                Current.BuildSettings?.LDD != null)
                yield return Current.BuildSettings.LDD;

            if (Current.BuildSettings?.Manual != null)
                yield return Current.BuildSettings.Manual;

            foreach (var cfg in Current.BuildSettings.UserDefined)
            {
                cfg.GenerateUniqueID();
                yield return cfg;
            }
        }


        #endregion


        public static bool IsWorkspaceDefined => !string.IsNullOrEmpty(Current.ProjectWorkspace);
    }
}
