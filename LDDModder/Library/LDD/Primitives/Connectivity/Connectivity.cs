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
        private int _SubType;
        private ConnectorInfo _Info;

        [XmlIgnore]
        public abstract ConnectivityType Type { get; }
        [XmlAttribute("type")]
        public int SubType
        {
            get { return _SubType; }
            set
            {
                _SubType = value;
                _Info = null;
            }
        }
        
        [XmlIgnore]
        public Transform Transform { get; set; }

        //[XmlAttribute("angle")]
        //public double Angle { get; set; }
        //[XmlAttribute("ax")]
        //public double Ax { get; set; }
        //[XmlAttribute("ay")]
        //public double Ay { get; set; }
        //[XmlAttribute("az")]
        //public double Az { get; set; }
        //[XmlAttribute("tx")]
        //public double Tx { get; set; }
        //[XmlAttribute("ty")]
        //public double Ty { get; set; }
        //[XmlAttribute("tz")]
        //public double Tz { get; set; }

        [XmlIgnore]
        public ConnectorInfo Info
        {
            get
            {
                if (_Info == null)
                    _Info = ConnectorInfo.Connectors.FirstOrDefault(c => c.Type == Type && c.SubType == SubType);
                return _Info;
            }
        }

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

        public static IEnumerable<XElement> Serialize(IEnumerable<Connectivity> connections)
        {
            foreach (var conObj in connections)
            {
                var result = LDDModder.Serialization.XSerializationHelper.Serialize(conObj);

                if (result != null)
                {
                    var tranformElem = LDDModder.Serialization.XSerializationHelper.Serialize(conObj.Transform);
                    foreach (var transformAttr in tranformElem.Attributes())
                        result.Add(transformAttr);
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
                var transfo = LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<Transform>(node);
                var connectivityObject = (Connectivity)LDDModder.Serialization.XSerializationHelper.DefaultDeserialize(node, connectivityType);
                connectivityObject.Transform = transfo;
                if (connectivityObject != null)
                    connectivityObject.OnDeserialized();
                return connectivityObject;
            }

            return null;
        }


        public bool ConnectsTo(Connectivity other)
        {
            //TODO: create a dictionnary/list/table/whatever that contains possibles connections (some connectors can fit with other than it's male/female counterpart)

            var myConnector = Info;
            var otherConnector = other.Info;
            if (myConnector == null || otherConnector == null)
                return false;

            return myConnector.Type == otherConnector.Type && 
                myConnector.Name == otherConnector.Name && 
                myConnector.IsMale != otherConnector.IsMale;
        }
    }
}
