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
        //public abstract Connector Connector { get; set; }

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

            return null;
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
            var elem = SerializeToXmlBase("Connection");
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
    }
}
