using LDDModder.LDD.General;
using LDDModder.Utilities;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace LDDModder.LDD.Primitives
{
    [Serializable, XmlRoot("LEGOPrimitive")]
    public class Primitive : XSerializable
    {
        private List<Connectivity> _Connections;
        private List<Collision> _Collisions;
        private List<int> _Aliases;

        public int Id { get; set; }

        public string Name { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }

        public BoundingBox Bounding { get; set; }

        public BoundingBox GeometryBounding { get; set; }

        public Orientation DefaultOrientation { get; set; }

        public MainGroup Group { get; set; }

        public Platform Platform { get; set; }

        public VersionInfo Version { get; set; }

        public int? DesignVersion { get; set; }

        public Decoration DecorationInfo { get; set; }

        public List<int> Aliases
        {
            get { return _Aliases; }
        }

        public List<Collision> Collisions
        {
            get { return _Collisions; }
        }

        public List<Connectivity> Connections
        {
            get { return _Connections; }
        }

        public Primitive()
        {
            _Connections = new List<Connectivity>();
            _Collisions = new List<Collision>();
            _Aliases = new List<int>();
            Id = 0;
            Name = string.Empty;
            Group = null;
            Platform = null;
            Version = null;
        }

        #region Xml Serialization

        protected override void DeserializeFromXElement(XElement element)
        {
            if (element.Attribute("versionMajor") != null)
            {
                (Version ?? (Version = new VersionInfo())).Major = element.GetIntAttribute("versionMajor");
            }

            if (element.Attribute("versionMinor") != null)
            {
                (Version ?? (Version = new VersionInfo())).Minor = element.GetIntAttribute("versionMinor");
            }

            var annotations = element.Element("Annotations");
            foreach (var annotation in annotations.Elements("Annotation"))
                DeserializeAnnotation(annotation);

            if (Aliases.Count > 0)
                Id = Aliases.First();

            var collisionsElem = element.Element("Collision");
            if (collisionsElem != null)
                _Collisions.AddRange(Collision.Deserialize(collisionsElem.Elements()));

            var conectivityElem = element.Element("Connectivity");
            if (conectivityElem != null)
                _Connections.AddRange(Connectivity.Deserialize(conectivityElem.Elements()));

            var boundingElem = element.Element("Bounding");
            if (boundingElem != null && boundingElem.HasElements)
                Bounding = XSerializationHelper.DefaultDeserialize<BoundingBox>(boundingElem.Elements().Single());//.First() or .Single() ??

            var geomBoundingElem = element.Element("GeometryBounding");
            if (geomBoundingElem != null && geomBoundingElem.HasElements)
                GeometryBounding = XSerializationHelper.DefaultDeserialize<BoundingBox>(geomBoundingElem.Elements().Single());//.First() or .Single() ??

            var physAttrElem = element.Element("PhysicsAttributes");
            if (physAttrElem != null)
                PhysicsAttributes = XSerializationHelper.DefaultDeserialize<PhysicsAttributes>(physAttrElem);

            var decorationElem = element.Element("Decoration");
            if (decorationElem != null)
                DecorationInfo = XSerializationHelper.DefaultDeserialize<Decoration>(decorationElem);//.First() or .Single() ??

            var defaultOrientationElem = element.Element("DefaultOrientation");
            if (defaultOrientationElem != null)
                DefaultOrientation = XSerializationHelper.DefaultDeserialize<Orientation>(defaultOrientationElem);

        }

        private void DeserializeAnnotation(XElement annotation)
        {
            var annotAttr = annotation.FirstAttribute;
            switch (annotAttr.Name.LocalName)
            {
                case "aliases":
                    var aliases = annotAttr.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < aliases.Length; i++)
                    {
                        int aliasId = 0;
                        if (int.TryParse(aliases[i], out aliasId))
                            _Aliases.Add(aliasId);
                    }
                    break;
                case "designname":
                    Name = annotAttr.Value; break;
                case "maingroupid":
                    if (Group == null)
                        Group = new MainGroup();
                    Group.Id = int.Parse(annotAttr.Value);
                    break;
                case "maingroupname":
                    if (Group == null)
                        Group = new MainGroup();
                    Group.Name = annotAttr.Value;
                    break;
                case "platformid":
                    if (Platform == null)
                        Platform = new Platform();
                    Platform.Id = int.Parse(annotAttr.Value);
                    break;
                case "platformname":
                    if (Platform == null)
                        Platform = new Platform();
                    Platform.Name = annotAttr.Value;
                    break;
                case "version":
                    DesignVersion = int.Parse(annotAttr.Value); break;
            }
        }

        protected override XElement SerializeToXElement()
        {
            var root = new XElement("LEGOPrimitive");
            if (Version != null)
            {
                root.Add(new XAttribute("versionMajor", Version.Major),
                         new XAttribute("versionMinor", Version.Minor));
            }

            //Annotations
            var annotElem = new XElement("Annotations");
            root.Add(annotElem);

            annotElem.Add(SerializeAnnotation("aliases", Aliases.Select(a => a.ToString()).Aggregate((i, j) => i + ";" + j)));
            annotElem.Add(SerializeAnnotation("designname", Name));

            if (Group != null)
            {
                annotElem.Add(SerializeAnnotation("maingroupid", Group.Id),
                              SerializeAnnotation("maingroupname", Group.Name));
            }

            if (Platform != null)
            {
                annotElem.Add(SerializeAnnotation("platformid", Platform.Id),
                              SerializeAnnotation("platformname", Platform.Name));
            }

            if (DesignVersion.HasValue)
                annotElem.Add(SerializeAnnotation("version", DesignVersion.Value));

            //Collisions
            if (Collisions.Count > 0)
            {
                var collElem = new XElement("Collision");
                root.Add(collElem);
                collElem.Add(Collision.Serialize(Collisions).ToArray());
            }

            //Connectivity
            if (Connections.Count > 0)
            {
                var connElem = new XElement("Connectivity");
                root.Add(connElem);
                connElem.Add(Connectivity.Serialize(Connections).ToArray());
            }

            //Other simple elements
            if (PhysicsAttributes != null)
                root.Add(XSerializationHelper.Serialize(PhysicsAttributes, "PhysicsAttributes"));

            if (DecorationInfo != null)
                root.Add(XSerializationHelper.Serialize(DecorationInfo, "Decoration"));

            if (Bounding != null)
                root.Add(new XElement("Bounding", XSerializationHelper.Serialize(Bounding, "AABB")));

            if (GeometryBounding != null)
                root.Add(new XElement("GeometryBounding", XSerializationHelper.Serialize(GeometryBounding, "AABB")));

            if (DefaultOrientation != null)
                root.Add(XSerializationHelper.Serialize(DefaultOrientation, "DefaultOrientation"));


            return root;
        }

        private static XElement SerializeAnnotation(string name, object value)
        {
            return new XElement("Annotation", new XAttribute(name, value));
        }

        #endregion

        public void Save(string filepath)
        {
            using (var fs = File.Create(filepath))
                Save(fs);
        }

        public void Save(Stream stream)
        {
            var xmlSerSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, NewLineChars = Environment.NewLine, OmitXmlDeclaration = true };
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var xmlSer = new XmlSerializer(typeof(Primitive));
            var xmlWriter = XmlTextWriter.Create(stream, xmlSerSettings);
            xmlWriter.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>\r\n");
            xmlSer.Serialize(xmlWriter, this, ns);
        }
    }
}
