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

        public static Stream GetResourceStream(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("LDDModder.BrickEditor.Resources." + resourceName);
            return stream;
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
