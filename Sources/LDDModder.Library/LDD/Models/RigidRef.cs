using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Models
{
    public class RigidRef
    {
        public int RefID { get; set; }

        public void LoadFromXml(XElement element)
        {
            RefID = element.ReadAttribute("rigidRef", 0);
        }
    }
}
