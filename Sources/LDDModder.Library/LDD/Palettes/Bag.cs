using LDDModder.LDD.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    [XmlRoot("BAXML")]
    public class Bag
    {
        public const string EXTENSION = "BAXML";
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
            FileVersion = new VersionInfo(1, 0);
            PaletteVersion = 1;
        }

        #region Save / Load

        public void Save(string filename)
        {
            using (var fs = File.Open(filename, FileMode.Create))
                Save(fs);
        }

        public void Save(Stream stream)
        {
            var doc = new XDocument(
                new XElement(EXTENSION,
                    new XAttribute("versionMajor", FileVersion.Major),
                    new XAttribute("versionMinor", FileVersion.Minor)
                    )
                )
            {
                Declaration = new XDeclaration("1.0", "UTF-8", "no")
            };

            var bagElem = doc.Root.AddElement("Bag", new XAttribute("name", Name ?? string.Empty));
            if (PaletteVersion > 0)
                bagElem.Add(new XAttribute("version", PaletteVersion));

            bagElem.Add(new XAttribute("countable", Countable));
            
            bagElem.AddBooleanAttribute("buyable", Buyable, LinqXmlExtensions.BooleanXmlRepresentation.TrueFalse);

            bagElem.Add(new XAttribute("brand", ParentBrand.ToString()));

            bagElem.AddBooleanAttribute("brandFilter", BrandFilter, LinqXmlExtensions.BooleanXmlRepresentation.TrueFalse);
            doc.Save(stream);
        }

        public static Bag Load(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return Load(fs);
        }

        public static Bag Load(Stream stream)
        {
            var doc = XDocument.Load(stream);
            doc.Root.TryGetIntAttribute("versionMajor", out int major);
            doc.Root.TryGetIntAttribute("versionMinor", out int minor);
            var bagElem = doc.Root.Element("Bag");

            var bag = new Bag()
            {
                FileVersion = new VersionInfo(major, minor),
                Name = bagElem.ReadAttribute("name", string.Empty),
                ParentBrand = bagElem.ReadAttribute("brand", Brand.LDD),
                PaletteVersion = bagElem.ReadAttribute("version", 0),
                Countable = bagElem.ReadAttribute("countable", false),
                Buyable = bagElem.ReadAttribute("buyable", false),
                BrandFilter = bagElem.ReadAttribute("brandFilter", false)
            };

            return bag;
        }

        #endregion

    }
}
