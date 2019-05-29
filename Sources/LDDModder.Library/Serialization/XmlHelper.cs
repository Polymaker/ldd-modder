using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Serialization
{
    public static class XmlHelper
    {
        public static XAttribute[] ToXmlAttributes(this Vector3 vector, string name1, string name2, string name3)
        {
            return new XAttribute[]
            {
                new XAttribute(name1, vector.X.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute(name2, vector.Y.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute(name3, vector.Z.ToString(NumberFormatInfo.InvariantInfo))
            };
        }
        internal static string GetTypeXmlRootName(Type xmlObjType)
        {
            var xmlRootAttr = (XmlRootAttribute[])xmlObjType.GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (xmlRootAttr != null && xmlRootAttr.Length > 0)
                return xmlRootAttr[0].ElementName;
            return xmlObjType.Name;
        }

        public static T DefaultDeserialize<T>(XElement elem)
        {
            return (T)DefaultDeserialize(elem, typeof(T));
        }

        public static object DefaultDeserialize(XElement elem, Type objType)
        {
            if (elem == null)
                return null;

            var realElemName = elem.Name;

            try
            {
                if (objType.GetInterface("IXmlObject") != null)
                {
                    var instance = (IXmlObject)Activator.CreateInstance(objType);
                    instance.LoadFromXml(elem);
                    return instance;
                }

                //XmlSerializer throws an exception if the root element name does not match the type name or the specified XmlRoot attribute.
                //so we fix this by changing the element's name
                elem.Name = GetTypeXmlRootName(objType);

                using (var xReader = elem.CreateReader())
                {
                    var ser = new XmlSerializer(objType);
                    return ser.Deserialize(xReader);
                }
            }
            finally
            {
                elem.Name = realElemName;
            }
        }

        public static XElement DefaultSerialize<T>(T obj, string elementName = null)
        {
            if (string.IsNullOrEmpty(elementName))
                elementName = GetTypeXmlRootName(typeof(T));

            var doc = new XDocument();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var ser = new XmlSerializer(typeof(T));

            using (var docWriter = doc.CreateWriter())
                ser.Serialize(docWriter, obj, ns);

            var rootElem = doc.Root;
            rootElem.Name = elementName;
            return rootElem;
        }
    }
}
