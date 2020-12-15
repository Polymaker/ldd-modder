using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddParts")]
    public class LddPart
    {
        [Key]
        public string DesignID { get; set; }

        public string Name { get; set; }

        public bool IsAssembly { get; set; }

        public string Aliases { get; set; }

        public string SubMaterials { get; set; }

        public string[] GetAliases()
        {
            if (!string.IsNullOrEmpty(Aliases))
            {
                return Aliases.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        public int[] GetSubMaterials()
        {
            var materialIDs = new List<int>();
            if (!string.IsNullOrEmpty(SubMaterials))
            {
                string[] idSplit = SubMaterials.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var idStr in idSplit)
                {
                    if (int.TryParse(idStr, out int matID))
                        materialIDs.Add(matID);
                }
            }
            return materialIDs.ToArray();
        }
    }
}
