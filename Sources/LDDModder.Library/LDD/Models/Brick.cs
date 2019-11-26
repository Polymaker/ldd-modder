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

        public string DesignID { get; set; }

        public Part Part { get; set; }


        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            UUID = element.ReadAttribute("uuid", string.Empty);
            DesignID = element.ReadAttribute("designID", string.Empty);

            Part = null;

            if (element.HasElement("Part", out XElement partElem))
            {
                Part = new Part();
                Part.LoadFromXml(partElem);
            }
        }
    }
}
