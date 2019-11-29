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

        public string LddPartID => LddPart?.DesignID;

        public PartMatchingFlags MatchingFlags { get; set; }

        public LddPart LddPart { get; set; }

        public LddElement LddElement { get; set; }

        public bool IsGeneratedElement { get; set; }

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
