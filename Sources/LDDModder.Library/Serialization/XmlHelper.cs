﻿using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using LDDModder.Utilities;

namespace LDDModder.Serialization
{
    public static class XmlHelper
    {
        #region Vector Serialization

        public static XAttribute ToXmlAttribute(this Vector3 vector, string name)
        {
            return new XAttribute(name, string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", vector.X, vector.Y, vector.Z));
        }

        public static XAttribute ToXmlAttribute(this Vector3d vector, string name)
        {
            return new XAttribute(name, string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", vector.X, vector.Y, vector.Z));
        }

        public static XAttribute[] ToXmlAttributes(this Vector3 vector)
        {
            return ToXmlAttributes(vector, "X", "Y", "Z");
        }

        public static XAttribute[] ToXmlAttributes(this Vector3 vector, string name1, string name2, string name3)
        {
            return new XAttribute[]
            {
                new XAttribute(name1, vector.X.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute(name2, vector.Y.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute(name3, vector.Z.ToString(NumberFormatInfo.InvariantInfo))
            };
        }

        public static XAttribute[] ToXmlAttributes(this Vector3d vector)
        {
            return ToXmlAttributes(vector, "X", "Y", "Z");
        }

        public static XAttribute[] ToXmlAttributes(this Vector3d vector, string name1, string name2, string name3)
        {
            return new XAttribute[]
            {
                new XAttribute(name1, vector.X.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute(name2, vector.Y.ToString(NumberFormatInfo.InvariantInfo)),
                new XAttribute(name3, vector.Z.ToString(NumberFormatInfo.InvariantInfo))
            };
        }

        public static Vector3 ReadVector3Attributes(XElement element, string xName = "X", string yName = "Y", string zName = "Z")
        {
            return new Vector3(
                    element.ReadAttribute(xName, 0f),
                    element.ReadAttribute(yName, 0f),
                    element.ReadAttribute(zName, 0f)
                );
        }

        public static Vector3d ReadVector3dAttributes(XElement element, string xName = "X", string yName = "Y", string zName = "Z")
        {
            return new Vector3d(
                    element.ReadAttribute(xName, 0d),
                    element.ReadAttribute(yName, 0d),
                    element.ReadAttribute(zName, 0d)
                );
        }

        public static Vector3 ParseVector3Attribute(XAttribute attribute)
        {
            string[] values = attribute.Value.Split(',');
            StringUtils.TryParse(values[0], out float x);
            StringUtils.TryParse(values[1], out float y);
            StringUtils.TryParse(values[2], out float z);
            return new Vector3(x, y, z);
        }

        public static Vector3d ParseVector3dAttribute(XAttribute attribute)
        {
            string[] values = attribute.Value.Split(',');
            StringUtils.TryParse(values[0], out double x);
            StringUtils.TryParse(values[1], out double y);
            StringUtils.TryParse(values[2], out double z);
            return new Vector3d(x, y, z);
        }

        #endregion



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

            if (typeof(T).GetInterface("IXmlObject") != null)
            {
                var resultElem = (obj as IXmlObject).SerializeToXml();
                if (elementName != typeof(T).Name)
                    resultElem.Name = elementName;
                return resultElem;
            }

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

        public static XObject ToXml<T>(Expression<Func<T>> expression)
        {
            var memberExpr = expression.Body as MemberExpression;

            
            var itemValue = expression.Compile().Invoke();
            var itemValueStr = string.Format(CultureInfo.InvariantCulture, "{0}", itemValue);

            var attributeInfo = memberExpr.Member.GetCustomAttribute<XmlAttributeAttribute>();
            if (attributeInfo != null)
            {
                string attributeName = string.IsNullOrEmpty(attributeInfo.AttributeName) ?
                    memberExpr.Member.Name : attributeInfo.AttributeName;
                    
                return new XAttribute(attributeName, itemValueStr);
            }

            var elementInfo = memberExpr.Member.GetCustomAttribute<XmlElementAttribute>();
            if (elementInfo != null)
            {
                string elementName = string.IsNullOrEmpty(elementInfo.ElementName) ?
                    memberExpr.Member.Name : elementInfo.ElementName;

                return new XElement(elementName, itemValueStr);
            }

            return null;
        }


    }
}
