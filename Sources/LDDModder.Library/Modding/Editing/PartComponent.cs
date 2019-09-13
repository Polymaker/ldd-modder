using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding.Editing
{
    public abstract class PartComponent
    {
        public string RefID { get; set; }
        public string Comments { get; set; }

        public virtual XElement SerializeToXml()
        {
            return SerializeToXmlBase(GetType().Name);
        }

        protected XElement SerializeToXmlBase(string elementName)
        {
            var elem = new XElement(elementName);
            if (!string.IsNullOrEmpty(RefID))
                elem.Add(new XAttribute("RefID", RefID));
            if (!string.IsNullOrEmpty(Comments))
                elem.Add(new XElement("Comments", Comments));
            return elem;
        }
    }
}
