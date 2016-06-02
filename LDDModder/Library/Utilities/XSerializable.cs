using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LDDModder.Utilities
{
    public abstract class XSerializable : IXmlSerializable
    {

        protected abstract XElement SerializeToXElement();

        protected abstract void DeserializeFromXElement(XElement element);

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            DeserializeFromXElement(XElement.Load(reader));
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            SerializeToXElement().WriteTo(writer);
        }

        //public static T Deserialize<T>(Stream
    }
}
