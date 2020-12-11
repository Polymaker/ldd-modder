using LDDModder.LDD;
using LDDModder.Modding.Editing;
using LDDModder.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                Current = new AppSettings();
            
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
                    string jsonData = File.ReadAllText(settingsPath);
                    var appSettings = JsonConvert.DeserializeObject<AppSettings>(jsonData);
                    MigrateLagacyFormat(appSettings, jsonData);

                    return appSettings;
                }
                catch { }
            }

            return null;
        }

        private static void MigrateLagacyFormat(AppSettings appSettings, string jsonData)
        {
            var jsonObj = JObject.Parse(jsonData);

            var lddPrgmPath = jsonObj.Value<string>("ldd.programFilesPath");
            if (!string.IsNullOrEmpty(lddPrgmPath) && string.IsNullOrEmpty(appSettings.LddSettings.ProgramFilesPath))
                appSettings.LddSettings.ProgramFilesPath = lddPrgmPath;

            var lddAppDataPath = jsonObj.Value<string>("ldd.appDataPath");
            if (!string.IsNullOrEmpty(lddAppDataPath) && string.IsNullOrEmpty(appSettings.LddSettings.ApplicationDataPath))
                appSettings.LddSettings.ApplicationDataPath = lddAppDataPath;

            try
            {
                if (jsonObj.Property("build.configurations") != null)
                {
                    var buildSettings = jsonObj["build.configurations"].ToObject<ProjectBuildSettings>();
                    if (buildSettings != null)
                    {
                        appSettings.BuildSettings = buildSettings;
                    }
                }
            }
            catch { }
            
            

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
                        Current.LddSettings.ApplicationDataPath,
                        LDDEnvironment.InstalledEnvironment.ApplicationDataPath
                    ) &&
                    StringUtils.EqualsIC(
                        Current.LddSettings.ProgramFilesPath,
                        LDDEnvironment.InstalledEnvironment.ProgramFilesPath
                    );
            }

            if (sameAsInstalled)
                LDDEnvironment.SetOverride(null);
            else
            {
                if (LDDEnvironment.IsInstalled)
                {
                    if (string.IsNullOrEmpty(Current.LddSettings.ProgramFilesPath))
                        Current.LddSettings.ProgramFilesPath = LDDEnvironment.InstalledEnvironment.ProgramFilesPath;
                    if (string.IsNullOrEmpty(Current.LddSettings.ApplicationDataPath))
                        Current.LddSettings.ApplicationDataPath = LDDEnvironment.InstalledEnvironment.ApplicationDataPath;
                }
                
                if (!string.IsNullOrEmpty(Current.LddSettings.ProgramFilesPath) || 
                    !string.IsNullOrEmpty(Current.LddSettings.ApplicationDataPath))
                {
                    var custom = LDDEnvironment.Create(
                        Current.LddSettings.ProgramFilesPath, 
                        Current.LddSettings.ApplicationDataPath);
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

            currentSettings.BuildSettings.InitializeDefaults();

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

        public static void AddRecentProject(RecentFileInfo fileInfo, bool moveToTop = false)
        {
            if (Current.RecentProjectFiles == null)
                Current.RecentProjectFiles = new List<RecentFileInfo>();

            ReloadFilesHistory();

            if (moveToTop || !Current.RecentProjectFiles.Any(x => x.ProjectFile == fileInfo.ProjectFile))
            {
                fileInfo.TemporaryPath = string.Empty;
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


        public static bool IsWorkspaceDefined => !string.IsNullOrEmpty(Current.EditorSettings?.ProjectWorkspace);
    }
}
