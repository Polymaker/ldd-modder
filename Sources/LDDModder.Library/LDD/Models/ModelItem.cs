using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public abstract class ModelItem
    {
        public int RefID { get; set; }

        public virtual void LoadFromXml(XElement element)
        {
            RefID = element.ReadAttribute("refID", 0);
        }
    }
}
