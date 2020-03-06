using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Settings
{
    static class SettingsManager
    {
        public static string AppDataFolder { get; set; }

        public const string DATABASE_FILENAME = "BrickDatabase.db";

        public static bool HasInitialized { get; private set; }

        static SettingsManager()
        {
            //AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //AppDataFolder = Path.Combine(AppDataFolder, "LDDModder", "PaletteMaker");
        }

        public static void Initialize()
        {
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataFolder = Path.Combine(AppDataFolder, "LDDModder", "PaletteMaker");

            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);

            HasInitialized = true;
        }

        public static bool DatabaseExists()
        {
            var dbPath = Path.Combine(AppDataFolder, DATABASE_FILENAME);
            return File.Exists(dbPath);
        }

        public static DB.PaletteDbContext GetDbContext()
        {
            var dbPath = Path.Combine(AppDataFolder, DATABASE_FILENAME);
            return new DB.PaletteDbContext($"Data Source={dbPath}");
        }

        public static string GetFilePath(string filename)
        {
            return Path.Combine(AppDataFolder, filename);
        }
    }
}
