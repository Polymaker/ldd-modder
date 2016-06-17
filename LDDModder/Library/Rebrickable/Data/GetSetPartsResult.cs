using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Rebrickable.Data
{
    [XmlRoot("set")]
    public class GetSetPartsResult
    {
        /// <summary>
        /// Set ID.
        /// </summary>
        [XmlElement("set_id")]
        public string SetId { get; set; }
        /// <summary>
        /// Set description.
        /// </summary>
        [XmlElement("descr")]
        public string Description { get; set; }
        /// <summary>
        /// Set image URL.
        /// </summary>
        [XmlElement("set_img_url")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// List of parts in the set.
        /// </summary>
        [XmlArray("parts"), XmlArrayItem("part")]
        public List<Part> Parts { get; set; }

        [XmlRoot("part")]
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

        public GetSetPartsResult()
        {
            SetId = String.Empty;
            Description = String.Empty;
            ImageUrl = String.Empty;
            Parts = new List<Part>();
        }
    }
}
