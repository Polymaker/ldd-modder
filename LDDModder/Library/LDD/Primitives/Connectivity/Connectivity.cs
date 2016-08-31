using LDDModder.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace LDDModder.LDD.Primitives
{
    [Serializable]
    public abstract class Connectivity
    {
        [XmlIgnore]
        public abstract ConnectivityType Type { get; }
        [XmlAttribute("type")]
        public int SubType { get; set; }
        [XmlAttribute("angle")]
        public double Angle { get; set; }
        [XmlAttribute("ax")]
        public double Ax { get; set; }
        [XmlAttribute("ay")]
        public double Ay { get; set; }
        [XmlAttribute("az")]
        public double Az { get; set; }
        [XmlAttribute("tx")]
        public double Tx { get; set; }
        [XmlAttribute("ty")]
        public double Ty { get; set; }
        [XmlAttribute("tz")]
        public double Tz { get; set; }

        protected virtual void OnDeserialized()
        {

        }

        public static IEnumerable<Connectivity> Deserialize(IEnumerable<XElement> nodes)
        {
            foreach (var node in nodes)
            {
                var result = Deserialize(node);
                if (result != null)
                    yield return result;
            }
        }

        public static IEnumerable<XElement> Serialize(IEnumerable<Connectivity> collisions)
        {
            foreach (var conObj in collisions)
            {
                var result = LDDModder.Serialization.XSerializationHelper.Serialize(conObj);
                if (result != null)
                {
                    OrderAttributes(result);
                    yield return result;
                }
            }
        }

        private static Type GetConnectivityType(string name)
        {
            var fullName = typeof(Connectivity).FullName + name;
            var myAssembly = System.Reflection.Assembly.GetAssembly(typeof(Connectivity));
            var conType = myAssembly.GetType(fullName);
            if (conType != null)
                return conType;
            foreach (Type subType in myAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Connectivity))))
            {
                if (LDDModder.Serialization.XSerializationHelper.GetTypeXmlRootName(subType).Equals(name))
                    return subType;
            }
            return null;
        }

        private static string[] GetAttributesOrder(Type connectivityType)
        {
            var attrOrderField = connectivityType.GetField("AttributeOrder", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Static);

            if (attrOrderField != null)
                return (string[])attrOrderField.GetValue(null);

            return new string[0];
        }

        private static void OrderAttributes(XElement elem)
        {

            var connectivityType = GetConnectivityType(elem.Name.LocalName);
            if (connectivityType == null)
                return;

            string[] attributeNamesOrdered = GetAttributesOrder(connectivityType);

            if (attributeNamesOrdered != null && attributeNamesOrdered.Length > 0)
                elem.SortAttributes(a => Array.IndexOf(attributeNamesOrdered, a.Name.LocalName));
        }

        public static Connectivity Deserialize(XElement node)
        {
            var connectivityType = GetConnectivityType(node.Name.LocalName);
            if (connectivityType != null)
            {
                var connectivityObject = (Connectivity)LDDModder.Serialization.XSerializationHelper.DefaultDeserialize(node, connectivityType);
                if (connectivityObject != null)
                    connectivityObject.OnDeserialized();
                return connectivityObject;
            }

            return null;
        }

    }
}
