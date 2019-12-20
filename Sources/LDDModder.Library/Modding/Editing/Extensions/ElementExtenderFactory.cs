using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding.Editing
{
    public static class ElementExtenderFactory
    {
        public static Dictionary<Type, List<Type>> ElementExtensions;

        static ElementExtenderFactory()
        {
            ElementExtensions = new Dictionary<Type, List<Type>>();
        }

        public static void RegisterExtension(Type elementType, Type extensionType)
        {
            if (!ElementExtensions.ContainsKey(elementType))
                ElementExtensions.Add(elementType, new List<Type>());

            var previousExtenders = ElementExtensions[elementType];

            if (previousExtenders.Count > 0)
            {
                if (previousExtenders.Count(x => x.IsAssignableFrom(extensionType)) > 1)
                {
                    throw new InvalidOperationException("");
                }
            }

            ElementExtensions[elementType].Add(extensionType);
        }

        public static bool CanExtendElement(Type elementType, Type extensionType)
        {
            foreach (var keyVal in ElementExtensions)
            {
                if (keyVal.Key == elementType || keyVal.Key.IsAssignableFrom(elementType))
                {
                    return keyVal.Value.Any(x => x == extensionType || extensionType.IsAssignableFrom(x));
                }
            }
            return false;
        }

        public static Type GetExtenderType(Type elementType, Type extensionType)
        {
            if (ElementExtensions.ContainsKey(elementType))
            {
                var extenderTypes = ElementExtensions[elementType];

                var descendantType = extenderTypes.FirstOrDefault(x => extensionType.IsAssignableFrom(x));
                if (descendantType != null)
                    return descendantType;

                if (extenderTypes.Contains(extensionType))
                    return extensionType;
            }
            else
            {
                foreach (var keyVal in ElementExtensions)
                {
                    if (keyVal.Key.IsAssignableFrom(elementType))
                    {
                        var extenderTypes = keyVal.Value;

                        var descendantType = extenderTypes.FirstOrDefault(x => extensionType.IsAssignableFrom(x));
                        if (descendantType != null)
                            return descendantType;

                        if (extenderTypes.Contains(extensionType))
                            return extensionType;
                    }
                }
            }
            return null;
        }

        public static IElementExtender CreateExtender(PartElement element, Type extensionType)
        {
            var finalType = GetExtenderType(element.GetType(), extensionType);
            if (finalType != null)
            {
                var ctor = finalType.GetConstructor(new Type[] { typeof(PartElement) });
                if (ctor != null)
                    return (IElementExtender)ctor.Invoke(new object[] { element });

                ctor = finalType.GetConstructor(new Type[] { element.GetType() });
                if (ctor != null)
                    return (IElementExtender)ctor.Invoke(new object[] { element });

                return (IElementExtender)Activator.CreateInstance(finalType);
            }
            return null;
        }
    }
}
