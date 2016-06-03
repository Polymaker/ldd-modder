using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDDModder.PaletteMaker.Rebrickable
{
    [XmlRoot("set"), Serializable]
    public class SetParts
    {
        [XmlElement("set_id")]
        public string SetId { get; set; }

        [XmlElement("descr")]
        public string Description { get; set; }

        [XmlElement("set_img_url")]
        public string ImageUrl { get; set; }

        [XmlArray("parts"), XmlArrayItem("part")]
        public SetParts.Part[] Parts { get; set; }

        [Serializable]
        public class Part
        {
            [XmlElement("part_id")]
            public string PartId { get; set; }
            [XmlElement("qty")]
            public int Quantity { get; set; }
            [XmlElement("ldraw_color_id")]
            public int LDrawColorId { get; set; }
            [XmlElement("type")]
            public int Type { get; set; }
            [XmlElement("part_name")]
            public string Name { get; set; }
            [XmlElement("color_name")]
            public string ColorName { get; set; }
            [XmlElement("part_img_url")]
            public string ImageUrl { get; set; }
            [XmlElement("element_id")]
            public string ElementId { get; set; }
            [XmlElement("element_img_url")]
            public string ElementImageUrl { get; set; }
            [XmlElement("rb_color_id")]
            public int RbColorId { get; set; }
            [XmlElement("part_type_id")]
            public int PartTypeId { get; set; }
        }
    }

    public class GetSetPartsParameters : ApiFunctionParameters
    {
        [ApiParameter("set")]
        public string SetId { get; set; }

        public GetSetPartsParameters(string setId)
        {
            SetId = setId;
        }

        public static implicit operator GetSetPartsParameters(string setId)
        {
            return new GetSetPartsParameters(setId);
        }
    }
}
