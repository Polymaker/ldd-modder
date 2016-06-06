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
        private List<SetParts.Part> PartNotFound;

        public Form1()
        {
            InitializeComponent();
            PartNotFound = new List<SetParts.Part>();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string setNumber = textBox1.Text;
            if (!setNumber.Contains('-'))
                setNumber += "-1";

            var setPartsInfo = RebrickableAPI.GetSetParts.Execute(setNumber);
            if (setPartsInfo == null)
            {
                label1.Text = "Set not found!";
                return;
            }

            label1.Text = setPartsInfo.Description;

            var paletteInfo = new Bag(setNumber + " " + setPartsInfo.Description, true);
            var setPalette = new Palette();

            PartNotFound.Clear();
            foreach (var rbPart in setPartsInfo.Parts)
            {
                //var lddPart = GetItemForRBPart(rbPart);
                var lddPart = PaletteBuilder.GetPaletteItem(rbPart);
                if (lddPart != null)
                    setPalette.Items.Add(lddPart);
                else
                    PartNotFound.Add(rbPart);
            }

            PaletteManager.CreateCustomPalette(paletteInfo, setPalette);

            foreach (var notFound in PartNotFound)
                Trace.WriteLine(string.Format("Part not found \"{3}\"\r\n\tID:{0} ElementID:{1} Color:{2}\r\nQuantity:{4}\r\n", notFound.PartId, notFound.ElementId, notFound.ColorName, notFound.Name, notFound.Quantity));
        }

    }
}
