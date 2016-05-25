using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
namespace LDDModder.Utilities
{
    public static class General
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
    }
}
