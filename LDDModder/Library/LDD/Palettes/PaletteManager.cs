using LDDModder.Configuration;
using LDDModder.LDD.Files;
using LDDModder.LDD.General;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    public static class PaletteManager
    {
        private static bool _HasInitialized;
        private static bool _IsLddPaletteExtended;
        private static bool isInitializing;
        private static List<PaletteFile> _Palettes;
        private static bool _CanExtendLddPalettes;
        private readonly static XmlSerializer BAXML_SERIALIZER;
        private readonly static XmlSerializer PAXML_SERIALIZER;

        public static string LddPalettesDirectory
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return SettingsManager.LddPalettesDirectory;
            }
        }

        public static string UserPalettesDirectory
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return SettingsManager.UserPalettesDirectory;
            }
        }

        public static bool CanExtendLddPalettes
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return _CanExtendLddPalettes;
            }
        }

        public static bool IsLddPaletteExtended
        {
            get
            {
                if (!HasInitialized)
                    Initialize();
                return _IsLddPaletteExtended;
            }
        }

        public static List<PaletteFile> Palettes
        {
            get { return _Palettes; }
        }

        public static bool HasInitialized
        {
            get { return _HasInitialized; }
        }

        static PaletteManager()
        {
            _Palettes = new List<PaletteFile>();
            BAXML_SERIALIZER = new XmlSerializer(typeof(Bag));
            PAXML_SERIALIZER = new XmlSerializer(typeof(Palette));
        }

        public static void Initialize()
        {
            if (isInitializing)
                return;

            isInitializing = true;

            if (!SettingsManager.HasLoadedOnce)
                SettingsManager.Load();

            _IsLddPaletteExtended = false;

            if (LDDManager.IsInstalled)
            {

                if (string.IsNullOrEmpty(SettingsManager.LddPalettesDirectory) || 
                    !ValidateLddPalettesDirectory(SettingsManager.LddPalettesDirectory))
                {
                    string currentPath = Path.Combine(LDDManager.ApplicationDataPath, "Palettes");
                    if (ValidateLddPalettesDirectory(currentPath))
                        SettingsManager.LddPalettesDirectory.Value = currentPath;
                }
                
                string customUserPalettesDir;
                if (LDDManager.GetSettingValue(PreferencesSettings.UserPalettes, LDDLocation.ProgramFiles, out customUserPalettesDir) ||
                    LDDManager.GetSettingValue("UserPalettes", LDDLocation.AppData, out customUserPalettesDir))
                {
                    SettingsManager.UserPalettesDirectory.Value = customUserPalettesDir;
                }
                else
                    SettingsManager.UserPalettesDirectory.Value = Path.Combine(LDDManager.ApplicationDataPath, "UserPalettes");

                //if DoServerCall is not turned off, LDD will delete custom directories and redownload missing (replaced?) lifs.
                //We can however prevent LDD from deleting a custom palette directory, but it is not desirable for an overwrited LDD palette because the entry will be doubled.
                //And a custom palette can be placed in the UserPalette directory, saving the trouble of removing delete rights, so the 'base' Palette directory will only be used to extend base palettes.
                _CanExtendLddPalettes = !LDDManager.GetSettingBoolean("DoServerCall", LDDLocation.ProgramFiles, true);

            }
            else
            {
                _CanExtendLddPalettes = false;
                SettingsManager.LddPalettesDirectory.Value = string.Empty;
                SettingsManager.UserPalettesDirectory.Value = string.Empty;
            }

            if (CanExtendLddPalettes)
            {
                foreach (var paletteDir in Directory.GetDirectories(LddPalettesDirectory))
                {
                    if (paletteDir.StartsWith("LDD"))
                    {
                        if (Directory.EnumerateFiles(paletteDir).Any())
                            _IsLddPaletteExtended = true;
                        break;
                    }
                }
            }

            if (SettingsManager.HasChanges)
                SettingsManager.Save();

            isInitializing = false;
            _HasInitialized = true;
        }

        

        public static string GetPaletteDirectory(PaletteType directory)
        {
            return directory == PaletteType.User ? UserPalettesDirectory : LddPalettesDirectory;
        }

        #region Validating Palettes directories

        private static bool ValidateLddPalettesDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                return false;
            if (Directory.EnumerateFileSystemEntries(directoryPath, "*.lif").Any())
                return true;

            return Directory.EnumerateFileSystemEntries(directoryPath, "*.paxml", SearchOption.AllDirectories).Any();
        }

        //private static readonly string[] UserPalettesExt = new string[] { ".lxfml", ".lxf"/*, "paxml", "lif"*/ };

        private static bool ValidateUserPalettesDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);//for user palettes we only check if the directory exists, and will mostly be empty
            //if (!Directory.Exists(directoryPath))
            //    return false;

            ////if (Directory.EnumerateFileSystemEntries(directoryPath).Any(file=> UserPalettesExt.Contains(Path.GetExtension(file))))
            ////    return true;

            //return false;
        }

        #endregion

        #region Palette loading

        public static void LoadPalettes()
        {
            if (!LDDManager.IsInstalled)
                return;
            Palettes.Clear();
            _IsLddPaletteExtended = false;
            foreach (var lifPath in Directory.GetFiles(LddPalettesDirectory, "*.lif"))
            {
                var palette = LoadFromLif(lifPath);
                if (palette != null)
                    Palettes.Add(palette);
            }

            foreach (var paletteDir in Directory.GetDirectories(LddPalettesDirectory))
            {
                var palette = LoadFromDirectory(paletteDir, PaletteType.ExtendedLDD);
                if (palette != null)
                {
                    Palettes.Add(palette);
                    if (palette.Name == "LDD")
                        _IsLddPaletteExtended = true;
                }
            }
            if (Directory.Exists(UserPalettesDirectory))
            {
                foreach (var paletteDir in Directory.GetDirectories(UserPalettesDirectory))
                {
                    var palette = LoadFromDirectory(paletteDir, PaletteType.User);
                    if (palette != null)
                        Palettes.Add(palette);
                }
            }
        }

        public static PaletteFile LoadFromLif(string filePath)
        {
            LifFile lifFile = null;
            try
            {
                lifFile = LifFile.Open(filePath);

                //if (!lifFile.Entries.Any(e => e.Name.Equals(Bag.FileName, StringComparison.InvariantCultureIgnoreCase) && e.IsFile))
                //    return null;

                var infoEntry = lifFile.Entries.FirstOrDefault(e => e.Name.Equals(Bag.FileName, StringComparison.InvariantCultureIgnoreCase) && e.IsFile) as LifFile.FileEntry;
                if (infoEntry == null)
                    return null;

                var palette = new PaletteFile(Bag.Load(infoEntry.OpenStream()), filePath, PaletteType.LDD);
                palette.Info.OriginFileName = Bag.FileName;//OriginFileName will be mainly used when extending lif palette to keep the same filenames when saving

                foreach (var entry in lifFile.Entries.OfType<LifFile.FileEntry>())
                {
                    if (entry.Name.EndsWith(".paxml"))
                    {
                        var paletteContent = Palette.Load(entry.OpenStream());
                        paletteContent.OriginFileName = entry.Name;
                        palette.Palettes.Add(paletteContent);
                    }
                }

                return palette;
            }
            catch
            {
                Trace.WriteLine(string.Format("Problem reading lif (\"{0}\")", filePath));
            }
            finally
            {
                if (lifFile != null)
                {
                    lifFile.Close();
                    lifFile.Dispose();
                    lifFile = null;
                }
            }
            return null;
        }

        private static PaletteFile LoadFromDirectory(string directoryPath, PaletteType directory)
        {
            string paletteInfoFilepath = Path.Combine(directoryPath, Bag.FileName);
            if (!File.Exists(paletteInfoFilepath))
                return null;

            var palette = new PaletteFile(Bag.Load(paletteInfoFilepath), directoryPath, directory);
            palette.Info.OriginFileName = Bag.FileName;

            foreach (string paxmlPath in Directory.GetFiles(directoryPath, "*.paxml"))
            {
                var paletteContent = Palette.Load(paxmlPath);
                paletteContent.OriginFileName = Path.GetFileName(paxmlPath);
                palette.Palettes.Add(paletteContent);
            }
            return palette;
        }

        #endregion

        public static void SavePalette(PaletteFile palette)
        {
            if (palette.Type != PaletteType.User && !CanExtendLddPalettes)
            {
                throw new InvalidOperationException("We can't save an LDD palette if the setting 'DoServerCall' is not set to '0'");
            }

            if (string.IsNullOrEmpty(palette.PalettePath))
            {
                string paletteName = GetShortPaletteName(palette.Info);
                palette.PalettePath = Path.Combine(
                    GetPaletteDirectory(palette.Type), 
                    string.Format("{0}-{1}", paletteName, palette.Info.PaletteVersion));
            }

            if (palette.Type == PaletteType.User && !Directory.Exists(UserPalettesDirectory))
                Directory.CreateDirectory(UserPalettesDirectory);

            if (palette.Type == PaletteType.LDD)
            {
                var paletteDir = Path.Combine(LddPalettesDirectory, Path.GetFileNameWithoutExtension(palette.PalettePath));
                Directory.CreateDirectory(paletteDir);
                //Extract lif as it can have other files (eg: LDDExtended palette has an xml file for overwriting the available colors in the material chooser)
                using(var lifFile = LifFile.Open(palette.PalettePath))
                    lifFile.Extract(paletteDir);
                LDDManager.CompressLif(palette.PalettePath);
                File.Delete(palette.PalettePath);
                palette.OverwritedLDD(paletteDir);
            }

            Directory.CreateDirectory(palette.PalettePath);

            var xmlSerSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, NewLineChars = Environment.NewLine };

            using (var fs = File.Create(Path.Combine(palette.PalettePath, Bag.FileName)))
            {
                var xmlWriter = XmlTextWriter.Create(fs, xmlSerSettings);
                BAXML_SERIALIZER.Serialize(xmlWriter, palette.Info);
            }

            foreach (var paletteContent in palette.Palettes)
            {
                if (string.IsNullOrEmpty(paletteContent.OriginFileName))
                {
                    paletteContent.OriginFileName = GetShortPaletteName(palette.Info);
                    if (palette.Palettes.Count > 1)
                        paletteContent.OriginFileName += "-" + palette.Palettes.IndexOf(paletteContent);
                    paletteContent.OriginFileName += ".paxml";
                }

                using (var fs = File.Create(Path.Combine(palette.PalettePath, paletteContent.OriginFileName)))
                {
                    var xmlWriter = XmlTextWriter.Create(fs, xmlSerSettings);
                    PAXML_SERIALIZER.Serialize(xmlWriter, paletteContent);
                }
            }
        }

        public static void CreateCustomPalette(Bag info, Palette content)
        {
    
            var paletteName = GetShortPaletteName(info);
            var paletteDirectory = string.Format("{0}-{1}", paletteName, info.PaletteVersion);
            paletteDirectory = Path.Combine(LddPalettesDirectory, paletteDirectory);

            //TODO: denying delete on the directory is no longer required, the setting DoServerCall=0 will prevent LDD from overwriting/deleting our stuff
            //(preference.ini in ProgramFiles folder)

            //if (Directory.Exists(paletteDirectory))
            //    SecurityHelper.AllowDeleteDirectory(paletteDirectory);
            //else
            //    SecurityHelper.Directory.CreateDirectory(paletteDirectory);

            Directory.CreateDirectory(paletteDirectory);

            //DoServerCall
            var xmlSerSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, NewLineChars = Environment.NewLine };

            using (var fs = File.Create(Path.Combine(paletteDirectory, Bag.FileName)))
            {
                var xmlSer = new XmlSerializer(typeof(Bag));
                var xmlWriter = XmlTextWriter.Create(fs, xmlSerSettings);
                xmlSer.Serialize(xmlWriter, info);
            }

            using (var fs = File.Create(Path.Combine(paletteDirectory, paletteName + ".paxml")))
            {
                var xmlSer = new XmlSerializer(typeof(Palette));
                var xmlWriter = XmlTextWriter.Create(fs, xmlSerSettings);
                xmlSer.Serialize(xmlWriter, content);
            }

            //SecurityHelper.DenyDeleteDirectory(paletteDirectory);
        }

        private static string GetShortPaletteName(Bag info)
        {
            string cleanName = info.Name.Replace(" ", string.Empty);

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                cleanName = cleanName.Replace(invalidChar.ToString(), string.Empty);

            int firstLetter = 0;
            for (; firstLetter < cleanName.Length; firstLetter++)
                if (char.IsLetter(cleanName[firstLetter]))
                    break;

            if (firstLetter > 0)
                cleanName = cleanName.Substring(firstLetter);

            return cleanName;
        }

    }
}
