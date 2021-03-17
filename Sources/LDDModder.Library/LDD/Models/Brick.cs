using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class Brick : ModelItem
    {
        public string UUID { get; set; }

        public int DesignID { get; set; }
        public int? ElementID { get; set; }

        public Part Part { get; set; }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            UUID = element.ReadAttribute("uuid", string.Empty);
            DesignID = element.ReadAttribute("designID", 0);
            ElementID = element.ReadAttribute("itemNos", (int?)null);
            Part = null;

            if (element.HasElement("Part", out XElement partElem))
            {
                Part = new Part();
                Part.LoadFromXml(partElem);
            }
            else
            {
                Part = new Part()
                {
                    DesignID = DesignID
                };
                if (element.TryReadAttribute("materialID", out int matId))
                    Part.Materials.Add(matId);
            }
        }

        protected override void SerializeElement(XElement element)
        {
            base.SerializeElement(element);
            if (!string.IsNullOrEmpty(UUID))
                element.WriteAttribute("uuid", UUID);
            element.WriteAttribute("designID", DesignID);
            if (ElementID.HasValue)
                element.WriteAttribute("itemNos", ElementID.Value);
            if (Part != null)
                element.Add(Part.SerializeToXml());
        }
    }
}
