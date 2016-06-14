using LDDModder.LDD;
using LDDModder.LDD.General;
using LDDModder.LDD.Palettes;
using LDDModder.LDD.Primitives;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder
{
    static class TestProgram
    {
        static void Main(string[] args)
        {

            //var testElem = new XElement("Root",
            //    new XAttribute("aassdd", "123"),
            //    new XAttribute("ab", "123"),
            //    new XAttribute("helo", "123"));
            //testElem.SortAttributes(a => a.Name.LocalName.Length);

            //int primitiveID = 58135;

            var derocationsIds = new List<string>();

            foreach(var filepath in Directory.GetFiles(LDDManager.GetDirectory(LDDManager.DbDirectories.Decorations),"*.png"))
            {
                derocationsIds.Add(Path.GetFileNameWithoutExtension(filepath));
            }

            XDocument decorationMappingDoc = null;

            using (var fs = File.OpenRead(Path.Combine(LDDManager.GetLifDirectory(LifInstance.Database), "DecorationMapping.xml")))
            {
                decorationMappingDoc = XDocument.Load(fs);
            }

            var decoUsageInfo = derocationsIds.ToDictionary(x => x, y => 0);

            foreach (var mappingElem in decorationMappingDoc.Descendants("Mapping"))
            {
                var mappingInfo = XSerializationHelper.DefaultDeserialize<DecorationMapping>(mappingElem);
                var decoIdStr = mappingInfo.DecorationID.ToString();
                if(decoUsageInfo.ContainsKey(decoIdStr))
                    decoUsageInfo[decoIdStr]++;
                else
                {
                    Trace.WriteLine(string.Format("Ref to unexisting decoration: {0} (DesignID: {1})", decoIdStr, mappingInfo.DesignID));
                }
            }
            Trace.WriteLine("Unused decorations:");
            foreach (var keyval in decoUsageInfo.Where(x => x.Value == 0))
                Trace.WriteLine("  " + keyval.Key);

            Trace.WriteLine("\r\nDecorations usage:");
            foreach (var keyval in decoUsageInfo.Where(x => x.Value > 0))
                Trace.WriteLine(string.Format("  {0}{1}", keyval.Key.PadRight(7), keyval.Value));

            //var decorationMappingDoc = XDocument.Load()

            //var lddBrick = Primitive.LoadFrom<Primitive>(Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives), primitiveID + ".xml"));
            //lddBrick.Save(primitiveID + ".xml");

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
