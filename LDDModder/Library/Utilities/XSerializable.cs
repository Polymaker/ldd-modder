using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string OriginFileName;

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

        public static T LoadFrom<T>(string filepath) where T : XSerializable
        {
            using (var fs = File.OpenRead(filepath))
                return LoadFrom<T>(fs);
        }

        public static T LoadFrom<T>(Stream stream) where T : XSerializable
        {
            var xmlSer = new XmlSerializer(typeof(T));
            T result = (T)xmlSer.Deserialize(stream);
            if (stream is FileStream)
                result.OriginFileName = ((FileStream)stream).Name;
            return result;
        }

        public static void Save<T>(T xObject, string filepath) where T : XSerializable
        {
            using (var fs = File.Create(filepath))
                Save<T>(xObject, fs);
        }

        public static void Save<T>(T xObject, Stream stream) where T : XSerializable
        {
            var xmlSer = new XmlSerializer(typeof(T));
            xmlSer.Serialize(stream, xObject);
        }

        //public static T Deserialize<T>(Stream
    }
}
