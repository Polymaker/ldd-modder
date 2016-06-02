using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Utilities
{
    public static class XSerializationHelper
    {
        public static T DefaultDeserialize<T>(XElement elem)// where T : IXmlSerializable
        {
            if (elem == null)
                return default(T);
            var realElemName = elem.Name;
            //XmlSerializer throws an exception if the root element name does not match the type name or the specified XmlRoot attribute.
            //so we fix this by changing the element's name
            elem.Name = GetTypeXmlRootName(typeof(T));
            
            try
            {
                var doc = new XDocument(elem);

                using (var xReader = doc.CreateReader())
                {
                    var ser = new XmlSerializer(typeof(T));
                    return (T)ser.Deserialize(xReader);
                }
            }
            finally
            {
                elem.Name = realElemName;
            }
        }

        private static string GetTypeXmlRootName(Type xmlObjType)
        {
            var xmlRootAttr = (XmlRootAttribute[])xmlObjType.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (xmlRootAttr.Length > 0)
                return xmlRootAttr[0].Namespace;
            return xmlObjType.Name;
        }

        public static int GetIntAttribute(this XElement element, XName attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return int.Parse(attribute.Value);
        }

        public static bool ContainsElement(this XElement element, XName childElementName)
        {
            return element.Element(childElementName) != null;
        }

        public static XElement Serialize<T>(T obj)
        {
            var doc = new XDocument();
            var ser = new XmlSerializer(typeof(T));
            ser.Serialize(doc.CreateWriter(), obj);
            var rootElem = doc.Root;
            rootElem.Name = GetTypeXmlRootName(typeof(T));
            return rootElem;
        }

        public static XElement Serialize(object obj, string rootName)
        {
            var doc = new XDocument();
            var ser = new XmlSerializer(obj.GetType());
            ser.Serialize(doc.CreateWriter(), obj);
            var rootElem = doc.Root;
            rootElem.Name = rootName;
            return rootElem;
        }
    }
}
