using LDDModder.LDD.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Resources
{
    public static class ResourceHelper
    {
        public static Assimp.AssimpContext AssimpContext;

        public static List<Platform> Platforms { get; set; }

        public static List<MainGroup> Categories { get; set; }

        static ResourceHelper()
        {
            Platforms = new List<Platform>();
            Categories = new List<MainGroup>();
        }

        public static void LoadPlatformsAndCategories()
        {
            var platformsJson = GetResourceText("Data.Platforms.json");
            var platforms = JsonConvert.DeserializeObject<List<Platform>>(platformsJson);
            Platforms.AddRange(platforms);
            var categoriesJson = GetResourceText("Data.Categories.json");
            var categories = JsonConvert.DeserializeObject<List<MainGroup>>(categoriesJson);
            Categories.AddRange(categories);
        }

        public static Stream GetResourceStream(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources." + resourceName);
            return stream;
        }

        public static string GetResourceText(string resourceName)
        {
            var stream = GetResourceStream(resourceName);
            if (stream != null)
            {
                using (var sr = new StreamReader(stream))
                    return sr.ReadToEnd();
            }
            return null;
        }

        public static Assimp.Scene GetResourceModel(string resourceName, string format)
        {
            if (AssimpContext == null)
                AssimpContext = new Assimp.AssimpContext();
            var stream = GetResourceStream(resourceName);

            return AssimpContext.ImportFileFromStream(stream, format);
        }
    }
}
