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

                setPart.RbPart = db.RbParts.FirstOrDefault(x => x.PartID == setPart.PartID);

                if (setPart.RbPart == null)
                {
                    setPart.MatchingFlags = PartMatchingFlags.InvalidRbPart;
                    continue;
                }

                if (!rbColors.TryGetValue(setPart.ColorID, out RbColor rbColor))
                {
                    setPart.MatchingFlags |= PartMatchingFlags.InvalidRbColor;
                    //continue;
                }

                var lddColor = rbColor.ColorMatches.Where(x => x.Platform == "LEGO").OrderBy(x => x.ColorID).FirstOrDefault();
                if (lddColor == null)
                {
                    setPart.MatchingFlags |= PartMatchingFlags.InvalidLddColor;
                    //continue;
                }

                setPart.LddColorID = lddColor?.ColorID ?? -1;

                if (!string.IsNullOrEmpty(setPart.ElementID))
                {
                    var foundElem = db.LddElements.FirstOrDefault(x => x.ElementID == setPart.ElementID);
                    if (foundElem != null)
                    {
                        setPart.LddElement = foundElem;
                        setPart.IsGeneratedElement = false;
                        setPart.LddPart = db.LddParts.FirstOrDefault(x => x.DesignID == foundElem.DesignID);

                        if (setPart.LddPart != null)
                        {
                            setPart.MatchingFlags = PartMatchingFlags.Matched;
                            continue;
                        }
                    }
                }

                var lddPart = FindLddPart(db, setPart.RbPart);

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
                    db.SaveChanges();
                }

                setPart.LddPart = lddPart;
                setPart.MatchingFlags = PartMatchingFlags.Matched;
            }

            foreach (var part in setParts.Where(x => x.MatchingFlags == PartMatchingFlags.Matched))
                Debug.WriteLine($"Matched Part '{part.PartName}' ({part.PartID}) to LDD ID {part.LddPartID}");

            var unmatchedParts = setParts.Where(x => x.MatchingFlags == PartMatchingFlags.LddPartNotFound).ToList();

            if (unmatchedParts.Any())
            {
                Debug.WriteLine("Querying rebrickable....");
                var newMappings = new List<Models.PartMapping>();

                var partDetails = RebrickableAPI.Parts.GetParts(
                    partNums: unmatchedParts.Select(x => x.PartID).Distinct(),
                    includePartDetails: true
                ).ToList();

                foreach (var partDetail in partDetails)
                {
                    if (partDetail.ExternalIds.LEGO?.Count > 0)
                    {
                        var relatedSetParts = unmatchedParts.Where(x => x.PartID == partDetail.PartNum).ToList();

                        var legoPartIDs = partDetail.ExternalIds.LEGO.ToArray();

                        relatedSetParts.ForEach(x => x.LegoIDs.AddRange(legoPartIDs));

                        var matchingParts = db.LddParts.Where(x => legoPartIDs.Contains(x.DesignID)).ToList();

                        var foundPart = matchingParts.FirstOrDefault();
                        if (foundPart != null)
                        {
                            Debug.WriteLine($"Matched Part '{partDetail.Name}' ({partDetail.PartNum}) to LDD ID {foundPart.DesignID}");
                            var rbPart = db.RbParts.FirstOrDefault(x => x.PartID == partDetail.PartNum);

                            if (!newMappings.Any(x => x.RebrickableID == (rbPart.ParentPartID ?? rbPart.PartID)))
                            {
                                newMappings.Add(new Models.PartMapping()
                                {
                                    RebrickableID = rbPart.ParentPartID ?? rbPart.PartID,
                                    LegoID = foundPart.DesignID,
                                    MatchLevel = 3,
                                    IsActive = true
                                });
                            }

                            foreach (var setPart in unmatchedParts.Where(x => x.PartID == partDetail.PartNum))
                            {
                                setPart.LddPart = foundPart;
                                setPart.MatchingFlags |= PartMatchingFlags.Matched;
                            }
                        }

                    }
                }
            
                if (newMappings.Any())
                {
                    db.PartMappings.AddRange(newMappings);
                    db.SaveChanges();
                }
            }

            

            foreach (var part in setParts.Where(x => x.LddPartFound && x.LddElement == null))
            {
                part.LddElement = GenererateElement(db, part);
                part.IsGeneratedElement = (part.LddElement != null);
            }


            foreach (var part in setParts.Where(x => x.MatchingFlags == PartMatchingFlags.LddPartNotFound))
                Debug.WriteLine($"Part '{part.PartName}' ({part.PartID}) not found in LDD");
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

            var foundLddPart = db.LddParts.FirstOrDefault(x => x.DesignID == partIDToFind);
            if (foundLddPart != null)
                return foundLddPart;

            var matches = db.PartMappings.Where(x => x.RebrickableID == partIDToFind && x.IsActive).ToList();

            //if (matches.Count == 0)
            //{
            //    var mapping = new Models.PartMapping()
            //    {
            //        RebrickableID = partIDToFind,
            //        LegoID = partIDToFind,
            //        IsActive = true,
            //        MatchLevel = 0
            //    };
            //    return lddPart;
            //}
            if (matches.Count == 1)
            {
                string legoID = matches[0].LegoID;
                var lddPart = db.LddParts.FirstOrDefault(x => x.DesignID == legoID);
                if (lddPart == null)
                {
                    Debug.WriteLine($"could not find ldd part {lddPart.DesignID} but a mapping exists");
                }
                return lddPart;
            }
            else if (matches.Count > 1)
            {
                Debug.WriteLine($"Multiple matches for part {partToFind.PartID} '{partToFind.Name}'!");

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
    
        public static LDDModder.LDD.Palettes.Palette GeneratePalette(PaletteDbContext db, List<SetPartWrapper> setParts)
        {
            var palette = new LDDModder.LDD.Palettes.Palette();

            foreach (var setPart in setParts)
            {
                if (setPart.LddPart == null)
                    continue;

                if (setPart.LddElement != null)
                {
                    var item = setPart.LddElement.ToPaletteItem(setPart.Quantity);
                    palette.Items.Add(item);
                }
                else if (!setPart.LddPart.IsAssembly)
                {
                    var brick = new LDD.Palettes.Brick(int.Parse(setPart.LddPartID), 
                        setPart.ElementID ?? string.Empty, setPart.Quantity);

                    brick.MaterialID = setPart.LddColorID;
                    palette.Items.Add(brick);
                }
                else
                {
                    var existingElem = db.LddElements
                        .Where(x => x.DesignID == setPart.LddPartID)
                        .OrderByDescending(x => x.Parts.FirstOrDefault().MaterialID == setPart.LddColorID)
                        .FirstOrDefault();

                    if (existingElem != null)
                    {
                        existingElem = existingElem.Clone();
                        existingElem.ElementID = setPart.ElementID ?? string.Empty;
                        existingElem.ChangeColor(setPart.LddColorID);
                        var item = existingElem.ToPaletteItem(setPart.Quantity);
                        palette.Items.Add(item);
                    }
                    else
                    {
                        var subParts = db.AssemblyParts.Where(x => x.AssemblyID == setPart.LddPartID).ToList();

                    }
                }
            }
            return palette;
        }
    
        public static LddElement GenererateElement(PaletteDbContext db, SetPartWrapper partWrapper)
        {
            if (string.IsNullOrEmpty(partWrapper.ElementID))
                return null;

            var existingElem = db.LddElements.FirstOrDefault(x => x.DesignID == partWrapper.LddPartID);

            if (existingElem != null)
            {
                var newElement = existingElem.Clone(partWrapper.ElementID);
                newElement.ChangeColor(partWrapper.LddColorID);
                newElement.RemoveDecorations();
                return newElement;
            }

            void AddElementPart(LddElement lddElement, LddPart lddPart, int colorID)
            {
                var partConfig = new ElementPart()
                {
                    PartID = lddPart.DesignID,
                    MaterialID = colorID
                };

                var subMats = lddPart.GetSubMaterials();

                if (subMats.Length > 0)
                {
                    var addedIndices = new List<int>();
                    for (int i = 0; i < subMats.Length; i++)
                    {
                        if (!addedIndices.Contains(subMats[i]) && subMats[i] != 0)
                        {
                            partConfig.SubMaterials.Add(new ElementMaterial(i, colorID));
                            addedIndices.Add(subMats[i]);
                        }
                    }
                }

                lddElement.Parts.Add(partConfig);
            }

            if (!partWrapper.LddPart.IsAssembly)
            {
                var newElement = new LddElement()
                {
                    DesignID = partWrapper.LddPartID,
                    ElementID = partWrapper.ElementID,
                    Flag = 1,
                    IsAssembly = false
                };
                AddElementPart(newElement, partWrapper.LddPart, partWrapper.LddColorID);
                return newElement;
            }
            else
            {
                var newElement = new LddElement()
                {
                    DesignID = partWrapper.LddPartID,
                    ElementID = partWrapper.ElementID,
                    Flag = 1,
                    IsAssembly = true
                };

                foreach (var assemPart in db.AssemblyParts.Where(x => x.AssemblyID == partWrapper.LddPartID))
                {
                    var subPart = db.LddParts.FirstOrDefault(x => x.DesignID == assemPart.PartID);
                    AddElementPart(newElement, subPart, partWrapper.LddColorID);
                }
                return newElement;
            }
        }
    }
}
