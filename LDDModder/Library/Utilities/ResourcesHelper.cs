using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LDDModder.Utilities
{
    public static class ResourcesHelper
    {
        private static Dictionary<Assembly, string> AssemResInfo;

        static ResourcesHelper()
        {
            AssemResInfo = new Dictionary<Assembly, string>();

            GetResourcesBasePath(Assembly.GetEntryAssembly());

            foreach (var assemName in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                var assembly = Assembly.Load(assemName);
                var assemAttrs = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (assemAttrs == null || assemAttrs.Length == 0)
                    continue;
                if ((assemAttrs[0] as AssemblyCompanyAttribute).Company == "PolyDev")
                    GetResourcesBasePath(assembly);
            }
        }

        public static Stream GetResourceByFullname(string resourceName)
        {
            var resourceAssembly = GetAssemblyFromResourcePath(resourceName);
            if (resourceAssembly != null)
                return GetResourceByFullname(resourceAssembly, resourceName);
            return null;
        }

        public static Stream GetResourceByFullname(Assembly resourceAssembly, string resourceName)
        {
            return resourceAssembly.GetManifestResourceStream(resourceName);
        }

        public static Stream GetResource(string resourceName)
        {
            var resourceAssembly = Assembly.GetCallingAssembly();
            return GetResourceByFullname(resourceAssembly, GetResourcesBasePath(resourceAssembly) + resourceName);
        }

        public static Stream GetResource<T>(string resourceName)
        {
            var resourceAssembly = typeof(T).Assembly;
            return GetResourceByFullname(resourceAssembly, GetResourcesBasePath(resourceAssembly) + resourceName);
        }

        private static readonly Regex RES_PATH_SUBSTR_REGX = new Regex("^.+?Resources?\\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Assembly GetAssemblyFromResourcePath(string resourceName)
        {
            var resBasePath = RES_PATH_SUBSTR_REGX.Match(resourceName);
            if (resBasePath.Success && AssemResInfo.Values.Contains(resBasePath.Value))
            {
                return AssemResInfo.First(kv => kv.Value == resBasePath.Value).Key;
            }
            return null;
        }

        private static string GetResourcesBasePath(Assembly assembly)
        {
            if (AssemResInfo.ContainsKey(assembly))
                return AssemResInfo[assembly];
            var resNames = assembly.GetManifestResourceNames();
            if (resNames.Length == 0)
                return assembly.GetName().Name;
            var resPaths = resNames.Where(n => n.Contains("Resource")).Select(n=> RES_PATH_SUBSTR_REGX.Match(n).Value).ToList();
            if (resPaths.Count == 0)
                return string.Empty;
            var commonPath = resPaths.GroupBy(n => n).OrderByDescending(g => g.Count()).First().Key;

            AssemResInfo.Add(assembly, commonPath);
            return commonPath;
        }
    }
}
