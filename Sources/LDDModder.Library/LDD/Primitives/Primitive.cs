﻿using LDDModder.LDD.Data;
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
        public VersionInfo FileVersion { get; set; }
        public int PartVersion { get; set; }

        public Dictionary<string,string> ExtraAnnotations { get; }

        private List<XElement> ExtraElements;

        public Platform Platform { get; set; }
        public MainGroup MainGroup { get; set; }

        public PhysicsAttributes PhysicsAttributes { get; set; }
        public BoundingBox Bounding { get; set; }
        public BoundingBox GeometryBounding { get; set; }
        public Transform DefaultOrientation { get; set; }
        public Camera DefaultCamera { get; set; }

        public int[] SubMaterials { get; set; }

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
            FileVersion = new VersionInfo(1, 0);
            PartVersion = 1;
        }

        public int GetSurfaceMaterialIndex(int surfaceIndex)
        {
            if (SubMaterials != null && 
                surfaceIndex >= 0 && 
                surfaceIndex < SubMaterials.Length)
                return SubMaterials[surfaceIndex];

            return 0;
        }

        public XElement SerializeToXml()
        {
            var rootElem = new XElement("LEGOPrimitive",
                new XAttribute("versionMajor", FileVersion.Major),
                new XAttribute("versionMinor", FileVersion.Minor));
            
            var annotations = rootElem.AddElement("Annotations");

            if (!Aliases.Contains(ID) && ID > 0)
                Aliases.Add(ID);

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

            annotations.Add(new XElement("Annotation", new XAttribute("version", PartVersion)));

            foreach (var extra in ExtraAnnotations)
                annotations.Add(new XElement("Annotation", new XAttribute(extra.Key, extra.Value)));

            if (Collisions.Any())
                rootElem.Add(new XElement("Collision", Collisions.Select(x => x.SerializeToXml())));

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

            if (SubMaterials != null)
            {
                var decorationElem = rootElem.AddElement("Decoration");
                decorationElem.Add(new XAttribute("faces", SubMaterials.Length));
                decorationElem.Add(new XAttribute("subMaterialRedirectLookupTable", string.Join(",", SubMaterials)));
            }

            if (FlexBones.Any())
                rootElem.Add(new XElement("Flex", FlexBones.Select(x => x.SerializeToXml())));

            if (DefaultOrientation != null)
                rootElem.Add(new XElement("DefaultOrientation", DefaultOrientation.ToXmlAttributes()));

            if (DefaultCamera != null)
                rootElem.Add(XmlHelper.DefaultSerialize(DefaultCamera, "DefaultCamera"));

            foreach (var elem in rootElem.Descendants("Custom2DField"))
            {
                int depth = elem.AncestorsAndSelf().Count();
                elem.Value = elem.Value.Indent(depth, "  ");
                elem.Value += StringExtensions.Tab(depth - 1, "  "); //fixes the closing tag indentation
            }

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

        public static Primitive Load(string filepath)
        {
            using (var fs = File.OpenRead(filepath))
                return Load(fs);
        }

        public static Primitive Load(Stream stream)
        {
            var document = XDocument.Load(stream);
            var primitive = new Primitive();
            primitive.LoadFromXml(document);

            if (stream is FileStream fs &&
                int.TryParse(Path.GetFileNameWithoutExtension(fs.Name), out int primitiveID))
                primitive.ID = primitiveID;
            
            return primitive;
        }

        private void LoadFromXml(XDocument document)
        {
            var rootElem = document.Root;

            if (rootElem.Name != "LEGOPrimitive")
                throw new InvalidDataException();
            rootElem.TryGetIntAttribute("versionMajor", out int vMaj);
            rootElem.TryGetIntAttribute("versionMinor", out int vMin);
            FileVersion = new VersionInfo(vMaj, vMin);

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
                                    PartVersion = int.Parse(value);
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
                        SubMaterials = new int[surfaceCount];
                        var values = element.ReadAttribute<string>("subMaterialRedirectLookupTable").Split(',');
                        for (int i = 0; i < surfaceCount; i++)
                            SubMaterials[i] = int.Parse(values[i]);
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

            if (ID == 0 && Aliases.Any())
                ID = Aliases.First();
        }

        public void Save(string filename)
        {
            var doc = new XDocument(SerializeToXml())
            {
                Declaration = new XDeclaration("1.0", "UTF-8", "no")
            };
            doc.Save(filename);
        }

        public void Save(Stream stream)
        {
            var doc = new XDocument(SerializeToXml())
            {
                Declaration = new XDeclaration("1.0", "UTF-8", "no")
            };
            doc.Save(stream);
        }
    }
}
