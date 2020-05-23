using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.LDD.Data
{
    public enum MaterialType
    {
        [XmlEnum(Name = "shinyPlastic")]
        ShinyPlastic,
        [XmlEnum(Name = "shinySteel")]
        ShinySteel,
        [XmlEnum(Name = "glitter")]
        Glitter
    }
}
