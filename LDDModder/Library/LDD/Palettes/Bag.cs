using LDDModder.LDD.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.LDD.Palettes
{
    /// <summary>
    /// AKA Palette Info
    /// </summary>
    [XmlRoot("BAXML")]
    public class Bag
    {

        public string Name { get; set; }

        public int PaletteVersion { get; set; }

        public bool Countable { get; set; }

        public Brand ParentBrand { get; set; }

        public VersionInfo FileVersion { get; set; }
    }
}
