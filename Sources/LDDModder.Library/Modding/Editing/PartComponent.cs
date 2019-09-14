using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.Modding.Editing
{
    public abstract class PartComponent
    {
        [XmlAttribute]
        public string RefID { get; set; }

        [XmlElement]
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
                elem.Add(XmlHelper.ToXml(() => Comments));

            return elem;
        }

        protected internal virtual void LoadFromXml(XElement element)
        {
            if (element.Attribute("RefID") != null)
                RefID = element.Attribute("RefID").Value;

            if (element.Element("Comments") != null)
                Comments = element.Element("Comments").Value;
        }
    }
}
