using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.General
{
    public static class PreferencesSettings
    {
        /// <summary>
        /// Purpose: Set the logging level (log output located at %AppData%\LEGO Company\DCLTrace.txt)
        /// Known values: 0 (no), 1 (yes)
        /// </summary>
        public const string Verbose = "verbose";//ProgramFiles preferences.ini
        /// <summary>
        /// Purpose: Allow or prevent LDD from making calls to the servers. When disabled, LDD will not overwrite/delete modded files
        /// Known values: 0 (no), 1 (yes)
        /// </summary>
        public const string DoServerCall = "DoServerCall";//ProgramFiles preferences.ini
        /// <summary>
        /// Purpose: Set LDD in developer mode
        /// Known values: 0|no, 1|yes (both numeric and text works)
        /// </summary>
        public const string DeveloperMode = "DeveloperMode";//AppData preferences.ini
        /// <summary>
        /// Purpose: Allow to show brick tooltip
        /// Known values: no, yes
        /// </summary>
        public const string ShowToolTip = "SHOWTOOLTIPS";//AppData preferences.ini
        /// <summary>
        /// Purpose: Allow to show extended info in brick tooltip (id, element id, subparts colors)
        /// Known values: no, yes
        /// </summary>
        public const string ShowExtendedBrickToolTip = "ShowExtendedBrickToolTip";//AppData preferences.ini
        /// <summary>
        /// Purpose: ?
        /// Known values: ?
        /// </summary>
        public const string UserPalettes = "UserPalettes";
        /// <summary>
        /// Purpose: Alter user palettes directory (UNTESTED)
        /// Known values: ?
        /// </summary>
        public const string UserPalettesPath = "UserPalettesPath";

        public const string UserModelDirectory = "UserModelDirectory";
        /// <summary>
        /// Purpose: Alter LDD palettes directory (UNTESTED)
        /// Known values: ?
        /// </summary>
        public const string PalettePath = "PalettePath";
        /// <summary>
        /// Purpose: Unknown (+/-TESTED)
        /// Known values: [0,1] ?
        /// </summary>
        public const string QAMode = "QAMode";//tested ProgramFiles and AppData preferences.ini
        /// <summary>
        /// Purpose: Unknown (UNTESTED)
        /// Known values: [0,1] ?
        /// </summary>
        public const string LoadMostRecentModel = "LoadMostRecentModel";//should be ProgramFiles preferences.ini
        /// <summary>
        /// Purpose: Prevent loading assemblies. All hidden (sub)parts will appear in the palette (TESTED)
        /// Known values: [0,1] ?
        /// </summary>
        public const string LoadAssemblies = "LoadAssemblies";//ProgramFiles preferences.ini

        public static string DecodePath(string pathStr)
        {
            if (string.IsNullOrEmpty(pathStr) || !pathStr.StartsWith("file/"))
                return pathStr;

            pathStr = pathStr.Substring(5).Replace('/', '\\');
            pathStr = pathStr.Insert(1, ":");
            return pathStr;
        }

        public static string EncodePath(string pathStr)
        {
            if (string.IsNullOrEmpty(pathStr))
                return pathStr;
            return "file/" + pathStr.Replace('\\', '/').Replace(":", string.Empty);
        }

    }
}
