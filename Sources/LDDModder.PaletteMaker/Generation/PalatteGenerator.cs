using LDDModder.LDD.Files;
using LDDModder.PaletteMaker.DB;
using LDDModder.PaletteMaker.Models.LDD;
using LDDModder.PaletteMaker.Models.Rebrickable;
using LDDModder.PaletteMaker.Rebrickable;
using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Generation
{
    public class PalatteGenerator
    {
        public static void CreatePaletteFromSet(string databaseFilePath, string setNumber)
        {
            var setInfo = RebrickableAPI.GetSet(setNumber);
            CreatePaletteFromSet(databaseFilePath, setInfo);

        }


        public static void CreatePaletteFromSet(string databaseFilePath, Rebrickable.Models.Set setInfo)
        {
            var setParts = RebrickableAPI.GetSetParts(setInfo.SetNum).ToList();
            var palette = new LDD.Palettes.Palette()
            {
                Name = setInfo.SetNum
            };
            int totalSetParts = setParts.Count;
            int totalMatchedParts = 0;
            var unmatchedParts = new List<Rebrickable.Models.SetPart>();
            var addedElements = new List<LddElement>();

            using (var db = new PaletteDbContext($"Data Source={databaseFilePath}"))
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
                                Debug.WriteLine($"Part match Rb: {setPart.Part.PartNum} Ldd: {lddPart.DesignID}");

                                var newLddElement = db.LddElements.FirstOrDefault(x => x.DesignID == lddPart.DesignID);

                                if (newLddElement != null)
                                {
                                    newLddElement = newLddElement.Clone(setPart.ElementId);
                                    newLddElement.Flag = 2;
                                    addedElements.Add(newLddElement);

                                    int mainColorID = newLddElement.Parts.First().MaterialID;

                                    foreach (var cfg in newLddElement.Parts)
                                    {
                                        if (cfg.MaterialID == mainColorID)
                                            cfg.MaterialID = lddColor.ColorID;
                                    }
                                    if (!rbPart.IsPrintOrPattern)
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
                                        IsAssembly = lddPart.IsAssembly,
                                        Flag = 2
                                    };
                                    addedElements.Add(newLddElement);
                                    if (!lddPart.IsAssembly)
                                    {
                                        newLddElement.Parts.Add(new Models.LDD.ElementPart()
                                        {
                                            PartID = lddPart.DesignID,
                                            MaterialID = lddColor.ColorID
                                        });
                                        if (!rbPart.IsPrintOrPattern)
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
                                                var partConfig = new Models.LDD.ElementPart()
                                                {
                                                    PartID = subPart.PartID,
                                                    MaterialID = lddColor.ColorID //materials.FirstOrDefault()
                                                };

                                                if (materials.Count > 1)
                                                {
                                                    for (int i = 1; i < materials.Count; i++)
                                                        partConfig.SubMaterials.Add(new Models.LDD.ElementMaterial(i, lddColor.ColorID/*materials[i]*/));
                                                }

                                                newLddElement.Parts.Add(partConfig);
                                            }

                                            //db.LddElements.Add(newLddElement);
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
                            Debug.WriteLine($"Part match Rb: {setPart.Part.PartNum} Ldd: {lddPart.DesignID}");
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

                //db.SaveChanges();

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

            //paletteFile.SaveToDirectory(Path.Combine(userPaletteDir, paletteFileName), false);

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

            if (originalId != partID)
                Debug.WriteLine($"Finding matching part for '{originalId}': {partID}");

            var foundPartMatch = db.PartMappings.FirstOrDefault(x => x.RebrickableID == partID && x.IsActive);
            if (foundPartMatch != null)
            {
                return db.LddParts.FirstOrDefault(x => x.DesignID == foundPartMatch.LegoID);
            }

            if (partID.EndsWith("a") || partID.EndsWith("b"))
            {
                var testPart = db.LddParts.FirstOrDefault(x => x.DesignID == partID.Substring(0, partID.Length - 1));
                if (testPart != null)
                {
                    db.PartMappings.Add(new Models.PartMapping()
                    {
                        RebrickableID = partID,
                        LegoID = testPart.DesignID,
                        IsActive = true
                    });
                    return testPart;
                }
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
                    if (relatedPart.RelationTypeFlag == Models.Rebrickable.RbRelationType.Print)
                        continue;

                    if (relatedPart.ChildPart.IsPrintOrPattern)
                        continue;

                    var found = FindMatchingPart(db, relatedPart.ChildPart, relatedPart.ChildPartID, originalId, tentatives);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }


        public static void FindLddPartsForSet(PaletteDbContext db, List<SetPartWrapper> setParts)
        {
            var rbColors = new Dictionary<int, RbColor>();
            foreach (var col in db.Colors)
                rbColors.Add(col.ID, col);

            foreach (var setPart in setParts)
            {
                if (setPart.CategoryID == 17 || setPart.CategoryID == 58)
                {
                    setPart.MatchingFlags = PartMatchingFlags.NonLegoPart;
                    continue;
                }

                var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == setPart.PartID);
                if (rbPart == null)
                {
                    setPart.MatchingFlags = PartMatchingFlags.InvalidRbPart;
                    continue;
                }

                if (!rbColors.TryGetValue(setPart.ColorID, out RbColor rbColor))
                {
                    setPart.MatchingFlags = PartMatchingFlags.InvalidRbColor;
                    continue;
                }

                if (!string.IsNullOrEmpty(setPart.ElementID))
                {
                    var foundElem = db.LddElements.FirstOrDefault(x => x.ElementID == setPart.ElementID);
                    if (foundElem != null)
                    {
                        setPart.LddElement = foundElem;
                        setPart.LddPart = db.LddParts.FirstOrDefault(x => x.DesignID == foundElem.DesignID);

                        if (setPart.LddPart != null)
                        {
                            setPart.MatchingFlags = PartMatchingFlags.Matched;
                            continue;
                        }
                    }
                }

                var lddPart = FindLddPart(db, rbPart);

                if (lddPart == null)
                {
                    setPart.MatchingFlags = PartMatchingFlags.LddPartNotFound;
                    continue;
                }

                //Fix LDD elements that references part aliases
                if (setPart.LddElement != null)
                {
                    Debug.WriteLine("Encountered LDD Element on part alias");
                    string aliasID = setPart.LddElement.DesignID;
                    setPart.LddElement.DesignID = lddPart.DesignID;
                    foreach (var p in setPart.LddElement.Parts)
                    {
                        if (p.PartID == aliasID)
                            p.PartID = lddPart.DesignID;
                    }
                    //db.SaveChanges();
                }

                setPart.LddPart = lddPart;
                setPart.MatchingFlags = PartMatchingFlags.Matched;
            }

            foreach (var part in setParts.Where(x => x.MatchingFlags == PartMatchingFlags.Matched))
                Debug.WriteLine($"Matched Part '{part.PartName}' ({part.PartID}) to LDD ID {part.LddPartID}");

            foreach (var part in setParts.Where(x => x.MatchingFlags == PartMatchingFlags.LddPartNotFound))
                Debug.WriteLine($"Part '{part.PartName}' ({part.PartID}) not found in LDD");

            var unmatchedParts = setParts.Where(x => x.MatchingFlags == PartMatchingFlags.LddPartNotFound).ToList();

            if (unmatchedParts.Any())
            {
                var partDetails = RebrickableAPI.GetPartsDetails(unmatchedParts.Select(x => x.PartID).Distinct()).ToList();
                foreach (var partDetail in partDetails)
                {
                    if (partDetail.ExternalIds.LEGO?.Count > 0)
                    {
                        var legoPartIDs = partDetail.ExternalIds.LEGO.ToArray();
                        var matchingParts = db.LddParts.Where(x => legoPartIDs.Contains(x.DesignID)).ToList();
                        var foundPart = matchingParts.FirstOrDefault();
                        if (foundPart != null)
                        {
                            var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == partDetail.PartNum);
                            db.PartMappings.Add(new Models.PartMapping()
                            {
                                RebrickableID = rbPart.ParentPartID ?? rbPart.PartID,
                                LegoID = foundPart.DesignID,
                                MatchLevel = 3,
                                IsActive = true
                            });

                            foreach (var setPart in unmatchedParts.Where(x => x.PartID == partDetail.PartNum))
                            {
                                setPart.LddPart = foundPart;
                                setPart.MatchingFlags = PartMatchingFlags.Matched;
                            }
                        }

                    }
                }
            }

            db.SaveChanges();
        }

        public static LddPart FindLddPart(PaletteDbContext db, RbPart partToFind, string alternateID = null, List<string> testedIDs = null)
        {
            if (testedIDs == null)
                testedIDs = new List<string>();

            string partIDToFind = alternateID ?? partToFind.PartID;

            if (testedIDs.Contains(partIDToFind))
                return null;
            else
                testedIDs.Add(partIDToFind);

            var matches = db.PartMappings.Where(x => x.RebrickableID == partIDToFind).ToList();

            if (matches.Any())
            {
                if (matches.Count == 1)
                {
                    string legoID = matches[0].LegoID;
                    var lddPart = db.LddParts.FirstOrDefault(x => x.DesignID == legoID);
                    return lddPart;
                }
                else
                {

                }
            }

            var rbPart = (partToFind.PartID == partIDToFind) ? 
                partToFind : 
                db.RbParts.FirstOrDefault(x => x.PartID == alternateID);

            if (rbPart != null)
            {
                if (!string.IsNullOrEmpty(rbPart.ParentPartID))
                {
                    var partFound = FindLddPart(db, partToFind, rbPart.ParentPartID, testedIDs);
                    if (partFound != null)
                        return partFound;
                }

                foreach(var alt in rbPart.Relationships)
                {
                    if (alt.RelationTypeFlag == RbRelationType.Print || alt.RelationTypeFlag == RbRelationType.Pattern)
                        continue;

                    var partFound = FindLddPart(db, partToFind, alt.ChildPartID, testedIDs);
                    if (partFound != null)
                        return partFound;
                }
            }

            return null;
        }
    }
}
