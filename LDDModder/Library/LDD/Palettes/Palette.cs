using LDDModder.LDD.General;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace LDDModder.LDD.Palettes
{
    /// <summary>
    /// AKA Palette content
    /// </summary>
    [XmlRoot("PAXML")]
    public class Palette : LDDModder.Serialization.XSerializable
    {
        // Fields...
        private List<PaletteItem> _Items;

        public List<PaletteItem> Items
        {
            get { return _Items; }
        }

        public VersionInfo FileVersion { get; set; }

        public Palette()
        {
            _Items = new List<PaletteItem>();
            FileVersion = new VersionInfo(1, 0);
        }

        public Palette(IEnumerable<PaletteItem> items)
        {
            _Items = new List<PaletteItem>(items);
            FileVersion = new VersionInfo(1, 0);
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
                _Items.Add(DeserializeItem(brickElem));
        }

        private static PaletteItem DeserializeItem(XElement element)
        {
            switch (element.Name.LocalName)
            {
                default:
                    return null;
                case "Brick":
                    return LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<Brick>(element);
                case "Assembly":
                    return LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<Assembly>(element);
            }
        }

        protected override XElement SerializeToXElement()
        {
            var rootElem = new XElement(RootElementName, new XElement("Bag"));
            if (FileVersion != null)
            {
                rootElem.Add(new XAttribute("versionMajor", FileVersion.Major),
                             new XAttribute("versionMinor", FileVersion.Minor));
            }

            var bagElem = rootElem.Element("Bag");

            foreach (var item in Items)
            {
                var itemElem = LDDModder.Serialization.XSerializationHelper.Serialize(item);
                SortPaletteItemAttrs(itemElem);

                if (itemElem.HasElements)
                {
                    foreach (var subElem in itemElem.Elements())
                        SortPaletteItemAttrs(subElem);
                }

                bagElem.Add(itemElem);
            }
            return rootElem;
        }

        private static void SortPaletteItemAttrs(XElement elem)
        {
            elem.SortAttributes(a => Array.IndexOf(PaletteItem.AttributeOrder, a.Name.LocalName));
        }

        public static Palette Load(string filepath)
        {
            return LDDModder.Serialization.XSerializable.LoadFrom<Palette>(filepath);
        }

        public static Palette Load(Stream stream)
        {
            return LDDModder.Serialization.XSerializable.LoadFrom<Palette>(stream);
        }

        public void Save(string filepath)
        {
            LDDModder.Serialization.XSerializable.Save<Palette>(this, filepath);
        }

        public void Save(Stream stream)
        {
            LDDModder.Serialization.XSerializable.Save<Palette>(this, stream);
        }
    }
}
