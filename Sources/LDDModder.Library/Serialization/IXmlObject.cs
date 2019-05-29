using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Serialization
{
    public interface IXmlObject
    {
        XElement SerializeToXml();

        void LoadFromXml(XElement element);
    }
}
