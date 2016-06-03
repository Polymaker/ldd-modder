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

        internal static string GetTypeXmlRootName(Type xmlObjType)
        {
            var xmlRootAttr = (XmlRootAttribute[])xmlObjType.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (xmlRootAttr != null && xmlRootAttr.Length > 0)
                return xmlRootAttr[0].ElementName;
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

        public static bool ContainsAttribute(this XElement element, XName attributeName)
        {
            return element.Attribute(attributeName) != null;
        }

        public static XElement Serialize(object obj)
        {
            var doc = new XDocument();
            var objType = obj.GetType();
            return Serialize(obj, GetTypeXmlRootName(objType));
        }

        public static XElement Serialize(object obj, string rootName)
        {
            var doc = new XDocument();
            var ser = new XmlSerializer(obj.GetType());
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var docWriter = doc.CreateWriter();
            ser.Serialize(docWriter, obj, ns);
            docWriter.Close();
            var rootElem = doc.Root;
            rootElem.Name = rootName;
            return rootElem;
        }
    }
}
