using LDDModder.LDD.Files;
using LDDModder.PaletteMaker.Models;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.Utilities;
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
        public string DatabaseFilePath;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string curPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var curDir = new DirectoryInfo(curPath);
            while (curDir.Name.ToLower() != "bin")
                curDir = curDir.Parent;
            DatabaseFilePath = Path.Combine(curDir.Parent.FullName, "Bricks.db");

            LDD.LDDEnvironment.Initialize();
            RebrickableAPI.ApiKey = "aU49o5xulf";
            RebrickableAPI.InitializeClient();

            InitializeDB();
        }

        private void InitializeDB()
        {
            string curPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            curPath = Path.GetDirectoryName(curPath);
            DatabaseFilePath = Path.Combine(curPath, "Bricks.db");

            if (!File.Exists(DatabaseFilePath))
            {
                File.Copy(Path.Combine(curPath, "Resources\\EmptyDatabase.db"), DatabaseFilePath);
                DatabaseInitializer.InitDb(DatabaseFilePath);
            }

            //DatabaseInitializer.InitRebrickableParts(DatabaseFilePath, "parts.csv");

            //DatabaseInitializer.InitRebrickablePartRelationships(DatabaseFilePath, "part_relationships.csv");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                CreatePaletteFromSet("75252-1");
            });
        }

        class PartMatchResult
        {
            public Rebrickable.Models.Part RebrickablePart { get; set; }
            public Models.LDD.LddPart LddPart { get; set; }
        }

        private void CreatePaletteFromSet(string setNumber)
        {
            var setInfo = RebrickableAPI.GetSet(setNumber);

            var setParts = RebrickableAPI.GetSetParts(setNumber).ToList();
            var palette = new LDD.Palettes.Palette()
            {
                Name = setInfo.SetNum
            };
            int totalSetParts = setParts.Count;
            int totalMatchedParts = 0;
            var unmatchedParts = new List<Rebrickable.Models.SetPart>();

            using (var db = new PaletteDbContext($"Data Source={DatabaseFilePath}"))
            {
                foreach (var setPart in setParts)
                {
                    if (setPart.Color == null || setPart.Color.Id == 9999)
                        continue;

                    if (!string.IsNullOrEmpty(setPart.ElementId))
                    {
                        var existingElem = db.LddElements.FirstOrDefault(x => x.ElementID == setPart.ElementId);

                        if (existingElem != null)
                        {
                            palette.Items.Add(existingElem.ToPaletteItem(setPart.Quantity));
                            totalMatchedParts++;
                        }
                        else
                        {
                            var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == setPart.Part.PartNum);
                            if (rbPart == null)
                                continue;

                            var rbColor = db.Colors.FirstOrDefault(x => x.ID == setPart.Color.Id);

                            if (rbColor == null)
                                continue;

                            var lddColor = rbColor.ColorMatches.OrderBy(x => x.ColorID).FirstOrDefault(x => x.Platform == "LEGO");

                            if (lddColor == null)
                            {
                                unmatchedParts.Add(setPart);
                                Debug.WriteLine($"Could not match Rb color {rbColor.ID}");
                                continue;
                            }

                            var lddPart = FindMatchingPart(db, rbPart, rbPart.PartID, setPart.Part.PartNum);

                            if (lddPart != null)
                            {
                                var newLddElement = db.LddElements.FirstOrDefault(x => x.DesignID == lddPart.DesignID);
                                
                                if (newLddElement != null)
                                {
                                    newLddElement = newLddElement.Clone(setPart.ElementId);
                                    newLddElement.Configurations.First().MaterialID = lddColor.ColorID;

                                    db.LddElements.Add(newLddElement);
                                    palette.Items.Add(newLddElement.ToPaletteItem(setPart.Quantity));
                                    totalMatchedParts++;
                                }
                                else
                                {
                                    newLddElement = new Models.LDD.LddElement()
                                    {
                                        DesignID = lddPart.DesignID,
                                        ElementID = setPart.ElementId,
                                        IsAssembly = lddPart.IsAssembly 
                                    };

                                    if (!lddPart.IsAssembly)
                                    {
                                        newLddElement.Configurations.Add(new Models.LDD.PartConfiguration()
                                        {
                                            DesignID = lddPart.DesignID,
                                            MaterialID = lddColor.ColorID
                                        });

                                        db.LddElements.Add(newLddElement);
                                        palette.Items.Add(newLddElement.ToPaletteItem(setPart.Quantity));
                                        totalMatchedParts++;
                                    }
                                    else
                                    {
                                        var assemParts = db.AssemblyParts.Where(x => x.AssemblyID == lddPart.DesignID).ToList();
                                        
                                        if (assemParts.Any())
                                        {
                                            foreach (var subPart in assemParts)
                                            {
                                                var materials = subPart.GetMaterials();
                                                var partConfig = new Models.LDD.PartConfiguration()
                                                {
                                                    DesignID = subPart.PartID,
                                                    MaterialID = materials.FirstOrDefault()
                                                };

                                                if (materials.Count > 1)
                                                {
                                                    for (int i = 1; i < materials.Count; i++)
                                                        partConfig.SubMaterials.Add(new Models.LDD.SubMaterial(i, materials[i]));
                                                }

                                                newLddElement.Configurations.Add(partConfig);
                                            }

                                            db.LddElements.Add(newLddElement);
                                            palette.Items.Add(newLddElement.ToPaletteItem(setPart.Quantity));
                                            totalMatchedParts++;
                                        }
                                        else
                                            unmatchedParts.Add(setPart);
                                    }
                                }
                            }
                            else
                            {
                                unmatchedParts.Add(setPart);
                            }
                        }
                    }
                    else
                    {
                        var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == setPart.Part.PartNum);
                        if (rbPart == null)
                            continue;

                        var rbColor = db.Colors.FirstOrDefault(x => x.ID == setPart.Color.Id);

                        if (rbColor == null)
                            continue;

                        var lddColor = rbColor.ColorMatches.OrderBy(x=>x.ColorID).FirstOrDefault(x => x.Platform == "LEGO");

                        if (lddColor == null)
                        {
                            unmatchedParts.Add(setPart);
                            Debug.WriteLine($"Could not match Rb color {rbColor.ID}");
                            continue;
                        }

                        var lddPart = FindMatchingPart(db, rbPart, rbPart.PartID, setPart.Part.PartNum);
                        if (lddPart != null)
                        {

                            if (!lddPart.IsAssembly)
                            {
                                var brick = new LDD.Palettes.Brick(
                                    int.Parse(lddPart.DesignID), string.Empty, setPart.Quantity);
                                brick.MaterialID = lddColor.ColorID;
                                palette.Items.Add(brick);
                                totalMatchedParts++;
                            }
                            else
                                unmatchedParts.Add(setPart);
                        }
                        else
                        {
                            unmatchedParts.Add(setPart);
                        }
                    }
                        
                }

                db.SaveChanges();

                Debug.WriteLine($"Matched {totalMatchedParts} of {totalSetParts} parts");
                Debug.WriteLine("Unmatched parts:");
                foreach (var part in unmatchedParts)
                    Debug.WriteLine($"{part.Part.PartNum} {part.Part.Name} Color: {part.Color.Name} ({part.ElementId})");
            }

            var paletteFile = new PaletteFile(new LDD.Palettes.Bag()
            {
                Name = $"{setInfo.SetNum} {setInfo.Name}",
                Countable = true,
                ParentBrand = LDD.Data.Brand.LDD
            });

            paletteFile.Palettes.Add(palette);

            var userPaletteDir = LDD.LDDEnvironment.Current.GetAppDataSubDir("UserPalettes");
            string paletteFileName = FileHelper.GetSafeFileName(setInfo.Name.Replace(" ", ""));

            paletteFile.SaveToDirectory(Path.Combine(userPaletteDir, paletteFileName), false);

            //paletteFile.SaveAsLif(Path.Combine(userPaletteDir, paletteFileName) + ".lif");
        }

        static Models.LDD.LddPart FindMatchingPart(PaletteDbContext db, Models.Rebrickable.RbPart part, string partID, string originalId, List<string> tentatives = null)
        {
            if (tentatives == null)
                tentatives = new List<string>();
            else
            {
                if (tentatives.Contains(partID))
                    return null;
                tentatives.Add(partID);
            }

            Debug.WriteLine($"Finding matching part for '{originalId}': {partID}");
            var foundPartMatch = db.PartMappings.FirstOrDefault(x => x.RebrickableID == partID && x.IsActive);
            if (foundPartMatch != null)
            {
                return db.LddParts.FirstOrDefault(x => x.DesignID == foundPartMatch.LddID);
            }

            if (!string.IsNullOrEmpty(part?.ParentPartID))
            {
                var parentPart = db.RbParts.FirstOrDefault(x => x.PartID == part.ParentPartID);

                var found = FindMatchingPart(db, parentPart, part.ParentPartID, originalId, tentatives);
                if (found != null)
                    return found;
            }

            if (part != null && part.Relationships.Any())
            {
                foreach (var relatedPart in part.Relationships)
                {
                    if (relatedPart.RelationType == Models.Rebrickable.RbRelationType.Print)
                        continue;
                    var found = FindMatchingPart(db, relatedPart.ChildPart, relatedPart.ChildPartID, originalId, tentatives);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }
    }
}
