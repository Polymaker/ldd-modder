using LDDModder.LDD;
using LDDModder.LDD.Palettes;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker
{
    public partial class Form1 : Form
    {
        private Palette lddPalette;
        private List<SetParts.Part> PartNotFound;
        private Dictionary<int, int> RbLddColorTable;

        public Form1()
        {
            InitializeComponent();
            lddPalette = XSerializable.LoadFrom<Palette>("LDD.PAXML");
            PartNotFound = new List<SetParts.Part>();
            RbLddColorTable = new Dictionary<int, int>();
            BuildColorTable();
        }

        private void BuildColorTable()
        {
            RbLddColorTable.Add(15, 1);
            RbLddColorTable.Add(19, 5);
            RbLddColorTable.Add(14, 24);
            RbLddColorTable.Add(25, 106);
            RbLddColorTable.Add(4, 21);
            RbLddColorTable.Add(2, 28);
            RbLddColorTable.Add(1, 23);
            RbLddColorTable.Add(6, 25);
            RbLddColorTable.Add(7, 2);
            RbLddColorTable.Add(8, 27);
            RbLddColorTable.Add(0, 26);
            RbLddColorTable.Add(47, 40);
            RbLddColorTable.Add(40, 111);
            RbLddColorTable.Add(143, 42);
            RbLddColorTable.Add(42, 49);
            RbLddColorTable.Add(36, 41);
            RbLddColorTable.Add(182, 47);
            RbLddColorTable.Add(46, 44);
            RbLddColorTable.Add(34, 48);
            RbLddColorTable.Add(383, 301);
            RbLddColorTable.Add(13, 16);
            RbLddColorTable.Add(22, 104);
            RbLddColorTable.Add(12, 4);
            RbLddColorTable.Add(100, 100);
            RbLddColorTable.Add(216, 216);
            RbLddColorTable.Add(92, 18);
            RbLddColorTable.Add(366, 12);
            RbLddColorTable.Add(462, 105);
            RbLddColorTable.Add(125, 125);
            RbLddColorTable.Add(18, 3);
            RbLddColorTable.Add(27, 119);
            RbLddColorTable.Add(120, 120);
            RbLddColorTable.Add(10, 37);
            RbLddColorTable.Add(74, 29);
            RbLddColorTable.Add(17, 6);
            RbLddColorTable.Add(3, 107);
            RbLddColorTable.Add(11, 116);
            RbLddColorTable.Add(118, 118);
            RbLddColorTable.Add(73, 102);
            RbLddColorTable.Add(110, 110);
            RbLddColorTable.Add(20, 39);
            RbLddColorTable.Add(21, 50);
            RbLddColorTable.Add(5, 221);
            RbLddColorTable.Add(378, 151);
            RbLddColorTable.Add(503, 103);
            RbLddColorTable.Add(230, 113);
            RbLddColorTable.Add(52, 126);
            RbLddColorTable.Add(373, 136);
            RbLddColorTable.Add(379, 135);
            RbLddColorTable.Add(77, 223);
            RbLddColorTable.Add(335, 153);
            RbLddColorTable.Add(320, 154);
            RbLddColorTable.Add(79, 20);
            RbLddColorTable.Add(142, 127);
            RbLddColorTable.Add(9, 45);
            RbLddColorTable.Add(272, 140);
            RbLddColorTable.Add(82, 310);
            RbLddColorTable.Add(135, 131);
            RbLddColorTable.Add(80, 309);
            RbLddColorTable.Add(484, 38);
            RbLddColorTable.Add(28, 138);
            RbLddColorTable.Add(81, 200);
            RbLddColorTable.Add(26, 124);
            RbLddColorTable.Add(313, 11);
            RbLddColorTable.Add(115, 115);
            RbLddColorTable.Add(148, 316);
            RbLddColorTable.Add(137, 145);
            RbLddColorTable.Add(288, 141);
            RbLddColorTable.Add(178, 187);
            RbLddColorTable.Add(134, 139);
            RbLddColorTable.Add(72, 199);
            RbLddColorTable.Add(71, 194);
            RbLddColorTable.Add(232, 232);
            RbLddColorTable.Add(70, 192);
            RbLddColorTable.Add(85, 268);
            RbLddColorTable.Add(78, 283);
            RbLddColorTable.Add(86, 217);
            RbLddColorTable.Add(69, 198);
            RbLddColorTable.Add(351, 22);
            RbLddColorTable.Add(179, 179);
            RbLddColorTable.Add(68, 36);
            RbLddColorTable.Add(112, 112);
            RbLddColorTable.Add(151, 208);
            RbLddColorTable.Add(114, 114);
            RbLddColorTable.Add(117, 117);
            RbLddColorTable.Add(129, 129);
            RbLddColorTable.Add(226, 226);
            RbLddColorTable.Add(29, 222);
            RbLddColorTable.Add(212, 212);
            RbLddColorTable.Add(35, 311);
            RbLddColorTable.Add(23, 196);
            RbLddColorTable.Add(191, 191);
            RbLddColorTable.Add(132, 132);
            RbLddColorTable.Add(297, 297);
            RbLddColorTable.Add(294, 294);
            RbLddColorTable.Add(308, 308);
            RbLddColorTable.Add(54, 157);
            RbLddColorTable.Add(84, 312);
            RbLddColorTable.Add(323, 323);
            RbLddColorTable.Add(321, 321);
            RbLddColorTable.Add(31, 325);
            RbLddColorTable.Add(326, 330);
            RbLddColorTable.Add(322, 322);
            RbLddColorTable.Add(30, 324);
            RbLddColorTable.Add(158, 326);
            RbLddColorTable.Add(1000, 329);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string setNumber = textBox1.Text;
            if (!setNumber.Contains('-'))
                setNumber += "-1";

            int initialCount = lddPalette.Items.Count;

            var setPartsInfo = RebrickableAPI.GetSetParts.Execute(setNumber);
            label1.Text = setPartsInfo.Description;

            var paletteInfo = new Bag(setNumber + " " + setPartsInfo.Description, true);
            var setPalette = new Palette();

            

            PartNotFound.Clear();
            foreach (var rbPart in setPartsInfo.Parts)
            {
                var lddPart = GetItemForRBPart(rbPart);
                if (lddPart != null)
                    setPalette.Items.Add(lddPart);
                else
                    PartNotFound.Add(rbPart);
            }

            if (initialCount != lddPalette.Items.Count)
                XSerializable.Save(lddPalette, "LDD.PAXML");

            PaletteManager.CreateCustomPalette(paletteInfo, setPalette);

            foreach (var notFound in PartNotFound)
                Trace.WriteLine(string.Format("Part not found \"{3}\"\r\n\tID:{0} ElementID:{1} Color:{2}\r\nQuantity:{4}\r\n", notFound.PartId, notFound.ElementId, notFound.ColorName, notFound.Name, notFound.Quantity));
        }

        private PaletteItem GetItemForRBPart(SetParts.Part rbPart)
        {
            PaletteItem itemForPart = GetPaletteItem(rbPart.ElementId, rbPart.Quantity);
            if (itemForPart != null)
                return itemForPart;

            int lddPartId = 0;

            if (!RbLddColorTable.ContainsKey(rbPart.RbColorId))
                return null;

            int lddColorId = RbLddColorTable[rbPart.RbColorId];

            if (int.TryParse(rbPart.PartId, out lddPartId))
            {
                itemForPart = GetPaletteItem(rbPart.ElementId, lddPartId, lddColorId, rbPart.Quantity);
                if (itemForPart != null)
                    return itemForPart;

                if (File.Exists(Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives), lddPartId + ".xml")))
                {
                    itemForPart = new Brick(lddPartId, rbPart.ElementId, lddColorId, rbPart.Quantity);
                    lddPalette.Items.Add(itemForPart);
                    return itemForPart;
                }
                else if (File.Exists(Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.Assemblies), lddPartId + ".lxfml")))
                {
                    itemForPart = new Assembly(lddPartId, rbPart.ElementId, rbPart.Quantity);
                    //itemForPart = new Brick(lddPartId, rbPart.ElementId, lddColorId, rbPart.Quantity);
                    //lddPalette.Items.Add(itemForPart);
                    return itemForPart;
                }
                if (lddPartId == 54200)
                {
                    return new Brick(50746, rbPart.ElementId, lddColorId, rbPart.Quantity);
                }
            }

            var partInfo = RebrickableAPI.GetPart.Execute(new GetPartParameters(rbPart.PartId, true, true, false));

            if (partInfo.ExternalParts != null && partInfo.ExternalParts.LegoDesignIds != null)
            {
                if (partInfo.ExternalParts.LegoDesignIds.Length > 0)
                {
                    for (int i = 0; i < partInfo.ExternalParts.LegoDesignIds.Length; i++)
                    {
                        itemForPart = GetPaletteItem(rbPart.ElementId, int.Parse(partInfo.ExternalParts.LegoDesignIds[i]), lddColorId, rbPart.Quantity);
                        if (itemForPart != null)
                            return itemForPart;
                    }
                }
            }

            return null;
        }

        private PaletteItem GetPaletteItem(string elementId, int quantity)
        {
            return GetPaletteItem(elementId, -1, -1, quantity);
        }

        private PaletteItem GetPaletteItem(string elementId, int designId, int colorId, int quantity)
        {
            PaletteItem item = null;
            if (lddPalette.Items.Any(i => i.ElementID == elementId))
            {
                item = (PaletteItem)lddPalette.Items.First(i => i.ElementID == elementId).Clone();
            }
            else if (designId > 0 && colorId > 0 && lddPalette.Items.OfType<Brick>().Any(b => b.DesignID == designId && b.MaterialID == colorId))
            {
                item = (PaletteItem)lddPalette.Items.OfType<Brick>().First(b => b.DesignID == designId && b.MaterialID == colorId).Clone();
            }
            else if (designId > 0 && colorId > 0 && lddPalette.Items.OfType<Brick>().Any(b => b.DesignID == designId))
            {
                item = (PaletteItem)lddPalette.Items.OfType<Brick>().First(b => b.DesignID == designId).Clone();
                (item as Brick).MaterialID = colorId;
            }
            else if (designId > 0 && colorId > 0 && lddPalette.Items.Any(i => i.DesignID == designId))
            {
                item = (PaletteItem)lddPalette.Items.First(b => b.DesignID == designId).Clone();
            }
            if(item != null)
                item.Quantity = quantity;
            return item;
        }

    }
}
