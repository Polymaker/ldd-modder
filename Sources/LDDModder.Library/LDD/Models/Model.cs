using LDDModder.LDD.Data;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class Model : IXmlObject
    {
        public const string EXTENSION = "LXFML";

        public string ModelName { get; set; }
        public VersionInfo FileVersion { get; set; }

        public string ApplicationName { get; set; }
        public VersionInfo ApplicationVersion { get; set; }

        public Brand Brand { get; set; }

        public int BrickSetVersion { get; set; }

        public List<Camera> Cameras { get; set; }

        public List<Brick> Bricks { get; set; }

        public List<RigidSystem> RigidSystems { get; set; }

        public List<GroupSystem> GroupSystems { get; set; }

        public Model()
        {
            Cameras = new List<Camera>();
            Bricks = new List<Brick>();
            RigidSystems = new List<RigidSystem>();
            GroupSystems = new List<GroupSystem>();
            ApplicationName = "LEGO Digital Designer";
            ModelName = string.Empty;
        }

        public XElement SerializeToXml()
        {

            var rootElem = new XElement(EXTENSION,
                new XAttribute("versionMajor", FileVersion.Major),
                new XAttribute("versionMinor", FileVersion.Minor),
                new XAttribute("name", ModelName));

            var metaElem = rootElem.AddElement("Meta");
            metaElem.Add(new XElement("Application",
                new XAttribute("name", ApplicationName),
                new XAttribute("versionMajor", ApplicationVersion.Major),
                new XAttribute("versionMinor", ApplicationVersion.Minor)
                ));
            metaElem.Add(new XElement("Brand",
                new XAttribute("name", Brand.ToString())
                ));
            metaElem.Add(new XElement("BrickSet",
                new XAttribute("version", BrickSetVersion)
                ));

            var bricksElem = rootElem.AddElement("Bricks", new XAttribute("cameraRef", 0));
            foreach(var brick in Bricks)
                bricksElem.Add(brick.SerializeToXml());
            var rigidElem = rootElem.AddElement("RigidSystems");
            foreach (var rigid in RigidSystems)
                rigidElem.Add(rigid.SerializeToXml());

            return rootElem;
        }

        public void LoadFromXml(XElement element)
        {
            
        }

        private XDocument GenerateXmlDocument()
        {
            var doc = new XDocument(SerializeToXml())
            {
                Declaration = new XDeclaration("1.0", "UTF-8", "no")
            };

            return doc;
        }

        public void Save(string filename)
        {
            var doc = GenerateXmlDocument();
            doc.Save(filename);
        }

        public void Save(Stream stream)
        {
            var doc = GenerateXmlDocument();
            doc.Save(stream);
        }
    }
}
