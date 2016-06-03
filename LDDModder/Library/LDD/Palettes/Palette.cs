using LDDModder.LDD.General;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace LDDModder.LDD.Palettes
{
    /// <summary>
    /// AKA Palette content
    /// </summary>
    [XmlRoot("PAXML")]
    public class Palette : XSerializable
    {
        // Fields...
        private List<PaletteItem> _Parts;

        public List<PaletteItem> Parts
        {
            get { return _Parts; }
        }

        public VersionInfo FileVersion { get; set; }

        public Palette()
        {
            _Parts = new List<PaletteItem>();
        }

        protected override void DeserializeFromXElement(XElement element)
        {
            if (element.Attribute("versionMajor") != null)
            {
                (FileVersion ?? (FileVersion = new VersionInfo())).Major = element.GetIntAttribute("versionMajor");
            }

            if (element.Attribute("versionMinor") != null)
            {
                (FileVersion ?? (FileVersion = new VersionInfo())).Minor = element.GetIntAttribute("versionMinor");
            }

            var bagElem = element.Element("Bag");
            foreach (var brickElem in bagElem.Elements())
                _Parts.Add(DeserializeBrick(brickElem));
        }

        private static PaletteItem DeserializeBrick(XElement element)
        {
            switch (element.Name.LocalName)
            {
                default:
                    return null;
                case "Brick":
                    return XSerializationHelper.DefaultDeserialize<Brick>(element);
                case "Assembly":
                    return XSerializationHelper.DefaultDeserialize<Assembly>(element);
            }
        }

        protected override XElement SerializeToXElement()
        {
            var rootElem = new XElement(RootElementName, new XElement("Bag"));
            return rootElem;
        }
    }
}
