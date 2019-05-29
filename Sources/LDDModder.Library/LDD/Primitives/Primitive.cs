using LDDModder.LDD.Data;
using LDDModder.LDD.Primitives.Collisions;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives
{
    public class Primitive
    {
        public int ID { get; set; }
        public List<int> Aliases { get; }
        public string Name { get; set; }

        public int DesignVersion { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }

        public Dictionary<string,string> ExtraAnnotations { get; }

        private List<XElement> ExtraElements;

        public Platform Platform { get; set; }
        public MainGroup MainGroup { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }
        public BoundingBox Bounding { get; set; }
        public BoundingBox GeometryBounding { get; set; }
        public Transform DefaultOrientation { get; set; }
        public Camera DefaultCamera { get; set; }

        public int[] SurfaceMappingTable { get; set; }

        public List<Connector> Connectors { get; }
        public List<Collisions.Collision> Collisions { get; }
        public List<FlexBone> FlexBones { get; }
        
        public Primitive()
        {
            Aliases = new List<int>();
            Connectors = new List<Connectors.Connector>();
            Collisions = new List<Collisions.Collision>();
            ExtraAnnotations = new Dictionary<string, string>();
            FlexBones = new List<FlexBone>();
            MajorVersion = 1;
            DesignVersion = 1;
        }

        public XElement SerializeToXml()
        {
            var rootElem = new XElement("LEGOPrimitive",
                new XAttribute("versionMajor", MajorVersion),
                new XAttribute("versionMinor", MinorVersion));
            
            var annotations = rootElem.AddElement("Annotations");

            if (Aliases.Any())
                annotations.Add(new XElement("Annotation", new XAttribute("aliases", string.Join(";", Aliases))));

            annotations.Add(new XElement("Annotation", new XAttribute("designname", Name)));

            if (MainGroup != null)
            {
                annotations.Add(new XElement("Annotation", new XAttribute("maingroupid", MainGroup.ID)));
                annotations.Add(new XElement("Annotation", new XAttribute("maingroupname", MainGroup.Name)));
            }

            if (Platform != null)
            {
                annotations.Add(new XElement("Annotation", new XAttribute("platformid", Platform.ID)));
                annotations.Add(new XElement("Annotation", new XAttribute("platformname", Platform.Name)));
            }

            annotations.Add(new XElement("Annotation", new XAttribute("version", DesignVersion)));

            foreach (var extra in ExtraAnnotations)
                annotations.Add(new XElement("Annotation", new XAttribute(extra.Key, extra.Value)));

            if (Collisions.Any())
                rootElem.Add(new XElement("Collisions", Collisions.Select(x => x.SerializeToXml())));

            if (Connectors.Any())
                rootElem.Add(new XElement("Connectivity", Connectors.Select(x => x.SerializeToXml())));

            if (PhysicsAttributes != null)
                rootElem.Add(PhysicsAttributes.SerializeToXml());

            if (Bounding != null)
            {
                var boundingElem = rootElem.AddElement("Bounding");
                boundingElem.Add(XmlHelper.DefaultSerialize(Bounding, "AABB"));
            }

            if (GeometryBounding != null)
            {
                var boundingElem = rootElem.AddElement("GeometryBounding");
                boundingElem.Add(XmlHelper.DefaultSerialize(GeometryBounding, "AABB"));
            }

            if (SurfaceMappingTable != null)
            {
                var decorationElem = rootElem.AddElement("Decoration");
                decorationElem.Add(new XAttribute("faces", SurfaceMappingTable.Length));
                decorationElem.Add(new XAttribute("subMaterialRedirectLookupTable", string.Join(",", SurfaceMappingTable)));
            }

            if (FlexBones.Any())
                rootElem.Add(new XElement("Flex", FlexBones.Select(x => x.SerializeToXml())));

            if (DefaultOrientation != null)
                rootElem.Add(new XElement("DefaultOrientation", DefaultOrientation.ToXmlAttributes()));

            if (DefaultCamera != null)
                rootElem.Add(XmlHelper.DefaultSerialize(DefaultCamera, "DefaultCamera"));

            if (ExtraElements != null)
            {
                foreach (var elem in ExtraElements)
                {
                    if (elem.Parent.Name.LocalName == "LEGOPrimitive")
                    {
                        elem.Remove(); //remove from parent
                        rootElem.Add(elem);
                    }
                }
            }
            return rootElem;
        }

        public static Primitive FromXmlFile(FileStream fs)
        {
            var document = XDocument.Load(fs);
            var primitive = new Primitive();

            if (int.TryParse(Path.GetFileNameWithoutExtension(fs.Name), out int primitiveID))
                primitive.ID = primitiveID;

            primitive.LoadFromXml(document);

            return primitive;
        }

        private void LoadFromXml(XDocument document)
        {
            var rootElem = document.Root;
            if (rootElem.Name != "LEGOPrimitive")
                throw new InvalidDataException();

            if (rootElem.TryGetIntAttribute("versionMajor", out int vMaj))
                MajorVersion = vMaj;
            if (rootElem.TryGetIntAttribute("versionMajor", out int vMin))
                MinorVersion = vMin;

            foreach (var element in rootElem.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "Annotations":

                        foreach (var annotationElem in element.Elements("Annotation"))
                        {
                            var annotationAttr = annotationElem.FirstAttribute;
                            string annotationName = annotationAttr.Name.LocalName;
                            string value = annotationAttr.Value;
                            bool handled = true;

                            switch (annotationName)
                            {
                                case "aliases":
                                    var aliases = value.Split(';');
                                    for (int i = 0; i < aliases.Length; i++)
                                        Aliases.Add(int.Parse(aliases[i]));
                                    break;
                                case "designname":
                                    Name = value;
                                    break;
                                case "version":
                                    DesignVersion = int.Parse(value);
                                    break;
                                case "maingroupid":
                                    if (MainGroup == null)
                                        MainGroup = new MainGroup();
                                    MainGroup.ID = int.Parse(value);
                                    break;
                                case "maingroupname":
                                    if (MainGroup == null)
                                        MainGroup = new MainGroup();
                                    MainGroup.Name = value;
                                    break;
                                case "platformid":
                                    if (Platform == null)
                                        Platform = new Platform();
                                    Platform.ID = int.Parse(value);
                                    break;
                                case "platformname":
                                    if (Platform == null)
                                        Platform = new Platform();
                                    Platform.Name = value;
                                    break;
                                default:
                                    handled = false;
                                    break;
                            }

                            if(!handled)
                                ExtraAnnotations.Add(annotationName, value);
                        }
                        break;

                    case "Collision":
                        foreach (var colElem in element.Elements())
                            Collisions.Add(Collision.DeserializeCollision(colElem));
                        break;

                    case "Connectivity":
                        foreach (var conElem in element.Elements())
                            Connectors.Add(Connector.DeserializeConnector(conElem));
                        break;

                    case "PhysicsAttributes":
                        PhysicsAttributes = XmlHelper.DefaultDeserialize<PhysicsAttributes>(element);
                        break;

                    case "Bounding":
                        Bounding = XmlHelper.DefaultDeserialize<BoundingBox>(element.Element("AABB"));
                        break;

                    case "GeometryBounding":
                        GeometryBounding = XmlHelper.DefaultDeserialize<BoundingBox>(element.Element("AABB"));
                        break;

                    case "Decoration":
                        int surfaceCount = element.ReadAttribute<int>("faces");
                        SurfaceMappingTable = new int[surfaceCount];
                        var values = element.ReadAttribute<string>("subMaterialRedirectLookupTable").Split(',');
                        for (int i = 0; i < surfaceCount; i++)
                            SurfaceMappingTable[i] = int.Parse(values[i]);
                        break;

                    case "Flex":
                        foreach (var boneElement in element.Elements())
                            FlexBones.Add(XmlHelper.DefaultDeserialize<FlexBone>(boneElement));
                        break;

                    case "DefaultOrientation":
                        DefaultOrientation = Transform.FromElementAttributes(element);
                        break;

                    case "DefaultCamera":
                        DefaultCamera = XmlHelper.DefaultDeserialize<Camera>(element);
                        break;

                    //case "Paths":
                    //    break;

                    default:
                        if (ExtraElements == null)
                            ExtraElements = new List<XElement>();
                        ExtraElements.Add(element);
                        break;
                }
            }
        }
    }
}
