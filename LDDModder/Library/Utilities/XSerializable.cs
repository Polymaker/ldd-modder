using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LDDModder.Utilities
{
    public abstract class XSerializable : IXmlSerializable
    {

        public string RootElementName
        {
            get { return XSerializationHelper.GetTypeXmlRootName(GetType()); }
        }

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
            var rootElement = SerializeToXElement();

            //writer has already written StartDocument (root node)
            //so we write each element manually
            foreach (var attr in rootElement.Attributes())
                writer.WriteAttributeString(attr.Name.LocalName, attr.Name.Namespace.NamespaceName, attr.Value);

            foreach (var child in rootElement.Elements())
                child.WriteTo(writer);
        }

        //public static T Deserialize<T>(Stream
    }
}
