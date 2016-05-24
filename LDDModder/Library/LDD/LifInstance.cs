using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.LDD
{
    public enum LifInstance
    {
        /// <summary>
        /// Lif file located in "%PROGRAMFILES%\LEGO Company\LEGO Digital Designer\".
        /// </summary>
        Assets,
        /// <summary>
        /// Lif file located in "%USERAPPDATA%\LEGO Company\LEGO Digital Designer\".
        /// </summary>
        Database,
        /// <summary>
        /// Lif file located inside the Assets lif file.
        /// </summary>
        AssetsDatabase
    }
}
