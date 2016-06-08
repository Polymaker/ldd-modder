using System;
using System.Collections.Generic;
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
        public static void CreateCustomPalette(Bag info, Palette content)
        {
    
            var paletteName = GetShortPaletteName(info);
            var paletteDirectory = string.Format("{0}-{1}", paletteName, info.PaletteVersion);
            paletteDirectory = Path.Combine(LDDManager.ApplicationDataPath, "Palettes", paletteDirectory);

            //TODO: denying delete on the directory is no longer required, the setting DoServerCall=0 will prevent LDD from ovewriting/deleting our stuff
            //(preference.ini in ProgramFiles folder)

            //if (Directory.Exists(paletteDirectory))
            //    AllowDeleteDirectory(paletteDirectory);
            //else
            //    Directory.CreateDirectory(paletteDirectory);

            Directory.CreateDirectory(paletteDirectory);

            //DoServerCall
            var xmlSerSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, NewLineChars = Environment.NewLine };

            using (var fs = File.Create(Path.Combine(paletteDirectory, "Info.baxml")))
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

            //DenyDeleteDirectory(paletteDirectory);
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

        public static void DenyDeleteDirectory(string directoryPath)
        {
            var dInfo = new DirectoryInfo(directoryPath);
            if (!dInfo.Exists)
                dInfo.Create();
            var ntAccountName = WindowsIdentity.GetCurrent().Name;
            var dSecurity = dInfo.GetAccessControl();

            dSecurity.AddAccessRule(new FileSystemAccessRule(ntAccountName,
                FileSystemRights.Delete | FileSystemRights.DeleteSubdirectoriesAndFiles,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None, AccessControlType.Deny));

            dInfo.SetAccessControl(dSecurity);
        }

        public static void AllowDeleteDirectory(string directoryPath)
        {
            var dInfo = new DirectoryInfo(directoryPath);
            if (!dInfo.Exists)
                return;
            var ntAccountName = WindowsIdentity.GetCurrent().Name;
            var dSecurity = dInfo.GetAccessControl();

            dSecurity.RemoveAccessRule(new FileSystemAccessRule(ntAccountName,
                FileSystemRights.Delete | FileSystemRights.DeleteSubdirectoriesAndFiles,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None, AccessControlType.Deny));

            dInfo.SetAccessControl(dSecurity);
        }
    }
}
