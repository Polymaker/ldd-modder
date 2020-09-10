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
        public VersionInfo FileVersion { get; set; }
        public int PartVersion { get; set; }
        public string Revision { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Dictionary<string,string> ExtraAnnotations { get; }

        public List<XElement> ExtraElements { get; private set; }

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
            ExtraElements = new List<XElement>();
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

            ExtraElements.Clear();
            foreach (var element in rootElem.Elements())
            {
                try { ReadPrimitiveSection(element); }
                catch (Exception ex)
                {
                    throw new Exception($"Error while reading {element.Name.LocalName}", ex);
                }
            }

            if (PhysicsAttributes == null)
                PhysicsAttributes = new PhysicsAttributes();

            if (ID == 0 && Aliases.Any())
                ID = Aliases.First();
        }

        #region XML Element Loading

        public const string SECTION_ANNOTATIONS = "Annotations";
        public const string SECTION_COLLISION = "Collision";
        public const string SECTION_CONNECTIVITY = "Connectivity";
        public const string SECTION_PHYSICSATTRIBUTES = "PhysicsAttributes";
        public const string SECTION_BOUNDING = "Bounding";
        public const string SECTION_GEOMETRYBOUNDING = "GeometryBounding";

        private void ReadPrimitiveSection(XElement element)
        {
            bool isElementHandled = true;

            switch (element.Name.LocalName)
            {
                case SECTION_ANNOTATIONS:

                    foreach (var annotationElem in element.Elements("Annotation"))
                    {
                        try { ReadAnnotation(annotationElem); }
                        catch { }
                    }
                    break;

                case SECTION_COLLISION:
                    foreach (var colElem in element.Elements())
                        Collisions.Add(Collision.DeserializeCollision(colElem));
                    break;

                case SECTION_CONNECTIVITY:
                    foreach (var conElem in element.Elements())
                        Connectors.Add(Connector.DeserializeConnector(conElem));
                    break;

                case SECTION_PHYSICSATTRIBUTES:
                    PhysicsAttributes = XmlHelper.DefaultDeserialize<PhysicsAttributes>(element);
                    break;

                case SECTION_BOUNDING:
                    Bounding = new BoundingBox();
                    Bounding.LoadFromXml(element.Element("AABB"));

                    break;

                case SECTION_GEOMETRYBOUNDING:
                    GeometryBounding = new BoundingBox();
                    GeometryBounding.LoadFromXml(element.Element("AABB"));
                    break;

                case "Color": // V2 Only
                    {
                        int surfaceCount = element.ReadAttribute<int>("faces");
                        SubMaterials = new int[surfaceCount];
                        break;
                    }

                case "Decoration":
                    if (FileVersion.Major == 1)
                    {
                        int surfaceCount = element.ReadAttribute<int>("faces");
                        SubMaterials = new int[surfaceCount];
                        var values = element.ReadAttribute<string>("subMaterialRedirectLookupTable").Split(',');
                        for (int i = 0; i < surfaceCount; i++)
                            SubMaterials[i] = int.Parse(values[i]);
                    }
                    else
                        isElementHandled = false;

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
                    isElementHandled = false;
                    break;
            }

            if (!isElementHandled)
            {
                var clonedElem = XElement.Parse(element.ToString());
                ExtraElements.Add(clonedElem);
            }
        }

        private void ReadAnnotation(XElement annotationElem)
        {
            var annotationAttr = annotationElem.FirstAttribute;
            string annotationName = annotationAttr.Name.LocalName;
            string value = annotationAttr.Value;

            switch (annotationName)
            {
                case "aliases":
                    var aliases = value.Split(';');
                    for (int i = 0; i < aliases.Length; i++)
                    {
                        if (int.TryParse(aliases[i], out int aliasID))
                            Aliases.Add(aliasID);
                    }
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
                case "revision":
                    Revision = value;
                    break;
                case "modifiedDate":
                    if (DateTime.TryParse(value, out DateTime modifDate))
                        ModifiedDate = modifDate;
                    break;
                default:
                    ExtraAnnotations.Add(annotationName, value);
                    break;
            }
        }

        #endregion

        #region XML 

        public XElement SerializeToXml()
        {
            var rootElem = new XElement("LEGOPrimitive",
                new XAttribute("versionMajor", FileVersion.Major),
                new XAttribute("versionMinor", FileVersion.Minor));

            rootElem.Add(SerializeAnnotations());

            if (Collisions.Any())
                rootElem.Add(new XElement("Collision", Collisions.Select(x => x.SerializeToXml())));

            if (Connectors.Any())
                rootElem.Add(new XElement("Connectivity", Connectors.Select(x => x.SerializeToXml())));

            if (FlexBones.Any())
                rootElem.Add(new XElement("Flex", FlexBones.Select(x => x.SerializeToXml())));

            if (PhysicsAttributes != null && !PhysicsAttributes.IsEmpty)
                rootElem.Add(PhysicsAttributes.SerializeToXml());

            if (Bounding != null)
            {
                var boundingElem = rootElem.AddElement("Bounding");
                boundingElem.Add(Bounding.SerializeToXml("AABB"));
            }

            if (GeometryBounding != null)
            {
                var boundingElem = rootElem.AddElement("GeometryBounding");
                boundingElem.Add(GeometryBounding.SerializeToXml("AABB"));
            }

            if (FileVersion.Major > 1)
            {
                var decorationElem = rootElem.AddElement("Color");
                decorationElem.Add(new XAttribute("faces", SubMaterials?.Length ?? 0));
                //decorationElem.Add(new XAttribute("subMaterialRedirectLookupTable", string.Join(",", SubMaterials)));
            }
            else if (SubMaterials != null)
            {
                var decorationElem = rootElem.AddElement("Decoration");
                decorationElem.Add(new XAttribute("faces", SubMaterials.Length));
                decorationElem.Add(new XAttribute("subMaterialRedirectLookupTable", string.Join(",", SubMaterials)));
            }

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
                    if (elem.Parent != null)
                    {
                        var parentElem = rootElem.Descendants().FirstOrDefault(x => x.Name.LocalName == elem.Parent.Name.LocalName);
                        //if (parentElem != null)
                        //    parentElem.Add(elem.)
                    }
                    else
                        rootElem.Add(elem);
                }
            }
            return rootElem;
        }

        private XElement SerializeAnnotations()
        {
            var annotations = new XElement("Annotations");

            void addAnnotation(string name, object value)
            {
                annotations.Add(new XElement("Annotation", new XAttribute(name, value)));
            }

            //if (!Aliases.Contains(ID) && ID > 0)
            //    Aliases.Add(ID);

            var aliasTmp = Aliases.ToList();
            if (!aliasTmp.Contains(ID))
                aliasTmp.Add(ID);

            if (aliasTmp.Any())
                addAnnotation("aliases", string.Join(";", aliasTmp));

            addAnnotation("designname", Name);

            if (MainGroup != null)
            {
                addAnnotation("maingroupid", MainGroup.ID);
                if (FileVersion.Major == 1)
                    addAnnotation("maingroupname", MainGroup.Name);
            }

            if (Platform != null)
            {
                addAnnotation("platformid", Platform.ID);
                if (FileVersion.Major == 1)
                    addAnnotation("platformname", Platform.Name);
            }

            if (PartVersion > 0)
                addAnnotation("version", PartVersion);

            if (FileVersion.Major > 1 && !string.IsNullOrEmpty(Revision))
                addAnnotation("revision", Revision);

            if (ModifiedDate.HasValue && FileVersion.Major == 2)
                addAnnotation("modifiedDate", ModifiedDate);

            foreach (var extra in ExtraAnnotations)
                addAnnotation(extra.Key, extra.Value);

            return annotations;
        }

        #endregion

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
