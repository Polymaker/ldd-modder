using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LDDModder.LDD;
using LDDModder.Modding;

namespace LDDModder.BrickInstaller
{
    internal static class Common
    {
        public static void ValidateLddInstall(IWin32Window owner = null)
        {
            if (!LDDManager.IsInstalled)
            {
                //TODO dialog to manually select LDD install dir
            }
            if (!LDDManager.IsLifExtracted(LifInstance.Database))
            {

            }
        }
    }
}
