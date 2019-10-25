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

        static SettingsManager()
        {
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDataFolder = Path.Combine(AppDataFolder, "LDDBrickEditor");

        }

        public static void Initialize()
        {
            if (!Directory.Exists(AppDataFolder))
                Directory.CreateDirectory(AppDataFolder);
        }
    }
}
