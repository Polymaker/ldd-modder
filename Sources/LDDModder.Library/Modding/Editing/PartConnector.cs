using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Utilities;

namespace LDDModder.Modding.Editing
{
    public interface IPartConnector
    {
        Connector Connector { get; }
    }

    public abstract class PartConnector : PartComponent, IPartConnector
    {
        public const string NODE_NAME = "Connection";

        public ItemTransform Transform { get; set; }

        public Connector Connector => GetConnector();

        public ConnectorType ConnectorType => Connector.Type;

        public PartConnector()
        {
            Transform = new ItemTransform();
        }

        public static PartConnector FromLDD(Connector connector)
        {
            var genType = typeof(PartConnector<>).MakeGenericType(connector.GetType());
            return (PartConnector)Activator.CreateInstance(genType, connector);
        }

        public static PartConnector FromXml(XElement element)
        {
            var connectorType = element.ReadAttribute<ConnectorType>("Type");
            var defaultConnector = Connector.CreateFromType(connectorType);
            var genType = typeof(PartConnector<>).MakeGenericType(defaultConnector.GetType());
            var partConn = (PartConnector)Activator.CreateInstance(genType, defaultConnector);
            partConn.LoadFromXml(element);
            return partConn;
        }

        protected abstract Connector GetConnector();
    }

    public class PartConnector<T> : PartConnector where T : Connector
    {
        public new T Connector { get; set; }

        protected override Connector GetConnector()
        {
            return Connector;
        }

        public PartConnector()
        {
        }

        public PartConnector(T connector)
        {
            Connector = connector;
            Transform = ItemTransform.FromLDD(connector.Transform);
        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);
            elem.Add(new XAttribute("Type", Connector.Type.ToString()));
            elem.Add(Transform.SerializeToXml());
            elem.Add(SerializeConnector());
            return elem;
        }

        protected virtual object[] SerializeConnector()
        {
            var connElem = Connector.SerializeToXml();
            var elements = new List<XElement>();
            string[] toRemove = new string[] { "angle", "ax", "ay", "az", "tx", "ty", "tz" };

            foreach (var connAttr in connElem.Attributes())
            {
                if (toRemove.Contains(connAttr.Name.LocalName))
                    continue;
                elements.Add(new XElement(connAttr.Name.LocalName.Capitalize(), connAttr.Value));
            }

            if (!string.IsNullOrEmpty(connElem.Value))
            {
                var cleanStr = connElem.Value.Replace("\r\n", string.Empty).Trim();
                elements.Add(new XElement("Data", cleanStr));
            }
            return elements.ToArray();
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            Transform = ItemTransform.FromXml(element.Element("Transform"));
            element.Element("Transform").Remove();
            var connElem = new XElement(ConnectorType.ToString());

            connElem.Add(Transform.ToLDD().ToXmlAttributes());

            foreach (var elem in element.Elements().ToArray())
            {
                if (elem.Name.LocalName == "Data")
                    connElem.Value = elem.Value;
                else
                    connElem.Add(new XAttribute(elem.Name.LocalName, elem.Value));
            }

            Connector.LoadFromXml(connElem);
        }
    }
}
