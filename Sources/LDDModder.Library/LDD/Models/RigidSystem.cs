using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class RigidSystem : IXmlObject
    {
        public List<RigidItem> RigidItems { get; set; }

        public RigidSystem()
        {
            RigidItems = new List<RigidItem>();
        }

        public XElement SerializeToXml()
        {
            var elem = new XElement("RigidSystem");
            foreach (var item in RigidItems)
                elem.Add(item.SerializeToXml());
            return elem;
        }

        public void LoadFromXml(XElement element)
        {
            foreach (var rigidElem in element.Elements("Rigid"))
            {
                var item = new RigidItem();
                item.LoadFromXml(element);
                RigidItems.Add(item);
            }
        }
    }
}
