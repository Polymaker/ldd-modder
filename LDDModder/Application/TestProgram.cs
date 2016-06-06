using LDDModder.LDD;
using LDDModder.LDD.Palettes;
using LDDModder.LDD.Primitives;
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
            var lddBrick = Primitive.LoadFrom<Primitive>(Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives), "99386" + ".xml"));
            lddBrick.Save("99386.xml");
            //PaletteManager.AllowDeleteDirectory(@"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\Palettes\Custom-1");
            //PaletteManager.AllowDeleteDirectory(@"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\Palettes\LDDExtended-1");

            //var pInfo = new Bag("LDD Extended", false);
            //var pItems = new Palette() { FileVersion = new LDD.General.VersionInfo(1, 0) };
            //pItems.Items.Add(new Brick(3004, "6022083", 226, 0));
            //pItems.Items.Add(new Brick(3004, "4245570", 141, 0));
            //pItems.Items.Add(new Brick(3004, "4153377", 151, 0));
            //PaletteManager.CreateCustomPalette(pInfo, pItems);

            //PaletteManager.CreatePaletteDirectory("Custom-1");

            //string palettePath = @"C:\Users\james\AppData\Roaming\LEGO Company\LEGO Digital Designer\Palettes\LDD-10\LDD.PAXML";
            //Palette lddPalette = null;
            //using (var fs = File.OpenRead(palettePath))
            //{
            //    var xmlSer = new XmlSerializer(typeof(Palette));
            //    lddPalette = (Palette)xmlSer.Deserialize(fs);
            //}
        }
    }
}
