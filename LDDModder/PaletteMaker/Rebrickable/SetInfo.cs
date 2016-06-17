using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.PaletteMaker.Rebrickable
{
    [XmlRoot("set"), Serializable]
    public class SetInfo
    {
        [XmlElement("set_id")]
        public string SetId { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("pieces")]
        public string Pieces { get; set; }

        [XmlElement("descr")]
        public string Description { get; set; }

        [XmlElement("theme")]
        public string Theme { get; set; }

        [XmlElement("year")]
        public string Year { get; set; }

        [XmlElement("img_tn")]
        public string ImageUrlTiny { get; set; }

        [XmlElement("img_sm")]
        public string ImageUrlSmall { get; set; }

        [XmlElement("img_big")]
        public string ImageUrlBig { get; set; }
    }

    public class GetSetParameters : ApiFunctionParameters
    {
        [ApiParameter("set_id")]
        public string SetId { get; set; }

        public GetSetParameters(string setId)
        {
            SetId = setId;
        }

        public static implicit operator GetSetParameters(string setId)
        {
            return new GetSetParameters(setId);
        }
    }
}
