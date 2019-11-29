using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Utilities;

namespace LDDModder.Modding.Editing
{
    [XmlRoot("Connection")]
    public /*abstract*/ class PartConnection : PartElement, IPhysicalElement
    {
        public const string NODE_NAME = "Connection";

        [XmlElement("Transform")]
        public ItemTransform Transform { get; set; }

        [XmlIgnore]
        public Connector Connector { get; set; }
        //public Connector Connector => _Connector;

        [XmlAttribute]
        public ConnectorType ConnectorType { get; set; }

        public PartConnection()
        {
            Transform = new ItemTransform();
        }

        protected PartConnection(ConnectorType connectorType)
        {
            Transform = new ItemTransform();
            ConnectorType = connectorType;
        }

        public PartConnection(Connector connector)
        {
            ConnectorType = connector.Type;
            Connector = connector;
            Transform = ItemTransform.FromLDD(connector.Transform);
        }

        public static PartConnection FromLDD(Connector connector)
        {
            return new PartConnection(connector);
        }

        public static PartConnection FromXml(XElement element)
        {
            var connectorType = element.ReadAttribute<ConnectorType>("Type");
            var partConn = new PartConnection(connectorType);
            partConn.LoadFromXml(element);
            return partConn;
        }

        public T GetConnector<T>() where T : Connector
        {
            return Connector as T;
        }

        public Connector GenerateLDD()
        {
            Connector.Transform = Transform.ToLDD();
            return Connector;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            Transform = ItemTransform.FromXml(element.Element("Transform"));
            element.Element("Transform").Remove();
            var connElem = new XElement(ConnectorType.ToString());

            connElem.Add(Transform.ToLDD().ToXmlAttributes());

            foreach (var attr in element.Element("Properties").Attributes())
                connElem.Add(new XAttribute(attr.Name.LocalName, attr.Value));
            if (element.HasElement("StudsArray", out XElement studs))
                connElem.Value = studs.Value;

            Connector = Connector.CreateFromType(ConnectorType);
            Connector.LoadFromXml(connElem);

        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            elem.Add(new XAttribute("Type", ConnectorType));

            if (Transform != null)
            {
                elem.Add(new XComment(Transform.GetLddXml().ToString()));
                elem.Add(Transform.SerializeToXml());
            }

            if (Connector != null)
            {
                var connectorXml = Connector.SerializeToXml();

                var propElem = elem.AddElement("Properties");

                foreach (var attr in connectorXml.Attributes())
                {
                    if (LDD.Primitives.Transform.AttributeNames.Contains(attr.Name.LocalName))
                        continue;

                    propElem.Add(new XAttribute(attr.Name.LocalName.Capitalize(), attr.Value));
                    //propElem.AddElement(attr.Name.LocalName.Capitalize(), attr.Value);
                }

                if (!string.IsNullOrEmpty(connectorXml.Value))//Custom2DField
                {
                    elem.AddElement("StudsArray", connectorXml.Value
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Trim()
                        );
                }
            }
            
            return elem;
        }
    }
    
}
