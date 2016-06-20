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
    /// AKA Palette Info
    /// </summary>
    [XmlRoot("BAXML")]
    public class Bag : XSerializable
    {
        public const string FileName = "Info.baxml";

        public string Name { get; set; }

        public int PaletteVersion { get; set; }

        public bool Countable { get; set; }

        public Brand ParentBrand { get; set; }

        public VersionInfo FileVersion { get; set; }

        public bool Buyable { get; set; }

        public bool BrandFilter { get; set; }

        public Bag()
        {
            Name = String.Empty;
            PaletteVersion = 0;
            Countable = false;
            ParentBrand = Brand.LDD;
            FileVersion = new VersionInfo();
            Buyable = false;
            BrandFilter = false;
        }

        public Bag(string name, bool countable)
        {
            Name = name;
            Countable = countable;
            PaletteVersion = 1;
            ParentBrand = Brand.LDD;
            FileVersion = new VersionInfo(1, 0);
            Buyable = false;
            BrandFilter = false;
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

            Name = bagElem.Attribute("name").Value;
            PaletteVersion = bagElem.GetIntAttribute("version");
            Countable = bagElem.ContainsAttribute("countable") ? bagElem.Attribute("countable").Value == "true" : false;
            ParentBrand = (Brand)Enum.Parse(typeof(Brand), bagElem.Attribute("brand").Value);

            if (bagElem.ContainsAttribute("buyable"))
                Buyable = bagElem.Attribute("buyable").Value == "true";
            else
                Buyable = false;

            if (bagElem.ContainsAttribute("brandFilter"))
                BrandFilter = bagElem.Attribute("brandFilter").Value == "true";
            else
                BrandFilter = false;
        }

        protected override XElement SerializeToXElement()
        {
            var rootElem = new XElement(RootElementName, 
                new XElement("Bag",
                    new XAttribute("name", Name),
                    new XAttribute("version", PaletteVersion),
                    new XAttribute("countable", Countable ? "true" : "false"),
                    new XAttribute("brand", ParentBrand.ToString())
                    )
                );

            if (FileVersion != null)
            {
                rootElem.Add(new XAttribute("versionMajor", FileVersion.Major),
                             new XAttribute("versionMinor", FileVersion.Minor));
            }

            var bagElem = rootElem.Element("Bag");

            if (Buyable)
                bagElem.Add(new XAttribute("buyable", Buyable ? "true" : "false"));

            if (BrandFilter)
                bagElem.Add(new XAttribute("brandFilter", BrandFilter ? "true" : "false"));


            return rootElem;
        }

        public static Bag Load(string filepath)
        {
            return XSerializable.LoadFrom<Bag>(filepath);
        }

        public static Bag Load(Stream stream)
        {
            return XSerializable.LoadFrom<Bag>(stream);
        }

        public void Save(string filepath)
        {
            XSerializable.Save<Bag>(this, filepath);
        }

        public void Save(Stream stream)
        {
            XSerializable.Save<Bag>(this, stream);
        }
    }
}
