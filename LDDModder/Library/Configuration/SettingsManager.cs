using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LDDModder.Configuration
{
    public static class SettingsManager
    {
        private static bool _HasLoadedOnce;
        internal static List<SettingEntry> _Settings;

        private static readonly SettingEntry<string> _LddInstallDirectory;
        private static readonly SettingEntry<string> _LddAppDataDirectory;
        private static readonly SettingEntry<string> _LddPalettesDirectory;
        private static readonly SettingEntry<string> _UserPalettesDirectory;

        public static bool HasLoadedOnce
        {
            get { return _HasLoadedOnce; }
        }

        public static bool HasChanges
        {
            get { return AllSettings.Any(e => e.IsValueChanged); }
        }

        public static IList<SettingEntry> AllSettings
        {
            get { return _Settings.AsReadOnly(); }
        }

        public static IEnumerable<SettingEntry> ApplicationSettings
        {
            get { return AllSettings.Where(s => s.Type == SettingType.Application); }
        }

        public static IEnumerable<SettingEntry> UserSettings
        {
            get { return AllSettings.Where(s => s.Type == SettingType.User); }
        }

        public static SettingEntry<string> LddInstallDirectory
        {
            get { return _LddInstallDirectory; }
        }

        public static SettingEntry<string> LddAppDataDirectory
        {
            get { return _LddAppDataDirectory; }
        }

        public static SettingEntry<string> LddPalettesDirectory
        {
            get {  return _LddPalettesDirectory; }
        }
        public static SettingEntry<string> UserPalettesDirectory
        {
            get { return _UserPalettesDirectory; }
        }

        static SettingsManager()
        {
            _Settings = new List<SettingEntry>();
            _LddInstallDirectory = new SettingEntry<string>(SettingSource.Common, SettingType.Application, "ldd_install");
            _LddAppDataDirectory = new SettingEntry<string>(SettingSource.Common, SettingType.Application, "ldd_appdata");
            _LddPalettesDirectory = new SettingEntry<string>(SettingSource.Common, SettingType.Application, "ldd_palettes");
            _UserPalettesDirectory = new SettingEntry<string>(SettingSource.Common, SettingType.Application, "user_palettes");

        }

        public static IEnumerable<SettingEntry> GetSettings(SettingType type)
        {
            return AllSettings.Where(e => e.Type == type);
        }

        public static IEnumerable<SettingEntry> GetSettings(SettingSource source, SettingType type)
        {
            return AllSettings.Where(e => e.Source == source && e.Type == type);
        }

        public static IEnumerable<SettingEntry> GetSettings(SettingSource source)
        {
            return AllSettings.Where(e => e.Source == source);
        }

        public static void Load()
        {
            _HasLoadedOnce = true;

            string settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LDDModder");
            if (!Directory.Exists(settingsDir))
                return;
            
            string configFilePath = Path.Combine(settingsDir, "Settings.xml");
            var settingsDoc = XDocument.Load(configFilePath);

            foreach (SettingSource source in Enum.GetValues(typeof(SettingSource)))
                LoadSettings(settingsDoc, source);

        }

        private static void LoadSettings(XDocument document, SettingSource source)
        {
            var sourceNode = document.Descendants(source.ToString()).FirstOrDefault();
            if (sourceNode == null)
                return;
            
            foreach (var entryKey in sourceNode.Descendants("Key"))
            {
                var settingType = entryKey.Parent.Name.LocalName.Contains("Appli") ? SettingType.Application : SettingType.User;
                var settingEntry = AllSettings.FirstOrDefault(e => e.Key == entryKey.Attribute("name").Value && e.Type == settingType);
                if (settingEntry == null)
                    continue;
                settingEntry.DeserializeValue(entryKey.Attribute("value").Value);
            }
        }

        public static void Save()
        {
            var settingsDoc = new XDocument();

            var rootElem = new XElement("Configuration");
            settingsDoc.Add(rootElem);

            foreach (SettingSource source in Enum.GetValues(typeof(SettingSource)))
                SaveSettings(rootElem, source);

            string settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LDDModder");

            Directory.CreateDirectory(settingsDir);
            string configFilePath = Path.Combine(settingsDir, "Settings.xml");
            settingsDoc.Save(configFilePath);
        }

        private static void SaveSettings(XElement parentNode, SettingSource source)
        {

            var sourceNode = new XElement(source.ToString());
            var entries = GetSettings(source);
            var appNode = new XElement("ApplicationSettings");
            var userNode = new XElement("UserSettings");

            foreach (var entry in entries)
            {
                var entryNode = entry.Type == SettingType.Application ? appNode : userNode;
                entryNode.Add(new XElement("Key", 
                    new XAttribute("name", entry.Key), 
                    new XAttribute("value", SerializeEntry(entry))));
            }

            if (appNode.HasElements)
                sourceNode.Add(appNode);
            if (userNode.HasElements)
                sourceNode.Add(userNode);
            if(sourceNode.HasElements)
                parentNode.Add(sourceNode);
        }

        private static string SerializeEntry(SettingEntry entry)
        {
            if (entry.ValueType == typeof(String))
                return (string)entry.Value;

            if (entry.ValueType == typeof(bool))
                return (bool)entry.Value ? "true" : "false";

            return entry.Value.ToString();
        }

    }
}
