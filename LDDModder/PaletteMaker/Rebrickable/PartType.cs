using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.PaletteMaker.Rebrickable
{
    [XmlRoot("part_types")]
    public class PartTypeList
    {
        [XmlElement("part_type")]
        public List<PartType> PartTypes { get; set; }
    }

    [XmlRoot("part_type")]
    public class PartType
    {
        [XmlElement("part_type_id")]
        public int TypeID { get; set; }

        [XmlElement("desc")]
        public string Description { get; set; }
    }
}
