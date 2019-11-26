using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LDDModder.PaletteMaker.Models.LDD
{
    [Table("LddAssemblyParts")]
    public class LddAssemblyPart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string AssemblyID { get;  set; }

        public string PartID { get; set; }

        public string DefaultMaterials { get; set; }

        public List<int> GetMaterials()
        {
            var materialIDs = new List<int>();
            var materials = DefaultMaterials.Split(',');
            for (int i = 0; i < materials.Length; i++)
            {
                if (int.TryParse(materials[i], out int matID))
                    materialIDs.Add(matID);
            }
            return materialIDs;
        }
    }
}
