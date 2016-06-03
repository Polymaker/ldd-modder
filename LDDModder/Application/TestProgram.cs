using LDDModder.LDD.Palettes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder
{
    static class TestProgram
    {
        static void Main(string[] args)
        {
            string palettePath = @"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\Palettes\LDD-10\LDD.PAXML";
            Palette lddPalette = null;
            using (var fs = File.OpenRead(palettePath))
            {
                var xmlSer = new XmlSerializer(typeof(Palette));
                lddPalette = (Palette)xmlSer.Deserialize(fs);
            }
        }
    }
}
