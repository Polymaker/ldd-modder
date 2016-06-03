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
    /// AKA Palette Info
    /// </summary>
    [XmlRoot("BAXML")]
    public class Bag : XSerializable
    {

        public string Name { get; set; }

        public int PaletteVersion { get; set; }

        public bool Countable { get; set; }

        public Brand ParentBrand { get; set; }

        public VersionInfo FileVersion { get; set; }

        public bool? Buyable { get; set; }

        public bool? BrandFilter { get; set; }

        public Bag()
        {
            Name = String.Empty;
            PaletteVersion = 0;
            Countable = false;
            ParentBrand = Brand.LDD;
            FileVersion = new VersionInfo();
            Buyable = null;
            BrandFilter = null;
        }

        public Bag(string name, bool countable)
        {
            Name = name;
            Countable = countable;
            PaletteVersion = 1;
            ParentBrand = Brand.LDD;
            FileVersion = new VersionInfo(1, 0);
            Buyable = null;
            BrandFilter = null;
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
            Countable = bagElem.Attribute("countable").Value == "true";
            ParentBrand = (Brand)Enum.Parse(typeof(Brand), bagElem.Attribute("brand").Value);

            if (bagElem.ContainsAttribute("buyable"))
                Buyable = bagElem.Attribute("buyable").Value == "true";
            else
                Buyable = null;

            if (bagElem.ContainsAttribute("brandFilter"))
                BrandFilter = bagElem.Attribute("brandFilter").Value == "true";
            else
                BrandFilter = null;
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

            if (Buyable.HasValue)
                bagElem.Add(new XAttribute("buyable", Buyable.Value ? "true" : "false"));

            if (BrandFilter.HasValue)
                bagElem.Add(new XAttribute("brandFilter", BrandFilter.Value ? "true" : "false"));


            return rootElem;
        }
    }
}
