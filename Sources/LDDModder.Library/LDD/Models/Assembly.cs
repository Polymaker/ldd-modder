using LDDModder.LDD.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class Assembly
    {
        public const string EXTENSION = "LXFML";

        public int ID { get; set; }
        public List<int> Aliases { get; set; }
        public string Name { get; set; }

        public Platform Platform { get; set; }
        public MainGroup MainGroup { get; set; }

        public VersionInfo FileVersion { get; set; }

        public List<Brick> Bricks { get; set; }

        public AssemblyScene Scene { get; set; }

        public Assembly()
        {
            Aliases = new List<int>();
            Bricks = new List<Brick>();
            FileVersion = new VersionInfo(1, 0);
        }

        public static Assembly Load(string filepath)
        {
            using (var fs = File.OpenRead(filepath))
                return Load(fs);
        }

        public static Assembly Load(Stream stream)
        {
            var document = XDocument.Load(stream);
            var primitive = new Assembly();
            primitive.LoadFromXml(document);

            if (stream is FileStream fs)
            {
                string filename = Path.GetFileNameWithoutExtension(fs.Name);
                var m = Regex.Match(filename, "^(\\d+)");
                if (m.Success)
                    primitive.ID = int.Parse(m.Groups[1].Value);
            }

            return primitive;
        }

        private void LoadFromXml(XDocument document)
        {
            var rootElem = document.Root;

            if (rootElem.Name != "LXFML")
                throw new InvalidDataException();
            rootElem.TryGetIntAttribute("versionMajor", out int vMaj);
            rootElem.TryGetIntAttribute("versionMinor", out int vMin);
            FileVersion = new VersionInfo(vMaj, vMin);

            if (rootElem.HasElement("Meta", out XElement metaElem))
            {
                foreach (var annotationElem in metaElem.Elements("Annotation"))
                {
                    var annotationAttr = annotationElem.FirstAttribute;
                    string annotationName = annotationAttr.Name.LocalName;
                    string value = annotationAttr.Value;
                    //bool handled = true;

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
                        //default:
                        //    handled = false;
                        //    break;
                    }

                    //if (!handled)
                    //    ExtraAnnotations.Add(annotationName, value);
                }
            }

            Bricks.Clear();
            if (rootElem.HasElement("Bricks", out XElement bricksElem))
            {
                foreach (var brickElem in bricksElem.Elements("Brick"))
                {
                    var brick = new Brick();
                    brick.LoadFromXml(brickElem);
                    Bricks.Add(brick);
                }
            }
            if (rootElem.HasElement("Scene", out XElement sceneElem))
            {
                foreach (var partElem in sceneElem.Descendants("Part"))
                {
                    var brick = new Brick();
                    brick.LoadFromXml(partElem);
                    Bricks.Add(brick);
                }
            }
            if (document.Root.HasAttribute("name"))
                ID = document.Root.ReadAttribute("name", 0);

            if (ID == 0 && Aliases.Any())
                ID = Aliases.First();
        }
    }
}
