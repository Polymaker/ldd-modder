using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace LDDModder.LDD.Palettes
{
    public static class PaletteManager
    {
        public static void CreateCustomPalette(Bag info, Palette content)
        {
            //if (!LDDManager.IsLifExtracted(LifInstance.Database))
            //    return;

            var dirName = string.Format("{0}-{1}", info.Name, info.PaletteVersion);
            dirName = Path.Combine(LDDManager.ApplicationDataPath, "Palettes", dirName);
            Directory.CreateDirectory(dirName);
            var dInfo = new DirectoryInfo(dirName);
            var ntAccountName = WindowsIdentity.GetCurrent().Name;
            var dSecurity = dInfo.GetAccessControl(AccessControlSections.All);
            var rules = dSecurity.GetAccessRules(true, true, typeof(NTAccount));
            dSecurity.AddAccessRule(new FileSystemAccessRule(ntAccountName, FileSystemRights.Delete | FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Deny));
        }

        public static void CreatePaletteDirectory(string paletteName)
        {
            //if (!LDDManager.IsLifExtracted(LifInstance.Database))
            //    return;
            
            var dirName = Path.Combine(LDDManager.ApplicationDataPath, "Palettes", paletteName);
            Directory.CreateDirectory(dirName);

            using (var fs = File.Create(Path.Combine(dirName, "new file.txt")))
            {
                using (var sw = new StreamWriter(fs))
                    sw.WriteLine("Hello");
            }

            var dInfo = new DirectoryInfo(dirName);
            var ntAccountName = WindowsIdentity.GetCurrent().Name;
            var dSecurity = dInfo.GetAccessControl();
            
            dSecurity.AddAccessRule(new FileSystemAccessRule(ntAccountName, 
                FileSystemRights.Delete | FileSystemRights.DeleteSubdirectoriesAndFiles, 
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, 
                PropagationFlags.None, AccessControlType.Deny));

            dInfo.SetAccessControl(dSecurity);
            
        }

        private static void DenyDeleteDirectory(string directoryPath)
        {
            var dInfo = new DirectoryInfo(directoryPath);
            var ntAccountName = WindowsIdentity.GetCurrent().Name;
            var dSecurity = dInfo.GetAccessControl();

            dSecurity.AddAccessRule(new FileSystemAccessRule(ntAccountName,
                FileSystemRights.Delete | FileSystemRights.DeleteSubdirectoriesAndFiles,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None, AccessControlType.Deny));

            dInfo.SetAccessControl(dSecurity);
        }

        private static void AllowDeleteDirectory(string directoryPath)
        {
            var dInfo = new DirectoryInfo(directoryPath);
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
