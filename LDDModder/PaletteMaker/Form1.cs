using LDDModder.LDD;
using LDDModder.LDD.Files;
using LDDModder.LDD.Palettes;
using LDDModder.Rebrickable;
using LDDModder.Rebrickable.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LDDModder.PaletteMaker
{
    public partial class Form1 : Form
    {
        private List<GetSetPartsResult.Part> PartNotFound;

        public Form1()
        {
            InitializeComponent();
            PartNotFound = new List<GetSetPartsResult.Part>();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateCustomPalette();

            //PaletteManager.LoadPalettes();
            //var heroPal = PaletteManager.Palettes.First(p => p.Name.Contains("Hero"));
            //PaletteManager.SavePalette(heroPal);
        }

        private void CreateCustomPalette()
        {
            string setNumber = textBox1.Text;
            if (!setNumber.Contains('-'))
                setNumber += "-1";

            var setPartsInfo = RebrickableAPIv2.GetSetParts.Execute(setNumber);
            if (setPartsInfo == null)
            {
                label1.Text = "Set not found!";
                return;
            }

            label1.Text = string.Format("{0} : {1}", setPartsInfo.SetId, setPartsInfo.Description);

            Application.DoEvents();

            var shortSetNumber = setNumber.Substring(0, setNumber.IndexOf('-'));
            var paletteInfo = new Bag(shortSetNumber + " " + setPartsInfo.Description, true);

            var setPalette = new Palette();

            PartNotFound.Clear();
            //foreach (var rbPart in setPartsInfo.Parts)
            //{
            //    //var lddPart = GetItemForRBPart(rbPart);
            //    var lddPart = PaletteBuilder.GetPaletteItem(rbPart);
            //    if (lddPart != null)
            //        setPalette.Items.Add(lddPart);
            //    else
            //        PartNotFound.Add(rbPart);
            //}
            PaletteManager.SavePalette(new PaletteFile(paletteInfo, setPalette));

            foreach (var notFound in PartNotFound)
                Trace.WriteLine(string.Format("Part not found \"{3}\"\r\n\tID:{0} ElementID:{1} Color:{2}\r\nQuantity:{4}\r\n", notFound.PartId, notFound.ElementId, notFound.ColorName, notFound.Name, notFound.Quantity));
        }

        private List<LddPalette> GetLddPalettes()
        {
            var paletteList = new List<LddPalette>();

            foreach (string plifPath in Directory.GetFiles(PaletteManager.LddPalettesDirectory, "*.lif"))
            {
                using (var lifStream = LifFile.Open(plifPath))
                {
                    var palette = new LddPalette();
                    foreach (var entry in lifStream.Entries.OfType<LifFile.FileEntry>())
                    {
                        if (entry.Name.EndsWith(".baxml"))
                        {
                            palette.Info = Bag.Load(entry.OpenStream());
                            palette.Info.OriginFileName = string.Format("{0} |{1}", plifPath, entry.FullPath);
                        }
                        else if (entry.Name.EndsWith(".paxml"))
                        {
                            var paletteContent = Palette.Load(entry.OpenStream());
                            paletteContent.OriginFileName = string.Format("{0} |{1}", plifPath, entry.FullPath);
                            palette.Palettes.Add(paletteContent);
                        }
                        
                    }
                    paletteList.Add(palette);
                }
            }

            foreach (string subDirPath in Directory.GetDirectories(PaletteManager.LddPalettesDirectory))
            {
                if (!File.Exists(Path.Combine(subDirPath, Bag.FileName)))
                    continue;
                var palette = new LddPalette();
                palette.Info = Bag.Load(Path.Combine(subDirPath, Bag.FileName));
                foreach (string paxmlPath in Directory.GetFiles(subDirPath, "*.paxml"))
                    palette.Palettes.Add(Palette.Load(paxmlPath));
                paletteList.Add(palette);
            }

            return paletteList;
        }

        class LddPalette
        {
            public Bag Info { get; set; }

            // Fields...
            private List<Palette> _Palettes;

            public string Name
            {
                get { return Info != null ? Info.Name : string.Empty; }
            }

            public List<Palette> Palettes
            {
                get { return _Palettes; }
            }

            public LddPalette()
            {
                Info = null;
                _Palettes = new List<Palette>();
            }

            public LddPalette(Bag info)
            {
                Info = info;
                _Palettes = new List<Palette>();
            }
        }
    }
}
