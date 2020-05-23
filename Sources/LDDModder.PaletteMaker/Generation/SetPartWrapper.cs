using LDDModder.PaletteMaker.Models.LDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker.Generation
{
    public class SetPartWrapper
    {
        public string PartID { get; set; }
        public string PartName { get; set; }
        public int CategoryID { get; set; }
        public string ElementID { get; set; }
        public int ColorID { get; set; }
        public int Quantity { get; set; }

        public Models.Rebrickable.RbPart RbPart { get; set; }

        public PartMatchingFlags MatchingFlags { get; set; }

        public LddPart LddPart { get; set; }

        public string LddPartID => LddPart?.DesignID;

        public LddElement LddElement { get; set; }

        public bool LddPartFound => LddPart != null;

        public int LddColorID { get; set; }

        public bool IsGeneratedElement { get; set; }

        public bool IsValid => LddPartFound && LddColorID >= 0;

        public List<string> LegoIDs { get; } = new List<string>();

        public SetPartWrapper()
        {

        }

        public SetPartWrapper(Rebrickable.Models.SetPart setPart)
        {
            PartID = setPart.Part.PartNum;
            PartName = setPart.Part.Name;
            CategoryID = setPart.Part.PartCatId;
            ElementID = setPart.ElementId;
            ColorID = setPart.Color.Id;
            Quantity = setPart.Quantity;
        }
    }
}
