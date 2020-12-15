using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Settings
{
    public class AppSettings
    {
        #region Static Properties

        public static string AppDataFolder { get; set; }

        public const string DATABASE_FILENAME = "BrickDatabase.db";
        public const string AppSettingsFileName = "settings.json";

        public static bool HasInitialized { get; private set; }

        public static AppSettings Current { get; set; }

        #endregion

        public string RebrickableApiKey { get; set; }

        public static void Initialize()
        {
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataFolder = Path.Combine(AppDataFolder, "LDDModder", "PaletteMaker");

            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            HasInitialized = true;
        }

        public static void Load()
        {
            if (!HasInitialized)
                Initialize();

            string settingsPath = Path.Combine(AppDataFolder, AppSettingsFileName);

            //AppSettings settings = null;
            if (File.Exists(settingsPath))
            {
                try
                {
                    Current = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(settingsPath));
                }
                catch { }
            }

            if (Current == null)
            {
                Current = new AppSettings();
                Save();
            }
        }

        public static void Save()
        {
            string settingsPath = Path.Combine(AppDataFolder, AppSettingsFileName);
            using (var fs = File.Open(settingsPath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
                sw.Write(JsonConvert.SerializeObject(Current, Formatting.Indented));
        }

        public static DB.PaletteDbContext GetDbContext()
        {
            var dbPath = Path.Combine(AppDataFolder, DATABASE_FILENAME);
            return new DB.PaletteDbContext($"Data Source={dbPath};Version=3;DateTimeFormat=CurrentCulture");
        }

        public static bool DatabaseExists()
        {
            var dbPath = Path.Combine(AppDataFolder, DATABASE_FILENAME);
            return File.Exists(dbPath);
        }
        /// <summary>
        /// Gets a path relative to the app settings folder
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFilePath(string filename)
        {
            return Path.Combine(AppDataFolder, filename);
        }
    }
}
