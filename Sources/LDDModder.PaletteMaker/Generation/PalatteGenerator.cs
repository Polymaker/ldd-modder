﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Generation
{
    public class PalatteGenerator
    {

        //private void CreatePaletteFromSet(string setNumber)
        //{
        //    var setInfo = RebrickableAPI.GetSet(setNumber);

        //    var setParts = RebrickableAPI.GetSetParts(setNumber).ToList();
        //    var palette = new LDD.Palettes.Palette()
        //    {
        //        Name = setInfo.SetNum
        //    };
        //    int totalSetParts = setParts.Count;
        //    int totalMatchedParts = 0;
        //    var unmatchedParts = new List<Rebrickable.Models.SetPart>();

        //    using (var db = new PaletteDbContext($"Data Source={DatabaseFilePath}"))
        //    {
        //        foreach (var setPart in setParts)
        //        {
        //            if (setPart.Color == null || setPart.Color.Id == 9999)
        //                continue;

        //            if (!string.IsNullOrEmpty(setPart.ElementId))
        //            {
        //                var existingElem = db.LddElements.FirstOrDefault(x => x.ElementID == setPart.ElementId);

        //                if (existingElem != null)
        //                {
        //                    palette.Items.Add(existingElem.ToPaletteItem(setPart.Quantity));
        //                    totalMatchedParts++;
        //                }
        //                else
        //                {
        //                    var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == setPart.Part.PartNum);
        //                    if (rbPart == null)
        //                        continue;

        //                    var rbColor = db.Colors.FirstOrDefault(x => x.ID == setPart.Color.Id);

        //                    if (rbColor == null)
        //                        continue;

        //                    var lddColor = rbColor.ColorMatches.OrderBy(x => x.ColorID).FirstOrDefault(x => x.Platform == "LEGO");

        //                    if (lddColor == null)
        //                    {
        //                        unmatchedParts.Add(setPart);
        //                        Debug.WriteLine($"Could not match Rb color {rbColor.ID}");
        //                        continue;
        //                    }

        //                    var lddPart = FindMatchingPart(db, rbPart, rbPart.PartID, setPart.Part.PartNum);

        //                    if (lddPart != null)
        //                    {
        //                        var newLddElement = db.LddElements.FirstOrDefault(x => x.DesignID == lddPart.DesignID);

        //                        if (newLddElement != null)
        //                        {
        //                            newLddElement = newLddElement.Clone(setPart.ElementId);
        //                            int mainColorID = newLddElement.Parts.First().MaterialID;

        //                            foreach (var cfg in newLddElement.Parts)
        //                            {
        //                                if (cfg.MaterialID == mainColorID)
        //                                    cfg.MaterialID = lddColor.ColorID;
        //                            }

        //                            db.LddElements.Add(newLddElement);
        //                            palette.Items.Add(newLddElement.ToPaletteItem(setPart.Quantity));
        //                            totalMatchedParts++;
        //                        }
        //                        else
        //                        {
        //                            newLddElement = new Models.LDD.LddElement()
        //                            {
        //                                DesignID = lddPart.DesignID,
        //                                ElementID = setPart.ElementId,
        //                                IsAssembly = lddPart.IsAssembly
        //                            };

        //                            if (!lddPart.IsAssembly)
        //                            {
        //                                newLddElement.Parts.Add(new Models.LDD.ElementPart()
        //                                {
        //                                    PartID = lddPart.DesignID,
        //                                    MaterialID = lddColor.ColorID
        //                                });

        //                                db.LddElements.Add(newLddElement);
        //                                palette.Items.Add(newLddElement.ToPaletteItem(setPart.Quantity));
        //                                totalMatchedParts++;
        //                            }
        //                            else
        //                            {
        //                                var assemParts = db.AssemblyParts.Where(x => x.AssemblyID == lddPart.DesignID).ToList();

        //                                if (assemParts.Any())
        //                                {
        //                                    foreach (var subPart in assemParts)
        //                                    {
        //                                        var materials = subPart.GetMaterials();
        //                                        var partConfig = new Models.LDD.ElementPart()
        //                                        {
        //                                            PartID = subPart.PartID,
        //                                            MaterialID = lddColor.ColorID //materials.FirstOrDefault()
        //                                        };

        //                                        if (materials.Count > 1)
        //                                        {
        //                                            for (int i = 1; i < materials.Count; i++)
        //                                                partConfig.SubMaterials.Add(new Models.LDD.ElementMaterial(i, lddColor.ColorID/*materials[i]*/));
        //                                        }

        //                                        newLddElement.Parts.Add(partConfig);
        //                                    }

        //                                    db.LddElements.Add(newLddElement);
        //                                    palette.Items.Add(newLddElement.ToPaletteItem(setPart.Quantity));
        //                                    totalMatchedParts++;
        //                                }
        //                                else
        //                                    unmatchedParts.Add(setPart);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        unmatchedParts.Add(setPart);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == setPart.Part.PartNum);
        //                if (rbPart == null)
        //                    continue;

        //                var rbColor = db.Colors.FirstOrDefault(x => x.ID == setPart.Color.Id);

        //                if (rbColor == null)
        //                    continue;

        //                var lddColor = rbColor.ColorMatches.OrderBy(x => x.ColorID).FirstOrDefault(x => x.Platform == "LEGO");

        //                if (lddColor == null)
        //                {
        //                    unmatchedParts.Add(setPart);
        //                    Debug.WriteLine($"Could not match Rb color {rbColor.ID}");
        //                    continue;
        //                }

        //                var lddPart = FindMatchingPart(db, rbPart, rbPart.PartID, setPart.Part.PartNum);
        //                if (lddPart != null)
        //                {

        //                    if (!lddPart.IsAssembly)
        //                    {
        //                        var brick = new LDD.Palettes.Brick(
        //                            int.Parse(lddPart.DesignID), string.Empty, setPart.Quantity);
        //                        brick.MaterialID = lddColor.ColorID;
        //                        palette.Items.Add(brick);
        //                        totalMatchedParts++;
        //                    }
        //                    else
        //                        unmatchedParts.Add(setPart);
        //                }
        //                else
        //                {
        //                    unmatchedParts.Add(setPart);
        //                }
        //            }

        //        }

        //        db.SaveChanges();

        //        Debug.WriteLine($"Matched {totalMatchedParts} of {totalSetParts} parts");
        //        Debug.WriteLine("Unmatched parts:");
        //        foreach (var part in unmatchedParts)
        //            Debug.WriteLine($"{part.Part.PartNum} {part.Part.Name} Color: {part.Color.Name} ({part.ElementId})");
        //    }

        //    var paletteFile = new PaletteFile(new LDD.Palettes.Bag()
        //    {
        //        Name = $"{setInfo.SetNum} {setInfo.Name}",
        //        Countable = true,
        //        ParentBrand = LDD.Data.Brand.LDD
        //    });

        //    paletteFile.Palettes.Add(palette);

        //    var userPaletteDir = LDD.LDDEnvironment.Current.GetAppDataSubDir("UserPalettes");
        //    string paletteFileName = FileHelper.GetSafeFileName(setInfo.Name.Replace(" ", ""));

        //    paletteFile.SaveToDirectory(Path.Combine(userPaletteDir, paletteFileName), false);

        //    //paletteFile.SaveAsLif(Path.Combine(userPaletteDir, paletteFileName) + ".lif");
        //}

        //static Models.LDD.LddPart FindMatchingPart(PaletteDbContext db, Models.Rebrickable.RbPart part, string partID, string originalId, List<string> tentatives = null)
        //{
        //    if (tentatives == null)
        //        tentatives = new List<string>();
        //    else
        //    {
        //        if (tentatives.Contains(partID))
        //            return null;
        //        tentatives.Add(partID);
        //    }

        //    Debug.WriteLine($"Finding matching part for '{originalId}': {partID}");
        //    var foundPartMatch = db.PartMappings.FirstOrDefault(x => x.RebrickableID == partID && x.IsActive);
        //    if (foundPartMatch != null)
        //    {
        //        return db.LddParts.FirstOrDefault(x => x.DesignID == foundPartMatch.LegoID);
        //    }

        //    if (!string.IsNullOrEmpty(part?.ParentPartID))
        //    {
        //        var parentPart = db.RbParts.FirstOrDefault(x => x.PartID == part.ParentPartID);

        //        var found = FindMatchingPart(db, parentPart, part.ParentPartID, originalId, tentatives);
        //        if (found != null)
        //            return found;
        //    }

        //    if (part != null && part.Relationships.Any())
        //    {
        //        foreach (var relatedPart in part.Relationships)
        //        {
        //            if (relatedPart.RelationType == Models.Rebrickable.RbRelationType.Print)
        //                continue;
        //            var found = FindMatchingPart(db, relatedPart.ChildPart, relatedPart.ChildPartID, originalId, tentatives);
        //            if (found != null)
        //                return found;
        //        }
        //    }

        //    return null;
        //}

    }
}
