using LDDModder.LDD.Data;
using LDDModder.LDD.Primitives.Connectors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.BrickEditor.Resources
{
    public static class ResourceHelper
    {
        public static Assimp.AssimpContext AssimpContext;

        public static List<Platform> Platforms { get; set; }

        public static List<MainGroup> Categories { get; set; }

        public static List<LDD.Primitives.Connectors.ConnectorInfo> Connectors { get; set; }

        static ResourceHelper()
        {
            Platforms = new List<Platform>();
            Categories = new List<MainGroup>();
            Connectors = new List<LDD.Primitives.Connectors.ConnectorInfo>();
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

        public static void LoadConnectors()
        {
            //var test = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var connectorsXml = GetResourceText("Data.Primitive connectors.xml");
            var xmlDoc = XDocument.Parse(connectorsXml);

            var connElems = xmlDoc.Descendants("Connector");
            Connectors.Clear();

            foreach (var connElem in connElems)
            {
                if (!connElem.TryReadAttribute("type", out ConnectorType connType))
                    continue;
                if (!connElem.TryReadAttribute("subtype", out int subType))
                    continue;

                //connElem.TryReadAttribute("gender", out string gender);

                var connInfo = new ConnectorInfo()
                {
                    Type = connType,
                    SubType = subType,
                    Description = connElem.ReadAttribute("description", string.Empty),
                };
                Connectors.Add(connInfo);
            }
        }

        public static Stream GetResourceStream(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources." + resourceName);
            return stream;
        }

        public static Bitmap GetResourceImage(string resourceName)
        {
            var stream = GetResourceStream(resourceName);
            if (stream != null)
                return (Bitmap)Image.FromStream(stream);
            return null;
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

        public static string GetComponentResourceString(this System.ComponentModel.Component form, string resourceName)
        {
            var compResource = new System.ComponentModel.ComponentResourceManager(form.GetType());
            
            return compResource.GetString(resourceName);
        }
    }
}
