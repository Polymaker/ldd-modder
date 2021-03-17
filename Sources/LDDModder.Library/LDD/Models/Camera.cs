using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class Camera : TransformableModelItem
    {
        public double FieldOfView { get; set; }
        public double Distance { get; set; }

        public override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);
            FieldOfView = element.ReadAttribute("fieldOfView", 0d);
            Distance = element.ReadAttribute("distance", 0d);
        }
    }
}
