using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.LDD.Models
{
    [Serializable, XmlRoot("LXFML")]
    public class ModelFile : XSerializable
    {


        protected override void DeserializeFromXElement(XElement element)
        {
            throw new NotImplementedException();
        }

        protected override XElement SerializeToXElement()
        {
            throw new NotImplementedException();
        }
    }
}
