using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.IO;
using System.Security.AccessControl;

namespace LDDModder.Utilities
{
    public static class SecurityHelper
    {
        private static int adminStatus = -1;

        public static bool IsUserAdministrator
        {
            get
            {
                if (adminStatus < 0)
                {
                    bool isAdmin;
                    try
                    {
                        WindowsIdentity user = WindowsIdentity.GetCurrent();
                        WindowsPrincipal principal = new WindowsPrincipal(user);
                        isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        isAdmin = false;
                    }
                    catch (Exception ex)
                    {
                        isAdmin = false;
                    }
                    adminStatus = isAdmin ? 1 : 0;
                    return isAdmin;
                }
                else
                {
                    return Convert.ToBoolean(adminStatus);
                }
            }
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
