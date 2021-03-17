using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public abstract class ModelItem : IXmlObject
    {
        public int RefID { get; set; }

        public virtual string NodeName
        {
            get
            {
                return GetType().Name;
            }
        }

        public virtual void LoadFromXml(XElement element)
        {
            RefID = element.ReadAttribute("refID", 0);
        }

        public XElement SerializeToXml()
        {
            var elem = new XElement(NodeName);
            elem.Add(new XAttribute("refID", RefID));
            SerializeElement(elem);
            return elem;
        }

        protected virtual void SerializeElement(XElement element)
        {

        }  
    }
}
