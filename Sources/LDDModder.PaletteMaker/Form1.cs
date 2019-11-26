using LDDModder.LDD.Files;
using LDDModder.PaletteMaker.Models;
using LDDModder.PaletteMaker.Rebrickable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //SQLiteConnection.CreateFile("Bricks.db");
            LDD.LDDEnvironment.Initialize();
            RebrickableAPI.ApiKey = "aU49o5xulf";
            RebrickableAPI.InitializeClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                ImportLddPalette();
            });
        }

        private PaletteFile GetLddPalette()
        {
            var appDataPalettes = LDD.LDDEnvironment.Current.GetAppDataSubDir("Palettes");
            if (File.Exists(Path.Combine(appDataPalettes, "LDD.lif")))
                return PaletteFile.FromLif(Path.Combine(appDataPalettes, "LDD.lif"));
            if (Directory.Exists(Path.Combine(appDataPalettes, "LDD")))
                return PaletteFile.FromDirectory(Path.Combine(appDataPalettes, "LDD"));
            string dbLifPath = Path.Combine(LDD.LDDEnvironment.Current.ApplicationDataPath, "db.lif");

            if (File.Exists(dbLifPath))
            {
                using (var lif = LifFile.Open(dbLifPath))
                {
                    var paletteEntry = lif.GetAllFiles().FirstOrDefault(x => x.Name == "LDD.lif");
                    if (paletteEntry != null)
                    {
                        using (var paletteLif = LifFile.Read(paletteEntry.GetStream()))
                            return PaletteFile.FromLif(paletteLif);
                    }
                }
            }

            return null;
        }

        private void ImportLddPalette()
        {
            var lddPalette = GetLddPalette();
            if (lddPalette == null)
                return;
            int processedCount = 0;

            var db = new PaletteDbContext("Data Source=Bricks.db");

            foreach (var item in lddPalette.Palettes[0].Items)
            {
                var legoElem = db.LegoElements.FirstOrDefault(x => x.DesignID == item.DesignID.ToString() && x.ElementID == item.ElementID);

                if (legoElem == null)
                {
                    legoElem = new Models.Palettes.LegoElement()
                    {
                        DesignID = item.DesignID.ToString(),
                        ElementID = item.ElementID,
                        IsAssembly = (item is LDD.Palettes.Assembly)
                    };
                    db.LegoElements.Add(legoElem);

                    if (item is LDD.Palettes.Assembly assy)
                    {
                        foreach (var part in assy.Parts)
                            legoElem.Configurations.Add(AssemblyPartToConfig(part));
                    }
                    else if (item is LDD.Palettes.Brick brick)
                    {
                        legoElem.Configurations.Add(PaletteBrickToConfig(brick));
                    }
                }

                processedCount++;
                if (processedCount % 100 == 0)
                {
                    Debug.WriteLine($"Total processed: {processedCount} of {lddPalette.Palettes[0].Items.Count}");
                    db.SaveChanges();
                    db.Dispose();
                    db = new PaletteDbContext("Data Source=Bricks.db");
                }
                
            }

            db.SaveChanges();
            db.Dispose();
        }

        private static Models.Palettes.PartConfiguration AssemblyPartToConfig(LDD.Palettes.Assembly.Part part)
        {
            var partConfig = new Models.Palettes.PartConfiguration()
            {
                DesignID = part.DesignID.ToString(),
                MaterialID = part.MaterialID,
            };
            if (part.Decorations?.Any() ?? false)
            {
                foreach (var deco in part.Decorations)
                {
                    partConfig.Decorations.Add(new Models.Palettes.Decoration()
                    {
                        SurfaceID = deco.SurfaceID,
                        DecorationID = deco.DecorationID
                    });
                }
            }
            if (part.SubMaterials?.Any() ?? false)
            {
                foreach (var subMat in part.SubMaterials)
                {
                    partConfig.SubMaterials.Add(new Models.Palettes.SubMaterial()
                    {
                        SurfaceID = subMat.SurfaceID,
                        MaterialID = subMat.MaterialID
                    });
                }
            }
            return partConfig;
        }

        private static Models.Palettes.PartConfiguration PaletteBrickToConfig(LDD.Palettes.Brick part)
        {
            var partConfig = new Models.Palettes.PartConfiguration()
            {
                DesignID = part.DesignID.ToString(),
                MaterialID = part.MaterialID,
            };
            if (part.Decorations?.Any() ?? false)
            {
                foreach (var deco in part.Decorations)
                {
                    partConfig.Decorations.Add(new Models.Palettes.Decoration()
                    {
                        SurfaceID = deco.SurfaceID,
                        DecorationID = deco.DecorationID
                    });
                }
            }
            if (part.SubMaterials?.Any() ?? false)
            {
                foreach (var subMat in part.SubMaterials)
                {
                    partConfig.SubMaterials.Add(new Models.Palettes.SubMaterial()
                    {
                        SurfaceID = subMat.SurfaceID,
                        MaterialID = subMat.MaterialID
                    });
                }
            }
            return partConfig;
        }
    }
}
