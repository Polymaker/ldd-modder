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
                    setPart.MatchingFlags = PartMatchingFlags.InvalidRbColor;
                    continue;
                }

                var lddColor = rbColor.ColorMatches.Where(x => x.Platform == "LEGO").OrderBy(x => x.ColorID).FirstOrDefault();
                if (lddColor == null)
                {
                    setPart.MatchingFlags = PartMatchingFlags.InvalidLddColor;
                    continue;
                }

                setPart.LddColorID = lddColor.ColorID;

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
                    //db.SaveChanges();
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
                            Debug.WriteLine($"Matched Part '{partDetail.Name}' ({partDetail.PartNum}) to LDD ID {foundPart.DesignID}");
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
                    var brick = new LDD.Palettes.Brick(int.Parse(setPart.LddPartID), string.Empty, setPart.Quantity);
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
    }
}
