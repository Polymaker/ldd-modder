using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddElements")]
    public class LddElement
    {
        [Key]
        public string ElementID { get; set; }

        public string DesignID { get; set; }

        public bool IsAssembly { get; set; }

        public int Flag { get; set; }

        public virtual ICollection<ElementPart> Parts { get; set; }

        public LddElement()
        {
            Parts = new List<ElementPart>();
        }

        public LDDModder.LDD.Palettes.PaletteItem ToPaletteItem(int quantity = 0)
        {
            if (IsAssembly)
            {
                var assembly = new LDDModder.LDD.Palettes.Assembly(int.Parse(DesignID), ElementID, quantity);

                foreach (var partConfig in Parts)
                {
                    var part = new LDDModder.LDD.Palettes.Assembly.Part(
                        int.Parse(partConfig.PartID), partConfig.MaterialID);

                    foreach (var dec in partConfig.Decorations)
                        part.Decorations.Add(new LDDModder.LDD.Palettes.Decoration(dec.SurfaceID, dec.DecorationID));

                    foreach (var mat in partConfig.SubMaterials)
                        part.SubMaterials.Add(new LDDModder.LDD.Palettes.SubMaterial(mat.SurfaceID, mat.MaterialID));
                    assembly.Parts.Add(part);
                }

                return assembly;
            }
            else
            {
                var brick = new LDDModder.LDD.Palettes.Brick(int.Parse(DesignID), ElementID, quantity);
                var partConfig = Parts.FirstOrDefault();
                brick.MaterialID = partConfig.MaterialID;
                foreach (var dec in partConfig.Decorations)
                    brick.Decorations.Add(new LDDModder.LDD.Palettes.Decoration(dec.SurfaceID, dec.DecorationID));
                foreach (var mat in partConfig.SubMaterials)
                    brick.SubMaterials.Add(new LDDModder.LDD.Palettes.SubMaterial(mat.SurfaceID, mat.MaterialID));
                return brick;
            }

        }
    
        public LddElement Clone(string newElementID = null)
        {
            var newElem = new LddElement()
            {
                DesignID = DesignID,
                IsAssembly = IsAssembly,
                ElementID = newElementID ?? ElementID
            };
            foreach (var conf in Parts)
                newElem.Parts.Add(conf.Clone());
            return newElem;
        }
    }
}
